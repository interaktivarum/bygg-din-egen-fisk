using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;
using TMPro;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using System.Net.NetworkInformation;

public struct FishIDs
{
    public string body;
    public string head;
    public string pattern;
}

public enum FishState
{
    idle,
    returning,
    hunting,
    attacking,
    escaping,
    dying
}

public class Fish : MonoBehaviour
{

    //Fish parts
    public FishIDs ids;
    public Bounds boundsSpawnOverride;
    public FishBody body;
    public FishHead head;

    // Behavoiur & animation
    Vector3 target;
    public bool targetSet = false;

    // Energy
    public float energy;
    //bool hunting = false;

    // States & timers
    public FishState state = FishState.idle;
    float timeHunt;
    Fish attacking = null;
    float timeAttack;
    float attackSuccessThreshold;
    Fish attacked = null;
    //bool escaping;
    float timeEscape;
    //bool dying;

    // Misc
    float seed;

    // Start is called before the first frame update
    void Start()
    {
        seed = Random.value;
        ResetEnergy();

        transform.LookAt(transform.position + new Vector3(1,0,0));

        //RandomTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (state != FishState.dying)
        {
            MoveUpdate();

            AttackUpdate();
            EscapeUpdate();

            EnergyUpdate();
        }

    }

    float GetSpeed (float magnitude)
    {
        switch (state)
        {
            case FishState.returning:
                return body.speed * 2;// * Mathf.Lerp(0.2f, 1, magnitude / 16); ;
            case FishState.attacking:
                return body.speedAttack;// * Mathf.Lerp(0.2f, 1, magnitude / 32); ;
            case FishState.escaping:
                return body.speedEscape;// * Mathf.Lerp(0.2f, 1, magnitude / 32); ;
            default:
                return body.speed * Mathf.Lerp(0.2f, 1, magnitude / 16);
        }
    }

    float GetRotationSpeed()
    {
        return body.rotationSpeed * (IsStressed() ? 5 : 1);
    }

    float GetAcceptableTargetDistance()
    {
        if (state == FishState.attacking)
        {
            return attackSuccessThreshold;
        }
        if (state != FishState.idle)
        {
            return 4;
        }
        //else if (body.idleMovement == IdleMovement.back && IsIdle())
        //{
        //    return 1f;
        //}
        return 2;
    }

    void MoveUpdate()
    {
        if (body)
        {
            Vector3 dir = (target - transform.position);

            if (targetSet && dir.magnitude < GetAcceptableTargetDistance())
            {
                TargetReached();
            }

            //if (dir.magnitude < GetAcceptableTargetDistance())
            //{
            if (body.idleMovement == IdleMovement.rotate)
                {
                    MoveRotationUpdate(dir);
                }
                else if (body.idleMovement == IdleMovement.back)
                {
                    if (IsIdle() && dir.magnitude < GetAcceptableTargetDistance())
                    {
                        MoveBackUpdate();
                    }
                    else
                    {
                        MoveRotationUpdate(dir);
                    }
                }
            //}
        }
    }

    void MoveRotationUpdate (Vector3 dir)
    {
        //if (dir.magnitude < GetAcceptableTargetDistance())
        //{
        //    TargetReached();
        //}
        //else {
            Vector3 _direction = dir.normalized;
            //create the rotation we need to be in to look at the target
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * GetRotationSpeed());

            //gameObject.GetComponentInChildren<BodyFront>().transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime);

