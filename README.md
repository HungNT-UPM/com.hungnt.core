# HungNT Core (`com.hungnt.core`)

Foundation package cho các module HungNT khác: tiện ích runtime, singleton, logging và **Service Locator**.

## Tính năng

| Namespace | Nội dung |
|-----------|----------|
| **`HungNT.Core`** | `DebugEx`, `MonoSingleton` / `MonoSingletonScene`, extension helpers (`ComponentExtensions`, string `Color` / `Bold`, …), contract **`IService`** |
| **`HungNT`** | **`ServiceLocator`**, **`ServiceRegister`**, extension **`GetService` / `RegisterService` / `TryGetService`** trên `MonoBehaviour` |

Inspector của locator/register dùng **Odin attributes** — cần cài Odin riêng (xem [Phụ thuộc bắt buộc](#phụ-thuộc-bắt-buộc)).

## Cài đặt

### OpenUPM (khuyên dùng)

Thêm scoped registry và dependency vào `Packages/manifest.json`:

```json
"scopedRegistries": [
  {
    "name": "OpenUPM",
    "url": "https://package.openupm.com",
    "scopes": ["com.hungnt", "com.cysharp"]
  }
],
"dependencies": {
  "com.hungnt.core": "1.0.3"
}
```

Hoặc dùng CLI:

```bash
openupm add com.hungnt.core@1.0.3
```

`com.cysharp.unitask` được kéo tự động qua OpenUPM.

### GitHub (manifest.json)

```json
"com.hungnt.core": "https://github.com/HungNT-UPM/com.hungnt.core.git#v1.0.3"
```

## Phụ thuộc bắt buộc / bên ngoài (không có trên OpenUPM)

Các package sau cài **thủ công** vào `Packages/manifest.json` của project game:

**Odin Inspector** (ServiceLocator / ServiceRegister inspector, Database, Datasave, EventDispatcher editor):

```json
"com.hungnt.odininspector": "https://github.com/HungNT-UPM/com.hungnt.odininspector.git#v3.1.14+3"
```

**DOTween** (nếu project dùng tween — cài từ Asset Store hoặc Git mirror của bạn):

```json
"com.demigiant.dotween": "https://github.com/HungNT-UPM/HungNT-DOTween.git#v1.2.765"
```

*(Thay URL/tag theo repo DOTween bạn đang host.)*

**Rainbow Folders** (tùy chọn, editor):

```json
"com.hungnt.rainbowfolder": "https://github.com/HungNT-UPM/com.hungnt.rainbowfolder.git#v2.4.2"
```

**UniTask** — tự resolve qua OpenUPM (`com.cysharp.unitask`) khi cài `core`.

## Demo

Assembly **`HungNT.Core.Demo`** — script `Demo/ServiceLocatorDemo.cs`:

1. Scene có `ServiceLocator` + `ServiceRegister` và service đã đăng ký (vd. `IAdsService`).
2. Gắn `ServiceLocatorDemo` lên GameObject.
3. Play Mode → bấm các nút Odin trên Inspector (`ShowBanner`, `ShowInterstitial`, `TryGetAdsService`, …).

```csharp
// Lấy service từ bất kỳ MonoBehaviour nào
var ads = this.GetService<IAdsService>();

if (this.TryGetService<IAdsService>(out var ads2))
{
    ads2.ShowBanner();
}
```

Đăng ký service (thường trong `Awake` của bootstrap hoặc qua `ServiceRegister`):

```csharp
ServiceLocator.Instance.Register<IAdsService>(myAdsService);
```
