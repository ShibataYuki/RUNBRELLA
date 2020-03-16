using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 空中状態のときにスピードをチェックして止まらないようにする
/// </summary>
public class PlayerAerial : MonoBehaviour
{
    // リジッドボディのコンポーネント
    private new Rigidbody2D rigidbody;
    // 地面のレイヤー
    [SerializeField]
    private LayerMask groundLayer = 0;
    // ブロックのレイヤー
    [SerializeField]
    private LayerMask blockLayer = 0;
    // 速度減衰値
    [SerializeField]
    float decaySpeed = 0.05f;
    Player player;

    [SerializeField]
    float aerialGravityScale = 3;
    // ファイル名
    private readonly string fileName = nameof(PlayerAerial) + "Data";
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        // コンポーネントの取得
        rigidbody = GetComponent<Rigidbody2D>();
        // Rayの発射位置の指定を足元に変更
        var collider = GetComponent<BoxCollider2D>();
        // テキストの読み込み
        //decaySpeed = TextManager.Instance.GetValue(fileName, nameof(decaySpeed));
        //aerialGravityScale = TextManager.Instance.GetValue(fileName, nameof(aerialGravityScale));
    }

    /// <summary>
    /// 開始時処理
    /// </summary>
    public void StartAerial()
    {
        player.Rigidbody.gravityScale = aerialGravityScale;
    }

    /// <summary>
    /// 終了時処理
    /// </summary>
    public void EndAerial()
    {
        player.Rigidbody.gravityScale = 1;
    }

    /// <summary>
    /// 最低速度を下回っているかチェックして
    /// 遅かったら最低速度に変更
    /// </summary>
    public void SpeedCheck()
    {
        // 速度の取得
        var velocity = rigidbody.velocity;
        
        // 速度の制限処理
        velocity.x -= decaySpeed;
        if (velocity.x < player.BaseSpeed)
        {
            velocity.x = player.BaseSpeed;
        }

        rigidbody.velocity = velocity;        

    }
          
}
