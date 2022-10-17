using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;
using TMPro;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public struct FishIDs
{
    public string body;
    public string head;
    public string pattern;
}

public class Fish : MonoBehaviour
{

    //Fish parts
    public FishIDs ids;
    public Bounds boundsSpawnOverride;
    FishBody body;
    FishHead head;

    // Behavoiur & animation
    Vector3 target;
    float energyStart = 30f; 
    public float energy;
    bool hunting = false;
    //Tween tweenTarget;

    // Start is called before the first frame update
    void Start()
    {

        ResetEnergy();

        Debug.Log(transform.forward);
        //transform.forward = new Vector3(1, 0, 0);
        Debug.Log(transform.forward);

        transform.LookAt(transform.position + new Vector3(1,0,0));

        //RandomTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(gameObject);
            Debug.Log(transform);
            Debug.Log(body);
            RandomTarget(body.boundsSpawn);
        }
        //Debug.Log(body);

        updateRotation();

        reduceEnergy();

    }

    void updateRotation ()
    {
        if (body)
        {
            Vector3 _dir = (target - transform.position);

            if (_dir.magnitude < 2)
            {
                TargetReached();
            }

            else
            {
                Vector3 _direction = _dir.normalized;
                //create the rotation we need to be in to look at the target
                Quaternion _lookRotation = Quaternion.LookRotation(_direction);

                //rotate us over time according to speed until we are in the required rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * body.rotationSpeed);

                //gameObject.GetComponentInChildren<BodyFront>().transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime);

                transform.position = transform.position + transform.forward * 0.01f;
            }
        }
    }

    void TargetReached () {
        if (hunting)
        {
            ResetEnergy();
            hunting = false;
        }

        // Set new target
        if(energy < 0)
        {
            RandomTarget(head.boundsEat);
            hunting = true;
        }
        else
        {
            RandomTarget(body.boundsSpawn);
        }
    }

    void reduceEnergy ()
    {
        energy -= Time.deltaTime;
    }

    void ResetEnergy ()
    {
        energy = energyStart + Random.Range(-5, 5);
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
        Debug.Log(id);
        FishBody fb = SetBody(FishFactory.GetBody(id));
        return fb;
    }

    FishBody SetBody(FishBody original)
    {
        Transform parent = transform.Find("Parts/Body");

        //Remove current body
        if (parent.childCount > 0)
        {
            GameObject.Destroy(parent.GetChild(0).gameObject);
        }

        //Clone selected body
        GameObject clone = Instantiate(original.gameObject, parent);
        clone.transform.localPosition = new Vector3(0, 0, 0);
        clone.transform.localScale = new Vector3(1, 1, 1);

        //body = clone;
        //Debug.Log(body);

        body = clone.GetComponentInChildren<FishBody>();

        RandomTarget(body.boundsSpawn);

        return clone.GetComponentInChildren<FishBody>();

    }

    public FishHead SetHead(string id)
    {
        ids.head = id;
        return SetHead(FishFactory.GetHead(id));
    }

    FishHead SetHead(FishHead original)
    {
        Transform parent = transform.Find("Parts/Head");

        //Remove current head
        if (parent.childCount > 0)
        {
            GameObject.Destroy(parent.GetChild(0).gameObject);
        }

        //Clone selected head
        GameObject clone = Instantiate(original.gameObject, parent);
        clone.transform.localPosition = new Vector3(0, 0, 0);
        clone.transform.localScale = new Vector3(1, 1, 10);

        head = clone.GetComponent<FishHead>();

        return head;

        //SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
    }

    void RandomTarget (Bounds bounds)
    {
        //target = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0);
        Bounds b = boundsSpawnOverride.extents.x > 0 ? boundsSpawnOverride : bounds;
        target = new Vector3(
            Random.Range(b.center.x-b.extents.x/2, b.center.x + b.extents.x / 2),
            Random.Range(b.center.y - b.extents.y / 2, b.center.y + b.extents.y / 2),
            Random.Range(b.center.z - b.extents.z / 2, b.center.z + b.extents.z / 2));
        target = transform.parent.position + target;
        //tweenTarget = transform.DOLocalMove(target, Random.Range(5, 15));
        //tweenTarget.OnComplete(RandomTarget);

        ////Debug.Log("Position / target / parent+target:");
        ////Debug.Log(transform.position);
        ////Debug.Log(target);
        ////Debug.Log(transform.parent.position + target);

        //transform.LookAt(transform.parent.position + target);

    }

}
