using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bend : MonoBehaviour
{
    public float speed = 1;
    Quaternion rotationPrev;

    public void SetPrev()
    {
        rotationPrev = transform.rotation;
    }

    public void UpdateRotation()
    {
        float s = GetComponentInParent<Fish>().GetRotationSpeed() / 100;
        transform.rotation = new Quaternion();
        transform.rotation = Quaternion.Slerp(rotationPrev, transform.rotation, s);
        //transform.rotation = Quaternion.Euler(0, 0, 1);
    }
}
