using UnityEngine;
using System.Collections;
using DuetCats.Scripts.Gameplay;

namespace DuetCats.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public NoteSpawner spawner;

        public bool IsPlaying { get; private set; } = false;
        public bool IsGameOver { get; private set; } = false;

        public int Score { get; private set; } = 0;
        public int Lives { get; private set; } = 2;
        public AudioManager audioManager;

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
            Lives = 2;

            UIManager.Instance.UpdateScore(Score);
            UIManager.Instance.UpdateLives(Lives);

            Debug.Log("GAME START");

            if (audioManager != null)
                audioManager.Play();
        }

        public void StopGame()
        {
            IsPlaying = false;
            IsGameOver = true;

            if (audioManager != null)
                audioManager.audioSource.Stop();

            InputController.Instance?.PlayLoseAnimation();
            StartCoroutine(DelayReset());
        }

        IEnumerator DelayReset()
        {
            yield return new WaitForSeconds(1.5f);

            UIManager.Instance.ShowStartUI();

            audioManager.ResetTime();

            if (spawner != null)
                spawner.ResetSpawner();

            ResetGameState();
        }

        void ResetGameState()
        {
            Score = 0;
            Lives = 2;

            UIManager.Instance.UpdateScore(Score);
            UIManager.Instance.UpdateLives(Lives);

            InputController.Instance.ResetInput();
        }

        //SCORE LIVES
        public void AddScore(int amount)
        {
            Score += amount;
            UIManager.Instance.UpdateScore(Score);
        }

        public void WinGame()
        {
            Debug.Log("WIN GAME CALLED");
            IsPlaying = false;
            StartCoroutine(WinDelay());
        }

        IEnumerator WinDelay()
        {
            yield return null;
            IsGameOver = true;
            audioManager.Stop();
            InputController.Instance.PlayWinAnimation();
        }

        public void LoseLife()
        {
            Lives--;
            if (Lives < 0) Lives = 0;

            Debug.Log("LoseLife → Lives = " + Lives);

            UIManager.Instance.UpdateLives(Lives);

            if (Lives == 0)
            {
                StopGame();
                Debug.Log("GAME OVER");
            }
        }
    }
}