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

        // LIVES (OPTIMIZED)
        [Header("Lives")]
        public Image[] heartFills;
        public Image[] heartEmpties;

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

        //================ SCORE =================
        public void UpdateScore(int score)
        {
            for (int i = 0; i < scoreTexts.Length; i++)
            {
                if (scoreTexts[i] != null)
                    scoreTexts[i].text = score.ToString();
            }
        }

        //================ LIVES =================
        public void UpdateLives(int lives)
        {
            int count = Mathf.Min(heartFills.Length, heartEmpties.Length);

            for (int i = 0; i < count; i++)
            {
                bool isAlive = i < lives;

                if (heartFills[i] != null)
                    heartFills[i].gameObject.SetActive(isAlive);

                if (heartEmpties[i] != null)
                    heartEmpties[i].gameObject.SetActive(!isAlive);
            }
        }

        //================ FLOW =================
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

        //================ HELPER =================
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