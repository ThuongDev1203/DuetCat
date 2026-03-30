using UnityEngine;
using DuetCats.Scripts.Data;
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

        [Header("Timing")]
        public float travelTime = 1.0f;

        private MidiNoteData[] notes;
        private int index = 0;

        bool hasSpawnedAll = false;
        bool hasWon = false;

        void Start()
        {
            notes = JSONLoader.Load();

            //thứ tự time
            System.Array.Sort(notes, (a, b) => a.ta.CompareTo(b.ta));
        }

        void Update()
        {
            if (!GameManager.Instance.IsPlaying) return;
            if (notes == null) return;

            float currentTime = audioManager.GetTime();

            // spawn trước travelTime
            while (index < notes.Length && notes[index].ta <= currentTime + travelTime)
            {
                var data = notes[index];

                bool isRight = IsRight(data.n);
                int lane = GetLane(data.n);

                SpawnSingle(data, isRight, lane);

                index++;
            }

            // spawn
            if (!hasSpawnedAll && index >= notes.Length)
            {
                hasSpawnedAll = true;
                Debug.Log("All notes spawned");
            }

            // WIN
            if (hasSpawnedAll
                && !hasWon
                && NoteManager.Instance.ActiveNoteCount() == 0
                && GameManager.Instance.IsPlaying)
            {
                hasWon = true;

                Debug.Log("🎉 WIN CONDITION MET");
                GameManager.Instance.WinGame();
            }
        }

        //SPAWN

        void SpawnSingle(MidiNoteData data, bool isRight, int lane)
        {
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

            GameObject obj = Instantiate(GetPrefab(data), startPos, Quaternion.identity);

            var noteComp = obj.GetComponent<Note>();
            noteComp.Init(data, startPos, endPos, audioManager);
        }

        Vector3 ViewportToWorld(float x, float y)
        {
            var pos = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 10));
            pos.z = 0;
            return pos;
        }

        //SIDE
        bool IsRight(int n)
        {
            return (n == 101 || n == 99);
        }

        //LANE

        int GetLane(int n)
        {
            if (n == 101 || n == 98)
                return 0;

            return 1;
        }

        //PREFAB

        GameObject GetPrefab(MidiNoteData d)
        {
            if (d.id == 50) return lolipop_Long;

            bool strong = d.v >= 120;
            bool isLong = d.d > 0.2f;

            if (IsRight(d.n))
                return isLong ? candy1_Long : strong ? candy1_Strong : candy1_Normal;
            else
                return isLong ? candy2_Long : strong ? candy2_Strong : candy2_Normal;
        }

        //RESET

        public void ResetSpawner()
        {
            index = 0;
            hasSpawnedAll = false;
            hasWon = false;

            foreach (var note in FindObjectsByType<Note>(FindObjectsSortMode.None))
            {
                Destroy(note.gameObject);
            }
        }
    }
}