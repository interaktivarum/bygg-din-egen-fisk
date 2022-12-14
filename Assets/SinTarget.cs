using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(5 * Mathf.Sin(Time.fixedTime * 4), 0, 50);
    }
}
