using SpawnDev.SpawnJS.Marshal;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class HeapViewDescriptorMarshaller : JSMarshallerFromString<HeapViewDescriptor>
    {
        public override HeapViewDescriptor JSToNet(string value) => throw new NotImplementedException();
        public override void NetToJS(SpawnJSObjectReference jsParent, double jsKey, HeapViewDescriptor value)
        {
            var json = $"{{ \"dotnetId\": {JS.DotnetInstance.Id}, \"offset\": {value.Offset}, \"length\": {value.Length}, \"type\": \"{value.Type}\" }}";
            jsParent.PropertySetWithReviver("__reviverHeapView", jsKey, json);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, HeapViewDescriptor value)
        {
            var json = $"{{ \"dotnetId\": {JS.DotnetInstance.Id}, \"offset\": {value.Offset}, \"length\": {value.Length}, \"type\": \"{value.Type}\" }}";
            jsParent.PropertySetWithReviver("__reviverHeapView", jsKey, json);
        }
    }
}
