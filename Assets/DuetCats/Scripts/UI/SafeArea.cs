using UnityEngine;

namespace DuetCats.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        private RectTransform panel;
        private Rect lastSafeArea = new Rect(0, 0, 0, 0);
        private Vector2Int lastScreenSize = Vector2Int.zero;

        void Awake()
        {
            panel = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        void Update()
        {
            if (Screen.safeArea != lastSafeArea ||
                Screen.width != lastScreenSize.x ||
                Screen.height != lastScreenSize.y)
            {
                ApplySafeArea();
            }
        }

        void ApplySafeArea()
        {
            lastSafeArea = Screen.safeArea;
            lastScreenSize = new Vector2Int(Screen.width, Screen.height);

            Vector2 anchorMin = lastSafeArea.position;
            Vector2 anchorMax = lastSafeArea.position + lastSafeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;

            if (Screen.width > Screen.height)
            {
                panel.localScale = Vector3.one * 0.85f;
            }
            else
            {
                panel.localScale = Vector3.one;
            }
        }
    }
}