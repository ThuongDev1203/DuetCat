using UnityEngine;
using TMPro;

namespace DuetCats.Scripts.UI
{
    public class FloatingText : MonoBehaviour
    {
        public float moveSpeed = 60f;
        public float lifeTime = 0.8f;

        private float timer;
        private TextMeshProUGUI text;
        private Color color;

        void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            if (text == null)
            {
                Debug.LogError("Missing TextMeshProUGUI!");
                return;
            }

            color = text.color;
            string[] messages = { "Sweet!", "Yummy!", "Tasty!" };
            text.text = messages[Random.Range(0, messages.Length)];
            transform.localScale = Vector3.zero;
            transform.localPosition += new Vector3(Random.Range(-20f, 20f), 0, 0);
        }

        void Update()
        {
            timer += Time.deltaTime;

            float scale = Mathf.Lerp(0f, 1f, timer * 10f);
            transform.localScale = new Vector3(scale, scale, scale);

            transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;

            color.a = Mathf.Lerp(1, 0, timer / lifeTime);
            text.color = color;

            if (timer >= lifeTime)
            {
                Destroy(gameObject);
            }
        }

        void LateUpdate()
        {
            transform.rotation = Quaternion.identity;

            Vector3 s = transform.localScale;
            s.x = Mathf.Abs(s.x);
            transform.localScale = s;
        }
    }
}