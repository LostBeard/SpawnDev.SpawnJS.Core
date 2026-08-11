using System;
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
    public class Callback : IDisposable
    {
        Dictionary<long, Callback> _callbacks = new Dictionary<long, Callback>();
        public Callback()
        {

        }
        public bool IsDisposed { get; private set; }
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;

        }
    }
    ///// <summary>
    ///// A Callback object wraps a .Net method and can be passed to Javascript and called directly.
    ///// </summary>
    //public class ActionCallback : Callback
    //{
    //    /// <summary>
    //    /// Implicitly converts a .Net method into a Callback
    //    /// </summary>
    //    /// <param name="callback">.Net target method</param>
    //    public static implicit operator ActionCallback?(Action? callback) => callback == null ? null : callback.CallbackGet(true);
    //    /// <summary>
    //    /// Creates a new instance
    //    /// </summary>
    //    /// <param name="callback"></param>
    //    /// <param name="once">If true, the Callback will be disposed after the first call</param>
    //    public ActionCallback(Action callback, bool once = false) : base(callback, once) { }
    //    /// <inheritdoc/>
    //    internal override object? InvokeHandler(object?[] args)
    //    {
    //        ((Action)Func)();
    //        return null;
    //    }
    //}
}
