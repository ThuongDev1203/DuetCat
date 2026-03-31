using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace DuetCats.Scripts.Core
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        // SCORE
        [Header("Score Texts")]
        public TextMeshProUGUI[] scoreTexts;

        // =========================
        // LIVES (FIX PORTRAIT + LANDSCAPE)
        // =========================
        [Header("Portrait Lives")]
        public Image[] portraitHeartFills;
        public Image[] portraitHeartEmpties;

        [Header("Landscape Lives")]
        public Image[] landscapeHeartFills;
        public Image[] landscapeHeartEmpties;

        // PANELS
        [Header("Panels")]
        public CanvasGroup[] tutorialGroups;
        public CanvasGroup[] startButtonGroups;
        public CanvasGroup[] gameUIGroups;

        [Header("Game Root")]
        public GameObject gameUI;

        // STATE
        bool isTutorialShowing = false;
        bool canClickTutorial = false;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            ShowStartUI();
        }

        void Update()
        {
            if (!isTutorialShowing || !canClickTutorial) return;

            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                StartFromTutorial();
            }
        }

        // =========================
        // SCORE
        // =========================
        public void UpdateScore(int score)
        {
            for (int i = 0; i < scoreTexts.Length; i++)
            {
                if (scoreTexts[i] != null)
                    scoreTexts[i].text = score.ToString();
            }
        }

        // =========================
        // LIVES (FIXED)
        // =========================
        public void UpdateLives(int lives)
        {
            UpdateLivesSet(portraitHeartFills, portraitHeartEmpties, lives);
            UpdateLivesSet(landscapeHeartFills, landscapeHeartEmpties, lives);
        }

        void UpdateLivesSet(Image[] fills, Image[] empties, int lives)
        {
            if (fills == null || empties == null) return;

            int count = Mathf.Min(fills.Length, empties.Length);

            for (int i = 0; i < count; i++)
            {
                bool isAlive = i < lives;

                if (fills[i] != null)
                    fills[i].gameObject.SetActive(isAlive);

                if (empties[i] != null)
                    empties[i].gameObject.SetActive(!isAlive);
            }
        }

        // =========================
        // FLOW
        // =========================
        public void OnClickStart()
        {
            if (gameUI != null)
                gameUI.SetActive(true);

            ShowGroups(startButtonGroups, false);
            ShowGroups(tutorialGroups, true);
            ShowGroups(gameUIGroups, false);

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

            ShowGroups(tutorialGroups, false);
            ShowGroups(gameUIGroups, true);

            GameManager.Instance.StartGame();
            InputController.Instance.StartAfterTutorial();
        }

        public void ShowStartUI()
        {
            ShowGroups(startButtonGroups, true);
            ShowGroups(tutorialGroups, false);
            ShowGroups(gameUIGroups, false);

            if (gameUI != null)
                gameUI.SetActive(false);

            isTutorialShowing = false;
            canClickTutorial = false;
        }

        // =========================
        // HELPER
        // =========================
        void ShowGroups(CanvasGroup[] groups, bool show)
        {
            if (groups == null) return;

            for (int i = 0; i < groups.Length; i++)
            {
                var cg = groups[i];
                if (cg == null) continue;

                cg.alpha = show ? 1 : 0;
                cg.interactable = show;
                cg.blocksRaycasts = show;
            }
        }
    }
}