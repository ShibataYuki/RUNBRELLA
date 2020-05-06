using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    
    // 速度減衰値
    //[SerializeField]
    //float decaySpeed = 0.05f;
    public float downSpeed = 2;
    [SerializeField]
    float rainDownSpeed = 4;
    public float defaultSpeed = 6;
    [SerializeField]
    float rainSpeed = 8;
    Player player;
    Rigidbody2D rigidbody2d;
    private PlayerAerial playerAerial;
    
    private void Start()
    {
        // rigidbodyをセット
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerAerial = GetComponent<PlayerAerial>();
        // 読み込むテキストのファイル名
        string fileName = nameof(PlayerRun) + "Data" + player.Type;
        // Textからの読み込み
        // decaySpeed = TextManager.Instance.GetValue_float(fileName, nameof(decaySpeed));
        downSpeed = TextManager.Instance.GetValue_float(fileName, nameof(downSpeed));
        rainDownSpeed = TextManager.Instance.GetValue_float(fileName, nameof(rainDownSpeed));
        defaultSpeed = TextManager.Instance.GetValue_float(fileName, nameof(defaultSpeed));
        rainSpeed = TextManager.Instance.GetValue_float(fileName, nameof(rainSpeed));
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
        // ｘ方向への速度変化
        ChangeVelocityXToBase();
    }

    /// <summary>
    /// 速度調整
    /// </summary>
    void ChangeVelocityXToBase()
    {
        // 速度が最高速度以下なら加速
        if (rigidbody2d.velocity.x < player.MaxSpeed)
        {
            SpeedUpToMaxSpeed();
        }
        // 最高速度以上最高速度に戻す
        else if (rigidbody2d.velocity.x > player.MaxSpeed)
        {
            SetMaxVelocity();
        }
        // ちょうどなら何もしない
    }


    /// <summary>
    /// 加速処理
    /// </summary>
    void SpeedUpToMaxSpeed()
    {
        // 加速処理
        rigidbody2d.AddForce(new Vector2(player.BaseAddSpeed, 0), ForceMode2D.Force);
        var velocity = rigidbody2d.velocity;
        // 速度が基本速度を下回っていたら基本速度に戻す
        if (velocity.x < player.BaseSpeed)
        {
            velocity.x = player.BaseSpeed;
        }
        // 速度が最高速度を上回っていたら最高速度に戻す
        if (velocity.x > player.MaxSpeed)
        {
            velocity.x = player.MaxSpeed;
        }

        rigidbody2d.velocity = velocity;
    }

    /// <summary>
    /// 減速処理
    /// </summary>
    void SetMaxVelocity()
    {
        // 最高速度に戻す
        var afterVelocity = rigidbody2d.velocity;
        rigidbody2d.velocity = new Vector2(player.MaxSpeed, afterVelocity.y);
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
