using SpawnDev.SpawnJS.Marshaller;

namespace SpawnDev.SpawnJS.Marshallers
{
    public class ITupleMarshallerFactory : JSMarshaller
    {
        public override ReturnType ReturnType => throw new NotImplementedException();
        static List<Type> SupportedGenericTypes = new List<Type> {
            { typeof(ValueTuple<>) },
            { typeof(ValueTuple<,>) },
            { typeof(ValueTuple<,,>) },
            { typeof(ValueTuple<,,,>) },
            { typeof(ValueTuple<,,,,>) },
            { typeof(ValueTuple<,,,,,>) },
            { typeof(ValueTuple<,,,,,,>) },
            { typeof(Tuple<>) },
            { typeof(Tuple<,>) },
            { typeof(Tuple<,,>) },
            { typeof(Tuple<,,,>) },
            { typeof(Tuple<,,,,>) },
            { typeof(Tuple<,,,,,>) },
            { typeof(Tuple<,,,,,,>) },
        };

        public override bool CanMarshal(Type type)
        {
            // ValueType is a value type and may be wrapped in Nullable<> if it is nullable(?)
            var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
            if (genericType == typeof(Nullable<>))
            {
                type = type.GenericTypeArguments[0];
                genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
            }
            return genericType != null && SupportedGenericTypes.Contains(genericType);
        }
        public override JSMarshaller<T> GetMarshaller<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        {
            var marshallerTyped = typeof(ITupleMarshaller<>).MakeGenericType(typeof(T));
            return (JSMarshaller<T>)Activator.CreateInstance(marshallerTyped)!;
        }
        ///// <summary>
        ///// Creates a JsonConverter instance
        ///// </summary>
        //public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        //{
        //    var isNullable = false;
        //    var genericType = typeToConvert.IsGenericType ? typeToConvert.GetGenericTypeDefinition() : null;
        //    if (genericType == typeof(Nullable<>))
        //    {
        //        isNullable = true;
        //        typeToConvert = typeToConvert.GenericTypeArguments[0];
        //        genericType = typeToConvert.IsGenericType ? typeToConvert.GetGenericTypeDefinition() : null;
        //    }
        //    return new ITupleMarshaller(typeToConvert, genericType!, isNullable);
        //}
    }
}
