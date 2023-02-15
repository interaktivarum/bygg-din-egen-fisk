using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using DG.Tweening;
using TMPro;

public class FishBuilder : MonoBehaviour
{
    App app;
    bool isOpen = false;
    public FishFactory fishFactory;
    public FishManager fishManager;
    Fish fish;
    //public FishBody bodyDefault;
    //public FishHead headDefault;
    public Transform container;

    public Color colorUnselected;

    int idBody;
    int idHead;
    int idPattern;

    [Header("Transforms")]
    public Transform menu;
    public Transform content;
    public Transform handle;
    public Transform selectBody;
    public Transform selectHead;
    public Transform selectPattern;

    public Bounds boundsShow;
    public Bounds boundsHide;

    // Start is called before the first frame update
    void Start()
    {
        app = FindObjectOfType<App>();
        CloseMenu();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToggleMenu() {
        if (!isOpen) {
            OpenMenu();
        }
        else {
            CloseMenu(false);
        }
    }

    public void OpenMenu () {
        menu.GetComponent<RectTransform>().DOAnchorPosX(0, 1);
        menu.Find("Background").GetComponent<Image>().DOFade(1, 1);
        menu.Find("Content").GetComponent<CanvasGroup>().DOFade(1, 1);
        menu.Find("Content").GetComponent<CanvasGroup>().interactable = true;
        transform.Find("Canvas/Fader").GetComponent<Image>().DOFade(0.75f, 1);
        handle.Find("BtnSliderRotate").transform.DOLocalRotate(new Vector3(0, 0, 180), 1);
        handle.Find("BtnSliderRotate").GetComponent<Pulse>().StopTween();
        if (container.childCount == 0) {
            CreateFish();
        }
        ShowFish();
        
        isOpen = true;
    }

    public void CloseMenu(bool releaseFish = false) {
        menu.GetComponent<RectTransform>().DOAnchorPosX(-760, 1);
        menu.Find("Background").GetComponent<Image>().DOFade(0, 1);
        menu.Find("Content").GetComponent<CanvasGroup>().DOFade(0, 1);
        menu.Find("Content").GetComponent<CanvasGroup>().interactable = false;
        transform.Find("Canvas/Fader").GetComponent<Image>().DOFade(0, 1);
        handle.Find("BtnSliderRotate").transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
        handle.Find("BtnSliderRotate").GetComponent<Pulse>().StartTween();
        if (releaseFish) {
            ReleaseFish();
        }
        else {
            HideFish();
        }
        isOpen = false;
    }

    void ResetFish() {
        idBody = 0;
        idHead = 0;
        idPattern = 0;
    }

    public void CreateFish() {
        ResetFish();
        fish = fishFactory.CreateFish(null, null, container);
        SetBody(idBody);
        SetHead(idHead);
        SetPattern(idPattern);
        fish.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        fish.transform.localRotation = Quaternion.Euler(0, -90, 0);
        fish.state = FishState.building;
        fish.transform.localPosition = boundsHide.center;
        fish.GetComponent<SortingGroup>().sortingOrder = 20;

        UpdateButtons();
    }

    public void ShowFish() {
        fish.boundsHomeOverride = boundsShow;
        fish.RandomTarget(boundsShow);
        fish.transform.localRotation = Quaternion.Euler(0, 90, 0);
    }

    public void HideFish() {
        if (fish) {
            fish.boundsHomeOverride = boundsHide;
            fish.RandomTarget(boundsHide);
        }
    }

    public void ReleaseFish() {
        app.timeHelper.WaitAndCallFunction(ResetSortingOrder, 5);
        fish.transform.parent = fishManager.transform.Find("Fishes");
        fish.state = FishState.released;
        fish.transform.DOScale(new Vector3(fish.body.scale, fish.body.scale, fish.body.scale), 15);
        fish.RandomTarget();
    }

    void ResetSortingOrder() {
        fish.GetComponent<SortingGroup>().sortingOrder = 0;
    }

    //public void SetBody(FishBody body) {
    //    fish.SetBody(body);
    //}

    public void SetBody(int id) {
        idBody = id;
        fish.SetBody(fishFactory.GetBodyById(idBody, idPattern));
        //selectBody.Find("Description/TextLocale").GetComponent<TextLocale>().SetTexts(body.GetComponent<TextsLocale>());
        selectBody.Find("Description/TextLocale").GetComponent<TextLocale>().SetTexts(selectBody.Find("Buttons").GetChild(id).GetComponent<TextsLocale>());
        UpdateButtons();
    }

    public void SetHead(int id) {
        idHead = id;
        fish.SetHead(fishFactory.GetHeadById(idHead, idPattern));
        selectHead.Find("Description/TextLocale").GetComponent<TextLocale>().SetTexts(selectHead.Find("Buttons").GetChild(id).GetComponent<TextsLocale>());
        UpdateButtons();
    }

    public void SetPattern(int id) {
        idPattern = id;
        fish.SetBody(fishFactory.GetBodyById(idBody, idPattern));
        fish.SetHead(fishFactory.GetHeadById(idHead, idPattern));
        selectPattern.Find("Description/TextLocale").GetComponent<TextLocale>().SetTexts(selectPattern.Find("Buttons").GetChild(id).GetComponent<TextsLocale>());
        UpdateButtons();
    }

    void UpdateButtons() {

        float alphaUnselected = 0.25f;

        // Update body buttons
        for (int i = 0; i < selectBody.childCount; i++) {
            float scale = i == idBody ? 1 : 0;
            selectBody.Find("Buttons").GetChild(i).Find("Selected").GetComponent<Image>().transform.DOScale(scale, 0.5f);
            float alpha = i == idBody ? 1 : alphaUnselected;
            
        }

        // Update head buttons
        for (int i = 0; i < selectHead.childCount; i++) {
            float scale = i == idHead ? 1 : 0;
            selectHead.Find("Buttons").GetChild(i).Find("Selected").GetComponent<Image>().transform.DOScale(scale, 0.5f);
        }

        // Update pattern buttons
        for (int i = 0; i < selectPattern.childCount; i++) {

            float scale = i == idPattern ? 1 : 0;
            selectPattern.Find("Buttons").GetChild(i).Find("Selected").GetComponent<Image>().transform.DOScale(scale, 0.5f);

            //float alpha = i == idPattern ? 1 : alphaUnselected;
            //selectPattern.Find("Buttons").GetChild(i).Find("Image").GetComponent<Image>().sprite = fishFactory.GetBodyPatternById(idBody, i);
        }
    }

}
