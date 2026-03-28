using System;

namespace DuetCats.Scripts.Data
{
    public static class JsonHelper
    {
        [Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }

        public static T[] FromJson<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            return UnityEngine.JsonUtility.FromJson<Wrapper<T>>(newJson).array;
        }
    }
}