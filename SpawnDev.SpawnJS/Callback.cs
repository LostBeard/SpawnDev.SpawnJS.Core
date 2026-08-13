using System.Collections.Concurrent;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// WORK IN PROGRESS - NOT WIRED UP YET. Intended to wrap a .Net method so it can be passed to JS and
    /// invoked directly (the JS -> .Net callback path). The JS-side dispatch (FireCallback / FireCallbackAsync
    /// in SpawnJSRuntime) is currently a logging placeholder. See the commented ActionCallback sketch below
    /// for the intended shape.
    /// </summary>
    public abstract class Callback : IDisposable
    {
        static double _callbackIdNext = 0;
        public double Id { get; private set; }
        public double CalledCount { get; private set; }
        public bool HasBeenCalled => CalledCount > 0;
        static ConcurrentDictionary<double, Callback> _callbacks = new ConcurrentDictionary<double, Callback>();
        /// <summary>
        /// Returns true if the Callback should only fire at most once
        /// </summary>
        public bool Once { get; private set; }
        /// <summary>
        /// Returns true if the Callback was sent to Javascript at least once.<br/>
        /// A Callback that is not sent to Javascript does not have to and will not notify Javascript when it disposes.<br/>
        /// </summary>
        public bool Sent { get; internal set; }
        public Callback(bool once)
        {
            Id = ++_callbackIdNext;
            Once = once;
            _callbacks.TryAdd(Id, this);
        }
        protected abstract void HandleCallback(SpawnJSObjectReference? args, double argsCount);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argsId">The incoming AND outgoing buffer</param>
        internal static void HandleCallback(double callbackId, double argsId, double argsCount)
        {
            if (_callbacks.TryGetValue(callbackId, out var callback))
            {
                // we use preventDispose = true on this SpawnJSObjectReference to save an unnecessary JS call in the dispose...
                // this array will be released after the cann returns anyways so calling release on it again would be a waste
                var args = SpawnJSObjectReference.FromID(argsId, preventDispose: true);
                // increment the CalledCount
                callback.CalledCount++;
                if (callback.Once) callback.Dispose();
                callback.HandleCallback(args, argsCount);
            }
        }
        public bool IsDisposed { get; private set; }
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            _callbacks.TryRemove(Id, out _);
            // notify JS to release the Calback to prevent additional calls.
            // only need to notify JS if the Callabck was actually sent
            // and Javascript has not already released it (it auto-releases Callbacks with Once == true)
            var jsSideReleasedIt = Once && HasBeenCalled;
            if (Sent && !jsSideReleasedIt) SpawnJSRuntime._releaseCallback(Id);
            Id = 0;
        }
    }
}
