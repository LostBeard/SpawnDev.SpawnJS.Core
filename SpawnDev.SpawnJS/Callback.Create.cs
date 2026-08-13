namespace SpawnDev.SpawnJS
{
    public partial class Callback
    {
        #region FuncCallbackCreate
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<TResult> Create<TResult>(Func<TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, TResult> Create<T1, TResult>(Func<T1, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, TResult> Create<T1, T2, TResult>(Func<T1, T2, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, TResult> Create<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, TResult> Create<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, TResult> Create<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, T5, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, TResult> Create<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, T5, T6, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult> Create<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(callback, false), callbackGroup);
#endregion
        #region FuncCallbackCreateOne
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<TResult> CreateOne<TResult>(Func<TResult> callback)
            => new FuncCallback<TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, TResult> CreateOne<T1, TResult>(Func<T1, TResult> callback)
            => new FuncCallback<T1, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, TResult> CreateOne<T1, T2, TResult>(Func<T1, T2, TResult> callback)
            => new FuncCallback<T1, T2, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, TResult> CreateOne<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> callback)
            => new FuncCallback<T1, T2, T3, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, TResult> CreateOne<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, TResult> CreateOne<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, T5, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, TResult> CreateOne<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, T5, T6, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult> CreateOne<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, TResult> CreateOne<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> CreateOne<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> CreateOne<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> callback)
            => new FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(callback, true);
        #endregion
        #region ActionCallbackCreate
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback Create(Action callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1> Create<T1>(Action<T1> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2> Create<T1, T2>(Action<T1, T2> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3> Create<T1, T2, T3>(Action<T1, T2, T3> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4> Create<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4, T5>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4, T5, T6>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4, T5, T6, T7>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8> Create<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(callback, false), callbackGroup);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback, CallbackGroup? callbackGroup = null)
            => CallbackGroup.TryAdd(new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(callback, false), callbackGroup);
        #endregion
        #region ActionCallbackCreateOne
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback CreateOne(Action callback)
            => new ActionCallback(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1> CreateOne<T1>(Action<T1> callback)
            => new ActionCallback<T1>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2> CreateOne<T1, T2>(Action<T1, T2> callback)
            => new ActionCallback<T1, T2>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3> CreateOne<T1, T2, T3>(Action<T1, T2, T3> callback)
            => new ActionCallback<T1, T2, T3>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4> CreateOne<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
            => new ActionCallback<T1, T2, T3, T4>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5> CreateOne<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> callback)
            => new ActionCallback<T1, T2, T3, T4, T5>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6> CreateOne<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> callback)
            => new ActionCallback<T1, T2, T3, T4, T5, T6>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7> CreateOne<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> callback)
            => new ActionCallback<T1, T2, T3, T4, T5, T6, T7>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8> CreateOne<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
            => new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateOne<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)
            => new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(callback, true);
        /// <summary>
        /// Creates a new Callback instance from the given .Net method that will be disposed after the first call
        /// </summary>
        public static ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateOne<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)
            => new ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(callback, true);
        #endregion
    }
}
