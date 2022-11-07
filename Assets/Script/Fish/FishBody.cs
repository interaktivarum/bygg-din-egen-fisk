using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBody : FishPart
{

    [Header("Where do I belong?")]
    public Bounds boundsSpawn;
    public Bounds boundsEscape;

    [Header("Movement")]
    public float speed = 0.01f;
    public float rotationSpeed = 1f;

    [Header("Who eats whom?")]
    public int size;
    public List<int> sizesEat;

    [Header("Other settings")]
    public bool pattern;

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
