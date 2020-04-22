using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField]
    float addVelocityX = 2f;
    // エフェクト
    private new ParticleSystem particleSystem;
    private RingEffectFactory ringEffectFactory;
    // 子オブジェクトのリング縮小用アニメーター
    [SerializeField]
    private Animator ringContractionAnimator = null;
    // リング通過時の音
    [SerializeField]
    AudioClip audioClip = null;

    private void Start()
    {
        // 子オブジェクトの参照
        var particleObject = transform.Find("Particle System").gameObject;
        // 子オブジェクトからコンポーネントを取得
        particleSystem = particleObject.GetComponent<ParticleSystem>();
        ringEffectFactory = GameObject.Find("RingEffectFactory").GetComponent<RingEffectFactory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            // 加速
            {
                //var player = collision.gameObject.GetComponent<Player>();
                //var addVelocity = new Vector2(addVelocityX, 0);
                //player.Rigidbody.velocity += addVelocity;
            }
            // リング通過時の音再生
            AudioManager.Instance.PlaySE(audioClip, 0.1f);
            // 弾数増加
            {
                var playerAttack = collision.gameObject.GetComponent<PlayerAttack>();
                playerAttack.AddBulletCount(1);
            }
            // 縮小エフェクト再生
            ringContractionAnimator.SetTrigger("StartTrigger");
        }
    }
    
}
