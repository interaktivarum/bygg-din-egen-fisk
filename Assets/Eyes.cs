using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Eyes : MonoBehaviour
{

    float timeDisapppear;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x == 0 && Time.time - timeDisapppear > 10) {
            transform.DOScale(0.5f, 10f);
        }
    }

    private void OnMouseDown() {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(0.75f, 0.1f));
        sequence.AppendInterval(0.5f);
        sequence.Append(transform.DOScale(0, 0.1f));
        sequence.Play();
        timeDisapppear = Time.time;
    }
}
