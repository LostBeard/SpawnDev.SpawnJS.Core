using SpawnDev.SpawnJS.Marshal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

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
        public double Id;
        static ConcurrentDictionary<double, Callback> _callbacks = new ConcurrentDictionary<double, Callback>();
        public bool Once { get; private set; }
        public Callback(Delegate target, bool once = false)
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
                callback.HandleCallback(args, argsCount);
            }
        }
        public bool IsDisposed { get; private set; }
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            _callbacks.TryRemove(Id, out _);
        }
    }
}
