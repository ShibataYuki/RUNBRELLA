using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpCheck : MonoBehaviour
{
    // 必要なコンポーネント
    Enemy enemy;
    EnemyHitCheck hitCheck;
    EnemyMoveCheck moveCheck;

    // 何秒後の弾の弾道と当たり判定を行うか
    [SerializeField]
    private float checkTime = 0.5f;

    /// <summary>
    /// フレーム更新前の初期化処理
    /// </summary>
    void Start()
    {
        // コンポーネントを取得
        enemy = GetComponent<Enemy>();
        hitCheck = GetComponent<EnemyHitCheck>();
        moveCheck = GetComponent<EnemyMoveCheck>();
    }

    /// <summary>
    /// ジャンプするシチュエーションかチェック
    /// </summary>
    /// <returns></returns>
    public bool JumpCheck()
    {
        // 射線上にいるか
        if (BulletLineAndSwordBoostCheck(out float limitTimeGround))
        {
            // ジャンプした場合に射線上にいるか
            if (BulletLineAndSwordBoostCheckWhenJump(out float limitTimeJump))
            {
                // いつ頃当たるかに応じてジャンプする/しないを決断する
                return (limitTimeGround > limitTimeJump);
            }
            // ジャンプすると射線から逃れれるなら
            else { return true; }
        } // if(射線上にいるか)
        // 射線上にいない場合
        else
        {
            // ジャンプした先にリングか手すりが無いかチェックする
            if (hitCheck.AdvantageObjectCheck())
            {
                // 自分の左上側にプレイヤーがいるかチェックする
                if (hitCheck.LeftTopCharacterCheck())
                { return false; }
                else { return true; }
            } // if (ジャンプした先にリングか手すりが無いかチェックする)
        } // else (射線上にいない場合)
        return false;
    } // JumpCheck()

    /// <summary>
    /// 弾の射線上にいるかチェック
    /// </summary>
    /// <returns></returns>
    private bool BulletLineAndSwordBoostCheck(out float limitTime)
    {
        // 当たるかもしれない弾とブースト中の剣キャラクターのRigidbodyのリスト
        var bulletRigidbodyList = new List<Rigidbody2D>();
        var swordCharacterRigidbodyList = new List<Rigidbody2D>();
        #region 当たるかもしれないRigidbodyをリストに追加
        // 上方向からの弾をチェックして、リストに追加
        hitCheck.BulletAndCharacterCheck_Upside(ref bulletRigidbodyList, ref swordCharacterRigidbodyList);
        // 左方向からの弾をチェックして、リストに追加
        hitCheck.BulletCheck_Leftside(ref bulletRigidbodyList, ref swordCharacterRigidbodyList);
        // 下方向からの弾をチェックして、リストに追加
        hitCheck.BulletCheck_Downside(ref bulletRigidbodyList, ref swordCharacterRigidbodyList);
        #endregion
        // 弾とキャラクター、キャラクターとブースト中の剣キャラクターが当たるかチェック
        return BulletAndSwordBoostHitCheck(bulletRigidbodyList, swordCharacterRigidbodyList, out limitTime);
    }

    /// <summary>
    /// 弾とキャラクター、キャラクターとブースト中の剣キャラクターが当たるかチェック
    /// </summary>
    /// <param name="bulletRigidbodyList"></param>
    /// <returns></returns>
    private bool BulletAndSwordBoostHitCheck
        (List<Rigidbody2D> bulletRigidbodyList, List<Rigidbody2D> swordCharacterRigidbodyList, out float limitTime)
    {
        // 弾のリストか剣キャラクターのリストに要素があるなら
        if (bulletRigidbodyList.Count > 0 || swordCharacterRigidbodyList.Count > 0)
        {
            #region 自身
            // ジャンプしなかった場合の自身のポジション
            Vector2 thisPosition = transform.position;
            // 自身の移動量
            var velocity = enemy.Rigidbody.velocity;
            #endregion
            // コライダーから弾のパラメータを取得
            hitCheck.GetColliderParameter(bulletRigidbodyList, out var bulletColliderRadiuses, out var bulletPositions);
            // コライダーから剣キャラクターのパラメータを取得
            hitCheck.GetColliderParameter(swordCharacterRigidbodyList, out var swordCharacterLeftBottoms, out var swordCharacterRightTops, out var swordCharacterPositions);
            // 弾の弾道と自身の移動ルートをチェック
            for (float fixedDeltaTime = Time.fixedDeltaTime; fixedDeltaTime <= checkTime; fixedDeltaTime += Time.fixedDeltaTime)
            {
                // 当たり判定の左下
                Vector2 leftBottom;
                // 当たり判定の右上
                Vector2 rightTop;
                // 当たり判定の領域を計算
                hitCheck.GetMovingQuadArea(ref thisPosition, velocity, out leftBottom, out rightTop);
                // 加速処理
                if (enemy.IsSlide == false)
                {
                    // 地上での加速処理を再現
                    velocity = moveCheck.Acceleration(velocity);
                }
                // 当たるかチェック
                if (hitCheck.HitCheckBulletAndBoostCharacter(bulletRigidbodyList, swordCharacterRigidbodyList, bulletColliderRadiuses, bulletPositions, swordCharacterLeftBottoms, swordCharacterRightTops, swordCharacterPositions, leftBottom, rightTop))
                {
                    // いつ頃当たるか
                    limitTime = fixedDeltaTime;
                    return true;
                }
            } // for(弾の弾道と自身の移動ルートをチェック)
        } // if(bulletRigidbodies.Count)
        limitTime = 0.0f;
        return false;
    }

    #region ジャンプ後の位置
    /// <summary>
    /// ジャンプした場合に弾もしくはブースト中の剣キャラクターと当たるか
    /// </summary>
    /// <returns></returns>
    private bool BulletLineAndSwordBoostCheckWhenJump(out float limitTime)
    {
        // 初期化
        limitTime = 0.0f;
        // 当たるかもしれない弾のリジッドボディのリスト
        var bulletRigidbodyList = hitCheck.GetBulletRigidbodyList();
        // 当たるかもしれないブースト中の剣キャラクターのリジッドボディのリスト
        var swordCharacterRigidbodyList = hitCheck.GetSwordBoostRigidbodyList();
        // 弾のリストかブースト中の剣キャラクターのリストに要素があるなら
        if (bulletRigidbodyList.Count > 0 || swordCharacterRigidbodyList.Count > 0)
        {
            #region 自身
            // ジャンプした後の自身のポジション
            Vector2 thisPosition = transform.position;
            // 自身の移動量
            var velocity = moveCheck.JumpVelocity();
            #endregion
            // コライダーから弾のパラメータを取得
            hitCheck.GetColliderParameter(bulletRigidbodyList, out var bulletColliderRadiuses, out var bulletPositions);
            // コライダーから剣キャラクターのパラメータを取得
            hitCheck.GetColliderParameter(swordCharacterRigidbodyList, out var swordCharacterLeftBottoms, out var swordCharacterRightTops, out var swordCharacterPositions);
            // 弾の弾道と自身の移動ルートをチェック
            return AerialDamageCheck(ref limitTime, bulletRigidbodyList, swordCharacterRigidbodyList, ref thisPosition, velocity, bulletColliderRadiuses, bulletPositions, swordCharacterLeftBottoms, swordCharacterRightTops, swordCharacterPositions);
        } // if(弾のリストかブースト中の剣キャラクターのリストに要素があるなら)
        return false;
    } // BulletLineAndSwordBoostCheckWhenJump

    /// <summary>
    /// 地面につくか手すりに摑まるまでに攻撃を受けるか
    /// </summary>
    /// <param name="limitTime"></param>
    /// <param name="bulletRigidbodyList"></param>
    /// <param name="swordCharacterRigidbodyList"></param>
    /// <param name="thisPosition"></param>
    /// <param name="velocity"></param>
    /// <param name="bulletColliderRadiuses"></param>
    /// <param name="bulletPositions"></param>
    /// <param name="swordCharacterLeftBottoms"></param>
    /// <param name="swordCharacterRightTops"></param>
    /// <param name="swordCharacterPositions"></param>
    /// <returns></returns>
    private bool AerialDamageCheck(ref float limitTime, List<Rigidbody2D> bulletRigidbodyList, List<Rigidbody2D> swordCharacterRigidbodyList, ref Vector2 thisPosition, Vector2 velocity, float[] bulletColliderRadiuses, Vector2[] bulletPositions, Vector2[] swordCharacterLeftBottoms, Vector2[] swordCharacterRightTops, Vector2[] swordCharacterPositions)
    {
        for (float fixedDeltaTime = Time.fixedDeltaTime; true; fixedDeltaTime += Time.fixedDeltaTime)
        {
            // 当たり判定の左下
            Vector2 leftBottom;
            // 当たり判定の右上
            Vector2 rightTop;
            // 当たり判定の領域を計算
            hitCheck.GetMovingQuadArea(ref thisPosition, velocity, out leftBottom, out rightTop);
            // 加速度の変更
            moveCheck.AerialAcceleration(ref velocity);
            // 当たるかチェック
            if (hitCheck.HitCheckBulletAndBoostCharacter(bulletRigidbodyList, swordCharacterRigidbodyList, bulletColliderRadiuses, bulletPositions, swordCharacterLeftBottoms, swordCharacterRightTops, swordCharacterPositions, leftBottom, rightTop))
            {
                // いつ頃当たるか
                limitTime = fixedDeltaTime;
                return true;
            }
            #region 終了確認
            // 手すりに掴めるか
            if (hitCheck.SliderHitCheck(thisPosition))
            {
                return false;
            }
            // 落ちているなら
            if (velocity.y < 0)
            {
                // 地面に接地するか
                if (hitCheck.GroundCheck(thisPosition))
                {
                    return false;
                }
            } // if(地面との当たり判定)
            #endregion
        } // for(弾の弾道と自身の移動ルートをチェック)
    }
    #endregion
}