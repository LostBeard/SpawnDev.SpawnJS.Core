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


    //public class INumberMarshaller<TNumber> : JSMarshallerFromDouble<TNumber>
    //{
    //    public override bool CanMarshal(Type type)
    //    {
    //        var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
    //        var ret = genericType == typeof(List<>);
    //        return ret;
    //    }
    //    public override TNumber JSToNet(double value) => value;
    //    public override void NetToJS(SpawnJSObjectReference jsParent, double jsKey, TNumber value) => jsParent.PropertySet(jsKey, value);
    //    public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TNumber value) => jsParent.PropertySet(jsKey, value);
    //}
}
