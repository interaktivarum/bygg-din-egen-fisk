using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{

    FishIDs fishes;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 10; i++)
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
