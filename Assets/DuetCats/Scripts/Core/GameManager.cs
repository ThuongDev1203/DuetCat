using UnityEngine;

namespace DuetCats.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public bool IsPlaying { get; private set; } = false;

        public int Score { get; private set; } = 0;
        public int Lives { get; private set; } = 2; // tối đa 2 mạng

        public AudioManager audioManager;

        void Awake()
        {
            Instance = this;
        }

        public void StartGame()
        {
            if (IsPlaying) return;

            IsPlaying = true;
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
        }

        // ======================= SCORE & LIVES =======================
        public void AddScore(int amount)
        {
            Score += amount;
            UIManager.Instance.UpdateScore(Score);
        }

        public void LoseLife()
        {
            Lives--;
            if (Lives < 0) Lives = 0;

            UIManager.Instance.UpdateLives(Lives);

            if (Lives == 0)
            {
                StopGame();
                Debug.Log("GAME OVER");
            }
        }
    }
}