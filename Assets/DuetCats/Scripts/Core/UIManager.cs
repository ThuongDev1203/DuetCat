using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

namespace DuetCats.Scripts.Core
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [Header("UI References")]
        public TextMeshProUGUI scoreText;
        public Image icon_heart_fill_1;
        public Image icon_heart_fill_2;
        public Image icon_heart_empty_1;
        public Image icon_heart_empty_2;

        void Awake()
        {
            Instance = this;
        }

        public void UpdateScore(int score)
        {
            if (scoreText != null)
                scoreText.text = score.ToString();
        }

        public void UpdateLives(int lives)
        {
            if (icon_heart_fill_1 != null)
                icon_heart_fill_1.gameObject.SetActive(lives > 0);

            if (icon_heart_fill_2 != null)
                icon_heart_fill_2.gameObject.SetActive(lives > 1);

            if (icon_heart_empty_1 != null)
                icon_heart_empty_1.gameObject.SetActive(lives == 0);

            if (icon_heart_empty_2 != null)
                icon_heart_empty_2.gameObject.SetActive(lives < 2);
        }
    }
}