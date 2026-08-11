// Manual browser smoke test for SpawnJS. Exercises the core paths end to end: creating the runtime,
// getting/setting globalThis properties (string + double), the Type -> <T> InvokeGeneric trick, a sync
// readback, an async readback, and an async DOM method call (document.write). The `var nmt = true;` /
// `var nmt1 = true;` lines are intentional debugger breakpoint anchors - leave them in place.
using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.Marshal;
using System;
using System.Diagnostics;

try
{
    var JS = SpawnJSRuntime.Instance;
    JS.Verbose = true;

    {
        var sw = Stopwatch.StartNew();
        var array = JS.NewApply("Array");
        JS.Set("_marray", array);
        var cnt = 10000;
        var callsPerIteration = 2;
        for (var i = 0; i < cnt; i++)
        {
            using var window2 = JS.PropertyGetSpawnJSObjectReference("window");
            array.PropertySet(i, window2);
        }
        var callCountTotal = callsPerIteration * cnt;
        var costPerCall = sw.Elapsed.TotalMicroseconds / (cnt * callsPerIteration); // 2 calls per iteration, teh window get and the array index set
        var elapsed = sw.Elapsed.TotalMicroseconds;
        Console.WriteLine($"SpawnJS Total .Net to JS calls: {callCountTotal} Cost per call: {costPerCall} microseconds - Total elapsed: {elapsed} microseconds");
        // 2026-08-11 SpawnJS  Total .Net to JS calls: 20000 Cost per call:  3.135 microseconds - Total elapsed:   62700 microseconds
        // 2026-08-11 BlazorJS Total .Net to JS calls: 20000 Cost per call: 91.285 microseconds - Total elapsed: 1825700 microseconds
    }

    ((Delegate)MyAction<object>).InvokeGeneric(typeof(string));
    void MyAction<T>()
    {
        var type = typeof(T);
        Console.WriteLine(type.Name);
    }

    ((Delegate)MyAction2<object>).InvokeGeneric(typeof(string), "Hello!");
    void MyAction2<T>(T value)
    {
        var type = typeof(T);
        Console.WriteLine(type.Name);
    }


    {

        JS.PropertySet("_test", "a");
        var rbI = JS.PropertyGetString("_test");
        var nmt = true;
    }
    {
        JS.PropertySet("_test", 5);
        var rbI = JS.PropertyGetDouble("_test");
        var nmt = true;
        var rbI2 = JS.Get<double>("_test");
        var rbI4 = await JS.GetAsync<double>("_test");
        if (rbI4 != 5) throw new Exception("Aync readback failed");
    }

    var nmt1 = true;
    using var document = JS.PropertyGetSpawnJSObjectReference("document");
    //document.CallApplyVoid("write", new object?[] { "Hello world!" });
    await document.CallApplyVoidAsync("write", new object?[] { "Hello world!" });
    Console.WriteLine("Test Success !");
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.ToString()}");
}
Console.WriteLine("Test Done");