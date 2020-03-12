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
    // 当たり判定の領域
    private Vector2 offsetBottomLeft = Vector2.zero;
    private Vector2 offsetTopRight = Vector2.zero;
    private Vector2 offsetBottomLeftUnder = Vector2.zero;
    // 地面のレイヤー
    [SerializeField]
    private LayerMask groundLayer = 0;
    // ブロックのレイヤー
    [SerializeField]
    private LayerMask blockLayer = 0;
    // Rayの長さ
    [SerializeField]
    private float rayLangth = 0.5f;
    // 速度減衰値
    [SerializeField]
    float decaySpeed = 0.05f;
    Player player;

    [SerializeField]
    float aerialGravityScale = 3;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        // コンポーネントの取得
        rigidbody = GetComponent<Rigidbody2D>();
        // Rayの発射位置の指定を足元に変更
        var collider = GetComponent<BoxCollider2D>();
        offsetBottomLeft = collider.offset;
        offsetTopRight = collider.offset;
        offsetBottomLeftUnder = collider.offset;
        offsetBottomLeft.x += +(collider.size.x * 0.5f);
        offsetBottomLeft.y += -(collider.size.y * 0.5f);
        offsetBottomLeftUnder.x += +(collider.size.x * 0.5f);
        offsetBottomLeftUnder.y += -(collider.size.y * 0.51f);
        offsetTopRight.x += +((collider.size.x * 0.5f) + rayLangth);
        offsetTopRight.y += +(collider.size.y * 0.5f);
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
        // 当たり判定の領域
        var bottomLeft = offsetBottomLeft + (Vector2)transform.position;
        // 上に向かっているなら
        if (velocity.y > 1f)
        {
            // 当たり判定の大きさを大きめにする
            bottomLeft = offsetBottomLeftUnder + (Vector2)transform.position;
        }
        // y方向のベクトルが0かそれに近いなら
        else if (velocity.y < 0.1f)
        {
            // 当たり判定の大きさを小さめにする
            bottomLeft = (offsetBottomLeft * 2 - offsetBottomLeftUnder) + (Vector2)transform.position;
        }
        

        // 速度の制限処理
        velocity.x -= decaySpeed;
        if (velocity.x < player.BaseSpeed)
        {
            velocity.x = player.BaseSpeed;
        }

        rigidbody.velocity = velocity;        

    }
          
}
