using System;
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
    public CardStaticData[] alliance;
    public CardStaticData[] horde;
    public CardStaticData[] human;
    public CardStaticData[] elf;
    public CardStaticData[] dwarf;
    public CardStaticData[] orc;
    public CardStaticData[] GetShuffleCopy(CardStaticData[] array)
    {
        CardStaticData[] randomized;
        randomized = (CardStaticData[])array.Clone();
        ArrayUtils.Shuffle(randomized);
        return randomized;
    }

    public void CreateMock()
    {
        human = new CardStaticData[10];
        elf = new CardStaticData[10];
        dwarf = new CardStaticData[10];
        orc = new CardStaticData[10];

        for (int i = 0; i < 10; i++)
        {
            human[i] = new CardStaticData();
            elf[i] = new CardStaticData();
            dwarf[i] = new CardStaticData();
            orc[i] = new CardStaticData();
        }
    }
}
