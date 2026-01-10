using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BitmapText : MonoBehaviour {
    public string text;
    string previousText = "";
    public int wordSpacing = 6;
    int previousSpacing;
    public int fontSize = 3;
    int previousFontSize;
    public bool centred = false;
    bool previousAlignment;
    public bool flash = false;
    bool previousStyle;
    public bool visible = true;

    int changes = 0;

    Dictionary<char, Sprite> font = new();
    Dictionary<char, Sprite> flashFont = new();

    void Awake() {
        SetupDictionary("SpaceWaver", font);
        SetupDictionary("SpaceWaverFlash", flashFont);
    }

    void SetupDictionary(string sprite, Dictionary<char, Sprite> letterStyle) {
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Font/{sprite}");

        foreach (var s in sprites) {
            if (s.name.Length == 1) {
                letterStyle[s.name[0]] = s;
            }
            else {
                switch (s.name) {
                    case "colon":
                        letterStyle[':'] = s;
                        break;
                    case "semicolon":
                        letterStyle[';'] = s;
                        break;
                    case "comma":
                        letterStyle[','] = s;
                        break;
                    case "period":
                        letterStyle['.'] = s;
                        break;
                    case "exclamation":
                        letterStyle['!'] = s;
                        break;
                    case "question":
                        letterStyle['?'] = s;
                        break;
                    case "openingbracket":
                        letterStyle['('] = s;
                        letterStyle['['] = s;
                        break;
                    case "closingbracket":
                        letterStyle[')'] = s;
                        letterStyle[']'] = s;
                        break;
                    case "apostrophe":
                        letterStyle['"'] = s;
                        break;
                }
            }
        }
    }

    void Update() {
        changes = 0;

        if (previousText != text) { previousText = text; changes++; }
        if (previousSpacing != wordSpacing) { previousSpacing = wordSpacing; changes++; }
        if (previousFontSize != fontSize) { previousFontSize = fontSize; changes++; }
        if (previousAlignment != centred) { previousAlignment = centred; changes++; }
        if (previousStyle != flash) { previousStyle = flash; changes++; }

        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(visible ? true : false);
        }

        if (changes > 0) {
            for (int i = 0; i < transform.childCount; i++) {
                GameObject child = transform.GetChild(i).gameObject;
                Destroy(child);
            }

            DrawText(flash ? flashFont : font);
        }
    }

    void DrawText(Dictionary<char, Sprite> letterStyle) {
        float totalWidth = 0f;
        foreach (char raw in text) {
            char c = char.ToUpper(raw);
            if (!letterStyle.TryGetValue(c, out Sprite sprite)) {
                if (c == ' ') totalWidth += wordSpacing * fontSize;
                continue;
            }

            totalWidth += (sprite.rect.width + ThinLetters(c)) * fontSize;
        }

        float x = centred ? totalWidth / -2f : 0f;

        foreach (char raw in text) {
            char c = char.ToUpper(raw);
            Sprite sprite;

            switch (c) {
                case ' ':
                    x += wordSpacing * fontSize;
                    continue;
                default:
                    if (!letterStyle.TryGetValue(c, out sprite)) continue;
                    break;
            }

            var obj = new GameObject(c.ToString(), typeof(RectTransform), typeof(CanvasRenderer));
            var rt = obj.GetComponent<RectTransform>();

            obj.transform.SetParent(transform, false);
            rt.anchoredPosition = new Vector2(x + sprite.rect.width * fontSize / 2f, 0);
            rt.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height) * fontSize;

            var image = obj.AddComponent<Image>();
            image.sprite = sprite;

            x += (sprite.rect.width + ThinLetters(c)) * fontSize;
        }
    }

    int ThinLetters(char c) {
        int xOffset = 0;

        switch (c) {
            case ':':
            case ';':
            case ',':
            case '.':
            case '!':
            case '"':
                xOffset = -2;
                break;
        }

        return xOffset;
    }
}