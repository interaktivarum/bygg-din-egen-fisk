using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour
{

    //Public variables
    [Header("Objects")]
    public ViewBase viewActive;
    public App app;

    // Start is called before the first frame update
    void Start()
    {

        app = FindObjectOfType(typeof(App)) as App;
        app.viewManager = this;

        // Init views
        foreach (ViewBase v in GetComponentsInChildren<ViewBase>())
        {
            v.gameObject.SetActive(false);
        }

        LoadFirstView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewFinished(ViewBase view)
    {
        UnloadView(view);
        Debug.Log("View finished: " + view.name);
        //if (view.name == "ViewScreenSaver")
        //{
        //    LoadView("ViewMenu");
        //}

        if (view.loadViewOnFinish)
        {
            LoadView(view.loadViewOnFinish);
        }
        else if (view.loadSceneOnFinish.Length > 0)
        {
            SceneManager.LoadScene(view.loadSceneOnFinish);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }

    void UnloadView(ViewBase view)
    {
        view.gameObject.SetActive(false);
    }

    void LoadView(ViewBase view)
    {
        Debug.Log("Load view: " + view.name);
        //AppSingleton.I.CurrentView = view;
        viewActive = view;
        viewActive.gameObject.SetActive(true);
    }

    void LoadFirstView()
    {
        LoadView(GetComponentsInChildren<ViewBase>(true)[0]);
    }

    public void WaitAndCallFunction(TimeHelper.CallbackDelegate callback, float time)
    {
        app.timeHelper.WaitAndCallFunction(callback, time);
    }

}
