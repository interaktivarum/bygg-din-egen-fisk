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
        if (Input.GetKeyDown(KeyCode.B)) {
            Animator animator = GetComponent<Animator>();
            if (animator) {
                SetBite(!animator.GetBool("BiteBool"));
            }
        }
    }

    public void SetBite(bool state) {
        Animator animator = GetComponent<Animator>();
        if (animator) {
            animator.SetBool("BiteBool", state);
        }
    }

    //public void OnMouseDown()
    //{
    //    Debug.Log("Clicked");
    //    EventHandler.fishHeadClickedEvent.Invoke(this);
    //    //clickedEvent.Invoke(this);
    //}

}
