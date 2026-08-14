using SpawnDev.SpawnJS.Marshaller;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class ITupleMarshaller<TTuple> : JSMarshallerFromSpawnJSObjectReference<TTuple> where TTuple : ITuple
    {
        Type TypeT;
        Type[] GenericTypes;
        public ITupleMarshaller()
        {
            TypeT = typeof(TTuple); ;
            GenericTypes = TypeT.GenericTypeArguments;
        }
        public override TTuple JSToNet(SpawnJSObjectReference value)
        {
            if (value == null) return default!;
            var list = new object?[GenericTypes.Length];
            for (var i = 0; i < GenericTypes.Length; i++)
            {
                list[i] = ((Delegate)readTyped<object>).InvokeGeneric(GenericTypes[i]);
                T readTyped<T>() => value.Get<T>(i);
            }
            var ret = (TTuple)Activator.CreateInstance(TypeT, list)!;
            return ret;
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TTuple value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            using var array = JS.New("Array");
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                array.Set(i, item);
            }
            jsParent.PropertySet(jsKey, array);
        }
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TTuple value)
        {
            if (value == null)
            {
                jsParent.PropertySetNull(jsKey);
                return;
            }
            using var array = JS.New("Array");
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                array.Set(i, item);
            }
            jsParent.PropertySet(jsKey, array);
        }
    }
}
