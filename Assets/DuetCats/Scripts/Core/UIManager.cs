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
        [Header("Score Texts (Portrait + Landscape)")]
        public TextMeshProUGUI[] scoreTexts;

        // LIVES
        [Header("Lives Icons (Portrait + Landscape)")]
        public Image[] heartFill1;
        public Image[] heartFill2;
        public Image[] heartEmpty1;
        public Image[] heartEmpty2;

        // PANELS
        [Header("Panels (Portrait + Landscape)")]
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
            if (!isTutorialShowing) return;
            if (!canClickTutorial) return;

            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                StartFromTutorial();
            }
        }

        // SCORE
        public void UpdateScore(int score)
        {
            foreach (var txt in scoreTexts)
            {
                if (txt != null)
                    txt.text = score.ToString();
            }
        }

        // LIVES (SAFE)
        public void UpdateLives(int lives)
        {
            int count = Mathf.Min(
                heartFill1.Length,
                heartFill2.Length,
                heartEmpty1.Length,
                heartEmpty2.Length
            );

            for (int i = 0; i < count; i++)
            {
                if (heartFill1[i] != null)
                    heartFill1[i].gameObject.SetActive(lives >= 1);

                if (heartFill2[i] != null)
                    heartFill2[i].gameObject.SetActive(lives >= 2);

                if (heartEmpty1[i] != null)
                    heartEmpty1[i].gameObject.SetActive(lives < 1);

                if (heartEmpty2[i] != null)
                    heartEmpty2[i].gameObject.SetActive(lives < 2);
            }
        }

        // FLOW
        //Click Start
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

        //Click screen after tutorial
        void StartFromTutorial()
        {
            isTutorialShowing = false;

            ShowGroups(tutorialGroups, false);
            ShowGroups(gameUIGroups, true);

            GameManager.Instance.StartGame();
            InputController.Instance.StartAfterTutorial();
        }

        //Reset UI (Game Over)
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

        // HELPER (SAFE)
        void ShowGroups(CanvasGroup[] groups, bool show)
        {
            if (groups == null) return;

            foreach (var cg in groups)
            {
                if (cg == null) continue;

                cg.alpha = show ? 1 : 0;
                cg.interactable = show;
                cg.blocksRaycasts = show;
            }
        }
    }
}