using System.Runtime.InteropServices.JavaScript;

namespace SpawnDev.SpawnJS
{
    public partial class SpawnJSRuntime
    {

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial string _propertyGetWithReplacerString(double sjsId, double methodIndex, int key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double _propertyGetWithReplacerDouble(double sjsId, double methodIndex, int key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool _propertyGetWithReplacerBoolean(double sjsId, double methodIndex, int key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double? _propertyGetWithReplacerDoubleNullable(double sjsId, double methodIndex, int key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool? _propertyGetWithReplacerBooleanNullable(double sjsId, double methodIndex, int key);


        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial string _propertyGetWithReplacerString(double sjsId, double methodIndex, int key, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double _propertyGetWithReplacerDouble(double sjsId, double methodIndex, int key, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool _propertyGetWithReplacerBoolean(double sjsId, double methodIndex, int key, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double? _propertyGetWithReplacerDoubleNullable(double sjsId, double methodIndex, int key, double replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool? _propertyGetWithReplacerBooleanNullable(double sjsId, double methodIndex, int key, double replacerConfig);


        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial string _propertyGetWithReplacerString(double sjsId, double methodIndex, int key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double _propertyGetWithReplacerDouble(double sjsId, double methodIndex, int key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool _propertyGetWithReplacerBoolean(double sjsId, double methodIndex, int key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial double? _propertyGetWithReplacerDoubleNullable(double sjsId, double methodIndex, int key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGetWithReplacer")]
        internal static partial bool? _propertyGetWithReplacerBooleanNullable(double sjsId, double methodIndex, int key, string replacerConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial string _propertyGetString(double sjsId, int key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial double _propertyGetDouble(double sjsId, int key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial bool _propertyGetBoolean(double sjsId, int key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial double? _propertyGetDoubleNullable(double sjsId, int key);

        [JSImport("globalThis.SpawnJSInterop.propertyGet")]
        internal static partial bool? _propertyGetBooleanNullable(double sjsId, int key);

        [JSImport("globalThis.SpawnJSInterop.propertyGetSpawnJSObjectReference")]
        internal static partial double? _propertyGetSpawnJSObjectReference(double sjsId, int key, bool force);

        [JSImport("globalThis.SpawnJSInterop.propertyGetJson")]
        internal static partial string? _propertyGetJson(double sjsId, int key);
        [JSImport("globalThis.SpawnJSInterop.propertyDelete")]
        internal static partial bool _propertyDelete(double sjsId, int key);
        [JSImport("globalThis.SpawnJSInterop.propertyIn")]
        internal static partial bool _propertyIn(double sjsId, int key);
        [JSImport("globalThis.SpawnJSInterop.propertySetNull")]
        internal static partial void _propertySetNull(double sjsId, int key);
        [JSImport("globalThis.SpawnJSInterop.propertySetUndefined")]
        internal static partial void _propertySetUndefined(double sjsId, int key);

        [JSImport("globalThis.SpawnJSInterop.propertySetCallback")]
        internal static partial void _propertySetCallback(double sjsId, int key, double dotnetId, double callbackId, bool once);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, int key, string value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, int key, bool value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, int key, double value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, int key, int value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, int key, bool? value);

        [JSImport("globalThis.SpawnJSInterop.propertySet")]
        internal static partial void _propertySet(double sjsId, int key, double? value);

        [JSImport("globalThis.SpawnJSInterop.propertySetSpawnJSObject")]
        internal static partial void _propertySetSpawnJSObject(double sjsId, int key, double value);

        [JSImport("globalThis.SpawnJSInterop.propertySetJson")]
        internal static partial void _propertySetJson(double sjsId, int key, string value);


        [JSImport("globalThis.SpawnJSInterop.propertySetHeapView")]
        internal static partial void _propertySetHeapView(double sjsId, int key, double dotnetId, double viewType, double offset, double length, bool copy);


        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, int key, string value, double reviverIndex);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, int key, double value, double reviverIndex);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, int key, string value, double reviverIndex, string reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, int key, double value, double reviverIndex, string reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, int key, string value, double reviverIndex, double reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, int key, double value, double reviverIndex, double reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, int key, string value, double reviverIndex, bool reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertySetWithReviver")]
        internal static partial void _propertySetWithReviver(double sjsId, int key, double value, double reviverIndex, bool reviverConfig);

        [JSImport("globalThis.SpawnJSInterop.propertyTypeInfo")]
        internal static partial string _propertyTypeInfo(double sjsId, int key);
    }
}
