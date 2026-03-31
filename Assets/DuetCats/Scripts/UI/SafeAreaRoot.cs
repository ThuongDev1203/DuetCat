using UnityEngine;

namespace DuetCats.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaRoot : MonoBehaviour
    {
        RectTransform rectTransform;
        Rect lastSafeArea = new Rect(0, 0, 0, 0);

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        void Update()
        {
            if (Screen.safeArea != lastSafeArea)
            {
                ApplySafeArea();
            }
        }

        void ApplySafeArea()
        {
            Rect safe = Screen.safeArea;
            lastSafeArea = safe;

            Vector2 anchorMin = safe.position;
            Vector2 anchorMax = safe.position + safe.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;

            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}