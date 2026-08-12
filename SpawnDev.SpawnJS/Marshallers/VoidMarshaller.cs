using SpawnDev.SpawnJS.Marshal;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals <see cref="VoidType"/>: writes nothing and reads back null. Used for void calls.</summary>
    public class VoidMarshaller : JSMarshallerFromVoid<VoidType>
    {
        public override VoidType JSToNet(Type type) => null!;
        public override void NetToJS(SpawnJSObjectReference jsParent, double jsKey, VoidType value) { }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, VoidType value) { }
    }
}
