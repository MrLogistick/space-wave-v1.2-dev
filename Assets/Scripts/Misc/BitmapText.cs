using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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

    public bool wrapping = false;
    bool previousWrapping;

    public bool visible = true;

    int changes = 0;

    Dictionary<char, Sprite> font = new();
    Dictionary<char, Sprite> flashFont = new();

    RectTransform rt;

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

    void Start() {
        rt = GetComponent<RectTransform>();
    }

    void Update() {
        changes = 0;

        if (previousText != text) { previousText = text; changes++; }
        if (previousSpacing != wordSpacing) { previousSpacing = wordSpacing; changes++; }
        if (previousFontSize != fontSize) { previousFontSize = fontSize; changes++; }
        if (previousAlignment != centred) { previousAlignment = centred; changes++; }
        if (previousStyle != flash) { previousStyle = flash; changes++; }
        if (previousWrapping != wrapping) { previousWrapping = wrapping; changes++; }

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

    float Centre(float value) {
        return centred ? value / -2f : 0f;
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

        List<float> lineLength = new List<float>();
        if (totalWidth > rt.rect.width && wrapping) {
            int count = Mathf.FloorToInt(totalWidth / rt.rect.width);
            for (int i = 0; i < count; i++) {
                lineLength.Add(rt.rect.width);
            }
            
            float remainder = totalWidth % rt.rect.width;
            if (remainder > 0f) {
                lineLength.Add(remainder);
            }
        }
        else {
            lineLength.Add(totalWidth);
        }

        int currentLine = 0;
        float x = centred ? lineLength[currentLine] / -2f : 0f;
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

            if (x >= (centred ? lineLength[currentLine] / 2f : lineLength[currentLine])) {
                currentLine++;
                x = centred ? lineLength[currentLine] / -2f : 0f;
            }

            var obj = new GameObject(c.ToString(), typeof(RectTransform), typeof(CanvasRenderer));
            var childrt = obj.GetComponent<RectTransform>();

            float pos = x - (centred ? 0f : rt.rect.xMax);

            obj.transform.SetParent(transform, false);
            childrt.anchoredPosition = new Vector2(pos + sprite.rect.width * fontSize / 2f, currentLine * -sprite.rect.height * fontSize);
            childrt.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height) * fontSize;

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