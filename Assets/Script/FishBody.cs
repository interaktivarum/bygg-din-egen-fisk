using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBody : FishPart
{

    public bool pattern;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        Debug.Log("Clicked");
        if (!pattern)
        {
            EventHandler.fishBodyClickedEvent.Invoke(this);
        }
        else
        {
            EventHandler.fishPatternClickedEvent.Invoke(this);
        }
        //clickedEvent.Invoke(this);
    }
}
