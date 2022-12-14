using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPart : MonoBehaviour
{
    //public string id;
    public string header;
    public string description;

    public Vector2 offset;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetOffset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetOffset ()
    {
        Vector3 pp = transform.parent.localPosition;
        pp.x += offset.x;
        pp.y += offset.y;
        transform.parent.localPosition = pp;
    }

}
