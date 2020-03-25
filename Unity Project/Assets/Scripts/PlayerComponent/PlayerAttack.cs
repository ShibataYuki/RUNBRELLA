using System.Collections;
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
    public int nowBulletCount = 6;
    // 通常の弾のリロード時間
    public float DefaultBulletChargeTime
    {
        get { return defaultBulletChargeTime; }
    }
    [SerializeField]
    private float defaultBulletChargeTime = 3;
    // 雨が降っているときの弾のリロード時間
    public float RainBulletChargeTime
    {
        get { return rainBulletChargeTime; }
    }
    [SerializeField]
    private float rainBulletChargeTime = 1;
    // 今の弾のリロード時間
    [SerializeField]
    private float nowBulletChargeTime;
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
    // 剣攻撃用当たり判定
    [SerializeField]
    private Collider2D sordCollider2d = null;
    // デバック用の剣攻撃の当たり判定のスプライト
    [SerializeField]
    private SpriteRenderer sordColliderSpriteRenderer = null;
    // 剣攻撃の当たり判定を表示するフレーム数
    [SerializeField]
    private int sordAttackFrame = 30;
    // 剣攻撃で弾をガードできるフレーム数
    [SerializeField]
    private int guardBulletFrame = 5;
    // 現在のフレーム数
    private int nowFrame = 0;
    // 剣攻撃中かどうか
    private bool isAttacking = false;
    // 剣で弾をガードできるかどうか
    public bool isGuardBullet = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region 銃攻撃関連
        bulletFactory = GameObject.Find("BulletFactory").GetComponent<BulletFactory>();
        animator = GetComponent<Animator>();
        var player = GetComponent<Player>();
        // 読み込むファイルのファイル名
        //string fileName = nameof(PlayerAttack) + "Data" + player.Type;
        // テキストの読み込み
        //bulletCount = TextManager.Instance.GetValue_int(fileName, nameof(bulletCount));
        //defaultBulletChargeTime = TextManager.Instance.GetValue_float(fileName, nameof(defaultBulletChargeTime));
        //rainBulletChargeTime = TextManager.Instance.GetValue_float(fileName, nameof(rainBulletChargeTime));
        //SEVolume = TextManager.Instance.GetValue_float(fileName, nameof(SEVolume));

        #endregion

        #region 剣攻撃関連
        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        #region 銃攻撃関連
        // 雨が降っているならチャージ時間を短くする
        if (player.IsRain)
        {
            if (nowBulletChargeTime != rainBulletChargeTime)
            {
                nowTime = 0;
                nowBulletChargeTime = rainBulletChargeTime;
            }
        }
        else
        {
            if (nowBulletChargeTime != defaultBulletChargeTime)
            {
                nowTime = 0;
                nowBulletChargeTime = defaultBulletChargeTime;
            }
        }

        // ゲームが開始したらチャージ開始
        if (SceneController.Instance.isStart)
        {
            // 弾をチャージ
            ChargeBulletOverTime();
        }

        #endregion

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
        if(player.charAttackType==GameManager.CHARATTACKTYPE.SORD)
        {
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
        if (nowBulletCount > 0)
        {
            // ゲージを消費
            ChangeBulletCount(-1);
            // 弾発射
            bulletFactory.ShotBullet(gameObject);
            // SEの再生
            AudioManager.Instance.PlaySE(shotSE, SEVolume);
            // アニメーターにセット
            animator.SetTrigger(shotID);
        }
    }


    /// <summary>
    /// 弾をチャージする時間
    /// </summary>
    void ChargeBulletOverTime()
    {
        nowTime += Time.deltaTime;
        if (nowTime >= nowBulletChargeTime)
        {
            nowTime = 0;
            ChangeBulletCount(1);
        }
    }

    /// <summary>
    /// 現在の所持弾数を変化させる関数
    /// </summary>
    /// <param name="value">増減させたい値</param>
    public void ChangeBulletCount(int value)
    {        
        nowBulletCount += value;
        // 最大値を超えていたら最大値に修正
        if(nowBulletCount >= MaxBulletCount)
        {
            nowBulletCount = MaxBulletCount;
        }
    }
    #endregion

    #region 剣攻撃関連関数
    /// <summary>
    /// 剣攻撃の開始処理をする関数
    /// </summary>
    void StartSlash()
    {
        // 剣攻撃用当たり判定を表示する
        sordCollider2d.enabled = true;
        // デバック用の円のスプライトを表示する
        sordColliderSpriteRenderer.enabled = true;
        // すでに剣攻撃中なら剣攻撃しない
        if(isAttacking==true)
        {
            return;
        }
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
        sordCollider2d.enabled = false;
        // デバック用の円のスプライトを非表示にする
        sordColliderSpriteRenderer.enabled = false;
        // 剣攻撃かどうかのフラグをOFFにする
        isAttacking = false;
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
            if(nowFrame>=sordAttackFrame)
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
            if (collision == sordCollider2d)
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
