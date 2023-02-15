using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IdleMovement
{
    rotate,
    back
}

public class FishBody : FishPart
{
    public BodyID idBody;
    public PatternID idPattern;
    public float scale = 1;

    [Header("Where do I belong?")]
    public Vector3 positionSpawn;
    public RectTransform rectHome;
    public Bounds boundsHome;
    public Bounds boundsEscape;

    [Header("Movement")]
    public float speed = 1;
    public float speedAttack = 20;
    public float speedEscape = 15;
    public float rotationSpeed = 2;
    public IdleMovement idleMovement = IdleMovement.rotate;
    public Bend lastBend;
    public Bend[] bends;
    public float swingSpeed = 1;

    [Header("Who eats whom?")]
    public int size;
    public List<int> sizesEat;
    public float energyStart = 100;
    public float energyAttackCost = 20;
    public float energyBite = 70;
    public float energySwallow = 50;
    public float energyFloor = 50;

    [Header("Other settings")]
    public bool pattern;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //Last bend bone
        bends = GetComponentsInChildren<Bend>();
        lastBend = bends[bends.Length-1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void OnMouseDown()
    //{
    //    Debug.Log("Clicked");
    //    if (!pattern)
    //    {
    //        EventHandler.fishBodyClickedEvent.Invoke(this);
    //    }
    //    else
    //    {
    //        EventHandler.fishPatternClickedEvent.Invoke(this);
    //    }
    //    //clickedEvent.Invoke(this);
    //}
}
