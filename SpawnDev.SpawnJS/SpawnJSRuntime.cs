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
    /// <see cref="spawnJSObjectHold(JSObject)"/> call that hands this app's DotnetInstance to the JS side -
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
        private SpawnJSObjectReference spawnJSObjects = new SpawnJSObjectReference(SpawnJSObjects);
        /// <summary>
        /// When true, marshaller selection is logged to the console. Off by default so libraries stay quiet.
        /// </summary>
        public bool Verbose;

        /// <summary>
        /// Creates the runtime. The base id is <see cref="GlobalThis"/> so the instance addresses JS
        /// <c>globalThis</c> directly. Registers the built-in marshallers in priority order (last wins).
        /// </summary>
        private SpawnJSRuntime() : base(GlobalThis)
        {
            _instance = this;
            // The one and only permitted JSObject use: hand this app's DotnetInstance to the JS side and
            // immediately reduce it to a numeric SpawnJSObjectReference id. Never touched as a JSObject again.
            DotnetInstance = new SpawnJSObjectReference((long)spawnJSObjectHold(JSHost.DotnetInstance));
            // Registration order matters: GetMarshaller scans this list in REVERSE, so a marshaller added
            // later takes precedence when more than one reports it can marshal a type. The more specific /
            // higher-priority handlers (arrays, object references) are therefore added last.
            Marshallers.Add(new VoidMarshaller());
            Marshallers.Add(new NullMarshaller());
            Marshallers.Add(new StringMarshaller());
            Marshallers.Add(new DoubleMarshaller());
            Marshallers.Add(new DoubleNullableMarshaller());
            Marshallers.Add(new BooleanMarshaller());
            Marshallers.Add(new BooleanNullableMarshaller());
            Marshallers.Add(new SpawnJSObjectReferenceMarshaller());
            Marshallers.Add(new ArrayMarshaller<object>());
            Marshallers.Add(new ListMarshaller<object>());
        }
    }
}
