using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassTypeHelper
{
    public static AttackType GetAttackType(ClassType classType)
    {
        switch (classType)
        {
            case ClassType.Warrior:
                return AttackType.AttackFirstFindOnly;
            case ClassType.Knight:
                return AttackType.AttackFirstFindOnly;
            case ClassType.Ranger:
                return AttackType.AttackFirstFindOnly;
            case ClassType.Mage:
                return AttackType.AttackOnlyRangeFind;
            case ClassType.Thief:
                return AttackType.AttackAllRangeFind;
            case ClassType.Paladin:
                return AttackType.AttackFirstFindOnly;
            case ClassType.None:
                return AttackType.AttackFirstFindOnly;
            default:
                return AttackType.AttackFirstFindOnly;

        }
    }
    public static List<BuffData> GetPosAttackBuffs(ClassType classType)
    {
        List<BuffData> buffList = new List<BuffData>();
        BuffData buffData = new BuffData();
        switch (classType)
        {
            case ClassType.Priest:
                buffData.StatType = StatType.Defense;
                buffData.AfterAttackType = BuffType.BuffAllies;
                buffData.BuffConditionType = BuffConditionType.None;
                break;
            case ClassType.Knight:
                buffData.StatType = StatType.Defense;
                buffData.AfterAttackType = BuffType.BuffSelf;
                buffData.BuffConditionType = BuffConditionType.NumberOfAllies;
                break;
            case ClassType.Thief:
                buffData.StatType = StatType.Defense;
                buffData.AfterAttackType = BuffType.BuffSelf;
                buffData.BuffConditionType = BuffConditionType.NumberOfAllies;
                break;
            case ClassType.Paladin:
                buffData.StatType = StatType.Defense;
                buffData.AfterAttackType = BuffType.BuffSelf;
                buffData.BuffConditionType = BuffConditionType.NumberOfAllies;
                break;


        }
        

        buffList.Add(buffData);

        return buffList;
    }
    public static List<BuffData> GetPreAttackBuffs(ClassType classType)
    {
        List<BuffData> buffList = new List<BuffData>();
        BuffData buffData = new BuffData();
        switch (classType)
        {
            case ClassType.Warrior:
                buffData.StatType = StatType.Attack;
                buffData.AfterAttackType = BuffType.BuffSelf;
                buffData.BuffConditionType = BuffConditionType.NumberOfEnemies;
                break;
            case ClassType.Knight:
                buffData.StatType = StatType.Attack;
                buffData.AfterAttackType = BuffType.BuffSelf;
                buffData.BuffConditionType = BuffConditionType.NumberOfAllies;
                break;
            case ClassType.Paladin:
                //buffData.StatType = StatType.Attack;
                //buffData.AfterAttackType = BuffType.BuffAllies;
                //buffData.BuffConditionType = BuffConditionType.NumberOfEnemies;
                break;

        }
       

        buffList.Add(buffData);

        return buffList;
    }
    public static string GetAttackDesc(ClassType classType)
    {
        switch (classType)
        {
            case ClassType.Warrior:
                return "+1 Attack for each Enemy arround";
            case ClassType.Knight:
                return "+1 Defense for each Enemy arround\n + 1 Attack for each Ally arround";
            case ClassType.Ranger:
                return "Add +1 Attack based on Distance";
            case ClassType.Mage:
                return "Attack only your Range";
            case ClassType.Thief:
                return "Attack all cards on Range\n + 1 Attack for each Ally arround";
            case ClassType.Paladin:
                return "+1 Defense for each Ally arround";
            case ClassType.Priest:
                return "+1 Defense to each Ally arround";
            default:
                return "";

        }
    }

    public static ClassType GetClassByString(string classType)
    {
        switch (classType.ToLower())
        {
            case "warrior":
                return ClassType.Warrior;
            case "knight":
                return ClassType.Knight;
            case "ranger":
                return ClassType.Ranger;
            case "mage":
                return ClassType.Mage;
            case "thief":
                return ClassType.Thief;
            case "paladin":
                return ClassType.Paladin;
            case "priest":
                return ClassType.Priest;
            default:
                return ClassType.None;

        }
    }

    internal static bool UseDistanceAsMultiplier(ClassType classType)
    {
        return classType == ClassType.Ranger;
    }
}
