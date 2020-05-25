using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // 弾のレイヤー
    private LayerMask bulletLayer;
    // プレイヤーのレイヤー
    private LayerMask playerLayer;
    // 自身の当たり判定の右上
    private Vector2 rightTop;
    // 自身の当たり判定の左下
    private Vector2 leftBottom;
    // 自身の後方の領域の左下
    [SerializeField]
    private Vector2 buckAreaLeftBottom;
    // 自身の上側の領域の左上
    // この領域の右下はrightTop
    [SerializeField]
    private Vector2 upsideAreaLeftTop;
    [SerializeField]
    private Vector2 undersideAreaRightBottom;
    // 何秒後の弾の弾道と当たり判定を行うか
    [SerializeField]
    private float checkTime = 0.5f;
    private float limitTime = 0.0f;
    #region ステート変数
    private IdleState idleState;
    private RunState runState;
    private AerialState aerialState;
    private GlideState glideState;
    private SlideState slideState;
    private AfterSlideState afterSlideState;
    private BoostState boostState;
    private DownState downState;
    #endregion

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        // ステートのコンポーネントを取得
        idleState = GetComponent<IdleState>();
        runState = GetComponent<RunState>();
        aerialState = GetComponent<AerialState>();
        glideState = GetComponent<GlideState>();
        slideState = GetComponent<SlideState>();
        afterSlideState = GetComponent<AfterSlideState>();
        boostState = GetComponent<BoostState>();
        downState = GetComponent<DownState>();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void Start()
    {
        base.Start();
        // レイヤーをセット
        bulletLayer = LayerMask.GetMask("Bullet");
        playerLayer = LayerMask.GetMask("Player");
    }
    #region ステートの変更
    public override void AerialStart()
    {
        ChangeState(aerialState);
    }

    public override void AfterSlideStart()
    {
        ChangeState(afterSlideState);
    }

    public override void BoostStart()
    {
        ChangeState(boostState);
    }

    public override void Down()
    {
        ChangeState(downState);
    }

    public override void GlideStart()
    {
        ChangeState(glideState);
    }

    public override void IdleStart()
    {
        ChangeState(idleState);
    }

    public override void RunStart()
    {
        ChangeState(runState);
    }

    public override void SlideStart()
    {
        ChangeState(slideState);
    }
    #endregion
    #region 現在のステートを確認するためのget
    public override bool IsIdle { get { return state == idleState; } }

    public override bool IsRun { get { return state == runState; } }

    public override bool IsAerial { get { return state == aerialState; } }

    public override bool IsGlide { get { return state == glideState; } }

    public override bool IsSlide { get { return state == slideState; } }

    public override bool IsAfterSlide { get { return state == afterSlideState; } }

    public override bool IsBoost { get { return state == boostState; } }

    public override bool IsDown { get { return state == downState; } }
    #endregion
    #region 特定のアクションを行うか
    public override bool IsJumpStart { get { return BulletLineCheck(); } }

    public override bool IsGlideStart => throw new System.NotImplementedException();

    public override bool IsGlideEnd => throw new System.NotImplementedException();

    public override bool IsSlideStart => throw new System.NotImplementedException();

    public override bool IsSlideEnd => throw new System.NotImplementedException();

    public override bool IsAttack => throw new System.NotImplementedException();

    public override bool IsCharge => throw new System.NotImplementedException();

    public override bool IsBoostStart => throw new System.NotImplementedException();
    #endregion
    // 1秒あたりの加速量
    float addVelocityXPersecond;
    // 最高速度
    float maxVelocityX;

    /// <summary>
    /// テキストからパラメータを取得
    /// </summary>
    protected override void ReadTextParameter()
    {
        // 親クラスのReadTextParameter
        base.ReadTextParameter();
        // 追加部分
        try
        {
            // 読み込むテキストの名前
            var textName = "";
            switch (charAttackType)
            {
                case GameManager.CHARATTACKTYPE.GUN:
                    textName = "Chara_Gun";
                    break;
                case GameManager.CHARATTACKTYPE.SWORD:
                    textName = "Chara_Sword";
                    break;
            }
            // テキストの中のデータをセットするディクショナリー        
            SheetToDictionary.Instance.TextToDictionary(textName, out var textDataDic);
            try
            {
                addVelocityXPersecond = textDataDic["1秒間に増加する速度"];
                maxVelocityX = textDataDic["最高速度の秒速"];
            }
            catch
            {
                Debug.Assert(false, textName + "でエラーが発生しました");
            }
        }
        catch
        {
            Debug.Assert(false, nameof(Enemy) + "でエラーが発生しました");
        }
    }

    /// <summary>
    /// 弾の射線上にいるかチェック
    /// </summary>
    /// <returns></returns>
    private bool BulletLineCheck()
    {
        // 上下に存在する弾のRigidbody
        var bulletRigidbodies = new List<Rigidbody2D>();
        #region 上からの銃弾
        BulletCheck_Upside(bulletRigidbodies);
        #endregion
        #region 左からの銃弾
        BulletCheck_Leftside(bulletRigidbodies);
        #endregion
        #region 下からの銃弾
        BulletCheck_Downside(bulletRigidbodies);
        #endregion
        return BulletHitCheck(bulletRigidbodies);
    }

    /// <summary>
    /// 弾とキャラクターが当たるかチェック
    /// </summary>
    /// <param name="bulletRigidbodies"></param>
    /// <returns></returns>
    private bool BulletHitCheck(List<Rigidbody2D> bulletRigidbodies)
    {
        if (bulletRigidbodies.Count > 0)
        {
            // ジャンプしなかった場合の自身のポジション
            Vector2 thisPosition = transform.position;
            // 自身の移動量
            var velocity = rigidBody.velocity;
            // 弾のコライダーの半径
            var bulletCollidersRadius = new float[bulletRigidbodies.Count];
            // 弾のポジション
            var bulletPositions = new Vector2[bulletRigidbodies.Count];
            for (int i = 0; i < bulletRigidbodies.Count; i++)
            {
                var bulletCollider = bulletRigidbodies[i].GetComponent<CircleCollider2D>();
                // コライダーの半径をセット
                bulletCollidersRadius[i] = bulletCollider.radius;
                // 弾のポジションをセット
                bulletPositions[i] = (Vector2)bulletRigidbodies[i].transform.position + bulletCollider.offset;
            }
            // 弾の弾道と自身の移動ルートをチェック
            for (float fixedDeltaTime = Time.fixedDeltaTime; fixedDeltaTime <= checkTime; fixedDeltaTime += Time.fixedDeltaTime)
            {
                // 前のフレームでの当たり判定の左上
                leftBottom = this.leftBottom + thisPosition;
                // 1フレーム後のポジションに移動
                thisPosition += (velocity * Time.fixedDeltaTime);
                // 加速
                velocity.x += addVelocityXPersecond * Time.fixedDeltaTime;
                // 速度制限
                velocity.x = Mathf.Clamp(velocity.x, 0.0f, maxVelocityX);
                // 新しいポジションでの当たり判定の右上
                rightTop = this.rightTop + thisPosition;
                // 全ての弾をチェック
                for (int i = 0; i < bulletRigidbodies.Count; i++)
                {
                    // 移動後のポジションを求める
                    bulletPositions[i] += bulletRigidbodies[i].velocity * Time.fixedDeltaTime;
                    // 弾の中心
                    var center = bulletPositions[i];
                    // 弾と当たるかチェック
                    if (HitCheckBoxAndCircle(leftBottom, rightTop, center, bulletCollidersRadius[i]))
                    {
                        // いつ頃当たるか
                        limitTime = fixedDeltaTime;
                        return true;
                    } // if(弾と当たるかチェック)
                } // for(全ての弾をチェック)
            } // for(弾の弾道と自身の移動ルートをチェック)
        }
        return false;
    }

    /// <summary>
    /// 左方向からの弾をチェックして、リストに追加
    /// </summary>
    /// <param name="bulletRigidbodies"></param>
    private void BulletCheck_Leftside(List<Rigidbody2D> bulletRigidbodies)
    {
        // 領域内判定の領域の左下
        var leftBottom = this.buckAreaLeftBottom + (Vector2)transform.position;
        // 領域内判定の領域の右上
        var rightTop = this.rightTop + (Vector2)transform.position;
        // 領域内の弾をすべてチェック
        var bulletObjects = Physics2D.OverlapAreaAll(leftBottom, rightTop, bulletLayer);
        for (int i = 0; i < bulletObjects.Length; i++)
        {
            var bulletRigidbody = bulletObjects[i].GetComponent<Rigidbody2D>();
            // リストに追加
            bulletRigidbodies.Add(bulletRigidbody);
        }
    }

    /// <summary>
    /// 下方向からの弾をチェックして、リストに追加
    /// </summary>
    /// <param name="bulletRigidbodies"></param>
    private void BulletCheck_Downside(List<Rigidbody2D> bulletRigidbodies)
    {
        Collider2D[] bulletObjects;
        // 領域内判定の領域の左上
        var undersideAreaLeftTop = leftBottom + (Vector2)transform.position;
        // 領域内判定の領域の右下
        var undersideAreaRightBottom = this.undersideAreaRightBottom + (Vector2)transform.position;
        // 領域内の弾をすべてチェック
        bulletObjects = Physics2D.OverlapAreaAll(undersideAreaLeftTop, undersideAreaRightBottom, bulletLayer);
        for (int i = 0; i < bulletObjects.Length; i++)
        {
            var bulletRigidbody = bulletObjects[i].GetComponent<Rigidbody2D>();
            // ベロシティが上向きなら
            if (bulletRigidbody.velocity.y > rigidBody.velocity.y)
            {
                // リストに追加
                bulletRigidbodies.Add(bulletRigidbody);
            }
        }
    }

    /// <summary>
    /// 上方向からの弾をチェックして、リストに追加
    /// </summary>
    /// <param name="bulletRigidbodies"></param>
    private void BulletCheck_Upside(List<Rigidbody2D> bulletRigidbodies)
    {
        // 領域内判定の領域の左上
        var upsideAreaLeftTop = this.upsideAreaLeftTop + (Vector2)transform.position;
        // 領域内判定の領域の右下
        var upsideAreaRightBttom = rightTop + (Vector2)transform.position;
        // 領域内の弾をすべてチェック
        var bulletObjects = Physics2D.OverlapAreaAll(upsideAreaLeftTop, upsideAreaRightBttom, bulletLayer);
        for (int i = 0; i < bulletObjects.Length; i++)
        {
            var bulletRigidbody = bulletObjects[i].GetComponent<Rigidbody2D>();
            // ベロシティが下向きなら
            if (bulletRigidbody.velocity.y < rigidBody.velocity.y)
            {
                // リストに追加
                bulletRigidbodies.Add(bulletRigidbody);
            }
        } // for
    }

    /// <summary>
    /// 矩形と円の当たり判定
    /// </summary>
    /// <param name="boxLeftBottom"></param>
    /// <param name="boxRightTop"></param>
    /// <param name="circleCenter"></param>
    /// <param name="circleRadis"></param>
    /// <returns></returns>
    private bool HitCheckBoxAndCircle(Vector2 boxLeftBottom, Vector2 boxRightTop, Vector2 circleCenter, float circleRadis)
    {
        // 円の中心が円の半径分上下左右に伸ばした矩形の内側かチェック
        if((circleCenter.x > boxLeftBottom.x - circleRadis) && (circleCenter.x < boxRightTop.x + circleRadis) && 
            (circleCenter.y > boxLeftBottom.y - circleRadis) && (circleCenter.y < boxRightTop.y + circleRadis))
        {
            // 左端チェック
            if(circleCenter.x < boxLeftBottom.x)
            {
                // 左下端をチェック
                if(circleCenter.y < boxLeftBottom.y)
                {
                    // 距離をチェック
                    if(DistanceSqr(circleCenter, boxLeftBottom) >= circleRadis * circleRadis)
                    {
                        return false;
                    } // if 距離をチェック
                } // if左下端をチェック
                // 左上端をチェック
                else if(circleCenter.y > boxRightTop.y)
                {
                    // 距離チェック
                    if(DistanceSqr(circleCenter, new Vector2(boxLeftBottom.x, boxRightTop.y)) >= circleRadis * circleRadis)
                    {
                        return false;
                    } // if 距離チェック
                } // else if 左上端をチェック
            } // if 左端チェック
            // 右端チェック
            else if(circleCenter.x > boxRightTop.x)
            {
                // 右下端をチェック
                if (circleCenter.y < boxLeftBottom.y)
                {
                    // 距離をチェック
                    if (DistanceSqr(circleCenter, new Vector2(boxRightTop.x, boxLeftBottom.y)) >= circleRadis * circleRadis)
                    {
                        return false;
                    } // if 距離をチェック
                } // if 右下端をチェック
                // 右上端をチェック
                else if (circleCenter.y > boxRightTop.y)
                {
                    // 距離チェック
                    if (DistanceSqr(circleCenter, boxRightTop) >= circleRadis * circleRadis)
                    {
                        return false;
                    } // if 距離チェック
                } // else if 右上端をチェック
            } // if
            return true;
        } // if 円の中心が円の半径分上下左右に伸ばした矩形の内側かチェック
        return false;
    }

    /// <summary>
    /// 距離の2乗を求める
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    private float DistanceSqr(Vector2 point1, Vector2 point2)
    {
        var distanceX = point1.x - point2.x;
        var distanceY = point1.y - point2.y;
        return (distanceX * distanceX) + (distanceY * distanceY);
    }
}