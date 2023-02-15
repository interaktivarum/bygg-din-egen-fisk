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

public enum BodyID
{
    Flat,
    Eel,
    Long,
    Mini
}

public enum HeadID
{
    Swallow,
    Floor,
    Bite
}

public enum PatternID
{
    Stripes,
    Dots,
    Silver
}


public struct FishIDs
{
    public string body;
    public string pattern;
    public string head;
    public override string ToString ()
    {
        return body + " " + pattern + " " + head;
    }
}

public enum FishState
{
    building,
    released,
    idle,
    returning,
    hunting,
    attacking,
    escaping,
    killed,
    dying
}

public class Fish : MonoBehaviour
{

    //Fish parts
    //public FishIDs ids;
    public Bounds boundsHomeOverride;
    public FishBody body;
    public FishHead head;

    // Behavoiur & animation
    Vector3 target;
    public bool targetSet = false;

    // Energy
    public float energy;
    //bool hunting = false;

    // States & timers
    public FishState state;
    float timeHunt = -100;
    Fish attacking = null;
    float timeAttack;
    float attackSuccessThreshold;
    Fish attacked = null;
    //bool escaping;
    float timeEscape;
    //bool dying;

    // Misc
    float seed;
    FishManager fishManager;

    [Header("Prefabs")]
    Tween tweenWarning;

    public override string ToString()
    {
        return body.idBody.ToString() + body.idPattern.ToString() + head.idHead.ToString();
    }

    private void Awake()
    {
        fishManager = FindObjectOfType<FishManager>();
        state = FishState.idle;

        float w = 1;
        tweenWarning = DOTween.To(() => w, SetWarning, 0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        tweenWarning.Pause();
    }

    // Start is called before the first frame update
    void Start()
    {

        seed = Random.value;
        ResetEnergy();
        //energy = body.energyStart * Random.Range(0.2f, 0.5f);

        transform.LookAt(transform.position + new Vector3(1,0,0));


        //RandomTarget();
        if (state != FishState.building) {
            RandomTarget();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Alive())
        {
            MoveUpdate();
            if (state != FishState.building) {

                AttackUpdate();
                EscapeUpdate();

                EnergyUpdate();
            }
        }

    }

    bool Alive() {
        return !(state == FishState.killed || state == FishState.dying);
    }

    //private void LateUpdate()
    //{
    //    MoveUpdate();
    //}

    public float GetSpeed (float magnitude = 1)
    {
        float s = 0;
        switch (state)
        {
            case FishState.returning:
                s = body.speed * 2;// * Mathf.Lerp(0.2f, 1, magnitude / 16); ;
                break;
            case FishState.released:
                s = body.speed * 3;// * Mathf.Lerp(0.2f, 1, magnitude / 16); ;
                break;
            case FishState.attacking:
                s = body.speedAttack;// * Mathf.Lerp(0.2f, 1, magnitude / 32); ;
                //if(body.idBody == BodyID.Long) {
                //    if(InArea(fishManager.areaCoral) || InArea(fishManager.areaWeed)) {
                //        s = 1;
                //    }
                //}
                break;
            case FishState.escaping:
                s = body.speedEscape;// * Mathf.Lerp(0.2f, 1, magnitude / 32); ;
                break;
            default:
                s = body.speed * Mathf.Lerp(0.2f, 1, magnitude / 16);
                //s = body.speed;
                break;
        }
        return s * Mathf.Pow(Mathf.Max(GetBend(),0),8);
    }

    public float GetRotationSpeed()
    {
        return body.rotationSpeed * (IsStressed() ? 5 : 0.5f);
    }

    float GetBend()
    {
        if (body.bends.Length > 0)
        {
            float dot = Vector3.Dot(body.bends[0].transform.forward.normalized, body.lastBend.transform.forward.normalized);
            return dot;
        }
        return 1;
    }

    float GetAcceptableTargetDistance()
    {
        if (state == FishState.attacking)
        {
            return 1;
            //return attackSuccessThreshold;
        }
        if (attacked || state == FishState.attacking)
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

            foreach (Bend b in GetComponentsInChildren<Bend>())
            {
                b.SetPrev();
            }

            if (body.idleMovement == IdleMovement.rotate)
            {
                MoveRotationUpdate(dir);
            }
            else if (body.idleMovement == IdleMovement.back)
            {
                if (IsIdle() && dir.magnitude < GetAcceptableTargetDistance() * 1.5f)
                {
                    MoveBackUpdate();
                }
                else
                {
                    MoveRotationUpdate(dir);
                }
            }

            foreach (BoneWave w in GetComponentsInChildren<BoneWave>())
            {
                w.UpdateBoneWave(seed, 1);
            }

            foreach (Bend b in GetComponentsInChildren<Bend>())
            {
                b.UpdateRotation();
            }
        }
    }

