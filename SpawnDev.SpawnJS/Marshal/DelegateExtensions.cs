using System.Collections.Concurrent;
using System.Reflection;

namespace SpawnDev.SpawnJS.Marshal
{
    /// <summary>
    /// The machinery behind the "runtime Type -> compile-time &lt;T&gt;" trick the marshaller pipeline relies
    /// on. Given a generic method group and a runtime <see cref="Type"/>, <see cref="InvokeGeneric(Delegate, Type)"/>
    /// closes the method over that type and invokes it - so a value whose type is only known at runtime can
    /// still be dispatched into a strongly-typed generic method (e.g. <c>writeTyped&lt;T1&gt;</c>) without
    /// boxing. Closed <see cref="MethodInfo"/>s are cached (single- and multi-type keys) so the expensive
    /// <see cref="MethodInfo.MakeGenericMethod"/> runs once per type combination. Async variants await and
    /// unwrap Task/ValueTask/duck-typed awaitables.
    /// </summary>
    public static class DelegateExtensions
    {
        private static readonly ConcurrentDictionary<CacheKeySingle, MethodInfo> _singleCache = new();
        private static readonly ConcurrentDictionary<CacheKey, MethodInfo> _multiCache = new();
        private static readonly ConcurrentDictionary<Type, (bool IsAwaitable, bool HasReturnValue)> _awaitableTypes = new();

        public static async Task<object?> GetResultFromUnknownObjectAsync(object? obj)
        {
            if (!IsAwaitable(obj, out bool hasReturnValue))
            {
                // Not a task/awaitable, just return the raw object as-is
                return obj;
            }

            // It is a Task, ValueTask, or Custom Awaitable. 
            // Awaiting it as a dynamic automatically forces the runtime engine to 
            // resolve the task state and extract the internal unwrapped value.
            dynamic dynamicAwaitable = obj!;

            if (hasReturnValue)
            {
                // The assignment here extracts the true returned T value perfectly, 
                // completely bypassing the need for unsafe .Result reflection lookups!
                object? result = await dynamicAwaitable;
                return result;
            }

            // It's a void-returning awaitable (Task / ValueTask)
            await dynamicAwaitable;
            return null;
        }

        public static bool IsAwaitable(object? obj, out bool hasReturnValue)
        {
            if (obj is null)
            {
                hasReturnValue = false;
                return false;
            }

            Type type = obj.GetType();

            var cachedInfo = _awaitableTypes.GetOrAdd(type, t =>
            {
                // 1. Check for standard void Task
                if (t == typeof(Task)) return (true, false);

                // 2. Check for generic Task<T>
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>)) return (true, true);

                // 3. Check for standard void ValueTask
                if (t == typeof(ValueTask)) return (true, false);

                // 4. Check for generic ValueTask<T>
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ValueTask<>)) return (true, true);

                // 5. Fallback for custom duck-typed awaitables
                MethodInfo? getAwaiterMethod = t.GetMethod("GetAwaiter", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                if (getAwaiterMethod != null)
                {
                    Type awaiterType = getAwaiterMethod.ReturnType;
                    MethodInfo? getResultMethod = awaiterType.GetMethod("GetResult", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                    bool customHasReturn = getResultMethod != null && getResultMethod.ReturnType != typeof(void);
                    return (true, customHasReturn);
                }

                return (false, false);
            });

