using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHead : FishPart
{

    public Bounds boundsEat;

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
        EventHandler.fishHeadClickedEvent.Invoke(this);
        //clickedEvent.Invoke(this);
    }
}
