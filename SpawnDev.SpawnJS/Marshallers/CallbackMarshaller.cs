using SpawnDev.SpawnJS.Marshal;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class CallbackMarshaller : JSMarshallerFromString<Callback?>
    {
        public override Callback? JSToNet(string value)
        {
            throw new NotImplementedException();
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, double jsKey, Callback? value)
        {
            
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, Callback? value)
        {
            
        }
    }
}
