using System.Collections;
using UnityEngine;

public class TimeHelper : MonoBehaviour
{
    public delegate void CallbackDelegate();

    public void WaitAndCallFunction(CallbackDelegate callback, float seconds)
    {
        StartCoroutine(DoWaitAndCallFunction(callback, seconds));
    }

    IEnumerator DoWaitAndCallFunction(CallbackDelegate callback, float seconds)
    {
        yield return new WaitForSeconds(seconds);
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