            hasReturnValue = cachedInfo.HasReturnValue;
            return cachedInfo.IsAwaitable;
        }

        // =======================================================
        // INVOCATION METHODS (UNCHANGED & WORKING PERFECTLY)
        // =======================================================
        public static async Task<object?> InvokeGenericAsync(this Delegate methodGroup, Type targetType)
        {
            var key = new CacheKeySingle(methodGroup.Method, targetType);
            var targetMethod = _singleCache.GetOrAdd(key, k => k._methodDefinition.MakeGenericMethod(k.TargetType));
            var ret = targetMethod.Invoke(methodGroup.Target, null);
            return await GetResultFromUnknownObjectAsync(ret);
        }

        public static async Task<object?> InvokeGenericAsync(this Delegate methodGroup, Type targetType, params object?[]? args)
        {
            var key = new CacheKeySingle(methodGroup.Method, targetType);
            var targetMethod = _singleCache.GetOrAdd(key, k => k._methodDefinition.MakeGenericMethod(k.TargetType));
            var ret = targetMethod.Invoke(methodGroup.Target, args);
            return await GetResultFromUnknownObjectAsync(ret);
        }

        public static Task<object?> InvokeGenericAsync(this Delegate methodGroup, Type[] targetTypes)
        {
            return InvokeGenericAsync(methodGroup, targetTypes, null);
        }

        public static async Task<object?> InvokeGenericAsync(this Delegate methodGroup, Type[] targetTypes, params object?[]? args)
        {
            var key = new CacheKey(methodGroup.Method, targetTypes);
            var targetMethod = _multiCache.GetOrAdd(key, k => k._methodDefinition.MakeGenericMethod(k.TargetTypes));
            var ret = targetMethod.Invoke(methodGroup.Target, args);
            return await GetResultFromUnknownObjectAsync(ret);
        }

        public static object? InvokeGeneric(this Delegate methodGroup, Type targetType)
        {
            var key = new CacheKeySingle(methodGroup.Method, targetType);
            var targetMethod = _singleCache.GetOrAdd(key, k => k._methodDefinition.MakeGenericMethod(k.TargetType));
            return targetMethod.Invoke(methodGroup.Target, null);
        }

        public static object? InvokeGeneric(this Delegate methodGroup, Type targetType, params object?[]? args)
        {
            var key = new CacheKeySingle(methodGroup.Method, targetType);
            var targetMethod = _singleCache.GetOrAdd(key, k => k._methodDefinition.MakeGenericMethod(k.TargetType));
            return targetMethod.Invoke(methodGroup.Target, args);
        }

        public static object? InvokeGeneric(this Delegate methodGroup, Type[] targetTypes)
        {
            return InvokeGeneric(methodGroup, targetTypes, null);
        }

        public static object? InvokeGeneric(this Delegate methodGroup, Type[] targetTypes, params object?[]? args)
        {
            var key = new CacheKey(methodGroup.Method, targetTypes);
            var targetMethod = _multiCache.GetOrAdd(key, k => k._methodDefinition.MakeGenericMethod(k.TargetTypes));
            return targetMethod.Invoke(methodGroup.Target, args);
        }

        // =======================================================
        // CACHE STRUCTS (UNCHANGED & WORKING PERFECTLY)
        // =======================================================
        internal readonly struct CacheKeySingle : IEquatable<CacheKeySingle>
        {
            internal readonly MethodInfo _methodDefinition;
            public readonly Type TargetType;
            private readonly int _hashCode;

            public CacheKeySingle(MethodInfo closedMethod, Type targetType)
            {
                _methodDefinition = closedMethod.IsGenericMethod ? closedMethod.GetGenericMethodDefinition() : closedMethod;
                TargetType = targetType;
                _hashCode = HashCode.Combine(_methodDefinition, TargetType);
            }

            public bool Equals(CacheKeySingle other)
            {
                return ReferenceEquals(_methodDefinition, other._methodDefinition) && TargetType == other.TargetType;
            }

            public override bool Equals(object? obj) => obj is CacheKeySingle other && Equals(other);
            public override int GetHashCode() => _hashCode;
        }

        internal readonly struct CacheKey : IEquatable<CacheKey>
        {
            internal readonly MethodInfo _methodDefinition;
            public readonly Type[] TargetTypes;
            private readonly int _hashCode;

            public CacheKey(MethodInfo closedMethod, Type[] targetTypes)
            {
                _methodDefinition = closedMethod.IsGenericMethod ? closedMethod.GetGenericMethodDefinition() : closedMethod;
                TargetTypes = (Type[])targetTypes.Clone();

                var hash = new HashCode();
                hash.Add(_methodDefinition);
                for (int i = 0; i < TargetTypes.Length; i++)
                {
                    hash.Add(TargetTypes[i]);
                }
                _hashCode = hash.ToHashCode();
            }

            public bool Equals(CacheKey other)
            {
                if (!ReferenceEquals(_methodDefinition, other._methodDefinition)) return false;
                if (TargetTypes.Length != other.TargetTypes.Length) return false;

                for (int i = 0; i < TargetTypes.Length; i++)
                {
                    if (TargetTypes[i] != other.TargetTypes[i]) return false;
                }
                return true;
            }

            public override bool Equals(object? obj) => obj is CacheKey other && Equals(other);
            public override int GetHashCode() => _hashCode;
        }
    }
}
