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
                        letterStyle['\''] = s;
                        break;
                }
            }
        }
    }

    void Start() {
        rt = GetComponent<RectTransform>();
    }

    void Update() {
        // If changes > 0, DrawText();
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

    void DrawText(Dictionary<char, Sprite> letterStyle) {
        // Getting total width and word sizes
        float totalWidth = 0f;
        int currentWord = 0;
        List<float> words = new List<float> { 0 };

        foreach (char raw in text) {
            char c = char.ToUpper(raw);
            if (!letterStyle.TryGetValue(c, out Sprite sprite)) {
                if (c == ' ') {
                    // On space, create new word and add to width
                    totalWidth += wordSpacing * fontSize;
                    currentWord++;
                    words.Add(0);
                }
                continue;
            }

            // On regular letter, add to wordsize and totalwidth
            totalWidth += (sprite.rect.width + ThinLetters(c)) * fontSize;
            words[currentWord] += (sprite.rect.width + ThinLetters(c)) * fontSize;
        }

        // Getting line lengths depending on wrapping true/false
        // List<float> lineLength = new List<float>();
        // if (totalWidth > rt.rect.width && wrapping) {
        //     int count = Mathf.FloorToInt(totalWidth / rt.rect.width);
        //     for (int i = 0; i < count; i++) {
        //         // For the lines largerthan rt.rect.width
        //         lineLength.Add(rt.rect.width);
        //     }
            
        //     // for the line smaller than rt.rect.width
        //     float remainder = totalWidth % rt.rect.width;
        //     if (remainder > 0f) {
        //         lineLength.Add(remainder);
        //     }
        // }
        // else {
        //     // One line only -- wrapping = false
        //     lineLength.Add(totalWidth);
        // }
        
        var maxWidth = rt.rect.width;
        List<float> lineWidths = new List<float>() { 0 };

        // Draw Text
        currentWord = 0;
        int currentLine = 0;

        float x = 0f;
        float lineStart = 0f;

        bool atWordStart = true;

        foreach (char raw in text) {
            char c = char.ToUpper(raw);
            Sprite sprite;

            switch (c) {
                case ' ':
                    // On space, next word
                    x += wordSpacing * fontSize;
                    lineWidths[currentLine] = Mathf.Max(lineWidths[currentLine], x - lineStart);
                    currentWord++;
                    if (currentWord >= words.Count) currentWord = words.Count - 1;
                    
                    atWordStart = true;
                    continue;
                default:
                    // Every other letter
                    if (!letterStyle.TryGetValue(c, out sprite)) continue;
                    break;
            }

            // Wrapping -- if this upcoming word exceeds lineLength, move the word to the next line.
            if (atWordStart) {
                var usedWidth = x - lineStart;
                if (wrapping && usedWidth + words[currentWord] >= maxWidth) {
                    currentLine++;
                    lineWidths.Add(0f);
                    x = 0f;
                    lineStart = 0f;
                }

                atWordStart = false;
            }

            // Sprite creation
            var obj = new GameObject(c.ToString(), typeof(RectTransform), typeof(CanvasRenderer));
            var childrt = obj.GetComponent<RectTransform>();

            // Positioning and size
            obj.transform.SetParent(transform, false);

            var xPos = x + sprite.rect.width * fontSize / 2f - (centred ? 0f : rt.rect.width / 2f);
            childrt.anchoredPosition = new Vector2(xPos, currentLine * -sprite.rect.height * fontSize);

            childrt.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height) * fontSize;

            // Imaging
            var image = obj.AddComponent<Image>();
            image.sprite = sprite;

            x += (sprite.rect.width + ThinLetters(c)) * fontSize;
            lineWidths[currentLine] = Mathf.Max(lineWidths[currentLine], x - lineStart);
        }

        if (centred) {
            for (int i  = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i) as RectTransform;
                int line = Mathf.RoundToInt(-child.anchoredPosition.y / (child.sizeDelta.y));
                float offset = lineWidths[line] / 2f;
                child.anchoredPosition += Vector2.left * offset;
            }
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
            case '\'':
                xOffset = -2;
                break;
        }

        return xOffset;
    }
}