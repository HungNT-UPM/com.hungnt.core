# HungNT Core (`com.hungnt.core`)

Foundation package cho các module HungNT khác: tiện ích runtime, singleton, logging, **Service Locator**, và editor tooling dùng chung.

## Tính năng

| Namespace | Nội dung |
|-----------|----------|
| **`HungNT.Core`** | `DebugEx`, `MonoSingleton` / `MonoSingletonScene`, extension helpers (`ComponentExtensions`, string `Color` / `Bold`, …), contract **`IService`** |
| **`HungNT`** | **`ServiceLocator`**, **`ServiceRegister`**, extension **`GetService` / `RegisterService` / `TryGetService`** trên `MonoBehaviour` |
| **`HungNT.Core.Editor`** | Scene Editor Window, Open First Scene, Package Manager (`Assets/BaseHungNT/Editor/PackageCatalogData.asset`), data menu, copy folder path |

**Odin Inspector** cần có trong project (không khai báo UPM dependency).

### Package catalog

Danh sách Git URL packages lưu tại **`Assets/BaseHungNT/Editor/PackageCatalogData.asset`** (ScriptableObject theo project). Mở **`HungNT/Package Manager`** → Reload sync từ `Packages/manifest.json`, tick/untick + Apply để cài/gỡ.

## Demo

Assembly **`HungNT.Core.Demo`** — script `Demo/ServiceLocatorDemo.cs`:

1. Scene có `ServiceLocator` + `ServiceRegister` và service đã đăng ký (vd. `IAdsService`).
2. Gắn `ServiceLocatorDemo` lên GameObject.
3. Play Mode → bấm các nút Odin trên Inspector (`ShowBanner`, `ShowInterstitial`, `TryGetAdsService`, …).

```csharp
var ads = this.GetService<IAdsService>();

if (this.TryGetService<IAdsService>(out var ads2))
{
    ads2.ShowBanner();
}

ServiceLocator.Instance.Register<IAdsService>(myAdsService);
```
