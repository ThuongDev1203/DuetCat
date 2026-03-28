using UnityEngine;
using Cysharp.Threading.Tasks;
using DuetCats.Scripts.Data;
using DuetCats.Scripts.Core;

namespace DuetCats.Scripts.Gameplay
{
    public enum NoteType
    {
        Normal,
        Strong,
        Long,
        Lollipop
    }
    public class Note : MonoBehaviour
    {
        public NoteType noteType;
        private MidiNoteData data;
        private Vector3 startPos;
        private Vector3 endPos;
        private AudioManager audioManager;

        public void Init(MidiNoteData data, Vector3 startPos, Vector3 endPos, AudioManager audioManager)
        {
            this.data = data;
            this.startPos = startPos;
            this.endPos = endPos;
            this.audioManager = audioManager;

            MoveNoteAsync().Forget();
        }

        private async UniTaskVoid MoveNoteAsync()
        {
            float startTime = audioManager.GetTime();
            float endTime = data.ta;
            float duration = Mathf.Max(0.01f, endTime - startTime);

            while (audioManager.GetTime() < endTime)
            {
                if (this == null || gameObject == null) return;

                float t = Mathf.Clamp01((audioManager.GetTime() - startTime) / duration);
                transform.position = Vector3.Lerp(startPos, endPos, t);
                await UniTask.Yield();
            }

            if (this != null && gameObject != null)
                Destroy(gameObject);
        }
    }
}