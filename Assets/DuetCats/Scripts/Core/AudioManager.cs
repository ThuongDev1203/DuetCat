using UnityEngine;

namespace DuetCats.Scripts.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public AudioSource audioSource;

        void Awake()
        {
            Instance = this;
        }

        public void Play()
        {
            audioSource.time = 0.1f;
            audioSource.Play();
        }

        public void Stop()
        {
            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();
        }

        public float GetTime()
        {
            return audioSource.time;
        }
    }
}