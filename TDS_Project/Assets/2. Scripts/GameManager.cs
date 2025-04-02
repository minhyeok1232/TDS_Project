using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState gameState = GameState.NONE;
    public float gameTime = 0.0f;

    // 한번만 실행을 하기 때문에 >> 외부에서 접근 금지
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
