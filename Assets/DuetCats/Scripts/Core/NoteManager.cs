using System.Collections.Generic;
using UnityEngine;
using DuetCats.Scripts.Gameplay;

namespace DuetCats.Scripts.Core
{
    public class NoteManager : MonoBehaviour
    {
        public static NoteManager Instance;

        List<Note> notes = new List<Note>();

        public float perfect = 0.07f;
        public float good = 0.15f;

        int combo = 0;

        void Awake()
        {
            Instance = this;
        }

        public void Register(Note note)
        {
            if (!notes.Contains(note))
                notes.Add(note);
        }

        public void Remove(Note note)
        {
            if (notes.Contains(note))
                notes.Remove(note);

            Debug.Log("Note removed, remain: " + notes.Count);
        }

        public void Hit(Note note)
        {
            combo++;

            int score = GetScore(note.noteType);
            GameManager.Instance.AddScore(score);

            Remove(note);
        }

        int GetScore(NoteType type)
        {
            switch (type)
            {
                case NoteType.Normal: return 2;
                case NoteType.Strong: return 5;
                case NoteType.Long: return 2;
                case NoteType.Lollipop: return 8;
                default: return 0;
            }
        }

        public int ActiveNoteCount()
        {
            Debug.Log("Active Count = " + notes.Count);
            return notes.Count;
        }

        public void Miss(Note note)
        {
            combo = 0;

            Debug.Log("MISS");

            GameManager.Instance.LoseLife();

            Remove(note);
        }
    }
}