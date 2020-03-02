using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlide : MonoBehaviour
{
    // 自身のリジットボディ
    Rigidbody2D rigidbody2d;
    SpriteRenderer sprite;

    [SerializeField]
    float maxVelocityY = 1.5f;

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
        rigidbody2d.gravityScale = 0.07f;
        // 上昇中なら上昇をやめる
        if(rigidbody2d.velocity.y > 0)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
        }
       // rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x * 0.7f, rigidbody2d.velocity.y);

        

    // デバッグ用色の変更
    sprite.color = Color.green;

    }

    /// <summary>
    /// Y方向への速度の制限処理
    /// </summary>
    public void RestrictVectorY()
    {
        if (rigidbody2d.velocity.y > maxVelocityY)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, maxVelocityY);
        }
    }

    /// <summary>
    /// 滑空の終了処理
    /// </summary>
    public void EndGlide()
    {
        rigidbody2d.gravityScale = 1;        
    }
}
