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
    [SerializeField]
    private LayerMask groundLayer = 0;

    // プレイヤーのRigidbody
    private Rigidbody2D Rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        player = GetComponent<Player>();
        Rigidbody2d = GetComponent<Rigidbody2D>();
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
        size.x *= 0.75f;
        size.y *= 0.125f;
        // ポジションを上下左右にずらす
        offsetLeftTop.x     += -(size.x * 0.5f);
        offsetRightBottom.x += +(size.x * 0.5f);
        offsetLeftTop.y     += +(size.y * 0.5f);
        offsetRightBottom.y += -(size.y * 0.5f);
    }

#if UNITY_EDITOR
    private void Update()
    {
        var leftTop = offsetLeftTop + (Vector2)transform.position;
        var rightBottom = offsetRightBottom + (Vector2)transform.position;
        // 上側の線
        var startPoint = new Vector2(leftTop.x, leftTop.y);
        var endPoint = new Vector2(rightBottom.x, leftTop.y);
        Debug.DrawLine(startPoint, endPoint, Color.red);
        // 下側の線
        startPoint.Set(leftTop.x, rightBottom.y);
        endPoint.Set(rightBottom.x, rightBottom.y);
        Debug.DrawLine(startPoint, endPoint, Color.red);
        // 右側の線
        startPoint.Set(rightBottom.x, leftTop.y);
        endPoint.Set(rightBottom.x, rightBottom.y);
        Debug.DrawLine(startPoint, endPoint, Color.red);
        // 左側の線
        startPoint.Set(leftTop.x, leftTop.y);
        endPoint.Set(leftTop.x, rightBottom.y);
        Debug.DrawLine(startPoint, endPoint, Color.red);
    }
#endif

    private void FixedUpdate()
    {
        if(Rigidbody2d.velocity.y > 0)
        {
            player.IsGround = false;
            return;
        }
        // 接地判定を行う領域を設定
        var leftTop     = offsetLeftTop     + (Vector2)transform.position;
        var rightBottom = offsetRightBottom + (Vector2)transform.position;
        // 接地判定を行う
        player.IsGround = Physics2D.OverlapArea(leftTop, rightBottom, groundLayer);
    }
}
