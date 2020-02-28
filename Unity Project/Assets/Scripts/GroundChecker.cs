using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    // プレイヤースクリプトの参照
    private Player player;

    // プレイヤーのポジションと接地判定の領域の差分
    private Vector2 offsetLeftTop;
    private Vector2 offsetRightBottom;

    // 地面のレイヤー情報
    private LayerMask groundLayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        player = GetComponent<Player>();
        // コライダーの取得
        var collider = GetComponent<BoxCollider2D>();
        // コライダーのオフセット設定
        offsetLeftTop     = collider.offset;
        offsetRightBottom = collider.offset;
        // プレイヤーの足元に変更
        offsetLeftTop.y     += -(collider.size.y * 0.5f);
        offsetRightBottom.y += -(collider.size.y * 0.5f);
        // サイズを小さくなるよう変更
        var size = collider.size;
        size.x *= 0.5f;
        size.y *= 0.25f;
        // ポジションを上下左右にずらす
        offsetLeftTop.x     += -(size.x * 0.5f);
        offsetRightBottom.x += +(size.x * 0.5f);
        offsetLeftTop.y     += +(size.y * 0.5f);
        offsetRightBottom.y += -(size.y * 0.5f);
    }
    private void FixedUpdate()
    {
        // 接地判定を行う領域を設定
        var leftTop     = offsetLeftTop     + (Vector2)transform.position;
        var rightBottom = offsetRightBottom + (Vector2)transform.position;
        // 接地判定を行う
        player.IsGround = Physics2D.OverlapArea(leftTop, rightBottom, groundLayer);
    }
}
