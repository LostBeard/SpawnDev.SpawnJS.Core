using SpawnDev.SpawnJS.Marshal;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSRuntime
    {
        // Per-type marshaller cache. Populated by GetMarshaller so a resolved marshaller can be reused.
        ConcurrentDictionary<Type, JSMarshaller> _typeMarshallerCache = new ConcurrentDictionary<Type, JSMarshaller>();
        /// <summary>
        /// Selects the marshaller for <typeparamref name="TType"/>. Marshallers are scanned in REVERSE
        /// registration order so later (more specific) registrations win. A marshaller may hand back a
        /// per-type specialization via <see cref="JSMarshaller.GetMarshaller{T}"/> (e.g. ArrayMarshaller
        /// returns one bound to the concrete element type); that specialization is what gets used and cached.
        /// </summary>
        public JSMarshaller<TType> GetMarshaller<TType>()
        {
            var type = typeof(TType);
            //var selectionType = Nullable.GetUnderlyingType(type) ?? type;
            if (_typeMarshallerCache.TryGetValue(type, out var cachedMarshaller))
            {
                return (JSMarshaller<TType>)cachedMarshaller;
            }
            JSMarshaller<TType>? marshaller = null;
            var length = Marshallers.Count;
            for (var i = length - 1; i >= 0; i--)
            {
                var candidate = Marshallers[i];
                if (!candidate.CanMarshal(type)) continue;
                // GetMarshaller lets a marshaller hand back a per-type specialization (UnionMarshaller
                // returns one bound to the concrete Union<...> arms). Cache and use THAT, not the
                // generic candidate - otherwise the specialization hook does nothing.
                var typeMarshaller = candidate.GetMarshaller<TType>();
                if (typeMarshaller == null) continue;
                marshaller = typeMarshaller;
                _typeMarshallerCache.TryAdd(type, typeMarshaller);
                break;
            }
            if (marshaller == null) throw new Exception($"GetMarshaller failed: {type?.Name}");
            if (Verbose) Console.WriteLine($"<< GetMarshaller: {type?.Name} {marshaller.GetType().Name}");
            return marshaller;
        }
        #region NewArray
        internal SpawnJSObjectReference NewJSArray() => new SpawnJSObjectReference((long)_spawnJSObjectNewArray());

        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshaller<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshaller<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshaller<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshaller<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshaller<T5>().NetToJS(jsArgs!, 4, arg5!);
            GetMarshaller<T6>().NetToJS(jsArgs!, 5, arg6!);
            GetMarshaller<T7>().NetToJS(jsArgs!, 6, arg7!);
            GetMarshaller<T8>().NetToJS(jsArgs!, 7, arg8!);
            GetMarshaller<T9>().NetToJS(jsArgs!, 8, arg9!);
            GetMarshaller<T10>().NetToJS(jsArgs!, 9, arg10!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshaller<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshaller<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshaller<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshaller<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshaller<T5>().NetToJS(jsArgs!, 4, arg5!);
            GetMarshaller<T6>().NetToJS(jsArgs!, 5, arg6!);
            GetMarshaller<T7>().NetToJS(jsArgs!, 6, arg7!);
            GetMarshaller<T8>().NetToJS(jsArgs!, 7, arg8!);
            GetMarshaller<T9>().NetToJS(jsArgs!, 8, arg9!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshaller<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshaller<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshaller<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshaller<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshaller<T5>().NetToJS(jsArgs!, 4, arg5!);
            GetMarshaller<T6>().NetToJS(jsArgs!, 5, arg6!);
            GetMarshaller<T7>().NetToJS(jsArgs!, 6, arg7!);
            GetMarshaller<T8>().NetToJS(jsArgs!, 7, arg8!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshaller<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshaller<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshaller<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshaller<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshaller<T5>().NetToJS(jsArgs!, 4, arg5!);
            GetMarshaller<T6>().NetToJS(jsArgs!, 5, arg6!);
            GetMarshaller<T7>().NetToJS(jsArgs!, 6, arg7!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshaller<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshaller<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshaller<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshaller<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshaller<T5>().NetToJS(jsArgs!, 4, arg5!);
            GetMarshaller<T6>().NetToJS(jsArgs!, 5, arg6!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshaller<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshaller<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshaller<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshaller<T4>().NetToJS(jsArgs!, 3, arg4!);
            GetMarshaller<T5>().NetToJS(jsArgs!, 4, arg5!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshaller<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshaller<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshaller<T3>().NetToJS(jsArgs!, 2, arg3!);
            GetMarshaller<T4>().NetToJS(jsArgs!, 3, arg4!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshaller<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshaller<T2>().NetToJS(jsArgs!, 1, arg2!);
            GetMarshaller<T3>().NetToJS(jsArgs!, 2, arg3!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1, T2>(T1 arg1, T2 arg2)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshaller<T1>().NetToJS(jsArgs!, 0, arg1!);
            GetMarshaller<T2>().NetToJS(jsArgs!, 1, arg2!);
            return jsArgs;
        }
        internal SpawnJSObjectReference NewJSArray<T1>(T1 arg1)
        {
            if (!_callArrays.TryDequeue(out var jsArgs)) jsArgs = NewJSArray();
            GetMarshaller<T1>().NetToJS(jsArgs!, 0, arg1!);
            return jsArgs;
        }
        #endregion

        internal SpawnJSObjectReference NewJSObject() => new SpawnJSObjectReference((long)_spawnJSObjectNewObject());

        /// <summary>
        /// Creates a new JS Array, holds it, and returns the sjsId
        /// </summary>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectNewObject")]
        internal static partial double _spawnJSObjectNewObject();

        /// <summary>
        /// Creates a new JS Array, holds it, and returns the sjsId
        /// </summary>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectNewArray")]
        internal static partial double _spawnJSObjectNewArray();

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial void SpawnJSObjectRelease(double sjsId);

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a bool
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial bool SpawnJSObjectReleaseBoolean(double sjsId);

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a double
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial double SpawnJSObjectReleaseDouble(double sjsId);

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a bool
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial bool? SpawnJSObjectReleaseBooleanNullable(double sjsId);

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a double
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial double? SpawnJSObjectReleaseDoubleNullable(double sjsId);

        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a string
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectRelease")]
        internal static partial string SpawnJSObjectReleaseString(double sjsId);
        /// <summary>
        /// Release a SPawnJSObject reference in Javascript and return the value as a string
        /// </summary>
        /// <param name="sjsId"></param>
        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectReleaseAsJson")]
        internal static partial string SpawnJSObjectReleaseJson(double sjsId);


        /// <summary>
        /// This is the ONLY JSImport/JSExport that is allowed to use JSObject and it is ONLY used to get a reference to this .Net Wasm apps DotNet Instance
        /// </summary>
        /// <param name="dotnetInstance">ONLY allowed JSObject in entire library and will ONLY be called once</param>
        /// <returns></returns>
        [JSImport("globalThis.SpawnJSInterop._registerInstance")]
        internal static partial double _registerInstance(
            JSObject dotnetInstance,
            [JSMarshalAs<JSType.Function>] Action onMethodAdded,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.String>>] Action<double, string> onAsyncResolvedVoid,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.Number, JSType.String>>] Action<double, double, string> onAsyncResolvedDouble,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.Boolean, JSType.String>>] Action<double, bool, string> onAsyncResolvedBool,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.String, JSType.String>>] Action<double, string, string> onAsyncResolvedString,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.Any, JSType.String>>] Action<double, object, string> onAsyncResolvedDoubleNullable,
            [JSMarshalAs<JSType.Function<JSType.Number, JSType.Any, JSType.String>>] Action<double, object, string> onAsyncResolvedBooleanNullable);

        [JSImport("globalThis.SpawnJSInterop.spawnJSObjectHoldExists")]
        internal static partial bool SpawnJSObjectHoldExists(double sjsId);

        [JSImport("globalThis.SpawnJSInterop.refreshMethodMap")]
        internal static partial string[] _refreshMethodMap();

        // propertyTypeInfo: returns "<typeof> <toStringTag>" for a property, or null if absent.
        [JSImport("globalThis.SpawnJSInterop.propertyTypeInfo")]
        internal static partial string _propertyTypeInfo(double sjsId, string value);

        [JSImport("globalThis.SpawnJSInterop.propertyTypeInfo")]
        internal static partial string _propertyTypeInfo(double sjsId, double value);


        // Property keys are ONLY ever string or double. There is deliberately no `object` key overload:
        // an `object`-typed key would force JSType.Any marshalling (the JSObject-flavored path this library
        // exists to avoid), and every real JS key is a string or an array index anyway. Each JSImport is
        // duplicated per key type (string + double) so no boxing or Any marshalling occurs.
        #region propertyGet

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial string _propertyGetWithReplacerString(double sjsId, double methodIndex, string key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double _propertyGetWithReplacerDouble(double sjsId, double methodIndex, string key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool _propertyGetWithReplacerBoolean(double sjsId, double methodIndex, string key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double? _propertyGetWithReplacerDoubleNullable(double sjsId, double methodIndex, string key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool? _propertyGetWithReplacerBooleanNullable(double sjsId, double methodIndex, string key);


        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial string _propertyGetWithReplacerString(double sjsId, double methodIndex, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double _propertyGetWithReplacerDouble(double sjsId, double methodIndex, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool _propertyGetWithReplacerBoolean(double sjsId, double methodIndex, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double? _propertyGetWithReplacerDoubleNullable(double sjsId, double methodIndex, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool? _propertyGetWithReplacerBooleanNullable(double sjsId, double methodIndex, double key);


        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial string _propertyGetWithReplacerString(double sjsId, double methodIndex, string key, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double _propertyGetWithReplacerDouble(double sjsId, double methodIndex, string key, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool _propertyGetWithReplacerBoolean(double sjsId, double methodIndex, string key, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double? _propertyGetWithReplacerDoubleNullable(double sjsId, double methodIndex, string key, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool? _propertyGetWithReplacerBooleanNullable(double sjsId, double methodIndex, string key, double replacerConfig);


        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial string _propertyGetWithReplacerString(double sjsId, double methodIndex, double key, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double _propertyGetWithReplacerDouble(double sjsId, double methodIndex, double key, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool _propertyGetWithReplacerBoolean(double sjsId, double methodIndex, double key, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double? _propertyGetWithReplacerDoubleNullable(double sjsId, double methodIndex, double key, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool? _propertyGetWithReplacerBooleanNullable(double sjsId, double methodIndex, double key, double replacerConfig);



        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial string _propertyGetWithReplacerString(double sjsId, double methodIndex, string key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double _propertyGetWithReplacerDouble(double sjsId, double methodIndex, string key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool _propertyGetWithReplacerBoolean(double sjsId, double methodIndex, string key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double? _propertyGetWithReplacerDoubleNullable(double sjsId, double methodIndex, string key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool? _propertyGetWithReplacerBooleanNullable(double sjsId, double methodIndex, string key, string replacerConfig);


        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial string _propertyGetWithReplacerString(double sjsId, double methodIndex, double key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double _propertyGetWithReplacerDouble(double sjsId, double methodIndex, double key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool _propertyGetWithReplacerBoolean(double sjsId, double methodIndex, double key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double? _propertyGetWithReplacerDoubleNullable(double sjsId, double methodIndex, double key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool? _propertyGetWithReplacerBooleanNullable(double sjsId, double methodIndex, double key, string replacerConfig);



        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial string _propertyGetString(double sjsId, string key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial double _propertyGetDouble(double sjsId, string key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial bool _propertyGetBoolean(double sjsId, string key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial double? _propertyGetDoubleNullable(double sjsId, string key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial bool? _propertyGetBooleanNullable(double sjsId, string key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetSpawnJSObjectReference")]
        internal static partial double? _propertyGetSpawnJSObjectReference(double sjsId, string key, bool force);

        [JSImport("globalThis.SpawnJSInterop.propertyGetJson")]
        internal static partial string? _propertyGetJson(double sjsId, string key);


        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial string _propertyGetString(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial double _propertyGetDouble(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial bool _propertyGetBoolean(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial double? _propertyGetDoubleNullable(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial bool? _propertyGetBooleanNullable(double sjsId, double key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetSpawnJSObjectReference")]
        internal static partial double? _propertyGetSpawnJSObjectReference(double sjsId, double key, bool force);

        [JSImport("globalThis.SpawnJSInterop.propertyGetJson")]
        internal static partial string? _propertyGetJson(double sjsId, double key);
        #endregion


        // propertyIn: JS `key in obj`.
        [JSImport("globalThis.SpawnJSInterop.propertyIn")]
        internal static partial bool _propertyIn(double sjsId, string key);

        [JSImport("globalThis.SpawnJSInterop.propertyIn")]
        internal static partial bool _propertyIn(double sjsId, double key);


        // propertyDelete: JS `delete obj[key]`.
        [JSImport("globalThis.SpawnJSInterop.propertyDelete")]
        internal static partial bool _propertyDelete(double sjsId, string key);

        [JSImport("globalThis.SpawnJSInterop.propertyDelete")]
        internal static partial bool _propertyDelete(double sjsId, double key);


        // propertySetNull: assigns JS `null` to obj[key].
        [JSImport("globalThis.SpawnJSInterop.propertySetNull")]
        internal static partial void _propertySetNull(double sjsId, string key);

        [JSImport("globalThis.SpawnJSInterop.propertySetNull")]
        internal static partial void _propertySetNull(double sjsId, double key);


        // propertySetUndefined: assigns JS `undefined` to obj[key].
        [JSImport("globalThis.SpawnJSInterop.propertySetUndefined")]
        internal static partial void _propertySetUndefined(double sjsId, string key);

        [JSImport("globalThis.SpawnJSInterop.propertySetUndefined")]
        internal static partial void _propertySetUndefined(double sjsId, double key);


        // propertySet: one overload per (key type x value type). Value types are limited to the primitives
        // that cross the boundary without boxing (string/double/bool and their nullables), a held object
        // reference (propertySetSpawnJSObject, passed by id), or JSON (propertySetJson). Anything richer is
        // decomposed by a marshaller into these primitives before it reaches here.
        #region Set
        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, string key, string value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, string key, bool value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, string key, double value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, string key, bool? value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, string key, double? value);

        [JSImport("globalThis.SpawnJSInterop.propertySetSpawnJSObject")]
        internal static partial void _propertySetSpawnJSObject(double sjsId, string key, double value);

        [JSImport("globalThis.SpawnJSInterop.propertySetJson")]
        internal static partial void _propertySetJson(double sjsId, string key, string value);


        [JSImport("globalThis.SpawnJSInterop.propertySetHeapView")]
        internal static partial void _propertySetHeapView(double sjsId, string key, double dotnetId, double viewType, double offset, double length, bool copy);


        [JSImport("globalThis.SpawnJSInterop.propertySetHeapView")]
        internal static partial void _propertySetHeapView(double sjsId, double key, double dotnetId, double viewType, double offset, double length, bool copy);


        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, string key, string value, double reviverIndex);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, string key, double value, double reviverIndex);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, string key, string value, double reviverIndex, string reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, string key, double value, double reviverIndex, string reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, string key, string value, double reviverIndex, double reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, string key, double value, double reviverIndex, double reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, string key, string value, double reviverIndex, bool reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, string key, double value, double reviverIndex, bool reviverConfig);



        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, string value, double reviverIndex);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, double value, double reviverIndex);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, string value, double reviverIndex, string reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, double value, double reviverIndex, string reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, string value, double reviverIndex, double reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, double value, double reviverIndex, double reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, string value, double reviverIndex, bool reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, double key, double value, double reviverIndex, bool reviverConfig);


        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, double key, string value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, double key, bool value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, double key, double value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, double key, bool? value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, double key, double? value);

        [JSImport("globalThis.SpawnJSInterop.propertySetSpawnJSObject")]
        internal static partial void _propertySetSpawnJSObject(double sjsId, double key, double value);

        [JSImport("globalThis.SpawnJSInterop.propertySetJson")]
        internal static partial void _propertySetJson(double sjsId, double key, string value);
        #endregion

        // Synchronous marshalled call. All six overloads bind to the same JS function _spawnJSInteropCall;
        // the C# return type chosen at the call site tells the runtime how to read the JS result, and the
        // returnType index tells JS how to shape it (see ReturnType and the JS _serializeToNet switch).
        #region MarshalledArgsAndReturnType
        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial bool? _spawnJSInteropCallBooleanNullable(double returnType, double methodIndex, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial double? _spawnJSInteropCallDoubleNullable(double returnType, double methodIndex, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial bool _spawnJSInteropCallBoolean(double returnType, double methodIndex, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial double _spawnJSInteropCallDouble(double returnType, double methodIndex, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial string _spawnJSInteropCallString(double returnType, double methodIndex, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCall")]
        internal static partial void _spawnJSInteropCallVoid(double returnType, double methodIndex, double argsId);
        #endregion

        // Asynchronous marshalled call. Fire-and-return: JS awaits the underlying promise, then calls back
        // into a [JSExport] resolver (AsyncCallResolved*) with the asyncCallId to complete the managed
        // TaskCompletionSource. dotnetId identifies this app's DotnetInstance so JS can reach the exports.
        #region MarshalledArgsAndReturnTypeAsync

        //[JSImport("globalThis.SpawnJSInterop._spawnJSInteropCallAsync")]
        //internal static partial void _spawnJSInteropCallAsync(double returnType, double dotnetId, double asyncCallId, string methodName, double argsId);

        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropCallAsync")]
        internal static partial void _spawnJSInteropCallAsync(double returnType, double dotnetId, double asyncCallId, double methodIndex, double argsId);


        // Loads this assembly's [JSExport] table on the JS side so the async resolvers can be invoked.
        [JSImport("globalThis.SpawnJSInterop._spawnJSInteropLoadExportsAsync")]
        internal static partial Task _spawnJSInteropLoadExportsAsync(double dotnetId, string assemblyName);
        #endregion


        [JSImport("globalThis.SpawnJSInterop.getTypeInfo")]
        internal static partial string _getTypeInfo(double sjsId);

        /// <summary>
        /// Call any SpawnJSInterop static method that returns nothing (void).
        /// </summary>
        internal void InteropCallApplyVoid(string methodName, object?[]? args = null) => InteropCallApply<VoidType>(methodName, args);


        /// <summary>
        /// Pool of reusable JS argument arrays so each call does not have to allocate a new one. The JS side
        /// empties the array's slot after consuming it (spawnJSObjectGetAndReplace), so the same held
        /// reference can be handed back out on the next call.
        /// </summary>
        Queue<SpawnJSObjectReference> _callArrays = new Queue<SpawnJSObjectReference>();

        internal T InteropCall<T>(string methodName)
            => _InteropCallApply<T>(methodName);
        internal T InteropCall<T1, T>(string methodName, T1 arg1)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1));
        internal T InteropCall<T1, T2, T>(string methodName, T1 arg1, T2 arg2)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2));
        internal T InteropCall<T1, T2, T3, T>(string methodName, T1 arg1, T2 arg2, T3 arg3)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3));
        internal T InteropCall<T1, T2, T3, T4, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4));
        internal T InteropCall<T1, T2, T3, T4, T5, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5));
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6));
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
        internal T InteropCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => _InteropCallApply<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
        /// <summary>
        /// Calls a SpawnJSInterop static method synchronously, marshalling <paramref name="args"/> into a JS
        /// array and reading the result back as <typeparamref name="T"/>.
        /// </summary>
        internal T InteropCallApply<T>(string methodName, object?[]? args = null)
        {
            var returnType = typeof(T);
            var inMarshaller = GetMarshaller<T>();
            // The JS side empties this array's slot after the call, so it can be returned to the pool.
            SpawnJSObjectReference? jsArgs = null;
            if (args != null && args.Length > 0)
            {
                if (!_callArrays.TryDequeue(out jsArgs))
                {
                    jsArgs = NewJSArray();
                }
                for (var i = 0; i < args.Length; i++)
                {
                    var item = args[i];
                    var itemType = item?.GetType()!;
                    if (itemType == null)
                    {
                        jsArgs.PropertySetNull(i);
                        continue;
                    }
                    // The Type -> <T> trick: each arg's runtime Type is bridged back into a compile-time
                    // generic via InvokeGeneric, so writeTyped<T1> runs the strongly-typed marshaller path
                    // (JSMarshaller<T1>.NetToJS) with NO boxing of the value. GetMarshaller<T1> matches on
                    // T1's exact type, and the value is written straight into the JS array by index.
                    ((Delegate)writeTyped<object>).InvokeGeneric(itemType, item);
                    void writeTyped<T1>(T1 value)
                    {
                        var marshaller = GetMarshaller<T1>();
                        if (marshaller == null) jsArgs.PropertySetNull(i);
                        else marshaller.NetToJS(jsArgs!, i, value!);
                    }
                }
            }
            return _InteropCallApply<T>(methodName, jsArgs);
        }
        internal T _InteropCallApply<T>(string methodName, SpawnJSObjectReference? jsArgs = null)
        {
            var returnType = typeof(T);
            var inMarshaller = GetMarshaller<T>();
            var returnTypeIndex = inMarshaller?.ReturnType ?? ReturnType.Void;
            T ret = default!;
            var argsId = jsArgs?.Id ?? UndefinedId;
            var methodIndex = InteropMethods.IndexOf(methodName);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {InteropMethods.Length} {methodName}");
            try
            {
                switch (returnTypeIndex)
                {
                    case ReturnType.Void:
                        {
                            _spawnJSInteropCallVoid((double)returnTypeIndex, methodIndex, argsId);
                        }
                        break;
                    case ReturnType.Double:
                        {
                            var fromJS = _spawnJSInteropCallDouble((double)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    case ReturnType.Boolean:
                        {
                            var fromJS = _spawnJSInteropCallBoolean((double)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    case ReturnType.DoubleNullable:
                        {
                            var fromJS = _spawnJSInteropCallDoubleNullable((double)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    case ReturnType.BooleanNullable:
                        {
                            var fromJS = _spawnJSInteropCallBooleanNullable((double)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    case ReturnType.String:
                        {
                            var fromJS = _spawnJSInteropCallString((double)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    case ReturnType.SpawnJSObjectReference:
                        {
                            var fromJS = _spawnJSInteropCallDouble((double)returnTypeIndex, methodIndex, argsId);
                            var spawnJSObjectReference = SpawnJSObjectReference.FromID(fromJS, false);
                            ret = inMarshaller!.JSToNet(spawnJSObjectReference!);
                        }
                        break;
                    case ReturnType.SpawnJSObjectReferenceNonNullable:
                        {
                            var fromJS = _spawnJSInteropCallDouble((double)returnTypeIndex, methodIndex, argsId);
                            var spawnJSObjectReference = SpawnJSObjectReference.FromID(fromJS, true);
                            ret = inMarshaller!.JSToNet(spawnJSObjectReference!);
                        }
                        break;
                    case ReturnType.Json:
                        {
                            var fromJS = _spawnJSInteropCallString((double)returnTypeIndex, methodIndex, argsId);
                            ret = inMarshaller!.JSToNet(fromJS);
                        }
                        break;
                    default:
                        throw new Exception($"Invalid ReturnType for marshaller: {inMarshaller?.GetType().Name} {returnTypeIndex}");
                }
            }
            finally
            {
                if (jsArgs != null)
                {
                    // returen the array to the usable call queue. it has already been reset by js
                    _callArrays.Enqueue(jsArgs);
                }
            }
            return ret;
        }
        /// <summary>
        /// Calls a SpawnJSInterop static method asynchronously. A per-call id is registered against a
        /// TaskCompletionSource; the JS side runs the underlying promise then invokes an AsyncCallResolved*
        /// [JSExport] with that id, which completes the task. The assembly export table is loaded once on
        /// the first async call.
        /// </summary>
        internal Task<T> InteropCallAsync<T>(string methodName)
            => _InteropCallApplyAsync<T>(methodName);
        internal Task<T> InteropCallAsync<T1, T>(string methodName, T1 arg1)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1));
        internal Task<T> InteropCallAsync<T1, T2, T>(string methodName, T1 arg1, T2 arg2)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2));
        internal Task<T> InteropCallAsync<T1, T2, T3, T>(string methodName, T1 arg1, T2 arg2, T3 arg3)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
        internal Task<T> InteropCallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>(string methodName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => _InteropCallApplyAsync<T>(methodName, NewJSArray(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
        async Task<T> _InteropCallApplyAsync<T>(string methodName, SpawnJSObjectReference? jsArgs = null)
        {
            var typeOfT = typeof(T);
            var returnMarshaller = GetMarshaller<T>();
            var returnTypeIndex = returnMarshaller.ReturnType;
            var tcs = new TaskCompletionSource<T>();
            var asyncCallbackId = ++_asyncCallbackId;
            switch (returnTypeIndex)
            {
                case ReturnType.Void:
                    {
                        _voidCallbacks.TryAdd(asyncCallbackId, (error) =>
                        {
                            if (error == null) tcs.TrySetResult(default!);
                            else tcs.TrySetException(new Exception(error));
                        });
                    }
                    break;
                case ReturnType.Double:
                    _doubleCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(new Exception(error));
                        else
                        {
                            var ret = returnMarshaller.JSToNet(value!);
                            tcs.TrySetResult(ret);
                        }
                    });
                    break;
                case ReturnType.Boolean:
                    {
                        _booleanCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                        {
                            if (error != null) tcs.TrySetException(new Exception(error));
                            else
                            {
                                var ret = returnMarshaller.JSToNet(value);
                                tcs.TrySetResult(ret);
                            }
                        });
                    }
                    break;
                case ReturnType.DoubleNullable:
                    _doubleNullableCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                    {
                        if (error != null) tcs.TrySetException(new Exception(error));
                        else
                        {
                            var ret = returnMarshaller.JSToNet(value!);
                            tcs.TrySetResult(ret);
                        }
                    });
                    break;
                case ReturnType.BooleanNullable:
                    {
                        _booleanNullableCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                        {
                            if (error != null) tcs.TrySetException(new Exception(error));
                            else
                            {
                                var ret = returnMarshaller.JSToNet(value);
                                tcs.TrySetResult(ret);
                            }
                        });
                    }
                    break;
                case ReturnType.String:
                    {
                        _stringCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                        {
                            if (error != null) tcs.TrySetException(new Exception(error));
                            else
                            {
                                var ret = returnMarshaller.JSToNet(value!);
                                tcs.TrySetResult(ret);
                            }
                        });
                    }
                    break;
                case ReturnType.SpawnJSObjectReference:
                    {
                        _doubleCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                        {
                            if (error != null) tcs.TrySetException(new Exception(error));
                            else
                            {
                                var spawnJSObjectReference = SpawnJSObjectReference.FromID(value, false);
                                var ret = returnMarshaller.JSToNet(spawnJSObjectReference!);
                                tcs.TrySetResult(ret);
                            }
                        });
                    }
                    break;
                case ReturnType.SpawnJSObjectReferenceNonNullable:
                    {
                        _doubleCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                        {
                            if (error != null) tcs.TrySetException(new Exception(error));
                            else
                            {
                                var spawnJSObjectReference = SpawnJSObjectReference.FromID(value, true);
                                var ret = returnMarshaller.JSToNet(spawnJSObjectReference!);
                                tcs.TrySetResult(ret);
                            }
                        });
                    }
                    break;
                case ReturnType.Json:
                    {
                        _stringCallbacks.TryAdd(asyncCallbackId, (value, error) =>
                        {
                            if (error != null) tcs.TrySetException(new Exception(error));
                            else
                            {
                                var ret = returnMarshaller.JSToNet(value!);
                                tcs.TrySetResult(ret);
                            }
                        });
                    }
                    break;
                default:
                    return default!;
            }
            try
            {
                var argsId = jsArgs?.Id ?? UndefinedId;
                var methodIndex = InteropMethods.IndexOf(methodName);
                if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {InteropMethods.Length} {methodName}");
                _spawnJSInteropCallAsync((double)returnMarshaller.ReturnType, DotnetInstance.Id, asyncCallbackId, methodIndex, argsId);
                // wait for the tcs to complete or throw
                return await tcs.Task.ConfigureAwait(false);
            }
            finally
            {
                if (jsArgs != null)
                {
                    // returen the array to the usable call queue. it has already been reset by js
                    _callArrays.Enqueue(jsArgs);
                }
            }
        }
        /// <summary>
        /// Calls a SpawnJSInterop static method asynchronously. A per-call id is registered against a
        /// TaskCompletionSource; the JS side runs the underlying promise then invokes an AsyncCallResolved*
        /// [JSExport] with that id, which completes the task. The assembly export table is loaded once on
        /// the first async call.
        /// </summary>
        internal async Task<T> InteropCallApplyAsync<T>(string methodName, object?[]? args = null)
        {
            // The JS side empties this array's slot after the call, so it can be returned to the pool.
            SpawnJSObjectReference? jsArgs = null;
            if (args != null && args.Length > 0)
            {
                if (!_callArrays.TryDequeue(out jsArgs))
                {
                    jsArgs = NewJSArray();
                }
                for (var i = 0; i < args.Length; i++)
                {
                    var item = args[i];
                    // Type -> <T> trick (see InteropCallApply): marshal each arg with no boxing.
                    var itemType = item?.GetType()!;
                    if (itemType == null)
                    {
                        jsArgs.PropertySetNull(i);
                        continue;
                    }
                    ((Delegate)writeTyped<object>).InvokeGeneric(itemType, item);
                    void writeTyped<T1>(T1 value)
                    {
                        var marshaller = GetMarshaller<T1>();
                        if (marshaller == null) jsArgs.PropertySetNull(i);
                        else marshaller.NetToJS(jsArgs!, i, value!);
                    }
                }
            }
            return await _InteropCallApplyAsync<T>(methodName, jsArgs);
        }
        //// NOTE: Callback (.Net method passed to JS and invoked directly) is not wired up yet. These two
        //// exports are placeholders for that path and currently only log.
        //[JSExport]
        //static void FireCallback(double callbackId, double argsId)
        //{
        //    Console.WriteLine($"FireCallback: {callbackId} {argsId}");
        //}
        //[JSExport]
        //static async Task FireCallbackAsync(double callbackId, double argsId)
        //{
        //    Console.WriteLine($"FireCallbackAsync: {callbackId} {argsId}");
        //    await Task.Delay(5000);
        //}

        // Monotonic id handed to JS with each async call and echoed back to match the completion to its task.
        double _asyncCallbackId = 0;

        // Pending async completions, keyed by asyncCallbackId, one dictionary per JS result shape. The
        // matching resolver [JSExport] below removes and invokes the entry when JS reports the result.
        static ConcurrentDictionary<double, Action<string?>> _voidCallbacks = new ConcurrentDictionary<double, Action<string?>>();
        static ConcurrentDictionary<double, Action<double, string?>> _doubleCallbacks = new ConcurrentDictionary<double, Action<double, string?>>();
        static ConcurrentDictionary<double, Action<double?, string?>> _doubleNullableCallbacks = new ConcurrentDictionary<double, Action<double?, string?>>();
        static ConcurrentDictionary<double, Action<bool, string?>> _booleanCallbacks = new ConcurrentDictionary<double, Action<bool, string?>>();
        static ConcurrentDictionary<double, Action<bool?, string?>> _booleanNullableCallbacks = new ConcurrentDictionary<double, Action<bool?, string?>>();
        static ConcurrentDictionary<double, Action<string?, string?>> _stringCallbacks = new ConcurrentDictionary<double, Action<string?, string?>>();
    }
}
