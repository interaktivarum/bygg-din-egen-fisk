using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class TextLocale : MonoBehaviour
{
    public string se;
    public string en;
    Dictionary<string, string> texts = new Dictionary<string, string>();
    TextMeshProUGUI tm; 

    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponentInChildren<TextMeshProUGUI>();
        texts.Add("se", se);
        texts.Add("en", en);
        tm.text = se;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLocale(string locale)
    {
        if (texts.ContainsKey(locale))
        {
            tm.text = texts[locale];
        }
    }

}
