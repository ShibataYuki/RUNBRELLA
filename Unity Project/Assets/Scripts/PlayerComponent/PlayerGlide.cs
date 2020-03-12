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
        
        //// 上昇中なら上昇をやめる
        //if(rigidbody2d.velocity.y > 0)
        //{
        //    rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
        //}
        //else
        //{
        //    rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, rigidbody2d.velocity.y * 0.5f);
        //}
       // rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x * 0.7f, rigidbody2d.velocity.y);           
    }

    /// <summary>
    /// 滑空中の処理
    /// </summary>
    public void Gride()
    {

        // 移動ベクトル
        Vector2 moveVec;

        // 最低速度の計算
        var minSpeed = player.IsRain ? grideRainSpeed : grideBaseSpeed;

        // プレイヤーの前方に地面があるなら
        if (playerAerial.FrontGroundCheck() == true)
        {
            // プレイヤーのベロシティのXを0にする
            rigidbody2d.velocity = new Vector2(0.0f, rigidbody2d.velocity.y);
        }
        // 速度の制限処理
        else if (player.VelocityXStorage <= minSpeed)
        {            
            moveVec = Vector2.right * minSpeed;
            rigidbody2d.velocity = new Vector2(moveVec.x, rigidbody2d.velocity.y);
        }
        else
        {            
            player.VelocityXStorage -= decaySpeed;            
            rigidbody2d.velocity = new Vector2(player.VelocityXStorage, rigidbody2d.velocity.y);
        }

        // 落下速度軽減処理
        Vector2 workVec = new Vector2(rigidbody2d.velocity.x, rigidbody2d.velocity.y * 0.9f);
        rigidbody2d.velocity = workVec;
    }
    
    /// <summary>
    /// Y方向への速度の制限処理
    /// </summary>
    //public void RestrictVectorY()
    //{
    //    var ScreenTop = Camera.main.ViewportToWorldPoint(Vector3.one).y;
    //    if (rigidbody2d.velocity.y > maxVelocityY || (transform.position.y > ScreenTop && rigidbody2d.velocity.y > 0))
    //    {          
    //        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
    //    }
        
    //}

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
