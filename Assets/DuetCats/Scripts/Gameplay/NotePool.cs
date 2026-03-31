using System.Collections.Generic;
using UnityEngine;

namespace DuetCats.Scripts.Gameplay
{
    public class NotePool : MonoBehaviour
    {
        public static NotePool Instance;

        private Dictionary<GameObject, Queue<GameObject>> pool = new();

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            if (prefab == null)
            {
                Debug.LogError("Prefab NULL!");
                return null;
            }

            if (!pool.ContainsKey(prefab))
                pool[prefab] = new Queue<GameObject>();

            GameObject obj;

            if (pool[prefab].Count > 0)
            {
                obj = pool[prefab].Dequeue();
                obj.transform.SetPositionAndRotation(pos, rot);
                obj.SetActive(true);
            }
            else
            {
                obj = Instantiate(prefab, pos, rot);
            }

            return obj;
        }

        public void Return(GameObject prefab, GameObject obj)
        {
            if (obj == null) return;

            obj.SetActive(false);

            if (!pool.ContainsKey(prefab))
                pool[prefab] = new Queue<GameObject>();

            pool[prefab].Enqueue(obj);
        }
    }
}