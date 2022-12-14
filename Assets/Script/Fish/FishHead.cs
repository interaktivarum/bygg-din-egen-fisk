using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHead : FishPart
{

    public HeadID idHead;

    public Bounds boundsEat;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
