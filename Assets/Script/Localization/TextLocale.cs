using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class TextLocale : MonoBehaviour
{
    //public string se;
    //public string en;
    //Dictionary<string, string> texts = new Dictionary<string, string>();
    //[SerializeField]
    //string[] texts;
    [SerializeField]
    public TextsLocale texts;
    int idLocale = 0;

    TextMeshProUGUI tm;

    // Start is called before the first frame update

    private void Awake() {
        tm = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        SetLocale(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void Init(int size) {
    //    texts = new string[2];
    //    if (size > 0) {
    //        SetLocale(0);
    //    }
    //}

    public void SetLocale(int id)
    {
        Debug.Log("Set locale: " + id);
        idLocale = id;
        UpdateText();
    }

    void UpdateText() {
        if (texts.texts.Length > idLocale) {
            Debug.Log(texts, tm);
            tm.text = texts.texts[idLocale].Replace("\\n", "\n");
        }
    }

    public void SetTexts(TextsLocale t) {
        texts = t;
        UpdateText();
    }

}
