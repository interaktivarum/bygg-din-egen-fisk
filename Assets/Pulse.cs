using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Pulse : MonoBehaviour
{

    Tween tween;
    public float scaleMin = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        //StartTween();
        tween = transform.DOScale(new Vector3(scaleMin, scaleMin, scaleMin), 1).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartTween() {
        //DOTween.Play(tween);
        tween.Play();
        //tween = transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopTween() {
        tween.Pause();
        //DOTween.Pause(tween);
        transform.DOScale(new Vector3(1, 1, 1), 1);
    }

}