            transform.position = transform.position + transform.forward * GetSpeed(dir.sqrMagnitude) * 0.01f;
        //}

    }

    void MoveBackUpdate()
    {
        Vector3 dirCenter = transform.parent.position - transform.position;
        Vector3 _direction = dirCenter.normalized;
        //create the rotation we need to be in to look at the target
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * GetRotationSpeed());

        //gameObject.GetComponentInChildren<BodyFront>().transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime);

        //transform.position += transform.forward * body.speed * Mathf.Sin(Time.fixedTime + seed);
        transform.GetChild(0).position += transform.forward * body.speed * 0.01f * Mathf.Sin(Time.fixedTime * (1 + seed)) / (2 + seed);
        //transform.GetChild(0).position += transform.GetChild(0).forward * body.speed * Mathf.Sin(Time.fixedTime) / (5 + Random.value);
    }

    void TargetReached() {

        targetSet = false;
        //Bounds boundsNext = body.boundsHome;

        //if (InBounds(head.boundsEat))
        //{
        //    energy = Mathf.Min(energy + body.energyFloor, body.energyStart);
        //}

        switch (state) {
            case FishState.idle:
                if (body.idleMovement == IdleMovement.rotate)
                {
                    RandomTarget(body.boundsHome);
                }
                break;
            case FishState.returning:
                state = FishState.idle;
                if (body.idleMovement == IdleMovement.rotate)
                {
                    RandomTarget();
                }
                break;
            case FishState.hunting:
                float energyAdd = 0;
                if (ids.head == "Floor")
                {
                    energyAdd = body.energyFloor;
                }
                else if (ids.head == "Bite")
                {
                    energyAdd = body.energyBite;
                }
                else if (ids.head == "Swallow")
                {
                    energyAdd = body.energySwallow;
                }
                energy = Mathf.Min(energy + energyAdd, body.energyStart);
                state = FishState.idle;
                
                RandomTarget();
                break;
            case FishState.attacking:
                energy += Mathf.Min(energy + body.energyBite, body.energyStart);
                AttackSuccess();
                break;
            case FishState.escaping:
                RandomTarget();
                break;
        }
    }

    bool IsIdle()
    {
        //return !(IsStressed() || hunting);
        return state == FishState.idle;
    }

    bool IsStressed()
    {
        return
            attacking || attacked ||
            state == FishState.attacking || state == FishState.escaping;
        //return attacking || attacked || escaping;
    }

    void ResetEnergy ()
    {
        energy = body.energyStart + Random.value * 15;
    } 

    void EnergyUpdate ()
    {
        if (Time.fixedTime - timeHunt > 30 && energy < body.energyStart / 2)
        {
            RandomTarget(head.boundsEat);
            state = FishState.hunting;
            timeHunt = Time.fixedTime;
        }
        ReduceEnergy();
    }

    void ReduceEnergy ()
    {
        energy -= Time.deltaTime;
        if(energy < 0 && !IsStressed())
        {
            Die();
        }
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

        RandomTarget(body.boundsHome);

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
    }

    public void RandomTarget ()
    {
        RandomTarget(body.boundsHome);
    }

    void RandomTarget (Bounds bounds)
    {
        Bounds b = boundsSpawnOverride.extents.x > 0 ? boundsSpawnOverride : bounds;
        Vector3 t = new Vector3(
            Random.Range(b.center.x-b.extents.x/2, b.center.x + b.extents.x / 2),
            Random.Range(b.center.y - b.extents.y / 2, b.center.y + b.extents.y / 2),
            Random.Range(b.center.z - b.extents.z / 2, b.center.z + b.extents.z / 2));
        SetTarget(transform.parent.position + t);
    }

    void SetTarget (Vector3 t)
    {
        target = t;
        targetSet = true;
    }

    public bool IsHome()
    {
        return InBounds(body.boundsHome);
    }

    public bool InBounds(Bounds bounds)
    {
        return bounds.Contains(transform.position - transform.parent.position);
    }

    bool DiscoverTest(Fish other)
    {
        float t = 1f;
        switch (other.ids.body)
        {
            case "Flat":
                if (other.ids.pattern == "Gold" && other.IsHome())
                {
                    t = 0.1f;
                }
                if (other.ids.pattern == "Silver" && !other.IsHome())
                {
                    t = 0.1f;
                }
                break;
            case "Eel":
                if (other.ids.pattern == "Gold" && other.IsHome())
                {
                    t = 0.1f;
                }
                if (other.ids.pattern == "Silver" && !other.IsHome())
                {
                    t = 0.1f;
                }
                break;
            case "Long":
                if (other.ids.pattern == "Silver")
                {
                    t = 0.1f;
                }
                break;
        }
        return Random.value < t;
    }

    float AttackSuccessTest(Fish other, float min, float max)
    {
        float t = 1f;
        if (other.ids.body == "Flat")
        {
            if (other.ids.pattern == "Silver")
            {
                t = other.IsHome() ? 1 : 0;
            }
            else if (other.ids.pattern == "Gold")
            {
                t = other.IsHome() ? 0 : 1;
            }
        }
        
        //Debug.Log("Attacked: " + other.ids.body + " " + other.ids.pattern + " " + other.IsHome());

        return Mathf.Lerp(min, max, t);
    }

    public void AttackStart(Fish other)
    {
        if (this.ids.body != other.ids.body && body.sizesEat.Contains(other.body.size))
        {
            if (DiscoverTest(other))
            {
                if (Time.fixedTime - timeAttack > 10)
                {
                    if (!(IsStressed() || other.attacking || other.attacked || other.state == FishState.returning || other.state == FishState.dying))
                    {
                        state = FishState.attacking;
                        energy -= body.energyAttackCost;
                        attackSuccessThreshold = AttackSuccessTest(other, 0.5f, 4);
                        //Debug.Log("Success threshold: " + attackSuccessThreshold);
                        attacking = other;
                        other.attacked = this;
                        timeAttack = Time.fixedTime;
                    }
                }
            }
            
        }
    }

    void AttackUpdate ()
    {
        if (attacking)
        {
            SetTarget(transform.parent.position + attacking.transform.localPosition);
            if(Time.fixedTime - timeAttack > 1)
            {
                Debug.Log("Attack unsuccessful: " + attackSuccessThreshold);
                AttackAbort();
            }
        }
    }

    void AttackSuccess()
    {
        Debug.Log("Attack success: " + attackSuccessThreshold);
        attacking.Die();
        AttackAbort();
    }

    void AttackAbort()
    {
        attacking.attacked = null;
        attacking = null;

        // Return quickly to home
        RandomTarget();
        state = FishState.returning;
        timeEscape = Time.fixedTime;
    }

    public void EscapeStart (Fish other)
    {
        if (this.ids.body != other.ids.body && other.body.sizesEat.Contains(body.size))
        {
            if (DiscoverTest(other))
            {
                //Debug.Log("Escaping: " + ids.body + " < " + other.ids.body);
                state = FishState.escaping;
                //escaping = true;
                timeEscape = Time.fixedTime;

                // Escape home and abort hunt
                RandomTarget();
                //hunting = false;
            }
        }
    }

    void EscapeUpdate()
    {
        if((state == FishState.escaping && Time.fixedTime - timeEscape > 0.2) && !attacked)
        {
            state = FishState.returning;
        }
    }

    public void Die()
    {
        if (state != FishState.dying)
        {
            state = FishState.dying;
            transform.DOScale(new Vector3(0, 0, 0), 0.5f).OnComplete(DoDie);
        }
    }

    void DoDie()
    {
        GetComponentInParent<FishManager>().RepopulateTest();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
    }

}
