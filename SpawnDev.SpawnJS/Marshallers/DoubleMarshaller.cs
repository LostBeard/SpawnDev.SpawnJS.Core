using SpawnDev.SpawnJS.Marshal;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="double"/> to/from a JS number (no conversion needed).</summary>
    public class DoubleMarshaller : JSMarshallerFromDouble<double>
    {
        public override double JSToNet(double value) => value;
        public override void NetToJS(SpawnJSObjectReference jsParent, double jsKey, double value) => jsParent.PropertySet(jsKey, value);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, double value) => jsParent.PropertySet(jsKey, value);
    }
}
