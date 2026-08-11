using SpawnDev.SpawnJS.Marshal;
using System.Collections;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals a .Net List to/from a JS array. Registered as <c>ListMarshaller&lt;object&gt;</c>, but
    /// when selected it re-specializes to the concrete element type (see <see cref="GetMarshaller{T}"/>) so
    /// each element goes through its own strongly-typed marshaller with no boxing.
    /// </summary>
    public class IListMarshaller<TElement> : JSMarshallerFromSpawnJSObjectReference<IList<TElement>?>
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type)
        {
            var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
            var ret = genericType == typeof(List<>);
            return ret;
        }
        /// <summary>
        /// Builds an <see cref="ArrayMarshaller{T}"/> bound to the concrete element type of
        /// <typeparamref name="T"/> (e.g. selecting for <c>int[]</c> yields an <c>ArrayMarshaller&lt;int&gt;</c>).
        /// </summary>
        public override JSMarshaller<T> GetMarshaller<T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var type = typeof(T);
            var elementType = type.GetGenericArguments()[0];
            Type openType = typeof(IListMarshaller<>);
            Type typedMarshaller = openType.MakeGenericType(elementType!);
            return (JSMarshaller<T>)Activator.CreateInstance(typedMarshaller)!;
        }
        /// <inheritdoc/>
        public override IList<TElement>? JSToNet(Type type, SpawnJSObjectReference value1)
        {
            if (value1 == null) return null;
            var elementType = type.GetElementType()!;
            // Read the JS array length, then pull each element back through the typed Get<TElement> path.
            var length = (int)value1.PropertyGetDouble("length");
            var retArray = new List<TElement>();
            for (var i = 0; i < length; i++)
            {
                retArray.Add(value1.Get<TElement>(i));
            }
            return retArray;
        }
        /// <inheritdoc/>
        public override void NetToJS(Type type, SpawnJSObjectReference jsParent, double jsKey, IList<TElement>? objects)
        {
            // Build a fresh JS array, write each element into it, then assign it to the parent property.
            if (objects == null) { jsParent.PropertySetNull(jsKey); return; }
            using var outArray = JS.NewJSArray();
            for (var i = 0; i < objects.Count; i++) outArray.Set(i, objects[i]);
            jsParent.PropertySet(jsKey, outArray);
        }
        /// <inheritdoc/>
        public override void NetToJS(Type type, SpawnJSObjectReference jsParent, string jsKey, IList<TElement>? objects)
        {
            // Build a fresh JS array, write each element into it, then assign it to the parent property.
            if (objects == null) { jsParent.PropertySetNull(jsKey); return; }
            using var outArray = JS.NewJSArray();
            for (var i = 0; i < objects.Count; i++) outArray.Set(i, objects[i]);
            jsParent.PropertySet(jsKey, outArray);
        }
    }
}
