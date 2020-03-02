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
    private float minSpeed = 0.1f;
    // 当たり判定の領域
    private Vector2 layStartPointOffset = Vector2.zero;
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
        layStartPointOffset = collider.offset;
        layStartPointOffset += (collider.size * 0.5f);
    }

    /// <summary>
    /// 最低速度を下回っているかチェックして
    /// 遅かったら最低速度に変更
    /// </summary>
    public void SpeedCheck()
    {
        // 速度の取得
        var velocity = rigidbody.velocity;
        var layStartPoint = layStartPointOffset + (Vector2)transform.position;
        // Rayが当たるなら
        if (Physics2D.Raycast(layStartPoint, Vector2.right, rayLangth, groundLayer))
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

    }
}
