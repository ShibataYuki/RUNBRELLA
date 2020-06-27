using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RING_MODE
{
    CONTRACTION,
    ROTATION,
}

public class Ring : MonoBehaviour
{
    // エフェクト
    private ParticleSystem ringParticleSystem;
    private RingEffectFactory ringEffectFactory;
    // 子オブジェクトのリング縮小用リング
    [SerializeField]
    private RingContractionManager ringContractionManager= default;
    [SerializeField]
    private Animator ringAnimator = default;
    // リング通過時の音
    [SerializeField]
    AudioClip audioClip = null;
    // リング通過時のリングの挙動のモード
    [SerializeField]
    private RING_MODE ringMode = default;
    // リング通過時のリングが回る時間
    [SerializeField]
    private float rotationTime = default;
    private bool isDeceleration = false;

    private void Start()
    {
        // 子オブジェクトの参照
        var particleObject = transform.Find("Particle System").gameObject;
        // 子オブジェクトからコンポーネントを取得
        ringParticleSystem = particleObject.GetComponent<ParticleSystem>();
        ringEffectFactory = GameObject.Find("RingEffectFactory").GetComponent<RingEffectFactory>();
        if(ringMode==RING_MODE.ROTATION)
        {
            ringAnimator.SetFloat("Speed", 0);
        }
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
            var playerType = collision.GetComponent<Character>().charType;
            if(playerType==GameManager.CHARTYPE.PlayerA)
            {
                Debug.Log("ring");
            }
            // リング通過時の音再生
            AudioManager.Instance.PlaySE(audioClip, 0.15f);
            // 弾数増加
            {
                var playerAttack = collision.gameObject.GetComponent<PlayerAttack>();
                playerAttack.AddBulletCount(1);
            }
            if(ringMode==RING_MODE.CONTRACTION)
            {
                ringContractionManager.UseRingContraction();
            }
            else
            {
                // 既にコルーチンが開始しているなら停止させる
                if(isDeceleration)
                {
                    StopCoroutine(DecelerationAnimatorSpeed());
                }
                StartCoroutine(DecelerationAnimatorSpeed());
            }
        }
    }

    /// <summary>
    /// アニメーションのスピードを減速させるメソッド
    /// </summary>
    private IEnumerator DecelerationAnimatorSpeed()
    {
        // 二次関数を用いて減速量を求める
        isDeceleration = true;
        var nowTime = 0f;
        while(true)
        {
            // 残り時間
            var remainingTime = rotationTime - nowTime;
            var timeScale = (remainingTime / rotationTime) * (remainingTime / rotationTime);
            timeScale = Mathf.Clamp01(timeScale);
            nowTime += Time.deltaTime;
            ringAnimator.SetFloat("Speed", timeScale);
            if(nowTime>=rotationTime)
            {
                ringAnimator.SetFloat("Speed", 0);
                isDeceleration = false;
                yield break;
            }
            yield return null;
            
        }
    }


}
