using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    
    // 速度減衰値
    [SerializeField]
    float decaySpeed = 0.05f;
    [SerializeField]
    float defaultSpeed = 6;
    [SerializeField]
    float RainSpeed = 8;
    Player player;
    Rigidbody2D rigidbody2d;
    private PlayerAerial playerAerial;

    private void Start()
    {
        // rigidbodyをセット
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerAerial = GetComponent<PlayerAerial>();
    }


    private void Update()
    {
        // 雨が降っているときはスピードを上げる
        if(player.IsRain)
        {
            if(player.BaseSpeed==RainSpeed)
            {
                return;
            }
            SetSpeed(RainSpeed);
        }
        else
        {
            if(player.BaseSpeed==RainSpeed)
            {
                SetSpeed(defaultSpeed);
            }
        }
    }


    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    public void Run()
    {
        //rigidbody2d = transform.GetComponent<Rigidbody2D>();
        // 移動ベクトル
        Vector2 moveVec;
        // 前方にブロックがあるなら
        if (playerAerial.FrontGroundCheck() == true)
        {
            // スピードを0にする。
            rigidbody2d.velocity = new Vector2(-rigidbody2d.velocity.x, rigidbody2d.velocity.y);
        }
        // 速度の制限処理
        else if (player.VelocityXStorage <= player.BaseSpeed)
        {            
            // プレイヤーのVelocity.xが-6以下なら変更しない
            if(player.VelocityXStorage<0)
            {
                rigidbody2d.velocity = new Vector2(player.VelocityXStorage, rigidbody2d.velocity.y);
                player.VelocityXStorage += decaySpeed;
                return;
            }
            moveVec = Vector2.right * player.BaseSpeed;
            rigidbody2d.velocity = new Vector2(moveVec.x, rigidbody2d.velocity.y);
        }
        else
        {           
            rigidbody2d.velocity = new Vector2 (player.VelocityXStorage, rigidbody2d.velocity.y);
            player.VelocityXStorage -= decaySpeed;
        }
        
    }


    /// <summary>
    /// プレイヤーの移動速度をセットする関数
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        player.BaseSpeed = speed;
    }

}
