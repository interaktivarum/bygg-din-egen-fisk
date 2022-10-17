using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;
using TMPro;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public struct FishIDs
{
    public string body;
    public string head;
    public string pattern;
}

public class Fish : MonoBehaviour
{
    public FishIDs ids;
    //public string idBody;
    //public string idHead;
    //public string idPattern;
    Vector3 target;
    Tween tweenTarget;

    // Start is called before the first frame update
    void Start()
    {
        RandomTarget();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SetAnimationClip()
    {
        //animator.SetFloat("speed", 1.2f);
    }

    public FishBody SetBody(string id)
    {
        string[] idSplit = id.Split(" ");
        ids.body = idSplit[0];
        ids.pattern= idSplit[1];
        return SetBody(FishFactory.GetBody(id));
    }

    FishBody SetBody(FishBody original)
    {
        Transform parent = transform.Find("Body");

        //Remove current body
        if (parent.childCount > 0)
        {
            GameObject.Destroy(parent.GetChild(0).gameObject);
        }

        //Clone selected body
        GameObject clone = Instantiate(original.gameObject, parent);
        clone.transform.localPosition = new Vector3(0, 0, 0);
        clone.transform.localScale = new Vector3(1, 1, 1);
        return clone.GetComponent<FishBody>();

    }

    public FishHead SetHead(string id)
    {
        ids.head = id;
        return SetHead(FishFactory.GetHead(id));
    }

    FishHead SetHead(FishHead original)
    {
        Transform parent = transform.Find("Head");

        //Remove current head
        if (parent.childCount > 0)
        {
            GameObject.Destroy(parent.GetChild(0).gameObject);
        }

        //Clone selected head
        GameObject clone = Instantiate(original.gameObject, parent);
        clone.transform.localPosition = new Vector3(0, 0, 0);
        clone.transform.localScale = new Vector3(1, 1, 10);

        return clone.GetComponent<FishHead>();

        //SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
    }

    void RandomTarget ()
    {
        target = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-10f, 10f));
        tweenTarget = transform.DOLocalMove(target, Random.Range(5,15));
        tweenTarget.OnComplete(RandomTarget);
    }

}
