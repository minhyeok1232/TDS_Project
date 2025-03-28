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
        // �÷��̾� ������
        public const int Hero_Damage = 2;
        // �÷��̾� ü��
        public const int Hero_HP = 100;
    }
    
    // Monster
    public struct Monster
    {
        // ���� �±�
        public const string Monster_Tag = "Monster";
        // ���� ������
        public const int    Monster_Damage = 1;
        // ���� ü��
        public const int    Monster_HP = 100;
        // ���� ����
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
