using UnityEngine;

namespace HungNT
{
    public class OnlyActiveOnDebug : MonoBehaviour
    {
        protected void OnEnable()
        {
#if !DEBUG
            gameObject.SetActive(false);
#endif
        }
    }
}