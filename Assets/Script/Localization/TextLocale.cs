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
    void Start()
    {
        tm = GetComponent<TextMeshProUGUI>();
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
            tm.text = texts.texts[idLocale];
        }
    }

    public void SetTexts(TextsLocale t) {
        texts = t;
        UpdateText();
    }

}
