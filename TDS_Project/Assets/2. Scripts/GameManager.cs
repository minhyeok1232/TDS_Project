using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState gameState = GameState.NONE;
    public float gameTime = 0.0f;

    // �ѹ��� ������ �ϱ� ������ >> �ܺο��� ���� ����
    protected override void OnAwake()
    {
        gameState = GameState.Run;
        Helpers.Log($"{gameState}");
    }
    
    
    void Update()
    {
        if (gameState == GameState.Run)
        {
            gameTime += Time.deltaTime;
        }
    }

    public void Pause()
    {
        gameState = GameState.Pause;
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;
    }
}
