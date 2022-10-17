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
            FishFactory.CreateRandomFish(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateFish (FishIDs ids)
    {
        FishFactory.CreateFish(ids, transform);
    }

}
