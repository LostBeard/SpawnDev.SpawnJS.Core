using SpawnDev.SpawnJS;
using SpawnDev.SpawnJS.JSObjects;
using System;
using System.Threading.Tasks;

var JS = SpawnJSRuntime.Instance;
JS.Marshallers.Add(new SpawnJSObjectMarshaller<SpawnJSObject>());
JS.Verbose = true;

using var document = JS.Get("document");
document.CallApplyVoid("write", new object?[] { $@"Starting...<br/>" });
await Task.Delay(1);

var width = 500;
var height = 500;
using var fpsDisplay = JS.Call<string, SpawnJSObjectReference>("document.createElement", "div");
document.CallVoid("body.append", fpsDisplay);
using var canvas = JS.Call<string, SpawnJSObjectReference>("document.createElement", "canvas");
document.CallVoid("body.append", canvas);
canvas.Set("width", width);
canvas.Set("height", height);
using var ctx = canvas.Call<string, SpawnJSObjectReference>("getContext", "2d");
using var performance = JS.Get("performance");

var maxIter = 80; // Balanced for real-time frame rates
var lastTime = performance.Call<double>("now");
double frameCount = 0;
double fps = 0;
double scale = 1.0;

Callback? cb = null;
// image data
var data = new byte[width * height * 4];
// direct heap view of image data
using var heapView = HeapView.Create(data);
// heap view as Uint8ArrayClamped
using var uint8ArrayClamped = heapView.As<Uint8ClampedArray>();
// get an ImageData view of our data to give to the canvas 2d context
using var imgData = JS.New<Uint8ClampedArray, int, int, SpawnJSObjectReference>("ImageData", uint8ArrayClamped, width, height);
// create the animationFrame callback
cb = Callback.Create((double currentTime) =>
{
    var frameStart = performance.Call<double>("now");
    // Smoothly scale the view boundary over time
    scale = 1.0 + Math.Sin(currentTime * 0.0005) * 0.4;

    var p = 0;
    for (var y = 0; y < height; y++)
    {
        // Map y to complex imaginary plane, scaled dynamically
        var ci = ((y / height) * 2.5 - 1.25) * scale - 0.1;

        for (var x = 0; x < width; x++)
        {
            // Map x to complex real plane, scaled dynamically
            double cr = ((x / width) * 3.5 - 2.5) * scale - 0.7;

            double zr = 0; double zi = 0; double n = 0;
            // Escape time algorithm loop
            while (n < maxIter && (zr * zr + zi * zi) <= 4)
            {
                double temp = zr * zr - zi * zi + cr;
                zi = 2 * zr * zi + ci;
                zr = temp;
                n++;
            }

            // Color mapping based on escape iteration
            double r = 0, g = 0, b = 0;
            if (n < maxIter)
            {
                // Dynamic color shifting using the current time
                double wave = Math.Sin(n * 0.1 + currentTime * 0.002);
                r = Math.Floor((wave + 1) * 127.5);
                g = Math.Floor((Math.Cos(n * 0.05) + 1) * 127.5);
                b = Math.Floor((n / maxIter) * 255);
            }

            data[p] = (byte)r;
            data[p + 1] = (byte)g;
            data[p + 2] = (byte)b;
            data[p + 3] = 255;
            p += 4;
        }
    }
    ctx.CallVoid("putImageData", imgData, 0, 0);

    // Performance Tracking
    double frameEnd = performance.Call<double>("now");
    double duration = frameEnd - frameStart;
    frameCount++;

    if (currentTime > lastTime + 1000)
    {
        fps = Math.Round((frameCount * 1000) / (currentTime - lastTime));
        frameCount = 0;
        lastTime = currentTime;
    }
    // update fps display
    fpsDisplay.Set("textContent", $"FPS: {fps} | Frame Time: {Math.Round(duration, 1)}ms");
    /// request next frame
    JS.CallVoid("requestAnimationFrame", cb);
});
/// request first frame
JS.CallVoid("requestAnimationFrame", cb);
// keep the using vars alive
await new TaskCompletionSource().Task;