using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{

    public int nFishes = 10;
    FishIDs fishes;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < nFishes; i++)
        {
            Fish fish = FishFactory.CreateRandomFish(transform);
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

    public void CreateFish (FishIDs ids)
    {
        FishFactory.CreateFish(ids, transform);
    }

    public void CreateRandomFish()
    {
        FishFactory.CreateRandomFish(transform);
    }

    public void RepopulateTest()
    {
        if(transform.childCount <= nFishes)
        {
            CreateRandomFish();
        }
    }

}
