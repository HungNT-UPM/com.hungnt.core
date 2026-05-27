# HungNT Core (`com.hungnt.core`)

Foundation package cho các module HungNT khác: tiện ích runtime, singleton, logging và **Service Locator**.

## Tính năng

| Namespace | Nội dung |
|-----------|----------|
| **`HungNT.Core`** | `DebugEx`, `MonoSingleton` / `MonoSingletonScene`, extension helpers (`ComponentExtensions`, string `Color` / `Bold`, …), contract **`IService`** |
| **`HungNT`** | **`ServiceLocator`**, **`ServiceRegister`**, extension **`GetService` / `RegisterService` / `TryGetService`** trên `MonoBehaviour` |

Inspector của locator/register dùng **Odin attributes** — cần cài Odin riêng trong project.

## Phụ thuộc

| Package | Ghi chú |
|---------|---------|
| **`com.cysharp.unitask`** | Kéo tự động qua OpenUPM khi cài `core` |
| **Odin Inspector** | Git mirror `com.hungnt.odininspector` — không có trên OpenUPM |
| **DOTween** | Asset Store / Git mirror (nếu project dùng tween) |
| **Rainbow Folders** | Tùy chọn, editor — Git mirror `com.hungnt.rainbowfolder` |

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
