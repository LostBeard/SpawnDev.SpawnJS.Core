using SpawnDev.SpawnJS.Marshal;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="double"/> to/from a JS number (no conversion needed).</summary>
    public class DoubleMarshaller : JSMarshallerFromDouble<double>
    {
        public override double JSToNet(double value) => value;
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, double value) => jsParent.PropertySet(jsKey, value);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, double value) => jsParent.PropertySet(jsKey, value);
    }
    public class Int32Marshaller : JSMarshallerFromInt32<int>
    {
        public override int JSToNet(int value) => value;
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, int value) => jsParent.PropertySet(jsKey, value);
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, int value) => jsParent.PropertySet(jsKey, value);
    }
}
