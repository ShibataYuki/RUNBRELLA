using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeline : MonoBehaviour
{
    Rigidbody2D rigidBody2D;       
    [SerializeField]
    float jumpGravityScale = 3;
    [SerializeField]
    AudioClip jumpSE = null;
    [SerializeField, Range(0, 1)]
    float SEVolume = 1;
    SpriteRenderer spriteRenderer;
    IEnumerator toEnableImpressionTimer;
   

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();       
        spriteRenderer = GetComponent<SpriteRenderer>();        
    }


    /// <summary>
    /// タイムライン中のジャンプ処理
    /// </summary>
    public void Jump_Timeline(float jumpPower)
    {        
        rigidBody2D.gravityScale = jumpGravityScale;
        // ジャンプさせる
        rigidBody2D.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        AudioManager.Instance.PlaySE(jumpSE, SEVolume);
    }

    /// <summary>
    /// スプライトを左右に切り替え
    /// </summary>
    public void FlipX()
    {
        spriteRenderer.flipX = !(spriteRenderer.flipX);
    }

    public void PlayImpressions(string childName)
    {
        var Impression = transform.Find("Impressions/" + childName);
        var effect = Impression.GetComponent<ParticleSystem>();
        effect.Play();
    }

}
