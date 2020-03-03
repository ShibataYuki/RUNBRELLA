using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 空中状態のときにスピードをチェックして止まらないようにする
/// </summary>
public class PlayerAerialSpeedCheck : MonoBehaviour
{
    // リジッドボディのコンポーネント
    private new Rigidbody2D rigidbody;
    // 最低速度
    [SerializeField]
    private float minSpeed = 6.0f;
    // 当たり判定の領域
    private Vector2 offsetBottomLeft = Vector2.zero;
    private Vector2 offsetTopRight   = Vector2.zero;
    private Vector2 offsetBottomLeftUnder = Vector2.zero;
    // 地面のレイヤー
    [SerializeField]
    private LayerMask groundLayer = 0;
    // Rayの長さ
    [SerializeField]
    private float rayLangth = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        rigidbody = GetComponent<Rigidbody2D>();
        // Rayの発射位置の指定を足元に変更
        var collider = GetComponent<BoxCollider2D>();
        offsetBottomLeft = collider.offset;
        offsetTopRight   = collider.offset;
        offsetBottomLeftUnder = collider.offset;
        offsetBottomLeft.x += +(collider.size.x * 0.5f);
        offsetBottomLeft.y += -(collider.size.y * 0.5f);
        offsetBottomLeftUnder.x += +(collider.size.x * 0.5f);
        offsetBottomLeftUnder.y += -(collider.size.y * 0.51f);
        offsetTopRight.x   += +((collider.size.x * 0.5f) + rayLangth);
        offsetTopRight.y   += +(collider.size.y * 0.5f);
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
        if (velocity.y > 0)
        {
            // 当たり判定の大きさを大きめにする
            bottomLeft = offsetBottomLeftUnder + (Vector2)transform.position;
        }
        var topRight   = offsetTopRight   + (Vector2)transform.position;
        // 前方のコライダーを検知
        bool isHit;
        
        isHit = Physics2D.OverlapArea(bottomLeft, topRight, groundLayer);
        
        if (isHit == true)
        {
            // 速度をにする
            velocity.x = 0.0f;
        }

        // 横方向の移動量が最低速度以下なら
        else if (Mathf.Abs(velocity.x) < Mathf.Abs(minSpeed))
        {
            // 横方向の移動量を最低速度に変更
            velocity.x = minSpeed;
        }
        rigidbody.velocity = velocity;
#if UNITY_EDITOR
        // 上側の線
        var startPoint = new Vector2(bottomLeft.x, topRight.y);
        var endPoint = new Vector2(topRight.x, topRight.y);
        Debug.DrawLine(startPoint, endPoint, Color.yellow);
        // 下側の線
        startPoint.Set(bottomLeft.x, bottomLeft.y);
        endPoint.Set(topRight.x, bottomLeft.y);
        Debug.DrawLine(startPoint, endPoint, Color.yellow);
        // 右側の線
        startPoint.Set(topRight.x, topRight.y);
        endPoint.Set(topRight.x, bottomLeft.y);
        Debug.DrawLine(startPoint, endPoint, Color.yellow);
        // 左側の線
        startPoint.Set(bottomLeft.x, topRight.y);
        endPoint.Set(bottomLeft.x, bottomLeft.y);
        Debug.DrawLine(startPoint, endPoint, Color.yellow);
#endif
    }

    /// <summary>
    /// 前方に地面があるかチェックするメソッド
    /// </summary>
    /// <returns>地面があるか</returns>
    public bool FrontGroundCheck()
    {
        // 速度の取得
        var velocity = rigidbody.velocity;
        // 当たり判定の領域
        var bottomLeft = offsetBottomLeft + (Vector2)transform.position;
        // 上に向かっているなら
        if (velocity.y > 0)
        {
            // 当たり判定の大きさを大きめにする
            bottomLeft = offsetBottomLeftUnder + (Vector2)transform.position;
        }
        var topRight = offsetTopRight + (Vector2)transform.position;
        // 前方のコライダーを検知
        bool isHit;

        isHit = Physics2D.OverlapArea(bottomLeft, topRight, groundLayer);
        return isHit;
    }
}
