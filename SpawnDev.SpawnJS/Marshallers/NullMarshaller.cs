using SpawnDev.SpawnJS.Marshal;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals a plain <see cref="object"/> (used for the null case): reads back as null, writes JS null.</summary>
    public class NullMarshaller : JSMarshallerFromVoid<object>
    {
        public override object JSToNet(Type typeToConvert) => null!;
        public override void NetToJS(Type? typeToConvert, SpawnJSObjectReference jsParent, double jsKey, object value) => jsParent.PropertySetNull(jsKey);
        public override void NetToJS(Type? typeToConvert, SpawnJSObjectReference jsParent, string jsKey, object value) => jsParent.PropertySetNull(jsKey);
    }
}
