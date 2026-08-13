namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<TResult> : Callback
    {
        Func<TResult> _callback;
        public FuncCallback(Func<TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            var ret = args == null ? _callback() : _callback();
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<T1, TResult> : Callback
    {
        Func<T1, TResult> _callback;
        public FuncCallback(Func<T1, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            var ret = args == null ? _callback(default!) : _callback(argsCount <= 0 ? default! : args.Get<T1>(0));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<T1, T2, TResult> : Callback
    {
        Func<T1, T2, TResult> _callback;
        public FuncCallback(Func<T1, T2, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            var ret = args == null ? _callback(default!, default!)
            : _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<T1, T2, T3, TResult> : Callback
    {
        Func<T1, T2, T3, TResult> _callback;
        public FuncCallback(Func<T1, T2, T3, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            var ret = args == null ? _callback(default!, default!, default!)
            : _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<T1, T2, T3, T4, TResult> : Callback
    {
        Func<T1, T2, T3, T4, TResult> _callback;
        public FuncCallback(Func<T1, T2, T3, T4, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            var ret = args == null ? _callback(default!, default!, default!, default!)
            : _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<T1, T2, T3, T4, T5, TResult> : Callback
    {
        Func<T1, T2, T3, T4, T5, TResult> _callback;
        public FuncCallback(Func<T1, T2, T3, T4, T5, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            var ret = args == null ? _callback(default!, default!, default!, default!, default!)
            : _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<T1, T2, T3, T4, T5, T6, TResult> : Callback
    {
        Func<T1, T2, T3, T4, T5, T6, TResult> _callback;
        public FuncCallback(Func<T1, T2, T3, T4, T5, T6, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            var ret = args == null ? _callback(default!, default!, default!, default!, default!, default!)
            : _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<T1, T2, T3, T4, T5, T6, T7, TResult> : Callback
    {
        Func<T1, T2, T3, T4, T5, T6, T7, TResult> _callback;
        public FuncCallback(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            var ret = args == null ? _callback(default!, default!, default!, default!, default!, default!, default!)
            : _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : Callback
    {
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> _callback;
        public FuncCallback(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            var ret = args == null ? _callback(default!, default!, default!, default!, default!, default!, default!, default!)
            : _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6),
                argsCount <= 7 ? default! : args.Get<T8>(7));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : Callback
    {
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> _callback;
        public FuncCallback(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            var ret = args == null ? _callback(default!, default!, default!, default!, default!, default!, default!, default!, default!)
            : _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6),
                argsCount <= 7 ? default! : args.Get<T8>(7),
                argsCount <= 8 ? default! : args.Get<T9>(8));
            args?.Set(argsCount, ret);
        }
    }
    /// <summary>
    /// An Func Callback
    /// </summary>
    public class FuncCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : Callback
    {
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> _callback;
        public FuncCallback(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, bool once = false) : base(once)
        {
            _callback = func;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            var ret = args == null ? _callback(default!, default!, default!, default!, default!, default!, default!, default!, default!, default!)
            : _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6),
                argsCount <= 7 ? default! : args.Get<T8>(7),
                argsCount <= 8 ? default! : args.Get<T9>(8),
                argsCount <= 9 ? default! : args.Get<T10>(9));
            args?.Set(argsCount, ret);
        }
    }
}
