using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AllCards { 
    public CardStaticData[] level1;
    public CardStaticData[] level2;
    public CardStaticData[] level3;
    public CardStaticData[] level4;
    public CardStaticData[] level5;
    public CardStaticData[] GetShuffleCopy(CardStaticData[] array)
    {
        CardStaticData[] randomized;
        randomized = (CardStaticData[])array.Clone();
        ArrayUtils.Shuffle(randomized);
        return randomized;
    }
}
