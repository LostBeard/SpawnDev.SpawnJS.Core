using SpawnDev.SpawnJS.Marshaller;
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
            // VoidType - nothing is marshalled
            Marshallers.Add(new VoidTypeMarshaller());
            // .Net: object <-> JS: Object
            Marshallers.Add(new ObjectMarshaller());
            // .Net: string <-> JS: string
            Marshallers.Add(new StringMarshaller());
            // .Net: double <-> JS: Number
            Marshallers.Add(new DoubleMarshaller());
            // .Net: double? <-> JS: Number?
            Marshallers.Add(new DoubleNullableMarshaller());
            // .Net: bool <-> JS: bool
            Marshallers.Add(new BooleanMarshaller());
            // .Net: bool? <-> JS: bool?
            Marshallers.Add(new BooleanNullableMarshaller());
            // .Net: SpawnJSObjectReference <-> JS: Any
            Marshallers.Add(new SpawnJSObjectReferenceMarshaller());
            // .Net: T[] <-> JS: Array<>
            Marshallers.Add(new ArrayMarshaller<object>());
            // .Net: List<> <-> JS: Array<>
            Marshallers.Add(new ListMarshaller<object>());
            // .Net: HeapViewDescriptor -> JS: ArrayBufferView (TypedArray, DataView; copy or persistent)
            Marshallers.Add(new HeapViewDescriptorMarshaller());
            // .Net: Action, Action<>, Func<> -> JS: Function
            Marshallers.Add(new CallbackMarshaller<Callback>());
            // .Net: byte[] <-> JS: Uint8Array
            Marshallers.Add(new ByteArrayMarshaller());
            // .Net: INumber<> <-> JS: Number
            Marshallers.Add(new INumberMarshaller<float>());
            // .Net: Task, Task<> <-> JS: Promise, Promise<T>
            Marshallers.Add(new TaskMarshaller());
            // .Net: BingInteger <-> JS: BigInt
            Marshallers.Add(new BigIntegerMarshaller());
            // The one and only permitted JSObject use: hand this app's DotnetInstance to the JS side and
            // immediately reduce it to a numeric SpawnJSObjectReference id. Never touched as a JSObject again.
            DotnetInstance = new SpawnJSObjectReference(
                _registerInstance(JSHost.DotnetInstance,
                MappedMethodsChanged,
                AsyncCallResolvedVoid,
                ResolveDouble,
                ResolveBoolean,
                ResolveString,
                ResolveDoubleNullable,
                ResolveBooleanNullable,
                ResolveInt32,
                ResolveInt32Nullable,
                Callback.HandleCallback));
            // load method names to enable indexed based interop calling (vs string)
            InteropMethods = _refreshMethodMap();
        }
        /// <summary>
        /// Get the current heap size
        /// </summary>
        /// <returns></returns>
        public long GetHeapSize() => InteropCall<double, long>("getHeapSize", DotnetInstance.Id);
        /// <summary>
        /// Force the heap to grow. Useful for debugging heap growth issues.
        /// </summary>
        /// <returns></returns>
        public long GrowHeap()
        {
            var tmp = new List<byte[]>();
            var heapSize = GetHeapSize();
            long diff = 0;
            while (true)
            {
                try
                {
                    var heapSizeNow = GetHeapSize();
                    diff = heapSizeNow - heapSize;
                    if (diff > 0) break;
                    var data = new byte[16000000];
                    tmp.Add(data);
                }
                catch
                {
                    break;
                }
            }
            tmp.Clear();
            GC.Collect(2, GCCollectionMode.Forced, blocking: true, compacting: true);
            return diff;
        }
        /// <summary>
        /// Returns value as type T
        /// </summary>
        /// <typeparam name="T">The type to return value as</typeparam>
        /// <returns>value as type T</returns>
        public T As<T1, T>(T1 value) => InteropCall<T1, T>("returnMe", value);
        public object? As(Type type, object? value)
        {
            return ((Delegate)As<object>).InvokeGeneric(type, value);
        }
        /// <summary>
        /// Compares two values using Javascript equality.<br/>
        /// full == true uses strict equality (===), otherwise loose equality (==)
        /// </summary>
        public bool ObjectEquals<T1, T2>(T1 obj1, T2 obj2, bool full = false) => InteropCall<T1, T2, bool, bool>("objectEquals", obj1, obj2, full);
    }
}
