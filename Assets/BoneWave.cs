using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneWave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateBoneWave()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 40 * Mathf.Pow(Mathf.Sin(Time.fixedTime * 2), 1), 0));
        //transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 80 * (Mathf.PerlinNoise(Time.fixedTime * 2,0)-0.5f)));
    }
}
