using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "AttackTrigger")
        {
            //Debug.Log("Escape: " + GetComponentInParent<Fish>().ids.body + " < " + other.GetComponentInParent<Fish>().ids.body);
            GetComponentInParent<Fish>().EscapeStart(other.GetComponentInParent<Fish>());
        }
    }
}
