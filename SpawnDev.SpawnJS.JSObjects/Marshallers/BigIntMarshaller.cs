//using SpawnDev.SpawnJS.Marshal;

//namespace SpawnDev.SpawnJS.JSObjects.Marshallers
//{
//    /// <summary>Marshals <see cref="string"/> to/from a JS string (no conversion needed).</summary>
//    public class BigIntMarshaller : JSMarshallerFromString<string>
//    {
//        public override string JSToNet(string value) => value;
//        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, string value) => jsParent.PropertySet(jsKey, value);
//        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, string value) => jsParent.PropertySet(jsKey, value);
//    }
//}
