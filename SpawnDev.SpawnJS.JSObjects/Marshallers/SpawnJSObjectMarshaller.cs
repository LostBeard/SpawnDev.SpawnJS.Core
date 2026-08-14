
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.Marshaller;

public class SpawnJSObjectMarshaller<TSpawnJSObject> : JSMarshallerFromSpawnJSObjectReference<TSpawnJSObject?> where TSpawnJSObject : SpawnJSObject
{
    /// <inheritdoc/>
    public override bool CanMarshal(Type type) => typeof(SpawnJSObject).IsAssignableFrom(type);
    /// <summary>
    /// Builds an <see cref="ArrayMarshaller{T}"/> bound to the concrete element type of
    /// <typeparamref name="T"/> (e.g. selecting for <c>int[]</c> yields an <c>ArrayMarshaller&lt;int&gt;</c>).
    /// </summary>
    public override JSMarshaller<T> GetMarshaller<T>()
    {
        if (this is JSMarshaller<T> _this) return _this;
        var marshallerTyped = typeof(SpawnJSObjectMarshaller<>).MakeGenericType(typeof(T));
        return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
    }
    public override TSpawnJSObject? JSToNet(SpawnJSObjectReference? value)
    {
        return value == null ? null : (TSpawnJSObject)Activator.CreateInstance(typeof(TSpawnJSObject), value)!;
    }
    public override void NetToJS(SpawnJSObjectReference jsParent, int jsKey, TSpawnJSObject? value)
    {
        jsParent.Set(jsKey, value?.JSRef);
    }
    public override void NetToJS(SpawnJSObjectReference jsParent, string jsKey, TSpawnJSObject? value)
    {
        jsParent.Set(jsKey, value?.JSRef);
    }
}