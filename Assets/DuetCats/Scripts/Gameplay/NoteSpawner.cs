using UnityEngine;
using System.Collections.Generic;
using DuetCats.Scripts.Data;
using Cysharp.Threading.Tasks;
using DuetCats.Scripts.Core;

namespace DuetCats.Scripts.Gameplay
{
    public class NoteSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject candy1_Normal, candy1_Strong, candy1_Long;
        public GameObject candy2_Normal, candy2_Strong, candy2_Long;
        public GameObject lolipop_Long;

        [Header("Positions")]
        public Transform leftStart, rightStart;
        public Transform leftEnd, rightEnd;

        [Header("Audio")]
        public AudioManager audioManager;

        public float spawnOffset = 1.5f;

        private MidiNoteData[] notes;
        private int index = 0;

        private int lastLeftLane = -1;
        private int lastRightLane = -1;

        private System.Random rand = new System.Random();

        void Start()
        {
            notes = JSONLoader.Load();
            audioManager.Play();
        }

        void Update()
        {
            if (!GameManager.Instance.IsPlaying) return;
            if (notes == null || index >= notes.Length) return;

            float currentTime = audioManager.GetTime();
            float threshold = 0.02f;

            while (index < notes.Length && notes[index].ta <= currentTime + spawnOffset)
            {
                float groupTime = notes[index].ta;
                int startIndex = index;

                while (index < notes.Length && Mathf.Abs(notes[index].ta - groupTime) < threshold)
                    index++;

                SpawnGroup(startIndex, index - startIndex);
            }
        }

        void SpawnGroup(int startIndex, int count)
        {
            List<MidiNoteData> left = new List<MidiNoteData>();
            List<MidiNoteData> right = new List<MidiNoteData>();

            for (int i = 0; i < count; i++)
            {
                var data = notes[startIndex + i];
                if (data.n >= 100) right.Add(data);
                else left.Add(data);
            }

            int leftLane = GetLane(ref lastLeftLane);
            int rightLane = GetLane(ref lastRightLane);

            SpawnSide(left, false, leftLane);
            SpawnSide(right, true, rightLane);
        }

        void SpawnSide(List<MidiNoteData> notes, bool isRight, int lane)
        {
            if (notes.Count == 0) return;

            Transform start = isRight ? rightStart : leftStart;
            Transform end = isRight ? rightEnd : leftEnd;

            float[] lanes = isRight
                ? new float[] { 0.65f, 0.85f }
                : new float[] { 0.15f, 0.35f };

            float x = lanes[lane];

            float startY = Camera.main.WorldToViewportPoint(start.position).y;
            float endY = Camera.main.WorldToViewportPoint(end.position).y;

            Vector3 startPos = ViewportToWorld(x, startY);
            Vector3 endPos = ViewportToWorld(x, endY);

            foreach (var data in notes)
            {
                GameObject obj = Instantiate(GetPrefab(data), startPos, Quaternion.identity);
                var noteComp = obj.GetComponent<Note>();
                noteComp.Init(data, startPos, endPos, audioManager);
            }
        }

        Vector3 ViewportToWorld(float x, float y)
        {
            var pos = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 10));
            pos.z = 0;
            return pos;
        }

        int GetLane(ref int last)
        {
            if (last != -1 && rand.NextDouble() < 0.6)
                return last;

            last = rand.Next(0, 2);
            return last;
        }

        GameObject GetPrefab(MidiNoteData d)
        {
            if (d.id == 50) return lolipop_Long;

            bool strong = d.v >= 120;
            bool isLong = d.d > 0.2f;

            if (d.n >= 100)
                return isLong ? candy1_Long : strong ? candy1_Strong : candy1_Normal;
            else
                return isLong ? candy2_Long : strong ? candy2_Strong : candy2_Normal;
        }
    }
}