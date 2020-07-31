using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    // 移動クラス
    PlayerMove move;
   
    private void Start()
    {
        move = GetComponent<PlayerMove>();                
    }

    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    public void Run()
    {
        // 加速度の蓄積
        move.AddAcceleration();
        // 速度の増加
        move.SpeedChange();        
    }
    
}
