using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum GameState
{
    NONE,
    Run,
    Pause,
    GameOver
}

public static class Helpers
{
    // Hero
    public struct Hero
    {
        // 플레이어 데미지
        public const int Hero_Damage = 2;
        // 플레이어 체력
        public const int Hero_HP = 100;
    }
    
    // Monster
    public struct Monster
    {
        // 몬스터 태그
        public const string Monster_Tag = "Monster";
        // 몬스터 데미지
        public const int    Monster_Damage = 1;
        // 몬스터 체력
        public const int    Monster_HP = 100;
        // 몬스터 스폰
        public const float  Monster_SpawnTime = 1.0f;
    }
    
    
    
    
    
    
    
    // ====================== Debug ======================
    public static void Log(string str)
    {
        #if UNITY_EDITOR
        Debug.Log(str);
        #endif
    }
    
    public static void LogWarning(string str)
    {
        #if UNITY_EDITOR
        Debug.LogWarning(str);
        #endif
    }
    
    public static void LogError(string str)
    {
        #if UNITY_EDITOR
        Debug.LogError(str);
        #endif
    }
}
