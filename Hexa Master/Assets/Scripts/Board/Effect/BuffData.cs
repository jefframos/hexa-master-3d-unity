using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffData
{
    private BuffType afterAttackType;
    private BuffConditionType buffConditionType;
    private StatType statType;

    public StatType StatType { get => statType; set => statType = value; }
    internal BuffConditionType BuffConditionType { get => buffConditionType; set => buffConditionType = value; }
    internal BuffType AfterAttackType { get => afterAttackType; set => afterAttackType = value; }
    internal float BuffValue { get => buffValue; set => buffValue = value; }

    private float buffValue = 10f;
}
