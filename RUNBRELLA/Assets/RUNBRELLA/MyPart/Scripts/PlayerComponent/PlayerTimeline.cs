using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイムライン中の演出でキャラクターを制御するクラス
/// </summary>
public class PlayerTimeline : MonoBehaviour
{
    // ジャンプ中にかける重力
    [SerializeField]
    float jumpGravityScale = 3;
    // ジャンプ時のSE
    [SerializeField]
    AudioClip jumpSE = null;
    // SEのボリューム
    [SerializeField, Range(0, 1)]
    float SEVolume = 1;
    // 必要なコンポーネント
    Rigidbody2D rigidBody2D;       
    SpriteRenderer spriteRenderer;       

    private void Awake()
    {
        // コンポーネント取得
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

    /// <summary>
    /// エフェクトの再生
    /// </summary>
    /// <param name="childName"></param>
    public void PlayImpressions(string childName)
    {
        var Impression = transform.Find("Impressions/" + childName);
        var effect = Impression.GetComponent<ParticleSystem>();
        effect.Play();
    }

}
