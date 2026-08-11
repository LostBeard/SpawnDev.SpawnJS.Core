using SpawnDev.SpawnJS.Marshal;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="VoidType"/>: writes nothing and reads back null. Used for void calls.</summary>
    public class VoidMarshaller : JSMarshallerFromVoid<VoidType>
    {
        public override VoidType JSToNet(Type typeToConvert) => null!;
        public override void NetToJS(Type? typeToConvert, SpawnJSObjectReference jsParent, double jsKey, VoidType value) { }
        public override void NetToJS(Type? typeToConvert, SpawnJSObjectReference jsParent, string jsKey, VoidType value) { }
    }
}
