using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ViewSea : ViewBase
{

    public CanvasGroup cgIntro;
    public Intro intro;
    public CanvasGroup cgMenu;
    public FishBuilder fishBuilder;

    // Start is called before the first frame update
    void Start()
    {
        viewManager.app.AddInteractionTimeoutListener(ShowIntro);
        //ShowIntro();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowIntro() {
        if (!intro.active) {
            intro.Show();
            fishBuilder.CloseMenu();
        }
    }

    public void HideIntro() {
        if (intro.active) {
            intro.Hide();
        }
    }

}
