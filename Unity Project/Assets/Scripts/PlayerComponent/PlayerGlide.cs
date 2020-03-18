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
    // 傘を開いた時のSE
    [SerializeField]
    private AudioClip openSE = null;
    // SEのボリューム
    private float SEVolume = 0.5f;

    private readonly string fileName = nameof(PlayerGlide) + "Data";

    // 加える力
    [SerializeField]
    private float addPower = 0.1f;
    [SerializeField]
    private float maxSpeed = 8f;

    private void Start()
    {
        // 変数の初期化
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerRun = GetComponent<PlayerRun>();      
        playerAerial = GetComponent<PlayerAerial>();
        // テキストの読み込み
        //decaySpeed = TextManager.Instance.GetValue(fileName, nameof(decaySpeed));
        //grideBaseSpeed = TextManager.Instance.GetValue(fileName, nameof(grideBaseSpeed));
        //grideRainSpeed = TextManager.Instance.GetValue(fileName, nameof(grideRainSpeed));
        //SEVolume = TextManager.Instance.GetValue(fileName, nameof(SEVolume));

    }

    /// <summary>
    /// 滑空の開始処理
    /// </summary>
    public void StartGlide()
    {
        // SEの再生
        AudioManager.Instance.PlaySE(openSE, SEVolume);
    }

    /// <summary>
    /// 滑空中の処理
    /// </summary>
    public void Gride()
    {

        // 最低速度の計算
        // var minSpeed = player.IsRain ? grideRainSpeed : grideBaseSpeed;

        rigidbody2d.AddForce(new Vector2(addPower, 0), ForceMode2D.Force);

        var velocity = rigidbody2d.velocity;
        // velocity.x -= decaySpeed;
        if (player.BaseSpeed > velocity.x)
        {
            velocity.x = player.BaseSpeed;
        }
        if(maxSpeed<velocity.x)
        {
            velocity.x = maxSpeed;
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
