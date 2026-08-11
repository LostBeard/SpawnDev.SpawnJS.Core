# SpawnDev.SpawnJS.Core - Code Audit

**Auditor:** Lt. Cmdr. Tuvok
**Date:** 2026-08-11
**Scope:** `SpawnDev.SpawnJS` library + `SpawnDev.SpawnJS.Demo` + the JS module `wwwroot/SpawnDev.SpawnJS.lib.module.js`
**Build state at audit:** `dotnet build` succeeds, 0 errors, 6 warnings (library). Captain confirmed it runs as-is.

This audit was produced alongside a comments-and-cleanup pass. **No logic was changed** during that pass - every issue below is reported here, not silently "fixed", because fixing them changes behavior and that was explicitly out of scope for the pass. Each item states what I verified by reading code vs. what still needs a runtime check.

Severity legend: 🔴 High (latent runtime break) · 🟠 Medium (wrong/misleading behavior or measurable perf loss) · 🟡 Low (edge case / hygiene). A ✅ prefix = fixed after the audit; kept as a record.

### Status (updated 2026-08-11, verified by reading current source)

| ID | Title | State |
|---|---|---|
| H1 | Async Boolean/nullable/object-ref resolvers missing on C# side | ✅ Fixed |
| H2 | Four `[JSExport]`s share the name `AsyncCallResolvedDouble` | ✅ Fixed |
| H3 | `bool` async registered in `_booleanCallbacks`, drained elsewhere | ✅ Fixed |
| M1 | `GetAsync(Type,key)` was synchronous | ✅ Fixed |
| M2 | Marshaller cache write-only (no fast path) | ✅ Fixed |
| M3 | Multiple `SpawnJSRuntime` construction | ✅ Fixed |
| L1 | `ReleaseAsJson<T>` throws on null JSON | 🟡 Open |
| L2 | Sync switch lacks `SpawnJSObjectReferenceNonNullable` | 🟡 Open |
| L3 | JS `propertyTypeInfo` tests full path not property name | 🟡 Open |
| L4 | Reference marshaller maps id ≤ 0 to null | 🟡 Open |

---

## ✅ H1 (FIXED 2026-08-11) - Async Boolean / nullable / object-reference resolvers were missing on the C# side

**Files:** `SpawnJSRuntime.Marshal.cs` (the `AsyncCallResolved*` `[JSExport]`s) and `wwwroot/SpawnDev.SpawnJS.lib.module.js` (`_spawnJSInteropCallAsync`).

**Was:** the JS async dispatcher called a differently-named resolver per return type, but C# only exported `AsyncCallResolvedVoid`, `AsyncCallResolvedDouble`, and `AsyncCallResolvedString`. So cases 2 (Boolean), 3 (DoubleNullable), 4 (BooleanNullable), 6 (SpawnJSObjectReference), and 7 (NonNullable) called names that did not exist - JS would throw `...AsyncCallResolvedBoolean is not a function`, the completion would never fire, and the awaiting `Task` would hang/fault. The mainstream case at risk was **#6, async returning a JS object reference** (`CallApplyAsync<SpawnJSObjectReference>` / `GetAsync<SomeWrapper>`).

**Now (verified by reading + grep):** C# exports all six distinctly-named resolvers - `AsyncCallResolvedVoid`, `AsyncCallResolvedDouble`, `AsyncCallResolvedDoubleNullable`, `AsyncCallResolvedBoolean`, `AsyncCallResolvedBooleanNullable`, `AsyncCallResolvedString` - and every name the JS side calls now resolves to an existing export. Cases 6/7 correctly route to `AsyncCallResolvedDoubleNullable` (draining `_doubleNullableCallbacks`).

**Still recommended:** a per-`ReturnType` async round-trip unit test (value path + error path) so this dispatch table can't silently drift again.

---

## ✅ H2 (FIXED 2026-08-11) - Four `[JSExport]` methods shared the exported name `AsyncCallResolvedDouble`

**File:** `SpawnJSRuntime.Marshal.cs`.

**Was:** four overloads all named `AsyncCallResolvedDouble` (differing only by value type: `double`, `double?`, `bool`, `bool?`). Since `[JSExport]` publishes by method name, all four collided on the single JS symbol `spawnJSExports.SpawnJSRuntime.AsyncCallResolvedDouble`; only one could be reachable, the others were dead.

**Now (verified by reading):** each resolver has a unique name (`AsyncCallResolvedDouble`, `AsyncCallResolvedDoubleNullable`, `AsyncCallResolvedBoolean`, `AsyncCallResolvedBooleanNullable`), so there is no name collision and each maps to its own JS symbol. Fixed together with H1/H3.

