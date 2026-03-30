using UnityEngine;

namespace DuetCats.Scripts.UI
{
    public class CanvasFixFlip : MonoBehaviour
    {
        Vector3 originalScale;

        void Start()
        {
            originalScale = transform.localScale;
        }

        void LateUpdate()
        {
            Transform parent = transform.parent;
            if (parent == null) return;

            Vector3 parentScale = parent.localScale;

            Vector3 newScale = originalScale;

            newScale.x = parentScale.x < 0 ? -originalScale.x : originalScale.x;

            transform.localScale = newScale;
        }
    }
}