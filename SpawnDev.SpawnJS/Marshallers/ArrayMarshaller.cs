using SpawnDev.SpawnJS.Marshal;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Marshals a .Net array to/from a JS array. Registered as <c>ArrayMarshaller&lt;object&gt;</c>, but
    /// when selected it re-specializes to the concrete element type (see <see cref="GetMarshaller{T}"/>) so
    /// each element goes through its own strongly-typed marshaller with no boxing.
    /// </summary>
    public class ArrayMarshaller<TElement> : JSMarshallerFromSpawnJSObjectReference<TElement[]?>
    {
        /// <inheritdoc/>
        public override bool CanMarshal(Type type)
        {
            if (type == null) return false;
            var ret = type.IsArray && type.HasElementType;
            return ret;
        }
        /// <summary>
        /// Builds an <see cref="ArrayMarshaller{T}"/> bound to the concrete element type of
        /// <typeparamref name="T"/> (e.g. selecting for <c>int[]</c> yields an <c>ArrayMarshaller&lt;int&gt;</c>).
        /// </summary>
        public override JSMarshaller<T> GetMarshaller<T>()
        {
            if (this is JSMarshaller<T> _this) return _this;
            var typeOfT = typeof(T);
            var elementType = typeOfT.GetElementType();
            Type openType = typeof(ArrayMarshaller<>);
            Type tyepdArrayMarshaller = openType.MakeGenericType(elementType!);
            return (JSMarshaller<T>)Activator.CreateInstance(tyepdArrayMarshaller)!;
        }
        /// <inheritdoc/>
        public override TElement[]? JSToNet(SpawnJSObjectReference value1)
        {
            if (value1 == null) return null;
            var elementType = RegisteredType.GetElementType()!;
            // Read the JS array length, then pull each element back through the typed Get<TElement> path.
            var length = (int)value1.PropertyGetDouble("length");
            var retArray = new TElement[length];
            for (var i = 0; i < length; i++)
            {
                retArray[i] = value1.Get<TElement>(i);
            }
            return retArray;
        }
        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TElement[]? objects)
        {
            // Build a fresh JS array, write each element into it, then assign it to the parent property.
            if (objects == null) { jsParent.PropertySetNull(jsKey); return; }
            using var outArray = JS.NewJSArray();
            for (var i = 0; i < objects.Length; i++) outArray.Set(i, objects[i]);
            jsParent.PropertySet(jsKey, outArray);
        }
        /// <inheritdoc/>
        public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TElement[]? objects)
        {
            // Build a fresh JS array, write each element into it, then assign it to the parent property.
            if (objects == null) { jsParent.PropertySetNull(jsKey); return; }
            using var outArray = JS.NewJSArray();
            for (var i = 0; i < objects.Length; i++) outArray.Set(i, objects[i]);
            jsParent.PropertySet(jsKey, outArray);
        }
    }
}
