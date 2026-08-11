using SpawnDev.SpawnJS.Marshal;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="Nullable{Boolean}"/> to/from a JS boolean or null/undefined.</summary>
    public class BooleanNullableMarshaller : JSMarshallerFromBooleanNullable<bool?>
    {
        public override bool? JSToNet(Type typeToConvert, bool? value) => value;
        public override void NetToJS(Type? typeToConvert, SpawnJSObjectReference jsParent, double jsKey, bool? value) => jsParent.PropertySet(jsKey, value);
        public override void NetToJS(Type? typeToConvert, SpawnJSObjectReference jsParent, string jsKey, bool? value) => jsParent.PropertySet(jsKey, value);
    }
}
