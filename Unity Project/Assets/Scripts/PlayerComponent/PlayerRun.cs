using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{

    // 保留
    //Player player;
    //Rigidbody2D rigidbody2d;
    //private PlayerAerial playerAerial;
    // 移動クラス
    PlayerMove move;
   
    private void Start()
    {
        move = GetComponent<PlayerMove>();        
        // 保留
        // rigidbodyをセット
        //rigidbody2d = transform.GetComponent<Rigidbody2D>();
        //player = GetComponent<Player>();
        //playerAerial = GetComponent<PlayerAerial>();
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
        // 保留
        // ｘ方向への速度変化
        //ChangeVelocityXToBase();
    }

    // 保留
    /// <summary>
    /// 速度調整
    /// </summary>
    //void ChangeVelocityXToBase()
    //{
    //    // 速度が最高速度以下なら加速
    //    if (rigidbody2d.velocity.x < player.MaxSpeed)
    //    {
    //        SpeedUpToMaxSpeed();
    //    }
    //    // 最高速度以上最高速度に戻す
    //    else if (rigidbody2d.velocity.x > player.MaxSpeed)
    //    {
    //        SetMaxVelocity();
    //    }
    //    // ちょうどなら何もしない
    //}

    // 保留
    /// <summary>
    /// 加速処理
    /// </summary>
    //void SpeedUpToMaxSpeed()
    //{
    //    // 加速処理
    //    rigidbody2d.AddForce(new Vector2(player.BaseAddSpeed, 0), ForceMode2D.Force);
    //    var velocity = rigidbody2d.velocity;
    //    // 速度が基本速度を下回っていたら基本速度に戻す
    //    if (velocity.x < player.BaseSpeed)
    //    {
    //        velocity.x = player.BaseSpeed;
    //    }
    //    // 速度が最高速度を上回っていたら最高速度に戻す
    //    if (velocity.x > player.MaxSpeed)
    //    {
    //        velocity.x = player.MaxSpeed;
    //    }

    //    rigidbody2d.velocity = velocity;
    //}

    // 保留
    /// <summary>
    /// 減速処理
    /// </summary>
    //void SetMaxVelocity()
    //{
    //    // 最高速度に戻す
    //    var afterVelocity = rigidbody2d.velocity;
    //    rigidbody2d.velocity = new Vector2(player.MaxSpeed, afterVelocity.y);
    //}

    // 保留
    /// <summary>
    /// プレイヤーの移動速度をセットする関数
    /// </summary>
    /// <param name="speed"></param>
    //public void SetSpeed(float speed)
    //{
    //    player.BaseSpeed = speed;
    //}

}
