using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LocaleHandler : MonoBehaviour
{

    public TextLocale[] textsLocale;
    public Image[] images;

    private void Start() {
        FindObjectOfType<App>().AddInteractionTimeoutListener(ResetLocale);
        textsLocale = FindObjectsOfType<TextLocale>(true);
        ResetLocale();
    }

    public void ResetLocale() {
        SetLocale(0);
    }

    public void SetLocale(int id) {
        foreach (TextLocale tl in FindObjectsOfType<TextLocale>(true)) {
            tl.SetLocale(id);
        }

        int i = 0;
        foreach(Image image in images) {
            float alpha = i == id ? 1 : 0.5f;
            image.DOFade(alpha, 0.5f);
            i++;
        }

    }
}
