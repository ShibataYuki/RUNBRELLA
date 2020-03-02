using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlide : MonoBehaviour
{
    // 自身のリジットボディ
    Rigidbody2D rigidbody2d;
    SpriteRenderer sprite;

    private void Start()
    {
        // 変数の初期化
        rigidbody2d = transform.GetComponent<Rigidbody2D>();

        sprite = GetComponent<SpriteRenderer>();

   }

    /// <summary>
    /// 滑空の開始処理
    /// </summary>
    public void StartGlide()
    {
        // 重力を0.5に変更
        rigidbody2d.gravityScale = 0.1f;
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
        // デバッグ用色の変更
        sprite.color = Color.green;

    }

    /// <summary>
    /// 滑空の終了処理
    /// </summary>
    public void EndGlide()
    {
        rigidbody2d.gravityScale = 1;
    }
}
