using Sirenix.OdinInspector;
using UnityEngine;
using HungNT.Advertisement;

namespace HungNT.Demo
{
    /// <summary>
    /// Demo: Cách sử dụng ServiceLocator.
    /// Gán vào một GameObject và nhấn các nút trong Inspector (Play Mode).
    /// </summary>
    public class ServiceLocatorDemo : MonoBehaviour
    {
        // ── Setup: Register services ─────────────────────────────────────────

        // [SerializeField]
        // public IAdsService AdsServiceImpl;
        //
        // private void Awake()
        // {
        //     // Đăng ký Null implementations làm fallback
        //     // Trong production thay bằng implementation thật (AdMobAdsService, FirebaseTrackingService, ...)
        //     ServiceLocator.Instance.Register<IAdsService>(new NullAdsService());
        //     ServiceLocator.Instance.Register<ILocalizationService>(new NullLocalizationService());
        //     ServiceLocator.Instance.Register<ITrackingService>(new NullTrackingService());
        // }
        //
        // private void OnDestroy()
        // {
        //     ServiceLocator.Instance.Unregister<IAdsService>();
        //     ServiceLocator.Instance.Unregister<ILocalizationService>();
        //     ServiceLocator.Instance.Unregister<ITrackingService>();
        // }

        // ── Ads ──────────────────────────────────────────────────────────────

        [Button]
        public void ShowBanner()
        {
            var ads = this.GetService<IAdsService>();
            ads.ShowBanner();
            Debug.Log($"[Demo] ShowBanner called via {ads.GetType().Name}");
        }

        [Button]
        public void ShowInterstitial()
        {
            var ads = this.GetService<IAdsService>();
            ads.ShowInterstitial(
                placement: AdsPlacement.DEFAULT,
                onSuccess: () => Debug.Log("[Demo] Interstitial shown successfully."),
                onFailure: () => Debug.Log("[Demo] Interstitial failed or skipped.")
            );
        }

        [Button]
        public void ShowRewarded()
        {
            var ads = this.GetService<IAdsService>();
            ads.ShowRewarded(
                placement: AdsPlacement.DOUBLE_COIN,
                onSuccess: () => Debug.Log("[Demo] Rewarded — user earned reward!"),
                onFailure: () => Debug.Log("[Demo] Rewarded — failed or closed early.")
            );
        }

        [Button]
        public void TryGetAdsService()
        {
            if (this.TryGetService<IAdsService>(out var ads))
            {
                Debug.Log($"[Demo] TryGet success: {ads.GetType().Name}");
            }
            else
            {
                Debug.LogWarning("[Demo] TryGet failed: IAdsService not registered.");
            }
        }
    }
}