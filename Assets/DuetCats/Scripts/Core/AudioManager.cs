using UnityEngine;

namespace DuetCats.Scripts.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public AudioSource audioSource;
        [SerializeField] float startOffset = 0.1f;

        void Awake()
        {
            Instance = this;
        }

        public void Play()
        {
            audioSource.time = startOffset;
            audioSource.Play();
        }

        public void Stop()
        {
            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();
        }

        public float GetTime()
        {
            return audioSource != null ? audioSource.time : 0f;
        }

        public void ResetTime()
        {
            audioSource.Stop();
            audioSource.time = 0.1f;
        }
    }
}