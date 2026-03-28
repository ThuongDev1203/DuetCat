using UnityEngine;

namespace DuetCats.Scripts.Core
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource audioSource;

        double startDsp;
        public float startDelay = 0.1f;

        void Awake()
        {

            audioSource.playOnAwake = false;
            audioSource.Stop();

            Debug.Log("Audio STOPPED INIT");
        }

        void Start()
        {
            audioSource.Stop();
        }

        public void Play()
        {
            Debug.Log("PLAY CALLED");

            if (audioSource.isPlaying) return;

            startDsp = AudioSettings.dspTime + startDelay;
            audioSource.PlayScheduled(startDsp);
        }

        public float GetTime()
        {
            return Mathf.Max(0, (float)(AudioSettings.dspTime - startDsp));
        }
    }
}