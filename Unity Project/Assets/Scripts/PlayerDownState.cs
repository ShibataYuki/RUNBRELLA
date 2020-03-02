using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownState : IState
{
    public void Entry(int ID)
    {
        // デバッグ用色変更
        var sprite = SceneManager.Instance.playerEntityData.players[ID].GetComponent<SpriteRenderer>();
        sprite.color = Color.red;
    }

    public void Do(int ID)
    {
        
    }

    public void Do_Fix(int ID)
    {
        
    }


    public void Exit(int ID)
    {
        
    }
}
