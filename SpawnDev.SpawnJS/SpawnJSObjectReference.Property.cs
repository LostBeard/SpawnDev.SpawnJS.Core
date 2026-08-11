using System.Text.Json;

namespace SpawnDev.SpawnJS
{
    // This file holds the Property* methods: the low-level, NON-marshalled property surface.
    //  - Keys and values are restricted to the minimal primitive types (string, double, bool and their
    //    nullables) plus held object references and JSON. Each binds directly to a typed [JSImport], so
    //    nothing here boxes or uses Any/JSObject marshalling.
    //  - Richer .Net types are handled one layer up by the marshaller pipeline (Get/Set/CallApply), which
    //    decomposes them into these primitives.
    //  - No `object`-typed keys or values, and NEVER JSObject. Keeping the marshalled and non-marshalled
    //    paths cleanly separated is what makes this base flexible and reliable.
    public partial class SpawnJSObjectReference
    {
        internal static SpawnJSObjectReference? FromID(double fromJS, bool nonNullable = false)
        {
            return !nonNullable && (fromJS == SpawnJSObjectReference.Null || fromJS == SpawnJSObjectReference.UndefinedId) ? null : new SpawnJSObjectReference((long)fromJS);
        }

        /// <summary>Returns "&lt;typeof&gt; &lt;toStringTag&gt;" for the property, or null if it is absent.</summary>
        public string PropertyTypeInfo(string key) => SpawnJSRuntime._propertyTypeInfo(Id, key);
        public string PropertyTypeInfo(double key) => SpawnJSRuntime._propertyTypeInfo(Id, key);

        /// <summary>JS <c>key in obj</c>.</summary>
        public bool PropertyIn(string key) => SpawnJSRuntime._propertyIn(Id, key);
        public bool PropertyIn(double key) => SpawnJSRuntime._propertyIn(Id, key);

        /// <summary>JS <c>delete obj[key]</c>.</summary>
        public bool PropertyDelete(string key) => SpawnJSRuntime._propertyDelete(Id, key);
        public bool PropertyDelete(double key) => SpawnJSRuntime._propertyDelete(Id, key);

        /// <summary>Assigns JS <c>null</c> to the property.</summary>
        public void PropertySetNull(string key) => SpawnJSRuntime._propertySetNull(Id, key);
        public void PropertySetNull(double key) => SpawnJSRuntime._propertySetNull(Id, key);

        /// <summary>Assigns JS <c>undefined</c> to the property.</summary>
        public void PropertySetUndefined(string key) => SpawnJSRuntime._propertySetUndefined(Id, key);
        public void PropertySetUndefined(double key) => SpawnJSRuntime._propertySetUndefined(Id, key);

        public void PropertySet(string key, string value) => SpawnJSRuntime._propertySet(Id, key, value);

        public void PropertySet(string key, bool value) => SpawnJSRuntime._propertySet(Id, key, value);

        public void PropertySet(string key, double value) => SpawnJSRuntime._propertySet(Id, key, value);

        public void PropertySet(string key, bool? value) => SpawnJSRuntime._propertySet(Id, key, value);

        public void PropertySet(string key, double? value) => SpawnJSRuntime._propertySet(Id, key, value);

        public void PropertySet(string key, SpawnJSObjectReference? value) => SpawnJSRuntime._propertySetSpawnJSObject(Id, key, value?.Id ?? Null);

        public void PropertySetJson(string key, object? value, JsonSerializerOptions? serializerOptions = null) => SpawnJSRuntime._propertySetJson(Id, key, JsonSerializer.Serialize(value, serializerOptions));



        public void PropertySet(double key, string value) => SpawnJSRuntime._propertySet(Id, key, value);

        public void PropertySet(double key, bool value) => SpawnJSRuntime._propertySet(Id, key, value);

        public void PropertySet(double key, double value) => SpawnJSRuntime._propertySet(Id, key, value);

        public void PropertySet(double key, bool? value) => SpawnJSRuntime._propertySet(Id, key, value);

        public void PropertySet(double key, double? value) => SpawnJSRuntime._propertySet(Id, key, value);

        public void PropertySet(double key, SpawnJSObjectReference? value) => SpawnJSRuntime._propertySetSpawnJSObject(Id, key, value?.Id ?? Null);

        public void PropertySetJson(double key, object? value, JsonSerializerOptions? serializerOptions = null) => SpawnJSRuntime._propertySetJson(Id, key, JsonSerializer.Serialize(value, serializerOptions));


        public string? PropertyGetString(string key) => SpawnJSRuntime._propertyGetString(Id, key);

        public double PropertyGetDouble(string key) => SpawnJSRuntime._propertyGetDouble(Id, key);

        public bool PropertyGetBoolean(string key) => SpawnJSRuntime._propertyGetBoolean(Id, key);

        public double? PropertyGetDoubleNullable(string key) => SpawnJSRuntime._propertyGetDoubleNullable(Id, key);

        public bool? PropertyGetBooleanNullable(string key) => SpawnJSRuntime._propertyGetBooleanNullable(Id, key);

        public SpawnJSObjectReference? PropertyGetSpawnJSObjectReference(string key, bool force = false)
        {
            var id = SpawnJSRuntime._propertyGetSpawnJSObjectReference(Id, key, force);
            return id == null ? null : new SpawnJSObjectReference((long)id.Value);
        }

        public T PropertyGetJson<T>(string key, JsonSerializerOptions? options = null)
        {
            var json = SpawnJSRuntime._propertyGetJson(Id, key);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, options)!;
        }

        public string? PropertyGetString(double key) => SpawnJSRuntime._propertyGetString(Id, key);

        public double PropertyGetDouble(double key) => SpawnJSRuntime._propertyGetDouble(Id, key);

        public bool PropertyGetBoolean(double key) => SpawnJSRuntime._propertyGetBoolean(Id, key);

        public double? PropertyGetDoubleNullable(double key) => SpawnJSRuntime._propertyGetDoubleNullable(Id, key);

        public bool? PropertyGetBooleanNullable(double key) => SpawnJSRuntime._propertyGetBooleanNullable(Id, key);

        public SpawnJSObjectReference? PropertyGetSpawnJSObjectReference(double key, bool force = false)
        {
            var id = SpawnJSRuntime._propertyGetSpawnJSObjectReference(Id, key, force);
            return id == null ? null : new SpawnJSObjectReference((long)id.Value);
        }

        public T PropertyGetJson<T>(double key, JsonSerializerOptions? options = null)
        {
            var json = SpawnJSRuntime._propertyGetJson(Id, key);
            return json == null ? default! : JsonSerializer.Deserialize<T>(json, options)!;
        }
    }
}
