using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    
    // 速度減衰値
    [SerializeField]
    float decaySpeed = 0.05f;
    public float downSpeed = 2;
    [SerializeField]
    float rainDownSpeed = 4;
    public float defaultSpeed = 6;
    [SerializeField]
    float rainSpeed = 8;
    Player player;
    Rigidbody2D rigidbody2d;
    private PlayerAerial playerAerial;
    private readonly string fileName = nameof(PlayerRun) + "Data";

    // 加える力
    [SerializeField]
    float addSpeed = 0.1f;

    private void Start()
    {
        // rigidbodyをセット
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerAerial = GetComponent<PlayerAerial>();
        // Textからの読み込み
        //decaySpeed = TextManager.Instance.GetValue(fileName, nameof(decaySpeed));
        //downSpeed  = TextManager.Instance.GetValue(fileName, nameof(downSpeed));
        //rainDownSpeed = TextManager.Instance.GetValue(fileName, nameof(rainDownSpeed));
        //defaultSpeed = TextManager.Instance.GetValue(fileName, nameof(defaultSpeed));
        //rainSpeed = TextManager.Instance.GetValue(fileName, nameof(rainSpeed));
    }


    private void Update()
    {
        // 雨が降っているときはスピードを上げる
        if(player.IsRain)
        {
            // プレイヤーがダウンしているなら
            if (player.state == PlayerStateManager.Instance.playerDownState)
            {
                if(player.BaseSpeed==rainDownSpeed)
                {
                    return;
                }
                SetSpeed(rainDownSpeed);
            }
            else
            {
                if (player.BaseSpeed == rainSpeed)
                {
                    return;
                }
                SetSpeed(rainSpeed);
            }
        }
        // 雨が降っていないならスピードを戻す
        else
        {
            // プレイヤーがダウンしているなら
            if(player.state==PlayerStateManager.Instance.playerDownState)
            {
                if(player.BaseSpeed==downSpeed)
                {
                    return;
                }
                SetSpeed(downSpeed);
            }
            else
            {
                if (player.BaseSpeed == defaultSpeed)
                {
                    return;
                }
                SetSpeed(defaultSpeed);
            }
        }
    }


    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    public void Run()
    {       
        // 速度の制限処理
        //velocity.x -= decaySpeed;
        //if (velocity.x < player.BaseSpeed)
        //{
        //    velocity.x = player.BaseSpeed;
        //}
        //rigidbody2d.velocity = velocity;
        // 速度の制限処理
        rigidbody2d.AddForce(new Vector2(addSpeed, 0), ForceMode2D.Force);
        var velocity = rigidbody2d.velocity;
        if(velocity.x<player.BaseSpeed)
        {
            velocity.x = player.BaseSpeed;
        }
        if (velocity.x > player.MaxSpeed)
        {
            velocity.x = player.MaxSpeed;
        }

        rigidbody2d.velocity = velocity;
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