    void MoveRotationUpdate (Vector3 dir)
    {
        //Debug.Log("New rotation");
        Vector3 _direction = dir.normalized;

        //create the rotation we need to be in to look at the target
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);

        //foreach (Bend b in GetComponentsInChildren<Bend>())
        //{
        //    b.SetPrev();
        //}

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * GetRotationSpeed());
        //Debug.Log(transform.rotation.eulerAngles - rotPrev);

        //transform.Find("Parts").LookAt(transform.Find("SinTarget"));

        //Transform parts = transform.Find("Parts");
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + Time.deltaTime * 20 * new Vector3(Mathf.Sin(Time.fixedTime * body.swingSpeed * seed), Mathf.Sin(Time.fixedTime * body.swingSpeed + seed),0));

        //move forward
        transform.position = transform.position + transform.forward * GetSpeed(dir.sqrMagnitude) * Time.deltaTime * 2;

        //bend body
        //foreach(Bend b in GetComponentsInChildren<Bend>())
        //{
        //    b.UpdateRotation();
        //}
        //body.boneBend.transform.rotation = new Quaternion();
        //body.boneBend.transform.rotation = Quaternion.Slerp(rotPrev, body.boneBend.transform.rotation, 0.01f);
        GetBend();
    }

    void MoveBackUpdate()
    {
        Vector3 dirCenter = transform.parent.position - transform.position;
        Vector3 _direction = dirCenter.normalized;
        //create the rotation we need to be in to look at the target
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * GetRotationSpeed());

        //transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + 0.1f * new Vector3(Mathf.Sin(Time.fixedTime * body.swingSpeed), 0, 0));
        //gameObject.GetComponentInChildren<BodyFront>().transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime);

        //transform.position += transform.forward * body.speed * Mathf.Sin(Time.fixedTime + seed);
        transform.GetChild(0).position += transform.forward * body.speed * Time.deltaTime * Mathf.Sin(Time.fixedTime * (1 + seed)) / (2 + seed);
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
            case FishState.building:
                RandomTarget(boundsHomeOverride);
                break;
            case FishState.released:
                state = FishState.idle;
                RandomTarget(body.boundsHome);
                break;
            case FishState.idle:
                state = FishState.idle;
                RandomTarget(body.boundsHome);
                break;
            case FishState.returning:
                state = FishState.idle;
                if (body.idleMovement == IdleMovement.rotate)
                {
                    RandomTarget();
                }
                break;
            case FishState.hunting:
                if (head.idHead == HeadID.Floor)
                {
                    EnergyAdd(body.energyFloor);
                    head.SetBite(true);
                    fishManager.app.timeHelper.WaitAndCallFunction(() => { RandomTarget(); head.SetBite(false); }, 3);    
                }
                else {
                    RandomTarget();
                }
                state = FishState.idle;
                   
                //RandomTarget();
                break;
            case FishState.attacking:
                if (head.idHead == HeadID.Bite)
                {
                    EnergyAdd(body.energyBite);
                }
                else if (head.idHead == HeadID.Swallow)
                {
                    EnergyAdd(body.energySwallow);
                }
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

    bool EnergyLow ()
    {
        return energy < body.energyStart / 2;
    }

    void EnergyAdd(float add) {
        energy = Mathf.Min(energy + add, body.energyStart);
        if (energy > 10) {
            WarningPause();
        }
    }

    public void ResetEnergy ()
    {
        energy = body.energyStart + Random.value * 15;
    } 

    void EnergyUpdate ()
    {
        float energyPrev = energy;
        if (Time.fixedTime - timeHunt > 30 && EnergyLow() && AttackTimeTest())
        {
            HuntStart();
        }
        ReduceEnergy();
        if(energyPrev > 10 && energy < 10) {
            //AnimateEnergyAlpha(0, 10);
            tweenWarning.Play();
        }
    }

    //void AnimateEnergyAlpha (float targetEnergy, float seconds) {
    //    float targetAlpha = Mathf.Lerp(0, 1, targetEnergy / 10);
    //    float a = body.GetComponent<SpriteRenderer>().color.a;
    //    if (tweenAlpha != null) {
    //        tweenAlpha.Kill();
    //    }
    //    tweenAlpha = DOTween.To(() => a, SetEnergyAlpha, targetAlpha, seconds).SetEase(Ease.InOutFlash);
    //}

    public void WarningPlay() {
        tweenWarning.Play();
    }

    public void WarningPause() {
        tweenWarning.Pause();
        WarningReset();
    }

    void SetWarning (float a) {
        //float a = Mathf.Lerp(0, 1, energy / 10);
        body.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
        head.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
        //body.GetComponent<SpriteRenderer>().DOFade(value, seconds);
        //head.GetComponent<SpriteRenderer>().DOFade(value, seconds);
    }

    void WarningReset() {
        float w = 1;
        DOTween.To(() => w, SetWarning, 1, 0.5f);
    }

    void ReduceEnergy ()
    {
        energy -= Time.deltaTime;
        if(energy < 0 && !IsStressed())
        {
            //Die();
            DieEnergy();
        }
        //if(energy < 10) {
        //    UpdateEnergyAlpha();
        //}
    }

    void HuntStart()
    {
        RandomTarget(head.boundsEat);
        state = FishState.hunting;
        timeHunt = Time.fixedTime;
    }

    void SetAnimationClip()
    {
        //animator.SetFloat("speed", 1.2f);
    }

    //public FishBody SetBody(BodyID idBody, PatternID idPattern)
    //{
    //    ids.body = idBody;
    //    ids.pattern = idPattern;
    //    FishBody fb = SetBody(fishManager.GetBody(idBody));
    //    return fb;
    //}

    public FishBody SetBody(FishBody original)
    {
        //Set ids
        //string[] idSplit = original.idBody.Split(" ");
        //ids.body = idSplit[0];
        //ids.pattern = idSplit[1];

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

        return clone.GetComponentInChildren<FishBody>();

    }

    //public FishHead SetHead(string id)
    //{
    //    ids.head = id;
    //    return SetHead(fishManager.GetHead(id));
    //}

    public FishHead SetHead(FishHead original)
    {
        //Set ids
        //ids.head = original.idHead;

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

    public void RandomTarget (Bounds bounds)
    {
        Bounds b = bounds;// boundsHomeOverride.extents.x > 0 ? boundsHomeOverride : bounds;
        Vector3 t = new Vector3(
            Random.Range(b.center.x-b.extents.x/2, b.center.x + b.extents.x / 2),
            Random.Range(b.center.y - b.extents.y / 2, b.center.y + b.extents.y / 2),
            Random.Range(b.center.z - b.extents.z / 2, b.center.z + b.extents.z / 2));
        SetTarget(transform.parent.position + t);
        //AnimateTarget(transform.parent.position + t);
    }

    void AnimateTarget (Vector3 t) {
        DOTween.To(() => target, x => target = x, t, 1);
    }

    void SetTarget (Vector3 t)
    {
        target = t;
        targetSet = true;
    }

    public bool IsHome() {
        return InBounds(body.boundsHome);
    }

    public bool InBounds(Bounds bounds)
    {
        return bounds.Contains(transform.position - transform.parent.position);
    }

    public bool InArea(RectTransform area) {
        Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);
        return RectTransformUtility.RectangleContainsScreenPoint(area, sp, Camera.main);
    }

    bool IsCamouflaged()
    {
        if (body.idPattern == PatternID.Stripes && InArea(fishManager.areaWeed)) {
            return true;
        }
        else if (body.idPattern == PatternID.Dots && InArea(fishManager.areaCoral)) {
            return true;
        }
        else if (body.idPattern == PatternID.Silver && !(InArea(fishManager.areaWeed) || InArea(fishManager.areaCoral))) {
            return true;
        }
        //switch (body.idBody)
        //{
        //    case BodyID.Flat:
        //        if (body.idPattern == PatternID.Stripes && IsHome())
        //        {
        //            return true;
        //        }
        //        if (body.idPattern == PatternID.Silver && !IsHome())
        //        {
        //            return true;
        //        }
        //        break;
        //    case BodyID.Eel:
        //        if (body.idPattern == PatternID.Dots && IsHome())
        //        {
        //            return true;
        //        }
        //        //if (body.idPattern == PatternID.Silver && !IsHome())
        //        //{
        //        //    return true;
        //        //}
        //        break;
        //    case BodyID.Long:
        //        if (body.idPattern == PatternID.Silver)
        //        {
        //            return true;
        //        }
        //        break;
        //    case BodyID.Mini:
        //        if (IsHome())
        //        {
        //            return true;
        //        }
        //        break;
        //}
        return false;
    }

    bool DiscoverTest(Fish other)
    {
        //float t = other.IsCamouflaged() ? 0.1f : 0.9f;
        //return Random.value < t;
        return !other.IsCamouflaged();
    }

    bool AttackTimeTest ()
    {
        return Time.fixedTime - timeAttack > 20;
    }

    float AttackSuccessTest(Fish other, float min, float max)
    {
        float t = other.IsCamouflaged() ? 0 : 1;
        return Mathf.Lerp(min, max, t);
    }

    public void AttackStart(Fish other)
    {
        if (Alive()) {
            if (this.body.idBody != other.body.idBody && EnergyLow()) {
                if ((head.idHead == HeadID.Bite && body.sizesEat.Contains(other.body.size)) ||
                    (head.idHead == HeadID.Swallow && other.body.idBody == BodyID.Mini)) {
                    if (DiscoverTest(other)) {
                        if (AttackTimeTest()) {
                            if (!(IsStressed() || other.attacking || other.attacked || other.state == FishState.returning || !other.Alive())) {
                                state = FishState.attacking;
                                head.SetBite(true);
                                energy -= body.energyAttackCost;
                                //attackSuccessThreshold = AttackSuccessTest(other, 0.5f, 3);
                                //Debug.Log("Success threshold: " + attackSuccessThreshold);
                                attacking = other;
                                other.attacked = this;
                                timeAttack = Time.fixedTime;
                            }
                        }
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

            if (body.idBody == BodyID.Long) {
                if (InArea(fishManager.areaCoralThick) || InArea(fishManager.areaWeedThick)) {
                    AttackAbort();
                }
            }

            if (Time.fixedTime - timeAttack > 1)
            {
                //Debug.Log("Attack unsuccessful: " + attackSuccessThreshold);
                AttackLog(attacking, false);
                AttackAbort();
            }
        }
    }

    void AttackSuccess()
    {
        AttackLog(attacking, true);
        BiteAndHold();
        AttackAbort();
    }

    void BiteAndHold() {
        attacking.transform.parent = head.transform;
        //attacking.transform.localPosition = new Vector3();
        //attacking.transform.Find("Parts").localPosition = new Vector3();
        attacking.transform.DOLocalMove(new Vector3(), 0.5f).SetEase(Ease.OutCubic);
        attacking.transform.Find("Parts").DOLocalMove(new Vector3(), 0.5f).SetEase(Ease.OutCubic);
        attacking.WarningPause();
        //attacking.transform.localRotation = Quaternion.Euler(0,180,90);
        attacking.transform.DOLocalRotate(new Vector3(0, 180, 90), 0.5f).SetEase(Ease.OutCubic);
        float dieTime = head.idHead == HeadID.Bite ? 10 : 0.5f;
        attacking.Die(dieTime);
    }

    void AttackAbort()
    {
        attacking.attacked = null;
        attacking = null;
        head.SetBite(false);

        // Return quickly to home
        RandomTarget();
        state = FishState.returning;
    }

    void AttackLog(Fish other, bool success)
    {
        string str = "";
        str += "Attack " + (success ? "success" : "fail") +
            " (" + attackSuccessThreshold + ", " +
            " weed:" + other.InArea(fishManager.areaWeed) + "/" +
            " coral: " + other.InArea(fishManager.areaCoral) + "\n";
        str += body.idBody.ToString() + body.idPattern.ToString() + head.idHead.ToString() + " > ";
        str += other.body.idBody.ToString() + other.body.idPattern.ToString() + other.head.idHead.ToString() + ".";
        Debug.Log(str);
    }

    public void EscapeStart (Fish other)
    {
        if (Alive()) {
            if (this.body.idBody != other.body.idBody &&
                (other.head.idHead == HeadID.Bite && other.body.sizesEat.Contains(body.size) && other.state != FishState.idle)
                || other.head.idHead == HeadID.Swallow && body.idBody == BodyID.Mini) {
                if (DiscoverTest(other)) {
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
    }

    void EscapeUpdate()
    {
        if((state == FishState.escaping && Time.fixedTime - timeEscape > 0.2) && !attacked)
        {
            if (EnergyLow())
            {
                HuntStart();
            }
            else
            {
                state = FishState.returning;
            }
        }
    }

    void DieEnergy() {
        transform.DOLocalRotate(new Vector3(0, 90, 180), 3);
        foreach(Animator a in GetComponentsInChildren<Animator>()) {
            a.speed = 0;
            a.StopPlayback();
        }
        transform.DOLocalMoveY(transform.localPosition.y + 5, 10);
        WarningPause();
        Die(10, true);
    }

    public void Die(float seconds = 0.5f, bool fade = false)
    {
        if (state != FishState.dying)
        {
            state = FishState.dying;
            if (fade) {
                float w = 1;
                tweenWarning = DOTween.To(() => w, SetWarning, 0, seconds).OnComplete(DoDie);
            }
            else {
                transform.DOScale(new Vector3(0, 0, 0), seconds).SetEase(Ease.InCubic).OnComplete(DoDie);
            }
        }
    }

    void DoDie()
    {
        fishManager.RepopulateTest();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
    }

}
