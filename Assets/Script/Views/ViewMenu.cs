using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewMenu : ViewBase
{
    GameObject menu;
    GameObject bodies;
    GameObject heads;
    GameObject patterns;

    public Transform fishPicker;
    public Transform buttons;
    public Fish fish;
    public RectTransform texts;
    public FishManager fishManager;

    // Start is called before the first frame update
    public override void StartInstance()
    {
        menu = transform.Find("Canvas/Menu").gameObject;
        bodies = fishPicker.Find("Bodies").gameObject;
        heads = fishPicker.Find("Heads").gameObject;
        patterns = fishPicker.Find("Patterns").gameObject;

        //Init
        fishManager.gameObject.SetActive(false);
        bodies.SetActive(false);
        heads.SetActive(false);
        patterns.SetActive(false);
        BodyInit();

        //Events
        EventHandler.fishBodyClickedEvent.AddListener(SelectBody);
        EventHandler.fishHeadClickedEvent.AddListener(SelectHead);
        EventHandler.fishPatternClickedEvent.AddListener(SelectPattern);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        //Events
        EventHandler.fishBodyClickedEvent.RemoveListener(SelectBody);
    }

    public void BodyInit()
    {
        // Buttons
        buttons.Find("BodyNext").gameObject.SetActive(true);

        // Fish parts
        bodies.SetActive(true);

        //Children
        foreach (Transform body in bodies.transform)
        {
            body.gameObject.SetActive(false);
        }
        string idPattern = fish.ids.pattern != null ? fish.ids.pattern : "Silver";
        bodies.transform.Find(idPattern).gameObject.SetActive(true);
    }

    public void SelectBody(FishBody body)
    {
        fish.SetBody(body.id);
        BodySelected();

        //Set texts
        texts.Find("Body").GetComponent<TextMeshProUGUI>().text = body.description;
    }

    public void BodySelected()
    {
        buttons.Find("BodyNext").GetComponent<Button>().interactable = true;
        buttons.Find("BodyNext").GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }

    public void BodyFinish ()
    {
        // Buttons
        buttons.Find("BodyNext").gameObject.SetActive(false);

        // Fish parts
        fishPicker.Find("Bodies").gameObject.SetActive(false);

        // Init heads
        HeadInit();
    }

    void HeadInit ()
    {
        // Buttons
        buttons.Find("HeadPrev").gameObject.SetActive(true);
        buttons.Find("HeadNext").gameObject.SetActive(true);

        // Fish parts
        Transform heads = fishPicker.Find("Heads");
        heads.gameObject.SetActive(true);
    }

    public void SelectHead(FishHead head)
    {
        fish.SetHead(head.id);
        HeadSelected();

        //Set texts
        texts.Find("Head").GetComponent<TextMeshProUGUI>().text = head.description;
    }

    public void HeadSelected()
    {
        buttons.Find("HeadNext").GetComponent<Button>().interactable = true;
        buttons.Find("HeadNext").GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }

    public void HeadBack()
    {
        // Buttons
        buttons.Find("HeadPrev").gameObject.SetActive(false);
        buttons.Find("HeadNext").gameObject.SetActive(false);

        // Fish parts
        fishPicker.Find("Heads").gameObject.SetActive(false);

        // Init bodies
        BodyInit();
    }

    public void HeadFinish()
    {
        // Buttons
        buttons.Find("HeadNext").gameObject.SetActive(false);
        buttons.Find("HeadPrev").gameObject.SetActive(false);

        // Fish parts
        fishPicker.Find("Heads").gameObject.SetActive(false);

        // Init pattern
        PatternInit();
    }

    public void PatternInit()
    {
        // Buttons
        buttons.Find("PatternPrev").gameObject.SetActive(true);
        buttons.Find("PatternNext").gameObject.SetActive(true);

        // Fish parts
        patterns.SetActive(true);

        //Children
        foreach (Transform pattern in patterns.transform)
        {
            pattern.gameObject.SetActive(false);
        }
        string idBody = fish.ids.body != null ? fish.ids.body : "Flat";
        patterns.transform.Find(idBody).gameObject.SetActive(true);
    }

    public void SelectPattern(FishBody body)
    {
        fish.SetBody(body.id);
        PatternSelected();

        //Set texts
        texts.Find("Pattern").GetComponent<TextMeshProUGUI>().text = body.description;
    }

    public void PatternSelected()
    {
        buttons.Find("PatternNext").GetComponent<Button>().interactable = true;
        buttons.Find("PatternNext").GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }

    public void PatternBack()
    {
        // Buttons
        buttons.Find("PatternPrev").gameObject.SetActive(false);
        buttons.Find("PatternNext").gameObject.SetActive(false);

        // Fish parts
        fishPicker.Find("Patterns").gameObject.SetActive(false);

        // Init heads
        HeadInit();
    }

    public void PatternFinish()
    {
        fishManager.CreateFish(fish.ids);
        FinishView();
    }

}
