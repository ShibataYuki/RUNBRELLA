using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlide : MonoBehaviour
{
    // 自身のリジットボディ
    Rigidbody2D rigidbody2d;    
    Player player;
    PlayerRun playerRun;    
    // 速度減衰値
    [SerializeField]
    float decaySpeed = 0.05f;
    [SerializeField]
    float grideBaseSpeed = 5;
	// 雨の場合のベーススピード
    [SerializeField]
    float grideRainSpeed = 7f;
    // 前方に地面があるかチェックするコンポーネント
    private PlayerAerial playerAerial = null;

    private void Start()
    {
        // 変数の初期化
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerRun = GetComponent<PlayerRun>();      
        playerAerial = GetComponent<PlayerAerial>();
    }

    /// <summary>
    /// 滑空の開始処理
    /// </summary>
    public void StartGlide()
    {
                     
    }

    /// <summary>
    /// 滑空中の処理
    /// </summary>
    public void Gride()
    {

        // 最低速度の計算
        var minSpeed = player.IsRain ? grideRainSpeed : grideBaseSpeed;
        
        var velocity = rigidbody2d.velocity;
        velocity.x -= decaySpeed;
        if (minSpeed > velocity.x)
        {
            velocity.x = minSpeed;
        }
        
        // 落下速度軽減処理
        Vector2 workVec = new Vector2(velocity.x, velocity.y * 0.9f);
        rigidbody2d.velocity = workVec;
    }
        
    /// <summary>
    /// 滑空の終了処理
    /// </summary>
    public void EndGlide()
    {
        if(rigidbody2d.velocity.y > 0)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
        }
    }
}
