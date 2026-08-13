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
        internal static SpawnJSObjectReference? FromID(double fromJS, bool nonNullable = false, bool preventDispose = false)
        {
            return !nonNullable && (fromJS == SpawnJSObjectReference.NullId || fromJS == SpawnJSObjectReference.UndefinedId) ? null : new SpawnJSObjectReference(fromJS) { PreventDispose = preventDispose };
        }
        /// <summary>
        /// If true, this item will not dispose when dispsoe is called
        /// </summary>
        public bool PreventDispose { get; set; }
        /// <summary>Sentinel id for JS <c>globalThis</c>.</summary>
        public const double GlobalThisId = -1;
        /// <summary>Sentinel id for JS <c>undefined</c> (also the id a handle is set to once released).</summary>
        public const double UndefinedId = -2;
        /// <summary>Sentinel id for JS <c>null</c>.</summary>
        public const double NullId = -3;
        /// <summary>Sentinel id for the JS object table itself.</summary>
        public const double SpawnJSObjectsId = -4;
        /// <summary>Sentinel id for SpawnJSInterop.</summary>
        public const double SpawnJSInteropId = -5;
        /// <summary>True once this handle has been disposed (its JS table entry released).</summary>
        public bool IsDisposed { get; private set; }
        /// <summary>Shortcut to the runtime singleton.</summary>
        static SpawnJSRuntime JS => SpawnJSRuntime.Instance;
        /// <summary>The JS object table id this handle references. Set to <see cref="UndefinedId"/> once released.</summary>
        public double Id { get; private set; }
        /// <summary>True if this handle references JS <c>undefined</c> (or has been released).</summary>
        public bool IsUndefined => Id == UndefinedId;
        /// <summary>True if this handle references JS <c>null</c>.</summary>
        public bool IsNull => Id == NullId;
        /// <summary>True if this handle references JS <c>globalThis</c>.</summary>
        public bool IsGlobalThis => Id == GlobalThisId;
        /// <summary>Wraps an existing JS object table id.</summary>
        public SpawnJSObjectReference(double sjsId)
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
        /// <summary>
        /// Releases the JS object table entry so the underlying value can be garbage-collected on the JS
        /// side. Only positive ids reference a real table entry; the negative sentinels (globalThis, null,
        /// etc.) have nothing to release.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed || PreventDispose) return;
            IsDisposed = true;
            var id = Id;
            Id = UndefinedId;
            if (id > 0)
            {
                SpawnJSRuntime.SpawnJSObjectRelease(id);
            }
        }
        public double MoveId()
        {
            var id = Id;
            Id = UndefinedId;
            return id;
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
