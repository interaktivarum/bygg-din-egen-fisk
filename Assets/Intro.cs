using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Intro : MonoBehaviour
{
    public CanvasGroup cg;
    public CanvasGroup cgCorals;
    public RectTransform corals;
    float yInit;
    public bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        yInit = corals.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show() {
        if (!active) {
            active = true;
            cg.blocksRaycasts = true;
            cgCorals.blocksRaycasts = true;
            Sequence seq = DOTween.Sequence();
            seq.Append(corals.DOLocalMoveY(yInit, 3));
            seq.Append(cg.DOFade(1, 1));
        }
    }

    public void Hide() {
        if (active) {
            active = false;
            cg.blocksRaycasts = false;
            cgCorals.blocksRaycasts = false;
            cg.DOFade(0, 1);
            corals.DOLocalMoveY(yInit - 1200, 3);
        }
    }

}
