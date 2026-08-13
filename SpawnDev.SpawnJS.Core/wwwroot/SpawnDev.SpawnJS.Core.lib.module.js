// SpawnJSInterop - the Javascript half of SpawnJS.
//
// Architecture: every JS value that .Net needs to reference is kept in an id-keyed table
// (spawnJSObjects) and addressed from .Net by that numeric id. .Net never receives a live JS object
// handle (no Microsoft JSObject), only ids and primitives - which is what lets the .Net side avoid
// JSObject and its disposal quirk entirely. .Net "holds" a value (spawnJSObjectHold -> id) and later
// "releases" it (spawnJSObjectRelease) so it can be garbage-collected.
//
// Negative ids are reserved sentinels resolved without a table lookup:
//   -1 globalThis, -2 undefined, -3 null, -4 the spawnJSObjects table, SpawnJSInterop.
//
// Calls flow through _spawnJSInteropCall (sync) / _spawnJSInteropCallAsync (async): .Net names a static
// method here plus a returnType code (see ReturnType.cs), and _serializeToNet shapes the result to match.
(function () {
    if (globalThis.SpawnJSInterop) return;
    class SpawnJSInterop {
        // enables verbose logging
        static verbose = false;
        // ArrayBufferView constructors
        static HeapViewCtors = [
            globalThis.BigInt64Array,                     // 0: BigInt64Array
            globalThis.BigUint64Array,                    // 1: BigUInt64Array
            globalThis.Float16Array,                      // 2: Float16Array
            globalThis.Float32Array,                      // 3: Float32Array
            globalThis.Float64Array,                      // 4: Float64Array
            globalThis.Int16Array,                        // 5: Int16Array
            globalThis.Int32Array,                        // 6: Int32Array
            globalThis.Int8Array,                         // 7: Int8Array
            globalThis.Uint16Array,                       // 8: Uint16Array
            globalThis.Uint32Array,                       // 9: Uint32Array
            globalThis.Uint8Array,                        // 10: Uint8Array
            globalThis.Uint8ClampedArray,                 // 11: Uint8ClampedArray
            globalThis.DataView                           // 12: DataView
        ];
        static _revivers = [];
        static _replacers = [];
        // method names for index based calling as an alternative to string
        static _methodMapNames = [];
        // methods mapped by index for index based calling as an alternative to string
        static _methodMap = [];
        // The id -> JS value table. Holds every value .Net currently references.
        static spawnJSObjects = {};
        // Monotonic id source; never reused, so a stale .Net id can never collide with a live value.
        static _sjsObjectIdNext = 0;
        // .Net Wasm app instance infos
        static _instances = {};
        // .Net callbacks
        static _callbacks = {};
        // static constructor
        static {
            // SpawnJSInterop.registerReviver('__reviverTest', (key, value) => {
            //     if (SpawnJSInterop.verbose) console.log('[SpawnJSInterop] reviver test', key, value);
            //     return value;
            // });
            // SpawnJSInterop.registerReplacer('__replacerTest', (key, value) => {
            //     if (SpawnJSInterop.verbose) console.log('[SpawnJSInterop] replacer test', key, value);
            //     return value;
            // });
            SpawnJSInterop.refreshMethodMap();
        }
        static _registerInstance(dotnet, onMethodAdded, resolveVoid, resolveDouble, resolveBoolean, resolveVoidString, resolveDoubleNullable, resolveBooleanNullable, resolveInt32, resolveInt32Nullable, handleCallback) {
            if (!dotnet) throw new Error('dotnet not set');
            var instanceInfo = SpawnJSInterop._getInstaceFromDotNet(dotnet);
            if (instanceInfo) return instanceInfo.dotnetId;
            var dotnetId = SpawnJSInterop.spawnJSObjectHold(dotnet);
            var instanceInfo = { dotnet, dotnetId, onMethodAdded, resolveVoid, resolveDouble, resolveBoolean, resolveVoidString, resolveDoubleNullable, resolveBooleanNullable, resolveInt32, resolveInt32Nullable, handleCallback };
            SpawnJSInterop._instances[dotnetId] = instanceInfo;
            if (SpawnJSInterop.verbose) console.log('[SpawnJSInterop] Instance registered', instanceInfo);
            return dotnetId;
        }
        static getInstace(dotnetId) {
            return SpawnJSInterop._instances[dotnetId];
        }
        static _getInstaceFromDotNet(dotnet) {
            for (const dotnetIdExisting in SpawnJSInterop._instances) {
                if (Object.hasOwn(SpawnJSInterop._instances, dotnetIdExisting)) {
                    var existingInfo = SpawnJSInterop._instances[dotnetIdExisting];
                    if (existingInfo.dotnet == dotnet) {
                        return existingInfo;
                    }
                }
            }
        }
        static __replacerJson(key, value, directCall) {
            if (directCall) value = JSON.stringify(value);
            return value;
        }
        static __reviverJson(key, value, directCall) {
            if (directCall) value = JSON.parse(value);
            return value;
        }
        static _unregisterInstance(dotnetId) {
            delete SpawnJSInterop._instances[dotnetId];
        }
        static _getMappedMethodNames() {
            return SpawnJSInterop._methodMapNames;
        }
        // refreshes the method map by looking for any new methods and adds them
        static refreshMethodMap() {
            var changed = false;
            var keys = Reflect.ownKeys(SpawnJSInterop);
            for (const pName of keys) {
                if (SpawnJSInterop._methodMapNames.indexOf(pName) !== -1) continue;
                var propVal = SpawnJSInterop[pName];
                if (typeof propVal === 'function') {
                    var fn = propVal.bind(SpawnJSInterop);
                    changed = true;
                    SpawnJSInterop._methodMap.push(fn);
                    SpawnJSInterop._methodMapNames.push(pName);
                    if (pName.indexOf('__reviver') === 0) {
                        SpawnJSInterop._revivers.push(fn);
                    } else if (pName.indexOf('__replacer') === 0) {
                        SpawnJSInterop._replacers.push(fn);
                    }
                }
            }
            if (changed) {
                // notify existing instances
                for (const dotnetIdExisting in SpawnJSInterop._instances) {
                    if (Object.hasOwn(SpawnJSInterop._instances, dotnetIdExisting)) {
                        var existingInfo = SpawnJSInterop._instances[dotnetIdExisting];
                        try {
                            existingInfo.onMethodAdded();
                        } catch { }
                    }
                }
            }
            return SpawnJSInterop._methodMapNames;
        }
        // array of { name, reviver }
        static registerReplacers(replacers) {
            var cnt = 0;
            if (!replacers) return cnt;
            for (var replacerObj of replacers) {
                var succ = SpawnJSInterop.registerReplacer(replacerObj.name, replacerObj.replacer);
                if (succ) cnt++;
            }
            return cnt;
        }
        static registerReplacer(name, replacer) {
            if (name.indexOf('__replacer') !== 0) throw new Error('Replacer names must start with __replacer');
            if (SpawnJSInterop[name]) {
                // already exists. fail quitely as it could just mean another app loaded that uses the same replacers
                return false;
            }
            SpawnJSInterop[name] = replacer;
            SpawnJSInterop.refreshMethodMap();
            return true;
        }
        // array of { name, reviver }
        static registerRevivers(revivers) {
            var cnt = 0;
            if (!revivers) return cnt;
            for (var reviverObj of revivers) {
                var succ = SpawnJSInterop.registerReviver(reviverObj.name, reviverObj.reviver);
                if (succ) cnt++;
            }
            return cnt;
        }
        static registerReviver(name, reviver) {
            if (name.indexOf('__reviver') !== 0) throw new Error('Reviver names must start with __reviver');
            if (SpawnJSInterop[name]) {
                // already exists. fail quitely as it could just mean another app loaded that uses the same revivers
                return false;
            }
            SpawnJSInterop[name] = reviver;
            SpawnJSInterop.refreshMethodMap();
            return true;
        }
        // creates a new Object, adds it to the hold and returns it
        static spawnJSObjectNewObject() {
            return SpawnJSInterop.spawnJSObjectHold({});
        }
        // creates a new Array, adds it to the hold and returns it
        static spawnJSObjectNewArray() {
            return SpawnJSInterop.spawnJSObjectHold([]);
        }
        // removes the object from the hold and returns it
        static spawnJSObjectRelease(sjsId) {
            var ret = SpawnJSInterop.spawnJSObjects[sjsId];
            delete SpawnJSInterop.spawnJSObjects[sjsId];
            return ret;
        }
        // removes the object from the hold, JSON.stringifies it and returns it
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
            if (objectToHold === SpawnJSInterop) return -5;
            var sjsId = ++SpawnJSInterop._sjsObjectIdNext;
            SpawnJSInterop.spawnJSObjects[sjsId] = objectToHold;
            return sjsId;
        }
        // get an object from the hold
        static spawnJSObjectGet(sjsId) {
            switch (sjsId) {
                case -1: return globalThis;
                case -2: return undefined;
                case -3: return null;
                case -4: return SpawnJSInterop.spawnJSObjects;
                case -5: return SpawnJSInterop;
            }
            if (!SpawnJSInterop.spawnJSObjectHoldExists(sjsId)) {
                throw new Error('SpawnJSObjectGet object not found.');
            }
            var ret = SpawnJSInterop.spawnJSObjects[sjsId];
            // passive revive (allows things like refreshing heap views)
            ret = SpawnJSInterop.reviveValue(null, ret, false);
            return ret;
        }// get an obejct from the hold and replace it with a new one
        static spawnJSObjectGetAndReplace(sjsId, newValue) {
            switch (sjsId) {
                case -1: return globalThis;
                case -2: return undefined;
                case -3: return null;
                case -4: return SpawnJSInterop.spawnJSObjects;
                case -5: return SpawnJSInterop;
            }
            if (!SpawnJSInterop.spawnJSObjectHoldExists(sjsId)) {
                throw new Error('SpawnJSObjectGet object not found.');
            }
            var ret = SpawnJSInterop.spawnJSObjects[sjsId];
            SpawnJSInterop.spawnJSObjects[sjsId] = newValue;
            return ret;
        }
        // returns true if the item id exists in the hold
        static spawnJSObjectHoldExists(sjsId) {
            return sjsId in SpawnJSInterop.spawnJSObjects;
        }
        // call a property constructor
        static propertyNewApply(sjsId, key, args) {
            var ret = undefined;
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            var pathInfo = SpawnJSInterop.pathObjectInfo(obj, key);
            if (pathInfo.shortCircuit) return ret;
            var ret = !args ? new pathInfo.target() : new pathInfo.target(...args);
            return ret;
        }
        // call a property constructor
        static propertyNew(sjsId, key, ...args) {
            var ret = undefined;
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            var pathInfo = SpawnJSInterop.pathObjectInfo(obj, key);
            if (pathInfo.shortCircuit) return ret;
            var ret = !args ? new pathInfo.target() : new pathInfo.target(...args);
            return ret;
        }
        // call a property
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
        // call a property
        static propertyCall(sjsId, key, ...args) {
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
        // get a property
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
        // get a property
        static propertyGetWithReplacer(sjsId, key, methodName, replacerConfig) {
            var ret = undefined;
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            var pathInfo = SpawnJSInterop.pathObjectInfo(obj, key);
            if (pathInfo.shortCircuit) return ret;
            if (typeof pathInfo.target === 'function') {
                ret = pathInfo.target.bind(pathInfo.parent);
            } else {
                ret = pathInfo.target;
            }
            var replacer = typeof methodName === 'string' ? SpawnJSInterop[methodName] : SpawnJSInterop._methodMap[methodName];
            if (!replacer) throw new Error(`Reviver not found: ${methodName}`);
            ret = !replacer ? ret : replacer(pathInfo.propertyName, ret, true, replacerConfig);
            return ret;
        }
        // useful when .Net Wasm wants to clonme a JSObjectReference or simply convert from one type to another
        static returnMe(value) {
            return value;
        }
        // get a property as json
        static propertyGetJson(sjsId, key) {
            var ret = SpawnJSInterop.propertyGet(sjsId, key);
            ret = JSON.stringify(ret);
            return ret;
        }
        // get a property as a SpawnJSObjectReference
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
        // set property to a SpawnJSObjectReference
        static propertySetSpawnJSObject(sjsId, key, valueId) {
            var value = SpawnJSInterop.spawnJSObjectGet(valueId);
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            parent[propertyName] = value;
        }
        // set property to a Json
        static propertySetJson(sjsId, key, json) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            var value = JSON.parse(json);
            parent[propertyName] = value;
        }
        static releaseCallback(dotnetId, callbackId) {
            var callbackIdPair = `${dotnetId}_${callbackId}`;
            delete SpawnJSInterop._callbacks[callbackIdPair];
        }
        static writeArrayBufferViewToHeap(dotnetId, arrayBufferView, srcOffset, destOffset, byteLength) {
            if (!arrayBufferView) throw new Error('writeArrayBufferViewToHeap arrayBufferView is required');
            if (byteLength === 0) return 0;
            var srcLength = arrayBufferView.byteLength;
            if (byteLength == -1) byteLength = srcLength;
            if (byteLength < -1) throw new Error('Invalid byteLength');
            // get the .Net heap
            var instance = SpawnJSInterop.getInstace(dotnetId);
            var buffer = SpawnJSInterop.wasmMemoryBuffer(instance.dotnet);
            var bufferView = new Uint8Array(buffer, destOffset, byteLength);
            // get a view of the exact source we want
            var offset = arrayBufferView.byteOffset + srcOffset;
            var sourceView = new Uint8Array(arrayBufferView.buffer, offset, byteLength);
            // copy to the .Net heap
            bufferView.set(sourceView);
            // return the bytes copied
            return byteLength;
        }
        // called using Call<>
        static propertySetNewPromise(sjsId, key) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            var promise = SpawnJSInterop.newEasyPromise();
            parent[propertyName] = promise;
            return promise;
        }
        // called using Call<>
        static propertySetResolvedPromise(sjsId, key, value) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            parent[propertyName] = Promise.resolve(value);
        }
        // called using Call<>
        static propertySetRejectedPromise(sjsId, key, value) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            parent[propertyName] = Promise.reject(value);
        }
        // // called using Call<>
        // static propertySetPromiseThenCatch(sjsId, key, thenCallback, catchCallback) {
        //     var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
        //     if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
        //     var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
        //     if (shortCircuit) return;
        //     var promise = parent[propertyName];

        //     var promise = SpawnJSInterop.newEasyPromise();
        //     parent[propertyName] = promise;
        // }
        static propertySetCallback(sjsId, key, dotnetId, callbackId, once) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            if (!callbackId || !dotnetId) {
                return null;
            }
            // get callback id that is globally unique
            var callbackIdPair = `${dotnetId}_${callbackId}`;
            // check if it exists and createa new function if not
            var value = SpawnJSInterop._callbacks[callbackIdPair];
            if (!value) {
                value = function (...args) {
                    // check if the callback has been removed
                    if (!SpawnJSInterop._callbacks[callbackIdPair]) {
                        return;
                    }
                    // get the SpawnJSRuntime instance's method that is used to report the callback 
                    var { handleCallback } = SpawnJSInterop.getInstace(dotnetId);
                    // get argsCnt because after when we call handleCallback .Net will write
                    // the return value to at the end of the array after the last argument (index argsCnt)
                    // unless the return value should be undefined
                    var argsCnt = args.length;
                    // get a temporary hold of the args (released after we notify SpawnJSRuntime)
                    var argsId = SpawnJSInterop.spawnJSObjectHold(args);
                    // notify SpawnJSRuntime with the argsId and the cnt
                    handleCallback(callbackId, argsId, argsCnt);
                    // release the args
                    SpawnJSInterop.spawnJSObjectRelease(argsId);
                    // if it was a 1 time use callback, release it
                    if (once) delete SpawnJSInterop._callbacks[callbackIdPair];
                    // return what is in index argsCnt (the designated place .Net will write to if there is a return value)
                    return args[argsCnt];
                };
                SpawnJSInterop._callbacks[callbackIdPair] = value;
            }
            parent[propertyName] = value;
        }
        // set property to a HeapView
        static propertySetHeapView(sjsId, key, dotnetId, viewType, offset, length, copy) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            // create the heapView meta data
            var heapViewInfo = { dotnetId, viewType, offset, length };
            if (!heapViewInfo.viewType) heapViewInfo.viewType = 'Uint8Array';
            heapViewInfo.instance = SpawnJSInterop.getInstace(heapViewInfo.dotnetId);
            heapViewInfo.dotnet = SpawnJSInterop.spawnJSObjectGet(heapViewInfo.dotnetId);
            heapViewInfo.sizeHistory = [];
            heapViewInfo.ctor = SpawnJSInterop.getArrayBufferViewConstructor(heapViewInfo.viewType);
            // refresh the heapView
            var value = SpawnJSInterop.heapViewRefresh(heapViewInfo);
            // set to the proeprty
            parent[propertyName] = value;
        }
        static heapViewRefresh(heapViewInfo) {
            var needsRefresh = !heapViewInfo.buffer || heapViewInfo.buffer.detached;
            if (needsRefresh) {
                heapViewInfo.buffer = SpawnJSInterop.wasmMemoryBuffer(heapViewInfo.dotnet);
                var value = null;
                var length = heapViewInfo.length === -1 ? /* entire buffer */ heapViewInfo.buffer.byteLength - offset : heapViewInfo.length;
                if (heapViewInfo.viewType === 13) {
                    // ArrayBuffer requested
                    if (copy) {
                        // create a copy
                        value = heapViewInfo.buffer.slice(offset, offset + length);
                    } else {
                        // can't use offet and length when now copying
                        if (offset != 0) throw new Error('Offset and length not supported creating an ArrayBuffer heap view without a copy');
                        value = heapViewInfo.buffer;
                    }
                    parent[propertyName] = value;
                } else {
                    // ArrayBufferView reqeusted
                    value = new heapViewInfo.ctor(heapViewInfo.buffer, heapViewInfo.offset, length);
                    value = copy ? value.slice() : value;
                }
                value._heapViewInfo = heapViewInfo;
                heapViewInfo.bufferLength = heapViewInfo.buffer.byteLength;
                heapViewInfo.sizeHistory.push(heapViewInfo.bufferLength);
                heapViewInfo.value = value;
                console.log('Heapview refreshd:', heapViewInfo);
            }
            return heapViewInfo.value;
        }
        static getArrayBufferViewConstructor(viewType) {
            var ctor = SpawnJSInterop.HeapViewCtors[viewType];
            if (!ctor) throw new Error(`Unsupported or missing ArrayBufferView constructor for enum index: ${viewType}`);
            return ctor;
        }
        static getHeapSize(dotnetId) {
            var dotnet = SpawnJSInterop.spawnJSObjectGet(dotnetId);
            var buffer = SpawnJSInterop.wasmMemoryBuffer(dotnet);
            return buffer.byteLength;
        }
        static __replacerHeapView(key, value, directCall, reviverConfig) {
            if (directCall) {
                if (value && typeof value === 'object' && SpawnJSInterop._in('_heapViewInfo', value)) {
                    // .Net wants HeapViewDescriptor
                    //SpawnJSInterop.heapViewRefresh(heapViewInfo);
                }
            }
            return value;
        }
        static __reviverHeapView(key, value, directCall, reviverConfig) {
            if (!directCall && value && typeof value === 'object' && SpawnJSInterop._in('_heapViewInfo', value)) {
                // auto-reattach if needed
                // this allows creating a fresh HeapView if needed on `set` and `call` (with the exception of call reattach only working if the view is in teh root args list. no object walking is done)
                var heapViewInfo = value._heapViewInfo;
                value = SpawnJSInterop.heapViewRefresh(heapViewInfo);
            }
            return value;
        }
        // create a new Promsie with the resolve and reject methods attached to the promise for easy calling from .Net
        static newEasyPromise() {
            var _resolve = null;
            var _reject = null;
            var promise = new Promise((resolve, reject) => {
                _resolve = resolve;
                _reject = reject;
            });
            promise.resolve = _resolve;
            promise.reject = _reject;
            return promise
        }
        // creates a new Object, adds it to the hold and returns it
        static spawnJSObjectNewHeapView(dotnetId, offset, length, type) {
            var instance = SpawnJSInterop.getInstace(dotnetId);
            var dotnet = SpawnJSInterop.spawnJSObjectGet(dotnetId);
            var buffer = SpawnJSInterop.wasmMemoryBuffer(dotnet);
            value = new globalThis[mapInfo.type](buffer, offset, length);
            // mark it
            value.__reviverHeap = {
                dotnetId,
                offset,
                length,
                type,
                dotnet,
            };
            //
            return SpawnJSInterop.spawnJSObjectHold(value);
        }
        //
        static stringToBigInt(value) {
            if (!globalThis.BigInt) throw new Error('BigInt not supported on this platform');
            return value === undefined || value === null ? null : new globalThis.BigInt(value);
        }
        // set property to using a SpawnJSInterop[methodName](value) call
        // methodName can be a SpawnJSInterop methodName or a methodIndex
        // revivers are Javascript methods
        static propertySetWithReviver(sjsId, key, value, methodName, reviverConfig) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            var reviver = typeof methodName === 'string' ? SpawnJSInterop[methodName] : SpawnJSInterop._methodMap[methodName];
            // if SpawnJSInterop does not have the reviver, try the glboalThis. This allows global types to be used
            if (!reviver) reviver = globalThis[methodName];
            if (!reviver) throw new Error(`Reviver not found: ${methodName}`);
            value = reviver(propertyName, value, true, reviverConfig);
            parent[propertyName] = value;
        }
        // set property
        static propertySet(sjsId, key, value) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            // revivers
            value = SpawnJSInterop.reviveValue(propertyName, value, false);
            parent[propertyName] = value;
        }
        // set property null
        static propertySetNull(sjsId, key) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            parent[propertyName] = null;
        }
        // set property undefined
        static propertySetUndefined(sjsId, key) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return;
            parent[propertyName] = undefined;
        }
        // deletes property
        static propertyDelete(sjsId, key) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return true;
            return delete parent[propertyName];
        }
        // returns bool if the key is in the target
        static propertyIn(sjsId, key) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            if (obj === void 0 || obj === null) throw new Error('obj null or undefined');
            var { parent, propertyName, shortCircuit } = SpawnJSInterop.pathObjectInfo(obj, key);
            if (shortCircuit) return false;
            return SpawnJSInterop._in(propertyName, parent);
        }
        // property info
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
        // object constructor names
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
        static reviveValue(key, initialValue, directCall) {
            // Pipe the value through each reviver sequentially
            return SpawnJSInterop._revivers.reduce((currentValue, currentReviver) => {
                // Short-circuit: if a previous reviver dropped the value, skip the rest
                if (currentValue === undefined) return undefined;
                return currentReviver(key, currentValue, directCall); // the true tells the reviver this is a call revive as opposed to a propertySet revive
            }, initialValue);
        }
        static replaceValue(key, initialValue, directCall) {
            // Pipe the value through each reviver sequentially
            return SpawnJSInterop._replacers.reduce((currentValue, currentReplacer) => {
                // Short-circuit: if a previous reviver dropped the value, skip the rest
                if (currentValue === undefined) return undefined;
                return currentReplacer(key, currentValue, directCall); // the true tells the reviver this is a call revive as opposed to a propertySet revive
            }, initialValue);
        }
        // returns string
        static getTypeInfo(sjsId) {
            var obj = SpawnJSInterop.spawnJSObjectGet(sjsId);
            var jsClass = Object.prototype.toString.call(obj).split(' ')[1].slice(0, -1);
            var jsType = typeof (obj);
            return `${jsType} ${jsClass}`;
        }
        // returns the types the object inherits from
        // returns string[]
        static getPropertyConstructorNames(parent, key) {
            return SpawnJSInterop.getConstructorNames(parent[key]);
        }
        // Main .Net to JS entrypoint (synchronous).
        // The args array is fetched AND replaced with a fresh empty [] in the same slot, so the .Net side's
        // pooled argument-array reference (same id) comes back emptied and ready to reuse on the next call.
        static _spawnJSInteropCall(returnType, methodName, argsId, replacerId, replacerConfig) {
            var target = typeof methodName === 'string' ? SpawnJSInterop[methodName] : SpawnJSInterop._methodMap[methodName];
            var args = argsId === null || argsId === undefined ? null : SpawnJSInterop.spawnJSObjectGetAndReplace(argsId, []);
            // iterate _revivers to process args if needed
            if (args) {
                for (let i = 0; i < args.length; i++) {
                    args[i] = SpawnJSInterop.reviveValue(i, args[i], false);
                }
            }
            var ret = !args ? target() : target(...args);
            // run replacers
            var replacer = !replacerId ? null : typeof replacerId === 'string' ? SpawnJSInterop[replacerId] : SpawnJSInterop._methodMap[replacerId];
            if (replacer) ret = replacer(null, ret, true, replacerConfig);
            else ret = SpawnJSInterop.replaceValue(null, ret, false);
            // shape the result to match the .Net side's expected returnType
            ret = SpawnJSInterop._serializeToNet(returnType, ret);
            return ret;
        }
        // Main .Net to JS entrypoint
        static async _spawnJSInteropCallAsync(returnType, dotnetId, asyncCallId, methodName, argsId, replacerId, replacerConfig) {
            var target = typeof methodName === 'string' ? SpawnJSInterop[methodName] : SpawnJSInterop._methodMap[methodName];
            var instance = SpawnJSInterop.getInstace(dotnetId);
            var dotnet = SpawnJSInterop.spawnJSObjectGet(dotnetId);
            var args = argsId === null || argsId === undefined ? null : SpawnJSInterop.spawnJSObjectGetAndReplace(argsId, []);
            if (args) {
                for (let i = 0; i < args.length; i++) {
                    args[i] = SpawnJSInterop.reviveValue(i, args[i], false);
                }
            }
            var error = null;
            var ret = null;
            try {
                ret = !args ? target() : target(...args);
                ret = await ret;
                // run replacers
                var replacer = !replacerId ? null : typeof replacerId === 'string' ? SpawnJSInterop[replacerId] : SpawnJSInterop._methodMap[replacerId];
                if (replacer) ret = replacer(null, ret, true, replacerConfig);
                else ret = SpawnJSInterop.replaceValue(null, ret, false);
                // prepare using returnType
                ret = SpawnJSInterop._serializeToNet(returnType, ret);
            } catch (ex) {
                // call the exceptionCallbackId
                error = SpawnJSInterop.errorToString(ex);
            }
            switch (returnType) {
                case 0: // void
                    instance.resolveVoid(asyncCallId, error);
                    break;
                case 1: // Double
                    instance.resolveDouble(asyncCallId, ret, error);
                    break;
                case 2: // Boolean
                    instance.resolveBoolean(asyncCallId, ret, error);
                    break;
                case 3: // DoubleNullable
                    instance.resolveDoubleNullable(asyncCallId, ret, error);
                    break;
                case 4: // BooleanNullable
                    instance.resolveBooleanNullable(asyncCallId, ret, error);
                    break;
                case 5: // String
                    instance.resolveString(asyncCallId, ret, error);
                    break;
                case 6: // SpawnJSObject
                    instance.resolveDouble(asyncCallId, ret, error);
                    break;
                case 7: // SpawnJSObjectNonNullable
                    instance.resolveDouble(asyncCallId, ret, error);
                    break;
                case 8: // Json
                    instance.resolveString(asyncCallId, ret, error);
                    break;
                case 9: // Int32
                    instance.resolveInt32(asyncCallId, ret, error);
                    break;
                case 10: // Int32Nullable
                    instance.resolveInt32Nullable(asyncCallId, ret, error);
                    break;
                default:
                    throw new Error(`Unsupported returnType ${returnType}`);
                    break;
            }
        }
        // converts to an error string or returns a generic error string
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
                case 0:  // void
                    return;
                case 6:  // SpawnJSObject - Number
                case 7:  // SpawnJSObjectNonNullable - Number
                    return SpawnJSInterop.spawnJSObjectHold(ret);
                case 8:  // Json
                    return JSON.stringify(ret);
                case 1:  // Double
                case 2:  // Boolean
                case 3:  // DoubleNullable
                case 4:  // BooleanNullable
                case 9:  // Int32
                case 10: // Int32Nullable
                    return ret;
                case 5:  // String
                    if (ret && typeof ret !== 'string') {
                        ret = Object(ret).toString();
                    }
                    return ret;
            }
            // the default is to return as is
            return ret;
        }
        static wasmMemoryBuffer(dotnet) {
            var found = SpawnJSInterop.#findWasmMemory(dotnet);
            if (!found) throw new Error('SpawnJSInterop: could not reach the WebAssembly memory buffer');
            return found.buffer;
        }
        // returns the name of the path the memory buffer was found under, or '' if it was not found
        static wasmMemoryBufferSource(dotnet) {
            var found = SpawnJSInterop.#findWasmMemory(dotnet);
            return found ? found.source : '';
        }
        static #findWasmMemory(dotnet) {
            var rt = dotnet;
            if (!rt) return null;
            var candidates = [
                ['Module.HEAPU8.buffer', () => rt.Module?.HEAPU8?.buffer],
                ['Module.wasmMemory.buffer', () => rt.Module?.wasmMemory?.buffer],
                ['localHeapViewU8().buffer', () => rt.localHeapViewU8?.()?.buffer],
                ['getHeapU8().buffer', () => rt.getHeapU8?.()?.buffer],
            ];
            for (var i = 0; i < candidates.length; i++) {
                var buffer;
                try { buffer = candidates[i][1](); } catch (ex) { continue; }
                if (buffer && typeof buffer.byteLength === 'number' && buffer.byteLength > 0) {
                    return { buffer: buffer, source: candidates[i][0] };
                }
            }
            return null;
        }
        // safely checks for a property existence
        // safely means will not throw, which is needed as simply checking for a
        // a proeprty can throw an exception (notably on cross-origin windows)
        static _in(key, obj) {
            if (obj === null || obj === void 0) return false;
            try {
                return key in Object(obj);
            } catch { }
            return false;
        }
        // returns the path info based on a base object and a property path: `window?.location.href`
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
})();