---

## ✅ H3 (FIXED 2026-08-11) - Non-nullable `bool` async completion was registered in one dictionary and drained from another

**File:** `SpawnJSRuntime.Marshal.cs`, `InteropCallApplyAsync<T>` + the resolvers.

**Was:** the `ReturnType.Boolean` case registered into `_booleanCallbacks` (`Action<bool, string?>`), but the resolver that fired drained `_booleanNullableCallbacks` - so `_booleanCallbacks` was written and never read, and a `bool` async return could never complete.

**Now (verified by reading):** the dedicated `AsyncCallResolvedBoolean(double, bool, string?)` export drains `_booleanCallbacks`, and `AsyncCallResolvedBooleanNullable(double, bool?, string?)` drains `_booleanNullableCallbacks` - registration and drain now line up for both shapes.

**H1+H2+H3 were one defect surface** (the async return-type dispatch table across JS names, C# export names, and C# callback dictionaries) and were fixed together. The remaining follow-up is the per-`ReturnType` async round-trip test noted in H1.

---

## ✅ M1 (FIXED 2026-08-11) - `GetAsync(Type, key)` overloads were synchronous

**File:** `SpawnJSObjectReference.cs`.

**Was:** the three non-generic `GetAsync(Type, key)` overloads called the synchronous `InteropCallApply` and returned `object?` instead of a `Task` - a blocking call behind an async name (copy-paste from the `Get(Type, key)` block).

**Now:** they call `InteropCallApplyAsync(type, ...)` and return `Task<object?>`. Fixed live during the audit; verified by reading. Left here as a record.

---

## ✅ M2 (FIXED 2026-08-11) - The per-type marshaller cache was populated but never read

**File:** `SpawnJSRuntime.Marshal.cs`, `GetMarshaller<TType>`.

**Was:** `GetMarshaller<TType>` wrote resolved marshallers into `_typeMarshallerCache` but never read it, so every resolution paid the full reverse linear scan (+ `CanMarshal` per candidate + a `GetMarshaller<T>()` specialization, which for `ArrayMarshaller` is an `Activator.CreateInstance` allocation) on every argument of every call and every return.

**Now (verified by reading):** `GetMarshaller<TType>` opens with `if (_typeMarshallerCache.TryGetValue(type, out var cachedMarshaller)) return (JSMarshaller<TType>)cachedMarshaller;` - the resolved specialization is cached on first use and reused thereafter, restoring the intended fast path.

**Follow-ups (still open, low priority):**
- The `if (type == null)` branch inside `GetMarshaller<TType>` remains dead (`type = typeof(TType)` is never null), so `_nullTypeMarshaller` is never assigned there. Harmless.
- The descriptor-level cache fields `CachedMarshaller` and `NameSlot` on `Reflection/ClassMemberJsonInfo.cs` are still never assigned (both `CS0649`) - a finer-grained caching layer that is described in comments but not yet wired. The runtime-level cache above is the main win; these are optional.

---

## ✅ M3 (FIXED 2026-08-11) - Multiple `SpawnJSRuntime` construction

**File:** `SpawnJSRuntime.cs`.

**Was:** the public parameterless constructor let callers construct more than one runtime (the Demo did `new SpawnJSRuntime()`), which would hold the DotnetInstance more than once and repoint `_instance`.

**Now (verified by reading + Demo build):** the constructor is `private`, so `SpawnJSRuntime.Instance` is the only construction path (single, lazy). The Demo was updated to `SpawnJSRuntime.Instance`; the Demo project builds green.

---

## 🟡 L1 - `ReleaseAsJson<T>` throws on a null/undefined held value

**File:** `SpawnJSObjectReference.cs`.

**Verified by reading.** `ReleaseAsJson<T>` does `JsonSerializer.Deserialize<T>(json, ...)` where `json` originates from JS `JSON.stringify(value)`. For a held `undefined`, `JSON.stringify` yields JS `undefined` → marshalled to .NET as `null` string → `Deserialize<T>((string)null)` throws `ArgumentNullException`. The sibling `PropertyGetJson<T>` guards this exact case (`json == null ? default!`); `ReleaseAsJson<T>` does not. Low likelihood, easy guard.

---

## 🟡 L2 - Sync call path throws on `SpawnJSObjectReferenceNonNullable`

**File:** `SpawnJSRuntime.Marshal.cs`, `InteropCallApply<T>` switch.

**Verified by reading.** The synchronous switch has no `case ReturnType.SpawnJSObjectReferenceNonNullable` and falls to `default: throw`. The async switch does handle it. No registered marshaller currently emits that `ReturnType`, so it is latent - but any future non-nullable-reference marshaller would work async and throw sync. Also worth a runtime check: the sync `SpawnJSObjectReference` case uses the **non-nullable** `_spawnJSInteropCallDouble` import; confirm a JS `null`/`undefined` reference result marshals cleanly to a .NET `double` there (a nullable-double import may be safer for the reference case).

---

## 🟡 L3 - JS `propertyTypeInfo` tests the full path instead of the resolved property name

**File:** `wwwroot/SpawnDev.SpawnJS.lib.module.js`, `propertyTypeInfo`.

**Verified by reading.** After `pathObjectInfo` resolves a dotted path to `{ parent, propertyName }`, `propertyTypeInfo` checks `if (!SpawnJSInterop._in(key, parent)) return null;` using the **original full `key`** rather than `propertyName`. For a dotted key like `"a.b"`, `parent` is the nested object and `propertyName` is `"b"`, so `_in("a.b", parent)` is false and the method wrongly returns `null`. Every other `property*` method correctly uses `propertyName`. Only affects dotted-path type probes.

---

## 🟡 L4 - `SpawnJSObjectReferenceMarshaller` maps any id ≤ 0 to null

**File:** `Marshallers/SpawnJSObjectReferenceMarshaller.cs`.

**Verified by reading.** `JSToNet(Type, double)` returns null for `value <= 0`, which folds the sentinel ids (e.g. `globalThis = -1`) into "no reference". In practice `propertyGetSpawnJSObjectReference` only ever returns a positive held id or JS null, so a sentinel id never arrives as a marshalled reference value - but the assumption is undocumented and would surface if a sentinel-backed reference were ever marshalled. Worth an explicit comment or an `== 0` check if only 0 is meant to be "none".

---

## 🟡 L5 - Compiler warnings (1 remaining, as of 2026-08-11)

**File:** one spot, intentional. TJ cleared all the others during the L5 cleanup (`CS0219 nmt`, `CS0219 sjsId`, `CS0168 ex`, `CS0649 NameSlot`, `CS0414 _AsyncJSTaskNext`, `CS8603`, and `CS8765`).

The `CS8603` → `TElement[]?` annotation fix on `ArrayMarshaller` also surfaced a real latent bug: `NetToJS` had no null check and would have thrown on a null array. Fixed while closing the follow-on `CS8765` (both `NetToJS` overloads now take `TElement[]?` and null-guard).

| Warning | Location | Note |
|---|---|---|
| `CS0649` `CachedMarshaller` never assigned | `ClassMemberJsonInfo.cs` | Kept deliberately as a forward-looking field for a future `ObjectMarshaller`. Documented; accept until that marshaller lands. |

---

## Not bugs - intentional / WIP (documented so they aren't re-flagged)

- **`Callback` is not wired up** (Captain confirmed). `Callback.cs` is a stub; `FireCallback` / `FireCallbackAsync` are logging placeholders. The JS→.NET callback path is future work.
- **The `var nmt = true;` / `var nmt1 = true;` lines** in the Demo are TJ's long-standing family-named breakpoint anchors. They are intentional; leave them. (The one formerly in `GetMarshaller` was removed by TJ during the M2 fix.)
- **The "no `object`, never `JSObject`" design.** The removed commented-out `object`-key overloads were deliberate - property keys are only ever `string` or `double`, and the single permitted `JSObject` use is `spawnJSObjectHold(JSHost.DotnetInstance)`. This is the core thesis of the rebuild and is correctly enforced.
- **The runtime `Type` → compile-time `<T>` trick** (`DelegateExtensions.InvokeGeneric` + the `writeTyped<T1>` local functions) is the mechanism that keeps marshalling boxing-free. It is deliberate and correct; M2 is only about caching its *selection*, not changing it.

---

## Suggested priority

1. **Async round-trip unit test per `ReturnType`** (value + error path). H1/H2/H3 are fixed, but they are the kind of dispatch table that silently drifts; a test locks them down. This is now the highest-value item.
2. **L1-L4** - edge-case guards (`ReleaseAsJson` null guard, sync `SpawnJSObjectReferenceNonNullable` case, JS `propertyTypeInfo` full-path bug, reference-marshaller id ≤ 0).
3. **L5** - clear the 6 remaining warnings.

**Fixed since the audit was written (2026-08-11):** H1, H2, H3, M1, M2, M3. Remaining open: L1, L2, L3, L4, L5. This document is kept current as a record of what's done and what's left.
