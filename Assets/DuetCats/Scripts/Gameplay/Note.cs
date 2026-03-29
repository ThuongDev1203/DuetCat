using UnityEngine;
using Cysharp.Threading.Tasks;
using DuetCats.Scripts.Data;
using DuetCats.Scripts.Core;

namespace DuetCats.Scripts.Gameplay
{
    public class Note : MonoBehaviour
    {
        public NoteType noteType;

        private MidiNoteData data;
        private Vector3 startPos;
        private Vector3 endPos;
        private AudioManager audioManager;

        private bool isHandled = false;
        private float hitTime;

        public void Init(MidiNoteData data, Vector3 startPos, Vector3 endPos, AudioManager audioManager)
        {
            this.data = data;
            this.startPos = startPos;
            this.endPos = endPos;
            this.audioManager = audioManager;

            hitTime = data.ta;

            NoteManager.Instance.Register(this);

            MoveNoteAsync().Forget();
        }

        private async UniTaskVoid MoveNoteAsync()
        {
            float spawnTime = audioManager.GetTime();

            while (true)
            {
                if (this == null) return;

                if (!GameManager.Instance.IsPlaying)
                {
                    if (!isHandled)
                    {
                        NoteManager.Instance.Remove(this);
                    }

                    Destroy(gameObject);
                    return;
                }

                if (isHandled)
                {
                    Destroy(gameObject);
                    return;
                }

                float currentTime = audioManager.GetTime();

                float t = Mathf.Clamp01((currentTime - spawnTime) / (hitTime - spawnTime));
                transform.position = Vector3.Lerp(startPos, endPos, t);

                // MISS
                if (currentTime > hitTime + NoteManager.Instance.good)
                {
                    isHandled = true;

                    Debug.Log("MISS by time");

                    NoteManager.Instance.Miss(this);
                    Destroy(gameObject);
                    return;
                }

                await UniTask.Yield();
            }
        }

        public void OnHit()
        {
            if (isHandled) return;
            if (!GameManager.Instance.IsPlaying) return;

            isHandled = true;

            NoteManager.Instance.Hit(this);

            Destroy(gameObject);
        }
    }
}