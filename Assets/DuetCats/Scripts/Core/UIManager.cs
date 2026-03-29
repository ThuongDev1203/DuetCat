using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace DuetCats.Scripts.Core
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [Header("Score & Lives UI")]
        public TextMeshProUGUI scoreText;
        public Image icon_heart_fill_1;
        public Image icon_heart_fill_2;
        public Image icon_heart_empty_1;
        public Image icon_heart_empty_2;

        [Header("Panel References")]
        public CanvasGroup tutorialGroup;
        public CanvasGroup startButtonGroup;
        public CanvasGroup gameUIGroup;

        [Header("GameUI")]
        public GameObject gameUI;


        bool isTutorialShowing = false;
        bool canClickTutorial = false;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if (!isTutorialShowing) return;
            if (!canClickTutorial) return;

            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                StartFromTutorial();
            }
        }

        public void UpdateScore(int score)
        {
            if (scoreText != null)
                scoreText.text = score.ToString();
        }

        public void UpdateLives(int lives)
        {
            icon_heart_fill_1.gameObject.SetActive(lives >= 1);
            icon_heart_fill_2.gameObject.SetActive(lives >= 2);

            icon_heart_empty_1.gameObject.SetActive(lives < 1);
            icon_heart_empty_2.gameObject.SetActive(lives < 2);
        }

        public void OnClickStart()
        {
            gameUI.SetActive(true);

            ShowCanvasGroup(tutorialGroup, true);
            ShowCanvasGroup(startButtonGroup, false);

            isTutorialShowing = true;

            StartCoroutine(DelayEnableClick());
        }

        IEnumerator DelayEnableClick()
        {
            yield return null;

            canClickTutorial = true;
        }

        void StartFromTutorial()
        {
            isTutorialShowing = false;

            ShowCanvasGroup(tutorialGroup, false);
            ShowCanvasGroup(gameUIGroup, true);

            GameManager.Instance.StartGame();
            InputController.Instance.StartAfterTutorial();
        }

        public void ShowStartUI()
        {
            ShowCanvasGroup(startButtonGroup, true);
            ShowCanvasGroup(tutorialGroup, false);

            if (gameUI != null)
                gameUI.SetActive(false);
        }


        void ShowCanvasGroup(CanvasGroup cg, bool show)
        {
            cg.alpha = show ? 1 : 0;
            cg.interactable = show;
            cg.blocksRaycasts = show;
        }
    }
}