using SpawnDev.SpawnJS.Marshal;
using System.Text.Json;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class HeapViewDescriptorMarshaller : JSMarshallerFromString<HeapViewDescriptor?>
    {
        public override HeapViewDescriptor? JSToNet(string value)
        {
            return string.IsNullOrEmpty(value) ? null : AppJsonContext.Deserialize<HeapViewDescriptor>(value);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, double jsKey, HeapViewDescriptor? value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            jsParent.PropertySetHeapView(jsKey, value.Type!, value.Offset, value.Length);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, HeapViewDescriptor? value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            jsParent.PropertySetHeapView(jsKey, value.Type!, value.Offset, value.Length);
        }
    }
}
