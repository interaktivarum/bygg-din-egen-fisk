using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;

public class FishOld : MonoBehaviour
{

    Animator animator;
    Transform bodies;
    Transform heads;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.Find("Fish").GetComponent<Animator>();
        bodies = transform.Find("Fish/bodies");
        heads = transform.Find("Fish/heads");
        SetAnimationClip();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SetAnimationClip()
    {
        animator.SetFloat("speed", 1.2f);

    }

    public void SelectBody(GameObject body)
    {
        bodies.GetChild(0).gameObject.SetActive(false);
        body.SetActive(true);
        body.transform.SetAsFirstSibling();
        animator.Rebind();
    }

    public void SelectHead(GameObject head)
    {
        heads.GetChild(0).gameObject.SetActive(false);
        head.SetActive(true);
        head.transform.SetAsFirstSibling();
        animator.Rebind();
    }

}
