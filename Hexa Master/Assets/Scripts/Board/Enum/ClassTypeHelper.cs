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

    public static string GetAttackDesc(ClassType classType)
    {
        switch (classType)
        {
            //case ClassType.Warrior:
            //    return AttackType.AttackFirstFindOnly;
            //case ClassType.Knight:
            //    return AttackType.AttackFirstFindOnly;
            case ClassType.Ranger:
                return "Add +1 Attack based on Distance";
            case ClassType.Mage:
                return "Attack only your Range";
            case ClassType.Thief:
                return "Attack all cards on Range";
            //case ClassType.Paladin:
            //    return AttackType.AttackFirstFindOnly;
            //case ClassType.None:
            //    return AttackType.AttackFirstFindOnly;
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
            default:
                return ClassType.None;

        }
    }

    internal static bool UseDistanceAsMultiplier(ClassType classType)
    {
        return classType == ClassType.Ranger;
    }
}
