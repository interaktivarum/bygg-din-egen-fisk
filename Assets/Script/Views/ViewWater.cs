using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewWater : ViewBase
{

    public FishManager fishManager;

    // Start is called before the first frame update
    public override void StartInstance()
    {

        fishManager = FindObjectOfType(typeof(FishManager), true) as FishManager;

        //Init
        fishManager.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        //Events
    }

}
