using SpawnDev.SpawnJS.Marshal;
using SpawnDev.SpawnJS.Marshallers;
using System.Text.Json;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSObjectReference
    {
        public string PropertyTypeInfo(int key) => SpawnJSRuntime._propertyTypeInfo(Id, key);
        public bool PropertyIn(int key) => SpawnJSRuntime._propertyIn(Id, key);
        public bool PropertyDelete(int key) => SpawnJSRuntime._propertyDelete(Id, key);
        #region New
        public T NewApply<T>(int key, object?[]? args = null) => JS.InteropCall<double, int, object?[]?, T>("propertyNewApply", Id, key, args);
        public SpawnJSObjectReference NewApply(int key, object?[]? args = null) => JS.InteropCall<double, int, object?[]?, SpawnJSObjectReference>("propertyNewApply", Id, key, args);
        #endregion
        #region Call
        public void CallApplyVoid(int key, object?[]? args = null) => JS.InteropCall<double, int, object?[]?, VoidType>("propertyCallApply", Id, key, args);
        public T CallApply<T>(int key, object?[]? args = null) => JS.InteropCall<double, int, object?[]?, T>("propertyCallApply", Id, key, args);
        // CallVoid
        public void CallVoid(int key)
            => JS.InteropCall<double, object, VoidType>("propertyCall", Id, key);
        public void CallVoid<T1>(int key, T1 arg1)
            => JS.InteropCall<double, object, T1, VoidType>("propertyCall", Id, key, arg1);
        public void CallVoid<T1, T2>(int key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, object, T1, T2, VoidType>("propertyCall", Id, key, arg1, arg2);
        public void CallVoid<T1, T2, T3>(int key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, object, T1, T2, T3, VoidType>("propertyCall", Id, key, arg1, arg2, arg3);
        public void CallVoid<T1, T2, T3, T4>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, object, T1, T2, T3, T4, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4);
        public void CallVoid<T1, T2, T3, T4, T5>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5);
        public void CallVoid<T1, T2, T3, T4, T5, T6>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public void CallVoid<T1, T2, T3, T4, T5, T6, T7>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public void CallVoid<T1, T2, T3, T4, T5, T6, T7, T8>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T8, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public void CallVoid<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T9, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public void CallVoid<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        // Call
        public T Call<T>(int key)
            => JS.InteropCall<double, object, T>("propertyCall", Id, key);
        public T Call<T1, T>(int key, T1 arg1)
            => JS.InteropCall<double, object, T1, T>("propertyCall", Id, key, arg1);
        public T Call<T1, T2, T>(int key, T1 arg1, T2 arg2)
            => JS.InteropCall<double, object, T1, T2, T>("propertyCall", Id, key, arg1, arg2);
        public T Call<T1, T2, T3, T>(int key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCall<double, object, T1, T2, T3, T>("propertyCall", Id, key, arg1, arg2, arg3);
        public T Call<T1, T2, T3, T4, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4);
        public T Call<T1, T2, T3, T4, T5, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5);
        public T Call<T1, T2, T3, T4, T5, T6, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T8, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T9, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public T Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCall<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        #endregion
        #region CallAsync
        public Task CallApplyVoidAsync(int key, object?[]? args = null) => JS.InteropCallAsync<double, int, object?[]?, VoidType>("propertyCallApply", Id, key, args);
        public Task<T> CallApplyAsync<T>(int key, object?[]? args = null) => JS.InteropCallAsync<double, int, object?[]?, T>("propertyCallApply", Id, key, args);
        // CallVoidAsync
        public Task CallVoidAsync(int key)
            => JS.InteropCallAsync<double, object, VoidType>("propertyCall", Id, key);
        public Task CallVoidAsync<T1>(int key, T1 arg1)
            => JS.InteropCallAsync<double, object, T1, VoidType>("propertyCall", Id, key, arg1);
        public Task CallVoidAsync<T1, T2>(int key, T1 arg1, T2 arg2)
            => JS.InteropCallAsync<double, object, T1, T2, VoidType>("propertyCall", Id, key, arg1, arg2);
        public Task CallVoidAsync<T1, T2, T3>(int key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCallAsync<double, object, T1, T2, T3, VoidType>("propertyCall", Id, key, arg1, arg2, arg3);
        public Task CallVoidAsync<T1, T2, T3, T4>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4);
        public Task CallVoidAsync<T1, T2, T3, T4, T5>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T5, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T5, T6, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6, T7>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T5, T6, T7, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6, T7, T8>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T5, T6, T7, T8, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T9, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public Task CallVoidAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, VoidType>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        // CallAsync
        public Task<T> CallAsync<T>(int key)
            => JS.InteropCallAsync<double, object, T>("propertyCall", Id, key);
        public Task<T> CallAsync<T1, T>(int key, T1 arg1)
            => JS.InteropCallAsync<double, object, T1, T>("propertyCall", Id, key, arg1);
        public Task<T> CallAsync<T1, T2, T>(int key, T1 arg1, T2 arg2)
            => JS.InteropCallAsync<double, object, T1, T2, T>("propertyCall", Id, key, arg1, arg2);
        public Task<T> CallAsync<T1, T2, T3, T>(int key, T1 arg1, T2 arg2, T3 arg3)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T>("propertyCall", Id, key, arg1, arg2, arg3);
        public Task<T> CallAsync<T1, T2, T3, T4, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T5, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T5, T6, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T7, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T5, T6, T7, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T9, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        public Task<T> CallAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>(int key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            => JS.InteropCallAsync<double, object, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>("propertyCall", Id, key, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        #endregion
        #region Set
        public void Set<T>(int key, T value) => JS.InteropCall<double, int, T, VoidType>("propertySet", Id, key, value);
        #endregion
        #region Get
        public SpawnJSObjectReference? Get(int key) => JS.InteropCall<double, int, SpawnJSObjectReference>("propertyGet", Id, key);
        public T Get<T>(int key) => JS.InteropCall<double, int, T>("propertyGet", Id, key);
        #endregion
        #region GetAsync
        public Task<T> GetAsync<T>(int key) => JS.InteropCallAsync<double, int, T>("propertyGet", Id, key);
        public Task<SpawnJSObjectReference> GetAsync(int key) => JS.InteropCallAsync<double, int, SpawnJSObjectReference>("propertyGet", Id, key);
        #endregion
        #region PropertySet
        public void PropertySetNull(int key) => SpawnJSRuntime._propertySetNull(Id, key);
        public void PropertySetUndefined(int key) => SpawnJSRuntime._propertySetUndefined(Id, key);
        public void PropertySet(int key, string value) => SpawnJSRuntime._propertySet(Id, key, value);
        public void PropertySet(int key, bool value) => SpawnJSRuntime._propertySet(Id, key, value);
        public void PropertySet(int key, double value) => SpawnJSRuntime._propertySet(Id, key, value);
        public void PropertySet(int key, int value) => SpawnJSRuntime._propertySet(Id, key, value);
        public void PropertySet(int key, bool? value) => SpawnJSRuntime._propertySet(Id, key, value);
        public void PropertySet(int key, double? value) => SpawnJSRuntime._propertySet(Id, key, value);
        public void PropertySet(int key, SpawnJSObjectReference? value) => SpawnJSRuntime._propertySetSpawnJSObject(Id, key, value?.Id ?? NullId);
        public void PropertySet(int key, Callback? value)
        {
            value?.Sent = true;
            SpawnJSRuntime._propertySetCallback(Id, key, JS.DotnetInstance.Id, value?.Id ?? 0, value?.Once ?? false);
        }
        public void PropertySetJson(int key, object? value, JsonSerializerOptions? serializerOptions = null)
            => SpawnJSRuntime._propertySetJson(Id, key, JsonSerializer.Serialize(value, serializerOptions));
        public void PropertySetWithReviver(string reviver, int key, string value)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex);
        }
        public void PropertySetWithReviver(string reviver, int key, string value, string reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        public void PropertySetWithReviver(string reviver, int key, string value, double reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        public void PropertySetWithReviver(string reviver, int key, string value, bool reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        public void PropertySetWithReviver(string reviver, int key, double value)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex);
        }
        public void PropertySetWithReviver(string reviver, int key, double value, string reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        public void PropertySetWithReviver(string reviver, int key, double value, double reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        public void PropertySetWithReviver(string reviver, int key, double value, bool reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        public void PropertySetHeapView(int key, long offset, long length, bool copy = true)
            => PropertySetHeapView(key, offset, length, JSArrayBufferView.Uint8Array, copy);
        public void PropertySetHeapView(int key, long offset, long length, JSArrayBufferView viewType, bool copy = true)
            => SpawnJSRuntime._propertySetHeapView(Id, key, JS.DotnetInstance.Id, (double)viewType, offset, length, copy);
        public void PropertySet(int key, HeapViewDescriptor value)
            => SpawnJSRuntime._propertySetHeapView(Id, key, JS.DotnetInstance.Id, (double)value.Type, value.Offset, value.Length, value.Copy);
        #endregion
        #region PropertyGet
        public string? PropertyGetWithReplacerString(string replacer, int key, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerString(Id, methodIndex, key, replacerConfig);
        }
        public double PropertyGetWithReplacerDouble(string replacer, int key, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDouble(Id, methodIndex, key, replacerConfig);
        }
        public bool PropertyGetWithReplacerBoolean(string replacer, int key, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBoolean(Id, methodIndex, key, replacerConfig);
        }
        public double? PropertyGetWithReplacerDoubleNullable(string replacer, int key, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDoubleNullable(Id, methodIndex, key, replacerConfig);
        }
        public bool? PropertyGetWithReplacerBooleanNullable(string replacer, int key, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBooleanNullable(Id, methodIndex, key, replacerConfig);
        }
        public string? PropertyGetWithReplacerString(string replacer, int key, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerString(Id, methodIndex, key, replacerConfig);
        }
        public double PropertyGetWithReplacerDouble(string replacer, int key, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDouble(Id, methodIndex, key, replacerConfig);
        }
        public bool PropertyGetWithReplacerBoolean(string replacer, int key, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBoolean(Id, methodIndex, key, replacerConfig);
        }
        public double? PropertyGetWithReplacerDoubleNullable(string replacer, int key, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDoubleNullable(Id, methodIndex, key, replacerConfig);
        }
        public bool? PropertyGetWithReplacerBooleanNullable(string replacer, int key, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBooleanNullable(Id, methodIndex, key, replacerConfig);
        }
        public string? PropertyGetWithReplacerString(string replacer, int key)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerString(Id, methodIndex, key);
        }
        public double PropertyGetWithReplacerDouble(string replacer, int key)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDouble(Id, methodIndex, key);
        }
        public bool PropertyGetWithReplacerBoolean(string replacer, int key)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBoolean(Id, methodIndex, key);
        }
        public double? PropertyGetWithReplacerDoubleNullable(string replacer, int key)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDoubleNullable(Id, methodIndex, key);
        }
        public bool? PropertyGetWithReplacerBooleanNullable(string replacer, int key)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBooleanNullable(Id, methodIndex, key);
        }
        public string? PropertyGetString(int key) => SpawnJSRuntime._propertyGetString(Id, key);
        public double PropertyGetDouble(int key) => SpawnJSRuntime._propertyGetDouble(Id, key);
        public bool PropertyGetBoolean(int key) => SpawnJSRuntime._propertyGetBoolean(Id, key);
        public double? PropertyGetDoubleNullable(int key) => SpawnJSRuntime._propertyGetDoubleNullable(Id, key);
        public bool? PropertyGetBooleanNullable(int key) => SpawnJSRuntime._propertyGetBooleanNullable(Id, key);
        public SpawnJSObjectReference? PropertyGetSpawnJSObjectReference(int key, bool force = false)
        {
            var id = SpawnJSRuntime._propertyGetSpawnJSObjectReference(Id, key, force);
            return id == null ? null : new SpawnJSObjectReference(id.Value);
        }
        public T PropertyGetJson<T>(int key, JsonSerializerOptions? options = null)
        {
            var json = SpawnJSRuntime._propertyGetJson(Id, key);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, options)!;
        }
        #endregion
    }
}
