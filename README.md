# com.hungnt.core

- **`HungNT.Core`**: `DebugEx`, `MonoSingleton` / `MonoSingletonScene`, extension helpers (`ComponentExtensions`, string `Color` / `Bold`, …), and the **`IService`** contract.
- **`HungNT`**: **`ServiceLocator`**, **`ServiceRegister`**, and **`GetService` / `RegisterService`** extensions on `MonoBehaviour` (depends on types in `HungNT.Core`; requires **Odin** attributes on the locator/register in the inspector).

Optional sample: `Demo/ServiceLocatorDemo.cs` in assembly **`HungNT.Core.Demo`** (`HungNT.Demo` namespace), referencing **Advertisement** for `IAdsService` demos.

Publishing: see [UPM Publishing](../../Docs/UPM_PUBLISHING.md).
