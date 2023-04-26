using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFish", menuName = "Fish/Fish", order = 1)]
[System.Serializable]
public class FishSO : ScriptableObject
{
    public FishBody body;
    public FishHead head;
    public Sprite pattern;
}
