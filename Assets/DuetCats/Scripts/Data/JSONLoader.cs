using UnityEngine;
using System.Collections.Generic;

namespace DuetCats.Scripts.Data
{
    public static class JSONLoader
    {
        public static MidiNoteData[] Load()
        {
            TextAsset json = Resources.Load<TextAsset>("JsonMidi_BabyMonster");
            return JsonHelper.FromJson<MidiNoteData>(json.text);
        }
    }
}