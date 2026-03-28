using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class AtlasImporter : MonoBehaviour
{
    public Texture2D atlasTexture;       // Brown_Cat.png
    public TextAsset atlasFile;          // Brown_Cat.atlas.txt

    void Start()
    {
        if (atlasTexture == null || atlasFile == null)
        {
            Debug.LogError("Assign atlas texture and atlas file in inspector!");
            return;
        }

        Dictionary<string, Rect> spritesData = ParseAtlas(atlasFile.text);

        foreach (var kvp in spritesData)
        {
            string name = kvp.Key;
            Rect rect = kvp.Value;

            // Chuyển từ pixels sang unit của Unity
            Sprite sprite = Sprite.Create(atlasTexture, rect, new Vector2(0.5f, 0.5f), 100f);
            sprite.name = name;

            // Tạo GameObject để xem thử
            GameObject go = new GameObject(name);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
        }
    }

    Dictionary<string, Rect> ParseAtlas(string text)
    {
        Dictionary<string, Rect> result = new Dictionary<string, Rect>();
        StringReader reader = new StringReader(text);

        string line;
        string currentName = null;
        float x = 0, y = 0, w = 0, h = 0;

        while ((line = reader.ReadLine()) != null)
        {
            line = line.Trim();
            if (string.IsNullOrEmpty(line) || line.EndsWith(".png") || line.Contains(":"))
            {
                // Ignore header lines
                if (line.EndsWith(".png")) continue;
                if (line.StartsWith("xy:"))
                {
                    string[] parts = line.Substring(3).Split(',');
                    x = float.Parse(parts[0]);
                    y = float.Parse(parts[1]);
                }
                else if (line.StartsWith("size:"))
                {
                    string[] parts = line.Substring(5).Split(',');
                    w = float.Parse(parts[0]);
                    h = float.Parse(parts[1]);
                    // Unity y = bottom-left, atlas y = top-left
                    Rect rect = new Rect(x, atlasTexture.height - y - h, w, h);
                    result[currentName] = rect;
                }
                continue;
            }
            else
            {
                currentName = line; // tên sprite
            }
        }

        return result;
    }
}