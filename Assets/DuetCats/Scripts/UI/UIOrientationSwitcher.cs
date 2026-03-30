using UnityEngine;

namespace DuetCats.Scripts.UI
{
    public class UIOrientationSwitcher : MonoBehaviour
    {
        public GameObject portraitUI;
        public GameObject landscapeUI;

        void Start()
        {
            UpdateUI();
        }

        void Update()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            bool isLandscape = Screen.width > Screen.height;

            portraitUI.SetActive(!isLandscape);
            landscapeUI.SetActive(isLandscape);
        }
    }
}