using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase : MonoBehaviour
{

    public ViewManager viewManager;
    public ViewBase loadViewOnFinish;
    public string loadSceneOnFinish;
    public bool ignoreInteractionTimeout = false;

    private void Awake()
    {
        viewManager = GetComponentInParent<ViewManager>();
        AwakeInstance();
    }

    // Start is called before the first frame update
    public void Start()
    {
        StartInstance();
        UpdateDataFields();
    }

    public virtual void AwakeInstance()
    {
    }

    public virtual void StartInstance()
    {
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    public virtual void FinishView(float time)
    {
        viewManager.WaitAndCallFunction(FinishView, 2);
    }

    public virtual void FinishView()
    {
        viewManager.ViewFinished(this);
    }

    protected AppData GetAppData()
    {
        return AppSingleton.I.GetAppData();
    }

    public virtual void UpdateDataFields()
    {
    }

}