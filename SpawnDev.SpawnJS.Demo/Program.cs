// Manual browser smoke test for SpawnJS. Exercises the core paths end to end: creating the runtime,
// getting/setting globalThis properties (string + double), the Type -> <T> InvokeGeneric trick, a sync
// readback, an async readback, and an async DOM method call (document.write). The `var nmt = true;` /
// `var nmt1 = true;` lines are intentional debugger breakpoint anchors - leave them in place.
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.Marshallers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


try
{
    var JS = SpawnJSRuntime.Instance;
    JS.Verbose = true;

    using var document = JS.PropertyGetSpawnJSObjectReference("document");
    document.CallApplyVoid("write", new object?[] { $@"Starting...<br/>" });
    await Task.Delay(1);


    byte[] data = new byte[] { 10, 20, 30, 40 };
    unsafe
    {
        fixed (byte* ptr = data)
        {
            IntPtr address = (IntPtr)ptr;
            var heapViewDescriptor = new HeapViewDescriptor(address, data.Length, false);
            // HeapViewDescriptor gets marshalled to JS as a Uint8Array (can be any ArrayBufferView)
            // It is pointed at this instances .Net heap ArrayBufffer
            JS.Set("_fromHeapViewDescriptor", heapViewDescriptor);
            // It can be converted into a Uint8Array using `SpawnJSRuntime.As`
            // ArrayBufferViews created from a HeapViewDescriptor are special;
            // They are tagged with `_heapViewInfo` which allow it to get rebuilt automatically by the HeapView reviver `__reviverHeapView`
            // This allows easier use of heap base ArrayBufferViews for performant .Net to JS data transfers
            using var uint8ArrayHeapView = JS.As<HeapViewDescriptor, SpawnJSObjectReference>(heapViewDescriptor);
            // settings the heap view Uint8Array to _fromUint8ArrayView
            // after we call GrowHeap _fromUint8ArrayView.buffer.detached will == true
            // but uint8ArrayHeapView can continue to be used because the HeapView reviver
            // will replace the detached view with a new view automatically when the view is used in a call
            JS.Set("_fromUint8ArrayViewBeforeGrow", uint8ArrayHeapView);
            // force the heap to grow. (a testing feature)
            var growth = JS.GrowHeap();
            // write how much the heap grew to console
            Console.WriteLine("growth", growth);
            // _fromUint8ArrayViewAfterGrow.buffer.detached == false
            JS.Set("_fromUint8ArrayViewAfterGrow", uint8ArrayHeapView);
            // check what is detached
            var oldDetached = JS.Get<bool>("_fromUint8ArrayViewBeforeGrow.buffer.detached");
            // oldDetached == true
            var newDetached = JS.Get<bool>("_fromUint8ArrayViewAfterGrow.buffer.detached");
            // oldDetached == false
            // uint8ArrayHeapView can continue to be used for calls even after the heap has resized
            var nmt = true;
        }
        

    }

    {

        var gg2 = new List<string> { "Hello", "world!42" };
        JS.Set("_test", gg2);
        var rbI = JS.Get<List<string>>("_test");
        var nmt = true;
    }
    {
        var gg2 = new string[] { "Hello", "world!42" };
        JS.Set("_test", gg2);
        var rbI = JS.Get<string[]>("_test");
        var nmt = true;
    }

    //{
    //    var sw = Stopwatch.StartNew();
    //    var array = JS.NewApply("Array");
    //    JS.Set("_marray", array);
    //    var cnt = 20000;
    //    var callsPerIteration = 1;
    //    using var window2 = JS.Get("window");
    //    for (var i = 0; i < cnt; i++)
    //    {
    //        array.Set(i, window2);
    //    }
    //    var callCountTotal = callsPerIteration * cnt;
    //    var costPerCall = sw.Elapsed.TotalMicroseconds / (cnt * callsPerIteration); // 2 calls per iteration, teh window get and the array index set
    //    var elapsed = sw.Elapsed.TotalMicroseconds;
    //    document.CallApplyVoid("write", [$"SpawnJS Total .Net to JS.Set calls: {callCountTotal} Cost per call: {costPerCall} microseconds - Total elapsed: {elapsed} microseconds<br/>"]);
    //    // 2026-08-12 SpawnJS.Core  Total .Net to JS calls: 20000 Cost per call:  0.984 microseconds - Total elapsed:   19799 microseconds
    //    // 2026-08-11 SpawnJS.Core  Total .Net to JS calls: 20000 Cost per call:  1.515 microseconds - Total elapsed:   30400 microseconds
    //    // 2026-08-11 SpawnJS (old) Total .Net to JS calls: 20000 Cost per call: 16.519 microseconds - Total elapsed:  330600 microseconds
    //    // 2026-08-11 BlazorJS      Total .Net to JS calls: 20000 Cost per call: 91.285 microseconds - Total elapsed: 1825700 microseconds
    //}

    //{
    //    var sw = Stopwatch.StartNew();
    //    var array = JS.NewApply("Array");
    //    JS.Set("_marray", array);
    //    double number = 42;
    //    JS.Set("_number", number);
    //    var cnt = 20000;
    //    var callsPerIteration = 1;
    //    for (var i = 0; i < cnt; i++)
    //    {
    //        var num = await JS.GetAsync<double>("_number");
    //        if (num != number)
    //        {
    //            throw new Exception();
    //        }
    //    }
    //    var elapsed = sw.Elapsed.TotalMicroseconds;
    //    var callCountTotal = callsPerIteration * cnt;
    //    var costPerCall = elapsed / (cnt * callsPerIteration); // 2 calls per iteration, teh window get and the array index set
    //    document.CallApplyVoid("write", [$"SpawnJS Total .Net to JS.GetAsync calls: {callCountTotal} Cost per call: {costPerCall} microseconds - Total elapsed: {elapsed} microseconds<br/>"]);
    //    // 2026-08-11 SpawnJS.Core  Total .Net to JS calls: 20000 Cost per call:  1.515 microseconds - Total elapsed:   30400 microseconds
    //    // 2026-08-11 SpawnJS (old) Total .Net to JS calls: 20000 Cost per call: 16.519 microseconds - Total elapsed:  330600 microseconds
    //    // 2026-08-11 BlazorJS      Total .Net to JS calls: 20000 Cost per call: 91.285 microseconds - Total elapsed: 1825700 microseconds
    //}
    //{
    //    var called = 0;
    //    var cnt = 1000000;
    //    var callsPerIteration = 1;
    //    var sw = Stopwatch.StartNew();
    //    for (var i = 0; i < cnt; i++)
    //    {
    //        await ((Delegate)MyAction<object, object>).InvokeGenericAsync([typeof(double), typeof(double)]);
    //        //await MyAction<double>();
    //        async ValueTask MyAction<T, T1>()
    //        {
    //            called++;

    //        }
    //    }
    //    var elapsed = sw.Elapsed.TotalMicroseconds;
    //    var callCountTotal = callsPerIteration * cnt;
    //    var costPerCall = elapsed / (cnt * callsPerIteration); // 2 calls per iteration, teh window get and the array index set
    //    document.CallApplyVoid("write", [$"InvokeGenericAsync calls ValueTask<T>: {callCountTotal} Cost per call: {costPerCall} microseconds - Total elapsed: {elapsed} microseconds Called: {called}<br/>"]);
    //}
    // New: InvokeGenericAsync calls ValueTask<T>: 1000000 Cost per call: 2.462 microseconds - Total elapsed: 2462100 microseconds Called: 1000000
    // New: InvokeGenericAsync calls ValueTask<T>: 1000000 Cost per call: 2.414 microseconds - Total elapsed: 2414099 microseconds Called: 1000000
    // New: InvokeGenericAsync calls ValueTask<T>: 1000000 Cost per call: 3.986 microseconds - Total elapsed: 3986200 microseconds Called: 1000000
    // New: InvokeGenericAsync calls ValueTask<T>: 1000000 Cost per call: 3.919 microseconds - Total elapsed: 3919000 microseconds Called: 1000000
    // Old: InvokeGenericAsync calls ValueTask<T>: 1000000 Cost per call: 4.362 microseconds - Total elapsed: 4418700 microseconds Called: 1000000


    //((Delegate)MyAction<object>).InvokeGeneric(typeof(string));
    //void MyAction<T>()
    //{
    //    var type = typeof(T);
    //    Console.WriteLine(type.Name);
    //}

    //((Delegate)MyAction2<object>).InvokeGeneric(typeof(string), "Hello!");
    //void MyAction2<T>(T value)
    //{
    //    var type = typeof(T);
    //    Console.WriteLine(type.Name);
    //}


    //{

    //    JS.PropertySet("_test", "a");
    //    var rbI = JS.PropertyGetString("_test");
    //    var nmt = true;
    //}
    //{
    //    JS.PropertySet("_test", 5);
    //    var rbI = JS.PropertyGetDouble("_test");
    //    var nmt = true;
    //    var rbI2 = JS.Get<double>("_test");
    //    var rbI4 = await JS.GetAsync<double>("_test");
    //    if (rbI4 != 5) throw new Exception("Aync readback failed");
    //}

    ////var nmt1 = true;
    document.CallApplyVoid("write", new object?[] { "Hello world!<br/>" });
    await document.CallApplyVoidAsync("write", new object?[] { "Hello world again!<br/>" });
    Console.WriteLine("Test Success !");
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.ToString()}");
}
Console.WriteLine("Test Done");