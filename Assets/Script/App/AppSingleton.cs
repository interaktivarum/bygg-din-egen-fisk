using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class AppSingleton : MonoBehaviour
{
    //The instance
    static AppSingleton _instance;
    public static AppSingleton I { get { return _instance; } }

    //App & view
    public App app;
    public ViewBase CurrentView;

    //App data
    public Dictionary<string, AppData> appData = new Dictionary<string, AppData>();
    public string language;

    private void Awake()
    {
        //Debug.Log("Current view name: " + CurrentView.name);
        if (_instance != null && _instance != this)
        {
            Debug.Log("Destroy");
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("Set instance");
            _instance = this;

            //Init app data
            ReadAppSettings();

        }

    }

    public void ReadAppSettings()
    {
        //string path = Application.streamingAssetsPath + "/appSettings.json";
        //string jsonString = File.ReadAllText(path);
        //AppSettings settings = JsonUtility.FromJson<AppSettings>(jsonString);
        //foreach (string l in settings.languages)
        //{
        //    ReadAppData(l);
        //}
        //if (settings.languages.Length > 0)
        //{
        //    language = settings.languages[0];
        //}

    }

    public void ReadAppData(string lang)
    {
        string path = Application.streamingAssetsPath + "/appData-" + lang + ".json";
        string jsonString = File.ReadAllText(path);
        appData.Add(lang, JsonUtility.FromJson<AppData>(jsonString));
    }

    public AppData GetAppData()
    {
        return appData[language];
    }

}