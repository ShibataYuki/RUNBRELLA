﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField]
    Player player = null;
    // 攻撃に当たったか
    [SerializeField]
    bool isHit = false;
    public bool IsHit { set { isHit = value; } get { return isHit; } }

    #region 銃攻撃関連
    BulletFactory bulletFactory;
    // 弾の最大所持数
    [SerializeField]
    public int MaxBulletCount
    {
        get { return maxBulletCount; }
    }
    [SerializeField]
    private int maxBulletCount = 3;
    // 現在の弾の所持数
    [SerializeField]
    private int nowBulletCount;
    public int NowBulletCount { set { nowBulletCount = value; } get { return nowBulletCount; } }
    // 現在の経過時間
    float nowTime = 0;
    // 弾発射のSE
    [SerializeField]
    private AudioClip shotSE = null;
    // SEのボリューム
    [SerializeField]
    private float SEVolume = 1f;

    // 必要なコンポーネント
    Animator animator;
    int shotID = Animator.StringToHash("Shot");

    #endregion

    #region 剣攻撃関連
    // 剣攻撃用当たり判定オブジェクト
    [SerializeField]
    private GameObject swordColliderObj = null;
    // 剣攻撃用当たり判定
    [SerializeField]
    private Collider2D swordCollider2d = null;
    // デバック用の剣攻撃の当たり判定のスプライト
    [SerializeField]
    private SpriteRenderer swordColliderSpriteRenderer = null;
    // 剣攻撃の当たり判定を表示するフレーム数
    [SerializeField]
    private int swordAttackFrame = 30;
    // 剣攻撃で弾をガードできるフレーム数
    [SerializeField]
    private int guardBulletFrame = 5;
    // 現在のフレーム数
    private int nowFrame = 0;
    // 剣攻撃中かどうか
    private bool isAttacking = false;
    // 剣で弾をガードできるかどうか
    public bool isGuardBullet = false;
    // 雨時の剣の当たり判定のサイズ
    [SerializeField]
    private Vector2 rainSwordColliderSize = new Vector2(0, 0);
    // 剣攻撃の音
    [SerializeField]
    AudioClip swordAudioClip = null;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region 銃攻撃関連
        bulletFactory = GameObject.Find("BulletFactory").GetComponent<BulletFactory>();
        animator = GetComponent<Animator>();
        var player = GetComponent<Player>();
        // 読み込むファイルのファイル名
        string fileName = nameof(PlayerAttack) + "Data" + player.Type;
        // テキストの読み込み
        maxBulletCount = TextManager.Instance.GetValue_int(fileName, nameof(maxBulletCount));
        SEVolume = TextManager.Instance.GetValue_float(fileName, nameof(SEVolume));

        #endregion

        #region 剣攻撃関連
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region 剣攻撃関連
        #endregion
    }

    public void Attack()
    {
        // 攻撃手段により呼ぶ関数を変更する
        if(player.charAttackType==GameManager.CHARATTACKTYPE.GUN)
        {
            Shot();
        }
        if(player.charAttackType==GameManager.CHARATTACKTYPE.SWORD)
        {
            // 雨天時は剣の当たり判定を変更
            if(player.IsRain)
            {
                ChangeSwordColliderSize();
            }
            StartSlash();
        }
    }

    #region 銃攻撃関連関数
    /// <summary>
    /// 弾を発射する関数です
    /// </summary>
    /// <param name="position"></param>
    void Shot()
    {
        if (NowBulletCount > 0)
        {
            // ゲージを消費
            AddBulletCount(-1);
            // 弾発射
            // 雨なら3WAY
            if(player.IsRain)
            {
                bulletFactory.WhenRainShotBullet(gameObject);
            }
            else
            {
                bulletFactory.ShotBullet(gameObject);
            }
            // SEの再生
            AudioManager.Instance.PlaySE(shotSE, SEVolume);
            // アニメーターにセット
            animator.SetTrigger(shotID);
        }
    }

    /// <summary>
    /// 現在の所持弾数を変化させる関数
    /// </summary>
    /// <param name="value">増減させたい値</param>
    public void AddBulletCount(int value)
    {        
        NowBulletCount += value;
        // 最大値を超えていたら最大値に修正
        if(NowBulletCount >= MaxBulletCount)
        {
            NowBulletCount = MaxBulletCount;
        }
    }
    #endregion

    #region 剣攻撃関連関数
    /// <summary>
    /// 剣攻撃の開始処理をする関数
    /// </summary>
    void StartSlash()
    {
        // すでに剣攻撃中なら剣攻撃しない
        if (isAttacking == true)
        {
            return;
        }
        // ゲージが0なら攻撃できない
        if(NowBulletCount<1)
        {
            return;
        }
        AudioManager.Instance.PlaySE(swordAudioClip, 0.5f);
        AddBulletCount(-1);
        // 剣攻撃用当たり判定を表示する
        swordCollider2d.enabled = true;
        // デバック用の円のスプライトを表示する
        swordColliderSpriteRenderer.enabled = true;
        // 剣攻撃かどうかのフラグをONにする
        isAttacking = true;
        // 剣で弾をガードできるかどうかのフラグをONにする
        isGuardBullet = true;
        // フレームカウント開始
        StartCoroutine(SordAttackFrameCount());
    }

    /// <summary>
    /// 剣攻撃の終了処理をする関数
    /// </summary>
    void EndSlash()
    {
        // 剣攻撃用当たり判定を非表示にする
        swordCollider2d.enabled = false;
        // デバック用の円のスプライトを非表示にする
        swordColliderSpriteRenderer.enabled = false;
        // 剣攻撃かどうかのフラグをOFFにする
        isAttacking = false;
    }


    /// <summary>
    /// 雨天時に剣攻撃用当たり判定を変化させる関数
    /// </summary>
    void ChangeSwordColliderSize()
    {
        // サイズを変更
        swordColliderObj.transform.localScale = rainSwordColliderSize;
        // オフセットを設定
        swordColliderObj.transform.localPosition = 
            new Vector3(rainSwordColliderSize.x, swordColliderObj.transform.localPosition.y, swordColliderObj.transform.localPosition.z);
    }

    #endregion

    #region 剣攻撃関連コルーチン
    /// <summary>
    /// フレームのカウントをするコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator SordAttackFrameCount()
    {
        while(true)
        {
            // フレームをカウント
            nowFrame++;
            if(nowFrame>=guardBulletFrame&&isGuardBullet)
            {
                isGuardBullet = false;
            }
            if(nowFrame>=swordAttackFrame)
            {
                nowFrame = 0;
                // 終了処理
                EndSlash();
                yield break;
            }
            yield return null;
        }
    }
    #endregion

    #region 被弾時処理
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        // 被弾時
        if(collision.tag=="Bullet")
        {
            var bullet = collision.GetComponent<Bullet>();
            // 自分が撃った弾なら被弾しない
            if(bullet.ID==player.ID)
            {
                return;
            }
            // すでに攻撃されているなら攻撃されない
            if (IsHit==true)
            {
                return;
            }
            IsHit = true;
            bullet.IsShoting = false;
            bulletFactory.ReturnBullet(collision.gameObject);
        }
        // 剣攻撃を食らった時
        if(collision.gameObject.tag=="SordCollider")
        {
            // 自分の攻撃の場合はヒットしない
            if (collision == swordCollider2d)
            {
                return;
            }
            // すでに攻撃されているなら攻撃されない
            if (IsHit == true)
            {
                return;
            }
            // ヒットフラグをONにする
            IsHit = true;
        }
    }
    #endregion

}
