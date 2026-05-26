# com.hungnt.core

- **`HungNT.Core`**: `DebugEx`, `MonoSingleton` / `MonoSingletonScene`, extension helpers (`ComponentExtensions`, string `Color` / `Bold`, …), and the **`IService`** contract.
- **`HungNT`**: **`ServiceLocator`**, **`ServiceRegister`**, and **`GetService` / `RegisterService`** extensions on `MonoBehaviour` (depends on types in `HungNT.Core`; requires **Odin** attributes on the locator/register in the inspector).

Optional sample: `Demo/ServiceLocatorDemo.cs` in assembly **`HungNT.Core.Demo`** (`HungNT.Demo` namespace), referencing **Advertisement** for `IAdsService` demos.

## Install (OpenUPM)

Add scoped registry + dependency (see [OpenUPM](https://openupm.com/packages/com.hungnt.core/)).

## Required packages (not on OpenUPM)

**Odin Inspector** is not redistributed via OpenUPM. Add to the **project** `Packages/manifest.json`:

```json
"com.hungnt.odininspector": "https://github.com/HungNT-UPM/com.hungnt.odininspector.git#v3.1.14+2"
```

Optional **Rainbow Folders** (editor only):

```json
"com.hungnt.rainbowfolder": "https://github.com/HungNT-UPM/com.hungnt.rainbowfolder.git#v2.4.1"
```

**UniTask** resolves automatically via OpenUPM (`com.cysharp.unitask`) when you use the OpenUPM scoped registry with scope `com.cysharp`.

Publishing: see [UPM Publishing](../../Docs/UPM_PUBLISHING.md).
