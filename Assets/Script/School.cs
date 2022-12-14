using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class School : MonoBehaviour
{

    float seed;
    Vector3 home;

    // Start is called before the first frame update
    void Start()
    {
        home = transform.localPosition;
        seed = Random.value * 10;
    }

    // Update is called once per frame
    void Update()
    {
        float n = Mathf.PerlinNoise(Time.fixedTime + seed, Time.fixedTime + seed);
        transform.localPosition = home + new Vector3(n, n / 10);
    }

}
