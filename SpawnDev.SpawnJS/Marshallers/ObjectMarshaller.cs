using SpawnDev.SpawnJS.Marshal;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>Marshals a plain <see cref="object"/> (used for the null case): reads back as null, writes JS null.</summary>
    public class ObjectMarshaller : JSMarshallerFromSpawnJSObjectReference<object>
    {
        public override object JSToNet(Type typeToConvert, SpawnJSObjectReference value) => value;
        public override void NetToJS(Type? typeToConvert, SpawnJSObjectReference jsParent, double jsKey, object value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            var type = value.GetType();
            if (type == typeof(object))
            {
                throw new NotImplementedException("TODO");
            }
            else
            {
                ((Delegate)writeTyped<object>).InvokeGeneric(type, value);
                void writeTyped<T>(T value)
                {
                    var marshaller = JS.GetMarshaller<T>();
                    marshaller.NetToJS(type, jsParent, jsKey, value);
                }
            }
        }
        public override void NetToJS(Type? typeToConvert, SpawnJSObjectReference jsParent, string jsKey, object value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            var type = value.GetType();
            if (type == typeof(object))
            {
                throw new NotImplementedException("TODO");
            }
            else
            {
                ((Delegate)writeTyped<object>).InvokeGeneric(type, value);
                void writeTyped<T>(T value)
                {
                    var marshaller = JS.GetMarshaller<T>();
                    marshaller.NetToJS(type, jsParent, jsKey, value);
                }
            }
        }
    }
}
