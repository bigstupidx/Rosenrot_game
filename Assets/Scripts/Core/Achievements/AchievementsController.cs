﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//хранение ачивок
//если 0 - не бралась ачика
//больше 0 - значение ачивки
//меньше 0 - ачивка получена

public static class AchievementsController
{
    public enum Type
    {
        //ачивки без уровня
        GoodStart = 0,
        RockyRoad = 1,
        DoingGreyt = 2,
        LetItBee = 3,
        CameToTheDarkSide = 4,
        BeOurGuest = 5,
        AlwaysRight = 6,
        ExtremeLeft = 7,
        SelfDestructive = 8,
        HeartBreaker = 9,
        SurvivorFinished = 10,
        EverybodysBestFriend = 11,
        RubyRubyRubyRuby = 12,
        JacobAndTheBeanstalk = 13,
        ExplosiveBehavior = 14,
        Catchy = 15,
        BigBang = 16,
        NeverGiveUp = 17,
        GotHigh = 18,
        Adept = 19,
    }

    public enum RewardType { Diamonds, Rubies, Seeds, Bombs, Health }


    public static void AddToAchievement(Type type, int addValue)
    {
        int val = GetAchievement(type);
        DataManager.Instance.SetAchievement((int)type, val + addValue);

        AddToListAchievement(type, val, val + addValue);
    }

    public static void DiscardAchievement(Type type)
    {
        DataManager.Instance.SetAchievement((int)type, 0);
    }

    private static void AddToListAchievement(Type type, int prevVal, int curVal)        //проверить не достигли ли ачивки, добавить в лист на вывод если достигли
    {
        int numberInResourcePrefab = -1;
        Achievement achievementInform = FindAchievementInResources(type, ref numberInResourcePrefab);

        for (int j = 0; j < achievementInform.m_NeedToAchieve.Length; j++)
        {
            if (curVal >= achievementInform.m_NeedToAchieve[j] && prevVal < achievementInform.m_NeedToAchieve[j])      //открыли ачивку
            {
                GetRevard(achievementInform.m_RevardType, achievementInform.m_LeveledRevards[j]);       //получили награду

                if (!GameController.Instance.AchievementsToShow.Contains(numberInResourcePrefab))
                    GameController.Instance.AchievementsToShow.Add(numberInResourcePrefab);      //добавить номер награды в рeсурсе
                break;
            }
        }
    }

    public static Achievement FindAchievementInResources(Type type, ref int numberInResourcePrefab)
    {
        for (int i = 0; i < GameController.Instance.AchievementRevards.Achievements.Length; i++)
        {
            if (GameController.Instance.AchievementRevards.Achievements[i].m_Achievement == type)
            {
                Achievement achievementInform = GameController.Instance.AchievementRevards.Achievements[i]; //нашли информацию по ачивке
                numberInResourcePrefab = i;
                return achievementInform;
            }
        }
        Debug.LogError("Not Found Achievement of Type: " + type + " in Resources/Achievements");
        return new Achievement();
    }

    public static int GetAchievement(Type type)      //текущее значение ачивки
    {
        return DataManager.Instance.GetAchievement((int)type);
    }
    
    public static void GetRevard(AchievementsController.RewardType rewardType, int reward)  //получить награды
    {
        switch (rewardType)
        {
            case AchievementsController.RewardType.Bombs:
                Market.Instance.Bomb += reward;
                break;
            case AchievementsController.RewardType.Diamonds:
                Market.Instance.Dimond += reward;
                break;
            case AchievementsController.RewardType.Health:
                Market.Instance.Health += reward;
                break;
            case AchievementsController.RewardType.Rubies:
                Market.Instance.Ruby += reward;
                break;
            case AchievementsController.RewardType.Seeds:
                Market.Instance.Seeds += reward;
                break;
        }



    }
}
