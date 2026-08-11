using SpawnDev.SpawnJS.Marshal;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="bool"/> to/from a JS boolean (no conversion needed).</summary>
    public class BooleanMarshaller : JSMarshallerFromBoolean<bool>
    {
        public override bool JSToNet(Type typeToConvert, bool value) => value;
        public override void NetToJS(Type? typeToConvert, SpawnJSObjectReference jsParent, double jsKey, bool value) => jsParent.PropertySet(jsKey, value);
        public override void NetToJS(Type? typeToConvert, SpawnJSObjectReference jsParent, string jsKey, bool value) => jsParent.PropertySet(jsKey, value);
    }
}
