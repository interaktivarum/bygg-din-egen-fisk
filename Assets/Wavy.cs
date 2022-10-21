using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavy : MonoBehaviour
{

    Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        material.SetFloat("Speed", Random.Range(0.9f,1.1f));
        material.SetFloat("Offset", Random.Range(0, 3));
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("Time", Time.time);
    }
}
