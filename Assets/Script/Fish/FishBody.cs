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

    [Header("Where do I belong?")]
    public Vector3 positionSpawn;
    public Bounds boundsHome;
    public Bounds boundsEscape;

    [Header("Movement")]
    public float speed = 0.01f;
    public float speedAttack = 0.1f;
    public float speedEscape = 0.08f;
    public float rotationSpeed = 1f;
    public IdleMovement idleMovement = IdleMovement.rotate;

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
