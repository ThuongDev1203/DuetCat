using UnityEngine;
using DuetCats.Scripts.Data;
using DuetCats.Scripts.Core;

namespace DuetCats.Scripts.Gameplay
{
    public class Note : MonoBehaviour
    {
        public NoteType noteType;
        public CatType catType;
        private GameObject prefabRef;
        private MidiNoteData data;
        private Vector3 startPos;
        private Vector3 endPos;
        private AudioManager audioManager;

        private bool isHandled;
        private bool isHit;
        private float hitTime;
        private float spawnTime;

        public void Init(MidiNoteData data, Vector3 startPos, Vector3 endPos, AudioManager audioManager, GameObject prefab)
        {
            this.data = data;
            this.startPos = startPos;
            this.endPos = endPos;
            this.audioManager = audioManager;
            this.prefabRef = prefab;

            hitTime = data.ta;
            spawnTime = audioManager.GetTime();

            isHandled = false;
            isHit = false;

            NoteManager.Instance.Register(this);
        }

        private void Update()
        {
            if (!GameManager.Instance.IsPlaying)
            {
                ReturnToPool();
                return;
            }

            if (isHandled)
            {
                ReturnToPool();
                return;
            }

            float currentTime = audioManager.GetTime();

            float duration = hitTime - spawnTime;
            if (duration <= 0.001f) duration = 0.001f;

            float t = Mathf.Clamp01((currentTime - spawnTime) / duration);
            transform.position = Vector3.Lerp(startPos, endPos, t);

            if (currentTime > hitTime + NoteManager.Instance.good)
            {
                isHandled = true;
                NoteManager.Instance.Miss(this);
                ReturnToPool();
            }
        }

        public void OnHit()
        {
            if (isHandled) return;

            isHandled = true;

            if (isHit) return;
            isHit = true;

            NoteManager.Instance.Hit(this);

            ReturnToPool();
        }

        void ReturnToPool()
        {
            if (NotePool.Instance != null)
                NotePool.Instance.Return(prefabRef, gameObject);
        }
    }
}