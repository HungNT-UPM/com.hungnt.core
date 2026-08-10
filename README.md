# com.hungnt.core

Nền tảng cho mọi package `com.hungnt.*`: hạ tầng Dependency Injection, tiện ích runtime, logging và editor tooling dùng chung.

## Yêu cầu

**VContainer là dependency bắt buộc** nhưng không khai trong `package.json` vì nó phân phối qua Git URL, không nằm trên registry nào mà UPM tự resolve được. Cài thủ công vào `Packages/manifest.json` trước khi thêm bất kỳ package `com.hungnt.*` nào:

```json
"jp.hadashikick.vcontainer": "https://github.com/hadashiA/VContainer.git?path=VContainer/Assets/VContainer"
```

Thiếu nó thì các package sẽ lỗi compile ngay.

**Odin Inspector** cũng cần có sẵn trong project (không khai UPM dependency).

## Nội dung

| Namespace | Nội dung |
|-----------|----------|
| **`HungNT`** | `IAppLifecycleService` + `AppLifecycleService`, `NullAppLifecycleService`, `CoreInstaller` |
| **`HungNT.Core`** | `DebugEx`, `MonoSingleton` / `MonoSingletonScene`, extension helpers (`ComponentExtensions`, string `Color` / `Bold`, …) |
| **`HungNT.Core.Editor`** | Scene Editor Window, Open First Scene, Package Manager, data menu, copy folder path |

## Cài đặt vào container

Gọi ở `Configure` của LifetimeScope gốc, trước các package khác:

```csharp
builder.InstallCore();
```

## IAppLifecycleService

VContainer chỉ cấp `Start` / `Tick` / `Dispose`. Những message vòng đời còn lại của Unity đi qua interface này, nhờ đó service không phải trở thành MonoBehaviour chỉ để nghe pause hay quit:

```csharp
public class SaveOnPause : IDisposable
{
    private readonly IAppLifecycleService _appLifecycle;

    public SaveOnPause(IAppLifecycleService appLifecycle)
    {
        _appLifecycle = appLifecycle;
        _appLifecycle.OnPaused += HandlePaused;
    }

    public void Dispose() => _appLifecycle.OnPaused -= HandlePaused;

    private void HandlePaused(bool paused) { }
}
```

`OnPaused` là hook lưu dữ liệu tin cậy nhất trên mobile — `OnQuitting` không được đảm bảo gọi khi OS kill process.

`AppLifecycleService` là MonoBehaviour duy nhất của base code; `CoreInstaller` tự tạo GameObject cho nó và giữ xuyên scene.

Ngoài container (công cụ Editor, EditMode test) dùng `NullAppLifecycleService` — không phát sự kiện nào.

## Nhận service ở MonoBehaviour

Đánh `[Inject]` thẳng lên field, không cần hàm trung gian:

```csharp
public class HealthBar : MonoBehaviour
{
    [Inject] private IEventBusService _eventBus;
}
```

Component phải được đăng ký ở scope thì container mới tiêm được:

```csharp
builder.RegisterComponentInHierarchy<HealthBar>();
```

## Package catalog

Danh sách Git URL packages lưu tại `Assets/BaseHungNT/Editor/PackageCatalogData.asset` (ScriptableObject theo project). Mở **`HungNT/Package Manager`** → Reload sync từ `Packages/manifest.json`, tick/untick + Apply để cài/gỡ.
