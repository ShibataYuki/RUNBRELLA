using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeline : MonoBehaviour
{
    Rigidbody2D rigidBody2D;
    [SerializeField]
    Vector2 jumpPower = new Vector2(0, 20f);   
    [SerializeField]
    float jumpGravityScale = 3;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();       
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// タイムライン中のジャンプ処理
    /// </summary>
    public void Jump_Timeline()
    {
        rigidBody2D.gravityScale = jumpGravityScale;
        // ジャンプさせる
        rigidBody2D.AddForce(jumpPower, ForceMode2D.Impulse);
    }

    /// <summary>
    /// スプライトを左右に切り替え
    /// </summary>
    public void FlipX()
    {
        spriteRenderer.flipX = !(spriteRenderer.flipX);
    }
}
