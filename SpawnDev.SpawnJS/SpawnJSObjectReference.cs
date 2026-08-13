using SpawnDev.SpawnJS.Marshal;
using System.Text.Json;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// A handle to a Javascript value, addressed by a numeric id rather than a Microsoft <c>JSObject</c>.
    /// The JS side keeps the actual value in its object table (see SpawnJSInterop.spawnJSObjects) keyed by
    /// this <see cref="Id"/>; disposing this handle releases that entry so JS can garbage-collect it.<br/>
    /// <br/>
    /// Negative ids are reserved sentinels (globalThis / undefined / null / the object table itself) that
    /// have no table entry and are never released. Positive ids are real held values. This id-only model is
    /// what lets the whole library avoid <c>JSObject</c> and its disposal quirk.
    /// </summary>
    public partial class SpawnJSObjectReference : IDisposable
    {
        /// <summary>Sentinel id for JS <c>globalThis</c>.</summary>
        public const long GlobalThis = -1;
        /// <summary>Sentinel id for JS <c>undefined</c> (also the id a handle is set to once released).</summary>
        public const long UndefinedId = -2;
        /// <summary>Sentinel id for JS <c>null</c>.</summary>
        public const long Null = -3;
        /// <summary>Sentinel id for the JS object table itself.</summary>
        public const long SpawnJSObjects = -4;
        /// <summary>Sentinel id for SpawnJSInterop.</summary>
        public const long SpawnJSInterop = -5;
        /// <summary>True once this handle has been disposed (its JS table entry released).</summary>
        public bool IsDisposed { get; private set; }
        /// <summary>Shortcut to the runtime singleton.</summary>
        static SpawnJSRuntime JS => SpawnJSRuntime.Instance;
        /// <summary>The JS object table id this handle references. Set to <see cref="UndefinedId"/> once released.</summary>
        public double Id { get; private set; }
        /// <summary>True if this handle references JS <c>undefined</c> (or has been released).</summary>
        public bool IsUndefined => Id == UndefinedId;
        /// <summary>True if this handle references JS <c>null</c>.</summary>
        public bool IsNull => Id == Null;
        /// <summary>True if this handle references JS <c>globalThis</c>.</summary>
        public bool IsGlobalThis => Id == GlobalThis;
        /// <summary>Wraps an existing JS object table id.</summary>
        public SpawnJSObjectReference(long sjsId)
        {
            Id = sjsId;
        }

        /// <summary>
        /// Returns the referenced Javascript value as type T
        /// </summary>
        /// <typeparam name="T">The type to return the referenced Javascript value as</typeparam>
        /// <returns>The referenced Javascript value as type T</returns>
        public T As<T>() => JS.As<SpawnJSObjectReference, T>(this);

        #region ReleaseAs
        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public double? ReleaseAsDoubleNullable()
        {
            var ret = SpawnJSRuntime.SpawnJSObjectReleaseDoubleNullable(Id);
            Id = UndefinedId;
            Dispose();
            return ret;
        }

        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public bool? ReleaseAsBooleanNullable()
        {
            var ret = SpawnJSRuntime.SpawnJSObjectReleaseBooleanNullable(Id);
            Id = UndefinedId;
            Dispose();
            return ret;
        }

        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public double ReleaseAsDouble()
        {
            var ret = SpawnJSRuntime.SpawnJSObjectReleaseDouble(Id);
            Id = UndefinedId;
            Dispose();
            return ret;
        }

        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public bool ReleaseAsBoolean()
        {
            var ret = SpawnJSRuntime.SpawnJSObjectReleaseBoolean(Id);
            Id = UndefinedId;
            Dispose();
            return ret;
        }

        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public string ReleaseAsString()
        {
            var ret = SpawnJSRuntime.SpawnJSObjectReleaseString(Id);
            Id = UndefinedId;
            Dispose();
            return ret;
        }

        /// <summary>
        /// releases the SpawnJSObject reference and returns underlying value
        /// </summary>
        public T ReleaseAsJson<T>(JsonSerializerOptions? serializerOptions = null)
        {
            var json = SpawnJSRuntime.SpawnJSObjectReleaseJson(Id);
            var ret = json == null ? default : JsonSerializer.Deserialize<T>(json, serializerOptions);
            Id = UndefinedId;
            Dispose();
            return ret!;
        }

        #endregion
        // Get/Set/CallApply are the marshalled convenience surface: they accept arbitrary .Net values and
        // route through the runtime's marshaller pipeline (InteropCallApply). For primitive keys/values the
        // typed, no-marshalling fast path in SpawnJSObjectReference.Property.cs (PropertyGet*/PropertySet)
        // is cheaper - these general overloads exist for when the value type isn't known at the call site.
        public void Set<T>(object key, T value) => JS.InteropCall<double, object, T, VoidType>("propertySet", Id, key, value);
        public void Set<T>(double key, T value) => JS.InteropCall<double, double, T, VoidType>("propertySet", Id, key, value);
        public void Set<T>(string key, T value) => JS.InteropCall<double, string, T, VoidType>("propertySet", Id, key, value);

        public T Get<T>(object key) => JS.InteropCall<double, object, T>("propertyGet", Id, key);
        public T Get<T>(double key) => JS.InteropCall<double, double, T>("propertyGet", Id, key);
        public T Get<T>(string key) => JS.InteropCall<double, string, T>("propertyGet", Id, key);

        public SpawnJSObjectReference? Get(object key) => JS.InteropCall<double, object, SpawnJSObjectReference>("propertyGet", Id, key);
        public SpawnJSObjectReference? Get(double key) => JS.InteropCall<double, double, SpawnJSObjectReference>("propertyGet", Id, key);
        public SpawnJSObjectReference? Get(string key) => JS.InteropCall<double, string, SpawnJSObjectReference>("propertyGet", Id, key);

        public Task<T> GetAsync<T>(object key) => JS.InteropCallAsync<double, object, T>("propertyGet", Id, key);
        public Task<T> GetAsync<T>(double key) => JS.InteropCallAsync<double, double, T>("propertyGet", Id, key);
        public Task<T> GetAsync<T>(string key) => JS.InteropCallAsync<double, string, T>("propertyGet", Id, key);

        public Task<SpawnJSObjectReference> GetAsync(object key) => JS.InteropCallAsync<double, object, SpawnJSObjectReference>("propertyGet", Id, key);
        public Task<SpawnJSObjectReference> GetAsync(double key) => JS.InteropCallAsync<double, double, SpawnJSObjectReference>("propertyGet", Id, key);
        public Task<SpawnJSObjectReference> GetAsync(string key) => JS.InteropCallAsync<double, string, SpawnJSObjectReference>("propertyGet", Id, key);

        public T CallApply<T>(object key, object?[]? args = null) => JS.InteropCall<double, object, object?[]?, T>("propertyCallApply", Id, key, args);
        public T CallApply<T>(double key, object?[]? args = null) => JS.InteropCall<double, double, object?[]?, T>("propertyCallApply", Id, key, args);
        public T CallApply<T>(string key, object?[]? args = null) => JS.InteropCall<double, string, object?[]?, T>("propertyCallApply", Id, key, args);


        public T Call<T>(object key) 
            => JS.InteropCall<double, object, T>("propertyCall", Id, key);
        public T Call<T1, T>(object key, T1 arg1) 
            => JS.InteropCall<double, object, T1, T>("propertyCall", Id, key, arg1);
        public T Call<T1, T2, T>(object key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, object, T1, T2, T>("propertyCall", Id, key, arg1, arg2);
        public T Call<T1, T2, T3, T>(object key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, object, T1, T2, T3, T>("propertyCall", Id, key, arg1, arg2, arg3);
        public T Call<T1, T2, T3, T4, T>(object key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4);
        public T Call<T1, T2, T3, T4, T5, T>(object key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5);
        public T Call<T1, T2, T3, T4, T5, T6, T>(object key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T>(object key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T8, T>(object key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) 
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

        public T Call<T>(double key) 
            => JS.InteropCall<double, object, T>("propertyCall", Id, key);
        public T Call<T1, T>(double key, T1 arg1)
            => JS.InteropCall<double, object, T1, T>("propertyCall", Id, key, arg1);
        public T Call<T1, T2, T>(double key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, object, T1, T2, T>("propertyCall", Id, key, arg1, arg2);
        public T Call<T1, T2, T3, T>(double key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, object, T1, T2, T3, T>("propertyCall", Id, key, arg1, arg2, arg3);
        public T Call<T1, T2, T3, T4, T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4);
        public T Call<T1, T2, T3, T4, T5, T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5);
        public T Call<T1, T2, T3, T4, T5, T6, T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T8, T>(double key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

        public T Call<T>(string key) 
            => JS.InteropCall<double, object, T>("propertyCall", Id, key);
        public T Call<T1, T>(string key, T1 arg1)
            => JS.InteropCall<double, object, T1, T>("propertyCall", Id, key, arg1);
        public T Call<T1, T2, T>(string key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, object, T1, T2, T>("propertyCall", Id, key, arg1, arg2);
        public T Call<T1, T2, T3, T>(string key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, object, T1, T2, T3, T>("propertyCall", Id, key, arg1, arg2, arg3);
        public T Call<T1, T2, T3, T4, T>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4);
        public T Call<T1, T2, T3, T4, T5, T>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5);
        public T Call<T1, T2, T3, T4, T5, T6, T>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T8, T>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

        public Task<T> CallApplyAsync<T>(object key, object?[]? args = null) => JS.InteropCallAsync<double, object, object?[]?, T>("propertyCallApply", Id, key, args);
        public Task<T> CallApplyAsync<T>(double key, object?[]? args = null) => JS.InteropCallAsync<double, double, object?[]?, T>("propertyCallApply", Id, key, args);
        public Task<T> CallApplyAsync<T>(string key, object?[]? args = null) => JS.InteropCallAsync<double, string, object?[]?, T>("propertyCallApply", Id, key, args);

        public void CallApplyVoid(object key, object?[]? args = null) => JS.InteropCall<double, object, object?[]?, VoidType>("propertyCallApply", Id, key, args);
        public void CallApplyVoid(double key, object?[]? args = null) => JS.InteropCall<double, double, object?[]?, VoidType>("propertyCallApply", Id, key, args);
        public void CallApplyVoid(string key, object?[]? args = null) => JS.InteropCall<double, string, object?[]?, VoidType>("propertyCallApply", Id, key, args);

        public Task CallApplyVoidAsync(object key, object?[]? args = null) => JS.InteropCallAsync<double, object, object?[]?, VoidType>("propertyCallApply", Id, key, args);
        public Task CallApplyVoidAsync(double key, object?[]? args = null) => JS.InteropCallAsync<double, double, object?[]?, VoidType>("propertyCallApply", Id, key, args);
        public Task CallApplyVoidAsync(string key, object?[]? args = null) => JS.InteropCallAsync<double, string, object?[]?, VoidType>("propertyCallApply", Id, key, args);

        public T NewApply<T>(object key, object?[]? args = null) => JS.InteropCall<double, object, object?[]?, T>("propertyNewApply", Id, key, args);
        public T NewApply<T>(double key, object?[]? args = null) => JS.InteropCall<double, double, object?[]?, T>("propertyNewApply", Id, key, args);
        public T NewApply<T>(string key, object?[]? args = null) => JS.InteropCall<double, string, object?[]?, T>("propertyNewApply", Id, key, args);

        public SpawnJSObjectReference NewApply(object key, object?[]? args = null) => JS.InteropCall<double, object, object?[]?, SpawnJSObjectReference>("propertyNewApply", Id, key, args);
        public SpawnJSObjectReference NewApply(double key, object?[]? args = null) => JS.InteropCall<double, double, object?[]?, SpawnJSObjectReference>("propertyNewApply", Id, key, args);
        public SpawnJSObjectReference NewApply(string key, object?[]? args = null) => JS.InteropCall<double, string, object?[]?, SpawnJSObjectReference>("propertyNewApply", Id, key, args);

        /// <summary>
        /// Releases the JS object table entry so the underlying value can be garbage-collected on the JS
        /// side. Only positive ids reference a real table entry; the negative sentinels (globalThis, null,
        /// etc.) have nothing to release.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            IsDisposed = true;
            var id = Id;
            Id = UndefinedId;
            if (id > 0)
            {
                SpawnJSRuntime.SpawnJSObjectRelease(id);
            }
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsDisposed) return;
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~SpawnJSObjectReference()
        {
            Dispose(false);
        }
    }
}
