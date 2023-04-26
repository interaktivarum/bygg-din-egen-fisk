using UnityEngine;
using TMPro;
//using UnityEngine.Video;
using System.IO;
//using DG.Tweening;
//using WorldSpaceTransitions;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class App: MonoBehaviour
{

    //Public variables
    //[Header("Objects")]
    //public Transform views;
    //public ViewBase viewActive;
    public int interactionTimeout = 60;
    bool interactionTimeoutActive;

    //Private variables
    float lastInteraction;
    //bool screensaver = true;
    public TimeHelper timeHelper;
    public ViewManager viewManager;

    
    UnityEvent interactionTimeoutEvent = new UnityEvent();

    private void Awake()
    {
        ReadAppSettings();
    }

    // Start is called before the first frame update
    void Start()
    {
        timeHelper = GetComponent<TimeHelper>();
        SetLastInteraction();

        // Init singleton
        //AppSingleton.I.app = this;        

        // Init views 
        //views = transform.Find("Views");
        //foreach (ViewBase v in views.GetComponentsInChildren<ViewBase>())
        //{
        //    v.gameObject.SetActive(false);
        //}

        //LoadFirstView();
    }

    public void ReadAppSettings()
    {
        //string path = Application.streamingAssetsPath + "/appSettingsLargeScreen.json";
        //string jsonString = File.ReadAllText(path);
        //settings = JsonUtility.FromJson<AppSettingsLargeScreen>(jsonString);
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardInput();
        InteractionTimeoutTest();
        if (Input.anyKey) {
            SetLastInteraction();
        }
    }

    void SetViewManager (ViewManager vm)
    {
        viewManager = vm;
    }

    void SetLastInteraction()
    {
        interactionTimeoutActive = false;
        lastInteraction = Time.fixedTime;
    }

    void KeyboardInput()
    {
        // Keyboard shortcuts
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ViewFinished(viewActive);
        //}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Views functions
    //public void ViewFinished(ViewBase view)
    //{
    //    UnloadView(view);
    //    Debug.Log("View finished: " + view.name);
    //    //if (view.name == "ViewScreenSaver")
    //    //{
    //    //    LoadView("ViewMenu");
    //    //}

    //    if (view.loadViewOnFinish)
    //    {
    //        LoadView(view.loadViewOnFinish);
    //    }
    //    else if (view.loadSceneOnFinish.Length > 0)
    //    {
    //        SceneManager.LoadScene(view.loadSceneOnFinish);
    //    }
    //    else
    //    {
    //        RestartApp();
    //    }
    //}

    //void UnloadView(ViewBase view)
    //{
    //    view.gameObject.SetActive(false);
    //}

    //void LoadView(ViewBase view)
    //{
    //    Debug.Log("Load view: " + view.name);
    //    AppSingleton.I.CurrentView = view;
    //    //viewActive = views.Find(viewName).gameObject;
    //    viewActive = view;
    //    viewActive.gameObject.SetActive(true);
    //}

    //void LoadFirstView()
    //{
    //    LoadView(views.GetComponentsInChildren<ViewBase>(true)[0]);
    //}

    void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Interaction timeout functions

    public void AddInteractionTimeoutListener(UnityAction callback) {
        interactionTimeoutEvent.AddListener(callback);
    }

    void InteractionTimeoutTest()
    {
        if (!interactionTimeoutActive && Time.fixedTime - lastInteraction > interactionTimeout && !viewManager.viewActive.ignoreInteractionTimeout) {
            interactionTimeoutActive = true;
            interactionTimeoutEvent.Invoke();
        }
    }

    public void RestartApp()
    {
        Debug.Log("Restart app");
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        //UnloadView(viewActive);
        //LoadFirstView();
        //SetLastInteraction();
    }

    //void SetScreenSaver(bool state)
    //{
    //    screensaver = state;
    //}

    public void WaitAndCallFunction(TimeHelper.CallbackDelegate callback, float time)
    {
        timeHelper.WaitAndCallFunction(callback, time);
    }
}
