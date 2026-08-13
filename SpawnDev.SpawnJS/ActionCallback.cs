namespace SpawnDev.SpawnJS
{
    public class ActionCallback : Callback
    {
        Action _callback;
        public ActionCallback(Action action, bool once = false) : base(action, once)
        {
            _callback = action;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            if (Once) Dispose();
            _callback();
        }
    }
    public class ActionCallback<T1> : Callback
    {
        Action<T1> _callback;
        public ActionCallback(Action<T1> action, bool once = false) : base(action, once)
        {
            _callback = action;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            if (Once) Dispose();
            _callback(args == null || argsCount <= 0 ? default! : args.Get<T1>(0));
        }
    }
    public class ActionCallback<T1, T2> : Callback
    {
        Action<T1, T2> _callback;
        public ActionCallback(Action<T1, T2> action, bool once = false) : base(action, once)
        {
            _callback = action;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            if (Once) Dispose();
            if (args == null) _callback(default!, default!);
            else _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1));
        }
    }
    public class ActionCallback<T1, T2, T3> : Callback
    {
        Action<T1, T2, T3> _callback;
        public ActionCallback(Action<T1, T2, T3> action, bool once = false) : base(action, once)
        {
            _callback = action;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            if (Once) Dispose();
            if (args == null) _callback(default!, default!, default!);
            else _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2));
        }
    }
    public class ActionCallback<T1, T2, T3, T4> : Callback
    {
        Action<T1, T2, T3, T4> _callback;
        public ActionCallback(Action<T1, T2, T3, T4> action, bool once = false) : base(action, once)
        {
            _callback = action;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            if (Once) Dispose();
            if (args == null) _callback(default!, default!, default!, default!);
            else _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3));
        }
    }
    public class ActionCallback<T1, T2, T3, T4, T5> : Callback
    {
        Action<T1, T2, T3, T4, T5> _callback;
        public ActionCallback(Action<T1, T2, T3, T4, T5> action, bool once = false) : base(action, once)
        {
            _callback = action;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            if (Once) Dispose();
            if (args == null) _callback(default!, default!, default!, default!, default!);
            else _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4));
        }
    }
    public class ActionCallback<T1, T2, T3, T4, T5, T6> : Callback
    {
        Action<T1, T2, T3, T4, T5, T6> _callback;
        public ActionCallback(Action<T1, T2, T3, T4, T5, T6> action, bool once = false) : base(action, once)
        {
            _callback = action;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            if (Once) Dispose();
            if (args == null) _callback(default!, default!, default!, default!, default!, default!);
            else _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5));
        }
    }
    public class ActionCallback<T1, T2, T3, T4, T5, T6, T7> : Callback
    {
        Action<T1, T2, T3, T4, T5, T6, T7> _callback;
        public ActionCallback(Action<T1, T2, T3, T4, T5, T6, T7> action, bool once = false) : base(action, once)
        {
            _callback = action;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            if (Once) Dispose();
            if (args == null) _callback(default!, default!, default!, default!, default!, default!, default!);
            else _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6));
        }
    }
    public class ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8> : Callback
    {
        Action<T1, T2, T3, T4, T5, T6, T7, T8> _callback;
        public ActionCallback(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, bool once = false) : base(action, once)
        {
            _callback = action;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            if (Once) Dispose();
            if (args == null) _callback(default!, default!, default!, default!, default!, default!, default!, default!);
            else _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6),
                argsCount <= 7 ? default! : args.Get<T8>(7));
        }
    }
    public class ActionCallback<T1, T2, T3, T4, T5, T6, T7, T8, T9> : Callback
    {
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> _callback;
        public ActionCallback(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, bool once = false) : base(action, once)
        {
            _callback = action;
        }
        /// <summary>
        // args is the incoming data AND where the outgoing result will be written (at index 0)
        // args does not have to be disposed. it is auto removed from the hold after the call ends
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer. Auto-released after the call</param>
        protected override void HandleCallback(SpawnJSObjectReference? args, double argsCount)
        {
            if (Once) Dispose();
            if (args == null) _callback(default!, default!, default!, default!, default!, default!, default!, default!, default!);
            else _callback(
                argsCount <= 0 ? default! : args.Get<T1>(0),
                argsCount <= 1 ? default! : args.Get<T2>(1),
                argsCount <= 2 ? default! : args.Get<T3>(2),
                argsCount <= 3 ? default! : args.Get<T4>(3),
                argsCount <= 4 ? default! : args.Get<T5>(4),
                argsCount <= 5 ? default! : args.Get<T6>(5),
                argsCount <= 6 ? default! : args.Get<T7>(6),
                argsCount <= 7 ? default! : args.Get<T8>(7),
                argsCount <= 8 ? default! : args.Get<T9>(8));
        }
    }
}
