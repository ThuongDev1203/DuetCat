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
        int score = 0;

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
        }

        // 🎯 HIT từ va chạm
        public void Hit(Note note, int add, string type)
        {
            combo++;
            score += add;

            Debug.Log($"{type} | Combo: {combo} | Score: {score}");

            Remove(note);
        }

        public void Miss(Note note)
        {
            combo = 0;
            Debug.Log("MISS");

            Remove(note);
        }
    }
}