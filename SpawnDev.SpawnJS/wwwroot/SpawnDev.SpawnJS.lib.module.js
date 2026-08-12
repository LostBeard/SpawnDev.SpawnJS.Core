console.log('SpawnJSInterop');

// SpawnJSInterop - the Javascript half of SpawnJS.
//
// Architecture: every JS value that .Net needs to reference is kept in an id-keyed table
// (spawnJSObjects) and addressed from .Net by that numeric id. .Net never receives a live JS object
// handle (no Microsoft JSObject), only ids and primitives - which is what lets the .Net side avoid
// JSObject and its disposal quirk entirely. .Net "holds" a value (spawnJSObjectHold -> id) and later
// "releases" it (spawnJSObjectRelease) so it can be garbage-collected.
//
// Negative ids are reserved sentinels resolved without a table lookup:
//   -1 globalThis, -2 undefined, -3 null, -4 the spawnJSObjects table itself.
//
// Calls flow through _spawnJSInteropCall (sync) / _spawnJSInteropCallAsync (async): .Net names a static
// method here plus a returnType code (see ReturnType.cs), and _serializeToNet shapes the result to match.
(function () {
    if (globalThis.SpawnJSInterop) return;

    class SpawnJSInterop {
        static _methodMapNames = [];
        static _methodMap = [];
        static verbose = false;
        // The id -> JS value table. Holds every value .Net currently references.
        static spawnJSObjects = {};
        // Monotonic id source; never reused, so a stale .Net id can never collide with a live value.
        static _sjsObjectIdNext = 0;
        static {
            SpawnJSInterop._methodMap = [];
            SpawnJSInterop._methodMapNames = [];
            var keys = Reflect.ownKeys(SpawnJSInterop);
            for (const pName of keys) {
                var propVal = SpawnJSInterop[pName];
                if (typeof propVal === 'function') {
                    SpawnJSInterop._methodMap.push(propVal.bind(SpawnJSInterop))
                    SpawnJSInterop._methodMapNames.push(pName);
                }
            }
        }
        static spawnJSObjectNewObject() {
            return SpawnJSInterop.spawnJSObjectHold({});
        }
        static spawnJSObjectNewArray() {
            return SpawnJSInterop.spawnJSObjectHold([]);
        }
        // removes the SpawnJSObject from spawnJSObjects so it can be garbage collected
        static spawnJSObjectRelease(sjsId) {
            var ret = SpawnJSInterop.spawnJSObjects[sjsId];
            delete SpawnJSInterop.spawnJSObjects[sjsId];
            return ret;
        }
        static spawnJSObjectReleaseAsJson(sjsId) {
            var ret = SpawnJSInterop.spawnJSObjects[sjsId];
            delete SpawnJSInterop.spawnJSObjects[sjsId];
            return JSON.stringify(ret);
        }
        // puts objectToHold (value of ANY type) into spawnJSObjects and returns the id
        // the object will stay held until released and it must be released to prevent memory leaks
        static spawnJSObjectHold(objectToHold) {
            if (objectToHold === globalThis) return -1;
            if (objectToHold === undefined) return -2;
            if (objectToHold === null) return -3;
            if (objectToHold === SpawnJSInterop.spawnJSObjects) return -4;
            var sjsId = ++SpawnJSInterop._sjsObjectIdNext;
            SpawnJSInterop.spawnJSObjects[sjsId] = objectToHold;
            return sjsId;
        }
        static spawnJSObjectGet(sjsId) {
            switch (sjsId) {
                case -1: return globalThis;
                case -2: return undefined;
                case -3: return null;
                case -4: return SpawnJSInterop.spawnJSObjects;
            }
            if (!SpawnJSInterop.spawnJSObjectHoldExists(sjsId)) throw new Error('SpawnJSObjectGet object not found.');
            return SpawnJSInterop.spawnJSObjects[sjsId];
        }
        static spawnJSObjectGetAndReplace(sjsId, newValue) {
            switch (sjsId) {
                case -1: return globalThis;
                case -2: return undefined;
                case -3: return null;
                case -4: return SpawnJSInterop.spawnJSObjects;
            }
            if (!SpawnJSInterop.spawnJSObjectHoldExists(sjsId)) throw new Error('SpawnJSObjectGet object not found.');
            var ret = SpawnJSInterop.spawnJSObjects[sjsId];
            SpawnJSInterop.spawnJSObjects[sjsId] = newValue;
            return ret;
        }
        // returns true if the hold exists
        static spawnJSObjectHoldExists(sjsId) {
            return sjsId in SpawnJSInterop.spawnJSObjects;
        }

        static propertyCallApplySJS(sjsId, key, argsId) {
            var args = SpawnJSInterop.spawnJSObjectGet(argsId);
            return propertyCallApply(sjsId, key, args);
        }

        static propertyNewApply(sjsId, key, args) {
            var ret = undefined;
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            var pathInfo = SpawnJSInterop.pathObjectInfo(obj, key);
            if (pathInfo.shortCircuit) return ret;
            var ret = !args ? new pathInfo.target() : new pathInfo.target(...args);
            return ret;
        }

        static propertyCallApply(sjsId, key, args) {
            var ret = undefined;
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            var pathInfo = SpawnJSInterop.pathObjectInfo(obj, key);
            if (pathInfo.shortCircuit) return ret;
            var ret = pathInfo.target.apply(pathInfo.parent, args);
            if (typeof ret === 'function') {
                ret = ret.bind(pathInfo.parent);
            }
            return ret;
        }

        static propertyGet(sjsId, key) {
            var ret = undefined;
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            var pathInfo = SpawnJSInterop.pathObjectInfo(obj, key);
            if (pathInfo.shortCircuit) return ret;
            if (typeof pathInfo.target === 'function') {
                ret = pathInfo.target.bind(pathInfo.parent);
            } else {
                ret = pathInfo.target;
            }
            return ret;
        }

        static propertyGetJson(sjsId, key) {
            var ret = SpawnJSInterop.propertyGet(sjsId, key);
            ret = JSON.stringify(ret);
            return ret;
        }

        static propertyGetSpawnJSObjectReference(sjsId, key, force) {
            var ret = SpawnJSInterop.propertyGet(sjsId, key);
            if (force) {
                ret = SpawnJSInterop.spawnJSObjectHold(ret);
            } else {
                ret = ret === null || ret === undefined ? null : SpawnJSInterop.spawnJSObjectHold(ret);
            }
            return ret;
        }

        // This is where SpawnJSObjectReference revives itself into JS via its sjsId
        // returns undefined
        static propertySetSpawnJSObject(sjsId, key, valueId) {
            var value = SpawnJSInterop.spawnJSObjectGet(valueId);
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            parent[propertyName] = value;
        }

        static propertySetJson(sjsId, key, json) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            var value = JSON.parse(json);
            parent[propertyName] = value;
        }

        // returns undefined
        static propertySet(sjsId, key, value) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            parent[propertyName] = value;
        }

        // returns undefined
        static propertySetNull(sjsId, key) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            parent[propertyName] = null;
        }

        // returns undefined
        static propertySetUndefined(sjsId, key) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            parent[propertyName] = undefined;
        }

        // returns bool
        static propertyDelete(sjsId, key) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return true;
            return delete parent[propertyName];
        }

        // returns bool
        static propertyIn(sjsId, key) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return false;
            return SpawnJSInterop._in(propertyName, parent);
        }

        // returns string or null if the property was not found
        static propertyTypeInfo(sjsId, key) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return null;
            if (!SpawnJSInterop._in(propertyName, parent)) return null;
            var value = parent[propertyName];
            var jsClass = Object.prototype.toString.call(value).split(' ')[1].slice(0, -1);
            var jsType = typeof (value);
            return `${jsType} ${jsClass}`;
        }

        // returns string[]
        static getConstructorNames(obj) {
            var constructorNames = [];
            if (obj === void 0 || obj === null) return constructorNames;
            var o = obj;
            var cName;
            while (1) {
                o = Object.getPrototypeOf(o);
                cName = o?.constructor?.name;
                if (!cName) break;
                if (constructorNames.indexOf(cName) !== -1) continue;
                constructorNames.push(cName);
            }
            return constructorNames;
        }

        // returns string[] of the target's property names.
        // hasOwnProperty true restricts to the object's own enumerable keys (Object.keys); false walks the
        // prototype chain too, which is what you need to enumerate a DOM object's API rather than just the
        // handful of own properties it happens to carry.
        static objectKeys(target, hasOwnProperty) {
            if (target === void 0 || target === null) return [];
            if (hasOwnProperty) return Object.keys(target);
            var keys = [];
            for (var key in target) {
                if (keys.indexOf(key) === -1) keys.push(key);
            }
            return keys;
        }
        // full ? strict equality : loose equality
        static objectEquals(obj1, obj2, full) {
            return full ? obj1 === obj2 : obj1 == obj2;
        }
        // returns string
        static getTypeInfo(sjsId) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            var jsClass = Object.prototype.toString.call(obj).split(' ')[1].slice(0, -1);
            var jsType = typeof (obj);
            return `${jsType} ${jsClass}`;
        }
        // returns string[]
        static getPropertyConstructorNames(parent, key) {
            return SpawnJSInterop.getConstructorNames(parent[key]);
        }

        // Main .Net to JS entrypoint (synchronous).
        // The args array is fetched AND replaced with a fresh empty [] in the same slot, so the .Net side's
        // pooled argument-array reference (same id) comes back emptied and ready to reuse on the next call.
        static _spawnJSInteropCall(returnType, methodName, argsId) {
            var target = typeof methodName === 'string' ? SpawnJSInterop[methodName] : SpawnJSInterop._methodMap[methodName];
            var args = argsId === null || argsId === undefined ? null : SpawnJSInterop.spawnJSObjectGetAndReplace(argsId, []);
            var ret = !args ? target() : target(...args);
            // shape the result to match the .Net side's expected returnType
            ret = SpawnJSInterop._serializeToNet(returnType, ret);
            return ret;
        }
        // Main .Net to JS entrypoint
        static async _spawnJSInteropCallAsync(returnType, dotnetId, asyncCallId, methodName, argsId) {
            var target = typeof methodName === 'string' ? SpawnJSInterop[methodName] : SpawnJSInterop._methodMap[methodName];
            var dotnet = SpawnJSInterop.spawnJSObjectGet(dotnetId);
            var args = argsId === null || argsId === undefined ? null : SpawnJSInterop.spawnJSObjectGetAndReplace(argsId, []);
            var error = null;
            var ret = null;
            try {
                ret = !args ? target() : target(...args);
                ret = await ret;
                // prepare using returnType
                ret = SpawnJSInterop._serializeToNet(returnType, ret);
            } catch (ex) {
                // call the exceptionCallbackId
                error = SpawnJSInterop.errorToString(ex);
            }
            switch (returnType) {
                case 0: // void
                    dotnet.spawnJSExports.SpawnJSRuntime.AsyncCallResolvedVoid(asyncCallId, error);
                    break;
                case 1: // Double
                    dotnet.spawnJSExports.SpawnJSRuntime.AsyncCallResolvedDouble(asyncCallId, ret, error);
                    break;
                case 2: // Boolean
                    dotnet.spawnJSExports.SpawnJSRuntime.AsyncCallResolvedBoolean(asyncCallId, ret, error);
                    break;
                case 3: // DoubleNullable
                    dotnet.spawnJSExports.SpawnJSRuntime.AsyncCallResolvedDoubleNullable(asyncCallId, ret, error);
                    break;
                case 4: // BooleanNullable
                    dotnet.spawnJSExports.SpawnJSRuntime.AsyncCallResolvedBooleanNullable(asyncCallId, ret, error);
                    break;
                case 5: // String
                    dotnet.spawnJSExports.SpawnJSRuntime.AsyncCallResolvedString(asyncCallId, ret, error);
                    break;
                case 6: // SpawnJSObject
                    dotnet.spawnJSExports.SpawnJSRuntime.AsyncCallResolvedDouble(asyncCallId, ret, error);
                    break;
                case 7: // SpawnJSObjectNonNullable
                    dotnet.spawnJSExports.SpawnJSRuntime.AsyncCallResolvedDouble(asyncCallId, ret, error);
                    break;
                case 8: // Json
                    dotnet.spawnJSExports.SpawnJSRuntime.AsyncCallResolvedString(asyncCallId, ret, error);
                    break;
                default:
                    throw new Error(`Unsupported returnType ${returnType}`);
                    break;
            }
        }
        static errorToString(error) {
            if (!error) return "Unknown error";
            // Handle native Error objects (e.g., new Error(), TypeError)
            if (error instanceof Error) {
                error = error.message;
            }
            else if (typeof error === 'object') {
                try {
                    error = error.message || JSON.stringify(error);
                } catch {
                    error = String(error);
                }
            }
            error ??= String(error);
            error ??= "Unknown error";
            return error;
        }
        // Main .Net to JS entrypoint
        static async _spawnJSInteropLoadExportsAsync(dotnetId, assemblyName) {
            var dotnet = SpawnJSInterop.spawnJSObjectGet(dotnetId);
            var assemblyExports = await dotnet.getAssemblyExports(assemblyName);
            var spawnJSExports = assemblyName.split('.').reduce((acc, key) => acc[key], assemblyExports);
            dotnet.spawnJSExports = spawnJSExports;
        }
        // prepares the variable for .Net based on returnType
        static _serializeToNet(returnType, ret) {
            switch (returnType) {
                case 0: // void
                    return;
                case 1: // Double
                case 2: // Boolean
                case 3: // DoubleNullable
                case 4: // BooleanNullable
                case 5: // String
                    return ret;
                case 6: // SpawnJSObject - Number
                case 7: // SpawnJSObjectNonNullable - Number
                    return SpawnJSInterop.spawnJSObjectHold(ret);
                case 8: // Json
                    return JSON.stringify(ret);
            }
            // the default is to wrap itand let .Net decide how to handle it
            return SpawnJSInterop.spawnJSObjectHold(ret);
        }

        static _in(key, obj) {
            if (obj === null || obj === void 0) return false;
            try {
                return key in Object(obj);
            } catch { }
            return false;
        }

        static pathObjectInfo(rootObject, path) {
            if (rootObject === null || rootObject === void 0) {
                // callers must call with the globalThis if they wish to use it as the rootObject.
                throw new Error('spawnJSInterop.pathObjectInfo error: rootObject cannot be null');
            }
            var parent = rootObject;
            var target;
            var propertyName;
            var shortCircuit = false;
            if (typeof path === 'string' && !(SpawnJSInterop._in(path, parent))) {
                var parts = path.split('.');
                propertyName = parts[parts.length - 1];
                var part;
                for (var i = 0; i < parts.length - 1; i++) {
                    part = parts[i];
                    if (part[part.length - 1] === '?') {
                        // ? null conditonal found
                        // if parent does not exist allow undefined/null parent instead of throwing exception
                        part = part.substring(0, part.length - 1);
                        parent = parent[part];
                        if (parent === void 0 || parent === null) {
                            shortCircuit = true;
                            break;
                        }
                    }
                    else {
                        parent = parent[part];
                    }
                }
                if (!shortCircuit) {
                    target = parent[propertyName];
                }
            }
            else {
                propertyName = path;
                target = parent[propertyName];
            }
            return {
                shortCircuit,   // bool - true if the pathfinding short circuited due to a null-conditional
                parent,         // any - only null or undefined if short circuited due to a null-conditional
                propertyName,   // any
                target,         // any
            };
        }

    }
    globalThis.SpawnJSInterop = SpawnJSInterop;
    SpawnJSInterop.init();
})();