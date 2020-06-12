using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    // ブースト時のスピードのディクショナリー
    private Dictionary<int, float> boostSpeed = new Dictionary<int, float>();
    // 必要なコンポーネント
    private Rigidbody2D playerRigidbody;
    // ブーストする時間のディクショナリー
    private Dictionary<int, float> boostTime = new Dictionary<int, float>();
    // 移動クラス
    PlayerMove move;
    // 経過時間
    private float deltaTime = 0.0f;
    // 弾を消すエリアを展開している時間
    [SerializeField]
    float vanishBulletsframe = 3f;
    // ブースト中の重力の大きさ
    [SerializeField]
    private float boostGravityScale = 0.0f;
    // ブースト前の重力の大きさ
    private float defaultGravityScale;
    /*保留
    // ブースト前のスピード
    private float beforeSpeed;
     */
     // ブーストのSE
    [SerializeField]
    private AudioClip boostSE = default;
    // SEのボリューム
    [SerializeField]
    private float seVoluem = 1.0f;
	// 地面のチェックするためのクラス
    private PlayerAerial aerial;

    // プレイヤーのコンポーネント
    private Character charcter;
    // ゲージを減らすために必要
    private PlayerAttack playerAttack;

    // 使用するゲージ
    private int gaugeCount;
    public int GaugeCount { get { return gaugeCount; } set { gaugeCount = value; } }
    // ブースト終了時の減速量（割合）のディクショナリー
    private Dictionary<int, float> afterSpeedDown = new Dictionary<int, float>();

    BoxCollider2D boxCollider2D;
    LayerMask layerMask;

    #region  剣を持つプレイヤーのみに必要なパラメータ
    // 当たり判定の領域計算用のポイント
    private Vector2 leftBottom;
    private Vector2 rightTop;

    // プレイヤーのレイヤー
    private LayerMask playerLayer;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        playerRigidbody = GetComponent<Rigidbody2D>();
        aerial = GetComponent<PlayerAerial>();
        charcter = GetComponent<Character>();
        playerAttack = GetComponent<PlayerAttack>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        move = GetComponent<PlayerMove>();

        ReadTextParameter();

        // レイヤーマスクを「Slider」に設定
        layerMask = LayerMask.GetMask(new string[] { "Bullet" });

        // 剣を持っているなら
        if (charcter.charAttackType == GameManager.CHARATTACKTYPE.SWORD)
        {
            // ブースト中の当たり判定用の領域を計算
            leftBottom = boxCollider2D.offset;
            rightTop = boxCollider2D.offset;
            leftBottom += -(boxCollider2D.size * 0.5f);
            rightTop += (boxCollider2D.size * 0.5f);

            // プレイヤーのレイヤーマスクをセット
            playerLayer = LayerMask.GetMask(new string[] { "Player" });
        }
    }

    /// <summary>
    /// テキストからパラメータ
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var textName = "";
        switch(charcter.charType)
        {
            case GameManager.CHARTYPE.PlayerA:
                textName = "Chara_A";
                break;
            case GameManager.CHARTYPE.PlayerB:
                textName = "Chara_B";
                break;
        }
        // テキストの中のデータをセットするディクショナリー
        Dictionary<string, float> boostDictionary;
        SheetToDictionary.Instance.TextToDictionary(textName, out boostDictionary);

        try
        {
            // ファイル読み込み
            for (int i = 1; i <= 5; i++)
            {
                boostTime.Add(i, boostDictionary[string.Format("消費したエネルギー量が{0}の場合のブーストする秒数", i)]);
                boostSpeed.Add(i, boostDictionary[string.Format("消費したエネルギー量が{0}の場合のブースト中のスピードの秒速", i)]);
                afterSpeedDown.Add(i, boostDictionary[string.Format("消費したエネルギー量が{0}の場合のブースト終了後の減速量の割合", i)]);
            }
            vanishBulletsframe = boostDictionary["弾を消すエリアを展開しているフレーム数"];
            boostGravityScale = boostDictionary["ブースト状態の場合における重力加速度の倍率"];
        }
        catch
        {
            Debug.Assert(false, nameof(PlayerBoost) + "でエラーが発生しました");
        }
    }

    /// <summary>
    /// エネルギーをチェックしてブーストの開始処理
    /// </summary>
    public void BoostStart()
    {
        // ブースト中の重力を使用する
        BoostGravityStart();
        /*保留
        beforeSpeed = rigidbody.velocity.x;
         */
        // ゲージを減らす
        playerAttack.NowBulletCount -= gaugeCount;
        // SEの再生
        AudioManager.Instance.PlaySE(boostSE, seVoluem);
    }

    public void VanishBulletsArea_ON()
    {
        StartCoroutine(nameof(VanishBulletsArea));
    }

    public void VanishBulletsArea_OFF()
    {
        StopCoroutine(nameof(VanishBulletsArea));
    }

    /// <summary>
    /// 後方に弾を消すエリアを展開
    /// </summary>
    public IEnumerator VanishBulletsArea()
    {
        float totalTime = 0f;
        // 1フレームを秒に直した値
        const float oneFrameAsSecond = 0.016f;
        // 秒からフレームに直す           
        vanishBulletsframe *= oneFrameAsSecond;
        while(true)
        {
            // 指定時間を経過したらエリアを解除
            if(totalTime >= vanishBulletsframe)
            {
                yield break;
            }
            
            // 弾削除エリア左上
            Vector2 vanishLeftTop;
            // 弾削除エリア右下
            Vector2 vanishRightBottom;
            // 当たった弾のコライダー
            Collider2D hit = null;
            BulletFactory bulletFactory = GameObject.Find("BulletFactory").GetComponent<BulletFactory>(); 
            // 弾削除エリアを割り出す計算
            {
                // プレイヤーのコライダーの中心座標
                var colliderCenterPos = (Vector2)transform.position + boxCollider2D.offset;
                // プレイヤーのコライダーの左上の座標
                var colliderLeftTop = colliderCenterPos + new Vector2(-boxCollider2D.size.x / 2, boxCollider2D.size.y / 2);
                // プレイヤーのコライダーの右下の座標
                var colliderRightBottom = colliderCenterPos + new Vector2(boxCollider2D.size.x / 2, -boxCollider2D.size.y / 2);
                var offset = new Vector2(boxCollider2D.size.x, 0);
                vanishLeftTop = colliderLeftTop -= offset * 2;
                vanishRightBottom = colliderRightBottom -= offset;
            }
            // エリアに入った弾のコライダー
            hit = Physics2D.OverlapArea(vanishLeftTop, vanishRightBottom, layerMask);
            // エリアに弾が入っていたら非アクティブ化
            if (hit)
            {
                var hitBullet = hit.GetComponent<Bullet>();
                hitBullet.IsShoting = false;
                bulletFactory.ReturnBullet(hitBullet.gameObject);
            }
            // 時間の更新
            totalTime += Time.deltaTime;
            //Debug.Log(totalTime);
            Debug.DrawLine(vanishLeftTop, vanishRightBottom, Color.cyan);
            yield return null;
        }
                        
    }


    /// <summary>
    /// ブースト中の重力を使用する
    /// </summary>
    public void BoostGravityStart()
    {
        defaultGravityScale = playerRigidbody.gravityScale;
        playerRigidbody.gravityScale = boostGravityScale;
    }

    /// <summary>
    /// ブースト処理
    /// </summary>
    public void Boost()
    {
        // 加速度の蓄積
        move.AddAcceleration();
        // 距離ベクトルを計算して、力を加える
        playerRigidbody.velocity = new Vector2(boostSpeed[gaugeCount], 0.0f);

        // 剣を持っているなら
        if (charcter.charAttackType == GameManager.CHARATTACKTYPE.SWORD)
        {
            // 当たり判定の領域を計算
            var workLeftBottom = leftBottom + (Vector2)transform.position;
            var workRightTop = rightTop + (Vector2)transform.position;
            // 範囲内のプレイヤーを検知
            var hitPlayersCollider = Physics2D.OverlapAreaAll(workLeftBottom, workRightTop, playerLayer);
            // 当たったプレイヤーをすべて取得
            foreach (var hitPlayerCollider in hitPlayersCollider)
            {
                // ゲームオブジェクトを取得
                var hitPlayerObject = hitPlayerCollider.gameObject;
                // 自分自身のオブジェクトならcontinue
                if (hitPlayerObject == gameObject)
                {
                    continue;
                }
                else
                {
                    // 自分自身でなければダメージを与える
                    var hitPlayerAttack = hitPlayerObject.GetComponent<PlayerAttack>();
                    hitPlayerAttack.IsHit = true;
                }
            }
        }
    }

    /// <summary>
    /// 終了するかチェック
    /// </summary>
    /// <returns>終了するかしないか</returns>
    public bool FinishCheck()
    {
        // 経過時間の計測
        deltaTime += Time.deltaTime;
        // 時間が残っているか
        if (deltaTime >= (boostTime[gaugeCount]))
        {
            // 経過時間のリセット
            deltaTime = 0.0f;
            EndBoost();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 重力をブーストする以前の重力に変更
    /// </summary>
    public void EndBoost()
    {
        // 重力をもとに戻す
        playerRigidbody.gravityScale = defaultGravityScale;
        /*保留
        // 速度を減速させる
        rigidbody.velocity = new Vector2((beforeSpeed * (1.0f - afterSpeedDown[gaugeCount])), rigidbody.velocity.y);
         */
    }
}