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
    Fish attacking = null;
    Fish attacked = null;
    float attackStart;

    bool escaping;
    float timeEscape;
    //Tween tweenTarget;

    // Start is called before the first frame update
    void Start()
    {

        ResetEnergy();

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

        UpdateRotation();

        UpdateAttack();
        UpdateAttacked();
        EscapeUpdate();

        reduceEnergy();

    }

    float GetSpeed (float magnitude)
    {
        if (!(attacking || attacked || escaping))
        {
            return body.speed * Mathf.Lerp(0.2f, 1, magnitude / 16);
        }
        else
        {
            return body.speed * 10;
        }
    }

    float GetRotationSpeed()
    {
        return body.rotationSpeed * (attacking || attacked || escaping ? 5 : 1);
    }

    float GetAcceptableTargetDistance()
    {
        return attacking ? 0.5f : 2f;
    }

    void UpdateRotation ()
    {
        if (body)
        {
            Vector3 _dir = (target - transform.position);

            if (_dir.magnitude < GetAcceptableTargetDistance())
            {
                TargetReached();
            }

            else
            {
                Vector3 _direction = _dir.normalized;
                //create the rotation we need to be in to look at the target
                Quaternion _lookRotation = Quaternion.LookRotation(_direction);

                //rotate us over time according to speed until we are in the required rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * GetRotationSpeed());

                //gameObject.GetComponentInChildren<BodyFront>().transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime);

                transform.position = transform.position + transform.forward * GetSpeed(_dir.sqrMagnitude);
            }
        }
    }

    void TargetReached () {
        if (hunting)
        {
            ResetEnergy();
            hunting = false;
        }

        if (attacking)
        {
            Debug.Log("Attack success!");
        }

        // Set new target
        if (energy < 0)
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
        clone.transform.localScale = new Vector3(1, 1, 1);

        head = clone.GetComponent<FishHead>();

        return head;

        //SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
    }

    public void RandomTarget ()
    {
        RandomTarget(body.boundsSpawn);
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

    public void Attack(Fish other)
    {
        if (this.ids.body != other.ids.body && body.sizesEat.Contains(other.body.size))
        {
            Debug.Log("Attack? " + this.ids.body + " > " + other.ids.body);
            if (!attacking && !attacked && !other.attacking && !other.attacked)
            {
                if (Time.fixedTime - attackStart > 10)
                {
                    Debug.Log("Attack!");
                    attacking = other;
                    other.attacked = this;
                    attackStart = Time.fixedTime;
                }
            }
            
        }
    }

    void UpdateAttack ()
    {
        if (attacking)
        {
            target = transform.parent.position + attacking.transform.localPosition;
            if(Time.fixedTime - attackStart > 2)
            {
                attacking.attacked = null;
                attacking.RandomTarget();
                attacking = null;
                RandomTarget();
            }
        }
    }

    void UpdateAttacked()
    {
        if (attacked)
        {
            if (Random.value > 0.001)
            {
                RandomTarget();
                //RandomTarget(body.boundsEscape);
            }
        }
    }

    public void EscapeStart (Fish other)
    {
        if (this.ids.body != other.ids.body && other.body.sizesEat.Contains(body.size))
        {
            Debug.Log("Escaping: " + ids.body + " < " + other.ids.body);
            escaping = true;
            timeEscape = Time.fixedTime;
        }
    }

    void EscapeUpdate()
    {
        if(escaping && Time.fixedTime - timeEscape > 1)
        {
            escaping = false;
        }
    }

}
