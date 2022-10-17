using System.Collections;
using UnityEngine;

public class TimeHelper : MonoBehaviour
{
    public delegate void CallbackDelegate();

    public void WaitAndCallFunction(CallbackDelegate callback, float time)
    {
        Debug.Log(callback);
        StartCoroutine(DoWaitAndCallFunction(callback, time));
    }

    IEnumerator DoWaitAndCallFunction(CallbackDelegate callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    public void CallNextFrame(CallbackDelegate callback)
    {
        StartCoroutine(DoCallNextFrame(callback));
    }

    IEnumerator DoCallNextFrame(CallbackDelegate callback)
    {
        yield return 0;
        callback();
    }
}
