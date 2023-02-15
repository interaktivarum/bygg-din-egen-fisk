using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavy : MonoBehaviour
{

    public float speed = 1;
    public float amplitude = 1;
    public float offset = 0;

    Material material;

    // Start is called before the first frame update
    void Start()
    {
        //offset = Random.value;
        material = GetComponent<SpriteRenderer>().material;
        material.SetFloat("_Speed", speed);
        material.SetFloat("_Amplitude", amplitude);
        material.SetFloat("_Offset", offset);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
