using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class FishFactory : MonoBehaviour
{
    [Header("Prefabs")]
    public Fish fish;
    public FishBody[] bodies;
    public FishHead[] heads;

    static Dictionary<string, FishBody> fishBodies = new Dictionary<string, FishBody>();
    static Dictionary<string, FishHead> fishHeads = new Dictionary<string, FishHead>();
    static Fish fishPrefab;

    static int layerOrder = 0;

    //Start is called before the first frame update
    void Awake()
    {
        foreach (FishBody f in bodies)
        {
            fishBodies.Add(f.id, f);
        }
        foreach (FishHead f in heads)
        {
            fishHeads.Add(f.id, f);
        }
        fishPrefab = fish;
    }

    public static FishBody GetBody(string id)
    {
        return fishBodies[id];
    }

    public static FishHead GetHead(string id)
    {
        return fishHeads[id];
    }

    public static Fish CreateFish(FishIDs ids, Transform parent)
    {
        Fish f = Instantiate(fishPrefab, parent);
        f.SetBody(ids.body + " " + ids.pattern);
        f.SetHead(ids.head);
        return f;
    }

    public static Fish CreateRandomFish (Transform parent)
    {
        Fish f = Instantiate(fishPrefab, parent);

        List<string> bodyKeys = new List<string>(fishBodies.Keys);
        List<string> headKeys = new List<string>(fishHeads.Keys);
        string bodyStr = bodyKeys[Random.Range(0, bodyKeys.Count)];
        string headStr = headKeys[Random.Range(0, headKeys.Count)];
        f.SetBody(bodyStr);
        f.SetHead(headStr);

        f.transform.localPosition = f.body.positionSpawn;

        return f;
    }

}
