using SpawnDev.SpawnJS.Marshal;
using SpawnDev.SpawnJS.Marshallers;
using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// The entry point for all .Net to Javascript interop in SpawnJS.<br/>
    /// <br/>
    /// Design rule that defines this library: <see cref="JSObject"/> (Microsoft's WASM interop handle) is
    /// NOT used anywhere. Its disposal quirk was the multi-year blocker for the previous interop attempts,
    /// and it leaked into Gemineachy. The ONLY place a <see cref="JSObject"/> is permitted is the single
    /// <see cref="_registerInstance(JSObject)"/> call that hands this app's DotnetInstance to the JS side -
    /// and never again. Everything else references JS values by a numeric id (see
    /// <see cref="SpawnJSObjectReference"/>), so nothing crosses the boundary that needs disposing on the
    /// Microsoft interop table.<br/>
    /// <br/>
    /// The runtime itself is a <see cref="SpawnJSObjectReference"/> whose id is <see cref="GlobalThis"/>,
    /// so calling <c>JS.PropertyGet*/PropertySet</c> operates directly on the JS <c>globalThis</c>.
    /// </summary>
    public partial class SpawnJSRuntime : SpawnJSObjectReference
    {
        /// <summary>
        /// The process-wide singleton. Created on first access if it does not already exist.
        /// </summary>
        public static SpawnJSRuntime Instance => _instance ??= new SpawnJSRuntime();
        private static SpawnJSRuntime? _instance;
        /// <summary>
        /// True once the singleton has been constructed.
        /// </summary>
        public static bool IsCreated => _instance != null;
        /// <summary>
        /// The ordered set of marshallers that convert values between .Net and Javascript. A marshaller is
        /// selected by asking each (last registered wins) whether it can handle a given .Net type - see
        /// <see cref="GetMarshaller{TType}"/>.
        /// </summary>
        public IList<JSMarshaller> Marshallers { get; private set; } = new List<JSMarshaller>();
        /// <summary>
        /// A reference to this .Net WASM app's DotnetInstance held on the JS side. Used to reach the
        /// assembly's [JSExport] methods (e.g. to resolve async calls) - see <see cref="InteropCallApplyAsync{T}"/>.
        /// </summary>
        public SpawnJSObjectReference DotnetInstance { get; private set; }
        /// <summary>
        /// A reference pointing at the JS-side object table (the <see cref="SpawnJSObjects"/> sentinel id).
        /// </summary>
        internal SpawnJSObjectReference spawnJSObjects = new SpawnJSObjectReference(SpawnJSObjectsId);
        /// <summary>
        /// A reference pointing at the JS-side object table (the <see cref="SpawnJSInterop"/> sentinel id).
        /// </summary>
        internal SpawnJSObjectReference spawnJSInterop = new SpawnJSObjectReference(SpawnJSInteropId);
        /// <summary>
        /// When true, marshaller selection is logged to the console. Off by default so libraries stay quiet.
        /// </summary>
        public bool Verbose;
        internal string[] InteropMethods;
        /// <summary>
        /// Creates the runtime. The base id is <see cref="GlobalThis"/> so the instance addresses JS
        /// <c>globalThis</c> directly. Registers the built-in marshallers in priority order (last wins).
        /// </summary>
        private SpawnJSRuntime() : base(GlobalThisId)
        {
            _instance = this;
            //AppJsonContext.Init();
            // Registration order matters: GetMarshaller scans this list in REVERSE, so a marshaller added
            // later takes precedence when more than one reports it can marshal a type. The more specific /
            // higher-priority handlers (arrays, object references) are therefore added last.
            Marshallers.Add(new VoidMarshaller());
            Marshallers.Add(new ObjectMarshaller());
            Marshallers.Add(new StringMarshaller());
            Marshallers.Add(new DoubleMarshaller());
            Marshallers.Add(new DoubleNullableMarshaller());
            Marshallers.Add(new BooleanMarshaller());
            Marshallers.Add(new BooleanNullableMarshaller());
            Marshallers.Add(new SpawnJSObjectReferenceMarshaller());
            Marshallers.Add(new ArrayMarshaller<object>());
            Marshallers.Add(new ListMarshaller<object>());
            Marshallers.Add(new HeapViewDescriptorMarshaller());
            Marshallers.Add(new CallbackMarshaller<Callback>());
            Marshallers.Add(new ByteArrayMarshaller());
            Marshallers.Add(new INumberMarshaller<float>());
            // The one and only permitted JSObject use: hand this app's DotnetInstance to the JS side and
            // immediately reduce it to a numeric SpawnJSObjectReference id. Never touched as a JSObject again.
            DotnetInstance = new SpawnJSObjectReference(
                _registerInstance(JSHost.DotnetInstance,
                _JSToNetMappedMethodsChanged,
                AsyncCallResolvedVoid,
                AsyncCallResolvedDouble,
                AsyncCallResolvedBoolean,
                AsyncCallResolvedString,
                AsyncCallResolvedDoubleNullable,
                AsyncCallResolvedBooleanNullable,
                Callback.HandleCallback));
            // load method names to enable indexed based interop calling (vs string)
            InteropMethods = _refreshMethodMap();
        }
        /// <summary>
        /// Get the current heap size
        /// </summary>
        /// <returns></returns>
        public long GetHeapSize() => (long)spawnJSInterop.Call<double, double>("getHeapSize", DotnetInstance.Id);
        /// <summary>
        /// Force the heap to grow. Useful for debugging heap growth issues.
        /// </summary>
        /// <returns></returns>
        public long GrowHeap()
        {
            var tmp = new List<byte[]>();
            var heapSize = GetHeapSize();
            while (true)
            {
                var heapSizeNow = GetHeapSize();
                var diff = heapSizeNow - heapSize;
                if (diff > 0) return diff;
                var data = new byte[16000000];
                tmp.Add(data);
            }
        }
        private void _JSToNetMappedMethodsChanged()
        {
            Console.WriteLine($"_JSToNetMappedMethodsChanged");
            InteropMethods = _refreshMethodMap();
        }

        /// <summary>
        /// Returns value as type T
        /// </summary>
        /// <typeparam name="T">The type to return value as</typeparam>
        /// <returns>value as type T</returns>
        public T As<T1, T>(T1 value) => InteropCall<T1, T>("returnMe", value);

        // Resolvers invoked by JS (_spawnJSInteropCallAsync) to complete a pending async call. error is
        // non-null when the JS promise rejected.
        void AsyncCallResolvedVoid(double asyncCallId, string? error)
        {
            if (_voidCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask(error);
        }
        void AsyncCallResolvedDouble(double asyncCallId, double value, string? error)
        {
            if (_doubleCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask(value, error);
        }
        void AsyncCallResolvedBoolean(double asyncCallId, bool value, string? error)
        {
            if (_booleanCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask(value, error);
        }
        void AsyncCallResolvedString(double asyncCallId, string? value, string? error)
        {
            if (_stringCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask(value, error);
        }
        void AsyncCallResolvedDoubleNullable(double asyncCallId, object? value, string? error)
        {
            if (_doubleNullableCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask((double?)value, error);
        }
        void AsyncCallResolvedBooleanNullable(double asyncCallId, object? value, string? error)
        {
            if (_booleanNullableCallbacks.TryRemove(asyncCallId, out var waitingTask)) waitingTask((bool?)value, error);
        }
    }
}
