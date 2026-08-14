using System.Text.Json;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSObjectReference
    {
        internal string PropertyTypeInfo(double key) => SpawnJSRuntime._propertyTypeInfo(Id, key);
        internal bool PropertyIn(double key) => SpawnJSRuntime._propertyIn(Id, key);
        internal bool PropertyDelete(double key) => SpawnJSRuntime._propertyDelete(Id, key);
        #region PropertySet
        internal void PropertySetNull(double key) => SpawnJSRuntime._propertySetNull(Id, key);
        internal void PropertySetUndefined(double key) => SpawnJSRuntime._propertySetUndefined(Id, key);
        internal void PropertySet(double key, string value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(double key, bool value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(double key, double value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(double key, int value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(double key, int? value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(double key, bool? value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(double key, double? value) => SpawnJSRuntime._propertySet(Id, key, value);
        internal void PropertySet(double key, SpawnJSObjectReference? value) => SpawnJSRuntime._propertySetSpawnJSObject(Id, key, value?.Id ?? NullId);
        internal void PropertySet(double key, Callback? value)
        {
            value?.Sent = true;
            SpawnJSRuntime._propertySetCallback(Id, key, JS.DotnetInstance.Id, value?.Id ?? 0, value?.Once ?? false);
        }
        internal void PropertySetJson(double key, object? value, JsonSerializerOptions? serializerOptions = null)
            => SpawnJSRuntime._propertySetJson(Id, key, JsonSerializer.Serialize(value, serializerOptions));
        internal void PropertySetWithReviver(string reviver, double key, string value)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex);
        }
        internal void PropertySetWithReviver(string reviver, double key, string value, string reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        internal void PropertySetWithReviver(string reviver, double key, string value, double reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        internal void PropertySetWithReviver(string reviver, double key, string value, bool reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        internal void PropertySetWithReviver(string reviver, double key, double value)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex);
        }
        internal void PropertySetWithReviver(string reviver, double key, double value, string reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        internal void PropertySetWithReviver(string reviver, double key, double value, double reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        internal void PropertySetWithReviver(string reviver, double key, double value, bool reviverConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(reviver);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {reviver}");
            SpawnJSRuntime._propertySetWithReviver(Id, key, value, methodIndex, reviverConfig);
        }
        internal void PropertySetHeapView(double key, long offset, long length, bool copy = true)
            => PropertySetHeapView(key, offset, length, JSArrayBufferView.Uint8Array, copy);
        internal void PropertySetHeapView(double key, long offset, long length, JSArrayBufferView viewType, bool copy = true)
            => SpawnJSRuntime._propertySetHeapView(Id, key, JS.DotnetInstance.Id, (double)viewType, offset, length, copy);
        internal void PropertySet(double key, HeapViewDescriptor value)
            => SpawnJSRuntime._propertySetHeapView(Id, key, JS.DotnetInstance.Id, (double)value.Type, value.Offset, value.Length, value.Copy);
        #endregion
        #region PropertyGet
        internal string? PropertyGetWithReplacerString(string replacer, double key, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerString(Id, methodIndex, key, replacerConfig);
        }
        internal double PropertyGetWithReplacerDouble(string replacer, double key, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDouble(Id, methodIndex, key, replacerConfig);
        }
        internal bool PropertyGetWithReplacerBoolean(string replacer, double key, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBoolean(Id, methodIndex, key, replacerConfig);
        }
        internal double? PropertyGetWithReplacerDoubleNullable(string replacer, double key, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDoubleNullable(Id, methodIndex, key, replacerConfig);
        }
        internal bool? PropertyGetWithReplacerBooleanNullable(string replacer, double key, double replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBooleanNullable(Id, methodIndex, key, replacerConfig);
        }
        internal string? PropertyGetWithReplacerString(string replacer, double key, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerString(Id, methodIndex, key, replacerConfig);
        }
        internal double PropertyGetWithReplacerDouble(string replacer, double key, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDouble(Id, methodIndex, key, replacerConfig);
        }
        internal bool PropertyGetWithReplacerBoolean(string replacer, double key, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBoolean(Id, methodIndex, key, replacerConfig);
        }
        internal double? PropertyGetWithReplacerDoubleNullable(string replacer, double key, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDoubleNullable(Id, methodIndex, key, replacerConfig);
        }
        internal bool? PropertyGetWithReplacerBooleanNullable(string replacer, double key, string replacerConfig)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBooleanNullable(Id, methodIndex, key, replacerConfig);
        }
        internal string? PropertyGetWithReplacerString(string replacer, double key)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerString(Id, methodIndex, key);
        }
        internal double PropertyGetWithReplacerDouble(string replacer, double key)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDouble(Id, methodIndex, key);
        }
        internal bool PropertyGetWithReplacerBoolean(string replacer, double key)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBoolean(Id, methodIndex, key);
        }
        internal double? PropertyGetWithReplacerDoubleNullable(string replacer, double key)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerDoubleNullable(Id, methodIndex, key);
        }
        internal bool? PropertyGetWithReplacerBooleanNullable(string replacer, double key)
        {
            var methodIndex = JS.InteropMethods.IndexOf(replacer);
            if (methodIndex == -1) throw new Exception($"Unknown SpawnJSInterop method. Index not found: {JS.InteropMethods.Length} {replacer}");
            return SpawnJSRuntime._propertyGetWithReplacerBooleanNullable(Id, methodIndex, key);
        }
        internal string? PropertyGetString(double key) => SpawnJSRuntime._propertyGetString(Id, key);
        internal double PropertyGetDouble(double key) => SpawnJSRuntime._propertyGetDouble(Id, key);
        internal bool PropertyGetBoolean(double key) => SpawnJSRuntime._propertyGetBoolean(Id, key);
        internal double? PropertyGetDoubleNullable(double key) => SpawnJSRuntime._propertyGetDoubleNullable(Id, key);
        internal bool? PropertyGetBooleanNullable(double key) => SpawnJSRuntime._propertyGetBooleanNullable(Id, key);
        internal SpawnJSObjectReference? PropertyGetSpawnJSObjectReference(double key, bool force = false)
        {
            var id = SpawnJSRuntime._propertyGetSpawnJSObjectReference(Id, key, force);
            return id == null ? null : new SpawnJSObjectReference(id.Value);
        }
        internal T PropertyGetJson<T>(double key, JsonSerializerOptions? options = null)
        {
            var json = SpawnJSRuntime._propertyGetJson(Id, key);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, options)!;
        }
        #endregion
    }
}
