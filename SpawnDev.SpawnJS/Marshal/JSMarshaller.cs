namespace SpawnDev.SpawnJS.Marshal
{
    /// <summary>
    /// Marshals data between .Net and Javascript using SpawnJSObjectReference
    /// </summary>
    public abstract class JSMarshaller
    {
        public abstract ReturnType ReturnType { get; }
        /// <summary>
        /// SpawnJSRuntime
        /// </summary>
        public SpawnJSRuntime JS => SpawnJSRuntime.Instance ?? throw new InvalidOperationException("SpawnJSRuntime has not been created.");
        /// <summary>
        /// Returns true if the data type can be marshalled.<br/>
        /// <paramref name="typeToConvert"/> may be null when the .Net value being marshalled is null.
        /// </summary>
        public abstract bool CanMarshal(Type typeToConvert);
        /// <summary>
        /// If this class reported true to CanMarshal, GetMarshaller may be called to get the marshaller to do the marshalling<br/>
        /// </summary>
        /// <returns></returns>
        public virtual JSMarshaller<T> GetMarshaller<T>() => (JSMarshaller<T>)this;

        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public abstract object JSToNetBoxed(Type typeToConvert, bool value);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public abstract object JSToNetBoxed(Type typeToConvert, bool? value);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public abstract object JSToNetBoxed(Type typeToConvert, double value);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public abstract object JSToNetBoxed(Type typeToConvert, double? value);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public abstract object JSToNetBoxed(Type typeToConvert, string value);
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public abstract object JSToNetBoxed(Type typeToConvert, SpawnJSObjectReference value);
    }
    /// <summary>
    /// Strongly-typed marshaller for <typeparamref name="TType"/>. This is the layer the no-boxing path
    /// targets: the runtime bridges a value's runtime Type back into <typeparamref name="TType"/> and calls
    /// <see cref="NetToJS(Type, SpawnJSObjectReference, double, TType)"/> / the <c>JSToNet</c> overloads
    /// directly, so the value never has to be boxed as <c>object</c>. The non-generic base's
    /// <c>JSToNetBoxed</c> overloads simply forward to the typed <c>JSToNet</c> for callers that only have a
    /// <see cref="Type"/>. Each subclass overrides only the one <c>JSToNet</c> overload matching its
    /// <see cref="JSMarshaller.ReturnType"/>; the rest throw <see cref="NotImplementedException"/> by design.
    /// </summary>
    public abstract class JSMarshaller<TType> : JSMarshaller
    {
        /// <summary>
        /// Returns true if the data type can be marshalled
        /// </summary>
        public override bool CanMarshal(Type typeToConvert) => typeof(TType) == typeToConvert;
        public virtual TType JSToNet(Type typeToConvert) => throw new NotImplementedException();
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(Type typeToConvert, bool value) => throw new NotImplementedException();
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(Type typeToConvert, bool? value) => throw new NotImplementedException();
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(Type typeToConvert, double value) => throw new NotImplementedException();
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(Type typeToConvert, double? value) => throw new NotImplementedException();
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(Type typeToConvert, string value) => throw new NotImplementedException();
        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public virtual TType JSToNet(Type typeToConvert, SpawnJSObjectReference value) => throw new NotImplementedException();



        /// <summary>
        /// Convert from ReturnType to .Net type<br/>
        /// Returns null when the JS value is null/undefined or the target type's default value is null.
        /// </summary>
        public override object JSToNetBoxed(Type typeToConvert, bool value) => JSToNet(typeToConvert, value)!;
        public override object JSToNetBoxed(Type typeToConvert, bool? value) => JSToNet(typeToConvert, value)!;
        public override object JSToNetBoxed(Type typeToConvert, double value) => JSToNet(typeToConvert, value)!;
        public override object JSToNetBoxed(Type typeToConvert, double? value) => JSToNet(typeToConvert, value)!;
        public override object JSToNetBoxed(Type typeToConvert, string value) => JSToNet(typeToConvert, value)!;
        public override object JSToNetBoxed(Type typeToConvert, SpawnJSObjectReference value) => JSToNet(typeToConvert, value)!;

        /// <summary>
        /// Given a JS parent object, the JS property key, and the .Net value: write the value.<br/>
        /// <paramref name="typeToConvert"/> and <paramref name="value"/> may be null when the .Net value being marshalled is null.
        /// </summary>
        public abstract void NetToJS(Type typeToConvert, SpawnJSObjectReference jsParent, string jsKey, TType value);
        /// <summary>
        /// Given a JS parent object, the JS property key, and the .Net value: write the value.<br/>
        /// <paramref name="typeToConvert"/> and <paramref name="value"/> may be null when the .Net value being marshalled is null.
        /// </summary>
        public abstract void NetToJS(Type typeToConvert, SpawnJSObjectReference jsParent, double jsKey, TType value);
    }
}
