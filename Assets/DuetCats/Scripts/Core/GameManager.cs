using UnityEngine;
using System.Collections;
using DuetCats.Scripts.Gameplay;

namespace DuetCats.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public NoteSpawner spawner;
        public AudioManager audioManager;

        public bool IsPlaying { get; private set; }
        public bool IsGameOver { get; private set; }

        public int Score { get; private set; }
        public int Lives { get; private set; }

        const int MAX_LIVES = 2;

        void Awake()
        {
            Instance = this;
        }

        public void StartGame()
        {
            if (IsPlaying) return;

            IsPlaying = true;
            IsGameOver = false;

            Score = 0;
            Lives = MAX_LIVES;

            UIManager.Instance.UpdateScore(Score);
            UIManager.Instance.UpdateLives(Lives);

            audioManager?.Play();
        }

        public void StopGame()
        {
            if (IsGameOver) return;

            IsPlaying = false;
            IsGameOver = true;

            audioManager?.audioSource.Stop();

            InputController.Instance?.PlayLoseAnimation();
            StartCoroutine(DelayReset());
        }

        IEnumerator DelayReset()
        {
            yield return new WaitForSeconds(1.5f);

            UIManager.Instance.ShowStartUI();

            audioManager?.ResetTime();
            spawner?.ResetSpawner();

            ResetGameState();
        }

        void ResetGameState()
        {
            Score = 0;
            Lives = MAX_LIVES;

            UIManager.Instance.UpdateScore(Score);
            UIManager.Instance.UpdateLives(Lives);

            InputController.Instance.ResetInput();
        }

        //================ SCORE =================
        public void AddScore(int amount)
        {
            Score += amount;
            UIManager.Instance.UpdateScore(Score);
        }

        //================ LIFE =================
        public void LoseLife()
        {
            if (IsGameOver) return;

            Lives = Mathf.Max(0, Lives - 1);

            UIManager.Instance.UpdateLives(Lives);

            if (Lives == 0)
                StopGame();
        }

        //================ WIN =================
        public void WinGame()
        {
            if (IsGameOver) return;

            IsPlaying = false;
            StartCoroutine(WinDelay());
        }

        IEnumerator WinDelay()
        {
            yield return null;

            IsGameOver = true;
            audioManager?.Stop();

            InputController.Instance.PlayWinAnimation();
            StartCoroutine(DelayReset());
        }
    }
}