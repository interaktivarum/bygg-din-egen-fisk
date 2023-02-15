using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [Header("Fishes")]
    public int nFishes = 10;
    public int nMinis = 10;
    //FishIDs fishes;
    Transform fishes;
    Transform minis;

    [Header("Areas")]
    public RectTransform areaWeed;
    public RectTransform areaWeedThick;
    public RectTransform areaCoral;
    public RectTransform areaCoralThick;

    [Header("Misc")]
    public App app;
    public FishFactory factory;

    // Start is called before the first frame update
    void Start()
    {
        fishes = transform.Find("Fishes");
        minis = transform.Find("Minis");

        for(int i = 0; i < nFishes; i++)
        {
            CreateRandomFish();
        }
        for (int i = 0; i < nMinis; i++)
        {
            CreateRandomMini();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CreateRandomFish();
        }
    }

    //public void CreateFish (FishIDs ids)
    //{
    //    factory.CreateFish(ids, fishes);
    //}

    //public void CreateFish(Fish fish)
    //{
    //    factory.CreateFish(fish, fishes);
    //}

    public void CreateRandomFish()
    {
        //Debug.Log("Create random fish");
        factory.CreateRandomFish(fishes);
    }

    public void CreateRandomMini()
    {
        //Debug.Log("Create random mini");
        factory.CreateRandomFish(minis, true);
    }

    //public FishBody GetBody(BodyID id)
    //{
    //    return factory.fishBodies[id];
    //}

    //public FishHead GetHead(HeadID id)
    //{
    //    return factory.fishHeads[id];
    //}

    public void RepopulateTest()
    {
        //Debug.Log("Repopulate in 60 seconds");
        if(fishes.childCount <= nFishes)
        {
            app.timeHelper.WaitAndCallFunction(CreateRandomFish, 60);
            //CreateRandomFish();
        }
        if (minis.childCount <= nMinis)
        {
            app.timeHelper.WaitAndCallFunction(CreateRandomMini, 60);
            //CreateRandomMini();
        }
    }

}
