using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitCheck : MonoBehaviour
{
    // 必要なコンポーネント
    EnemyMoveCheck moveCheck;
    Rigidbody2D enemyRigidbody;
    #region 当たり判定を行うレイヤーマスク
    // 弾のレイヤー
    private LayerMask bulletLayer;
    // 手すりのレイヤー
    private LayerMask sliderLayer;
    // 地面のレイヤー
    private LayerMask groundLayer;
    // キャラクターのレイヤー
    private LayerMask characterLayer;
    // リングのレイヤー
    private LayerMask ringLayer;
    #endregion
    #region 当たり判定用の領域
    // 自身の当たり判定の右上
    private Vector2 rightTop;
    // 自身の当たり判定の左下
    private Vector2 leftBottom;
    // 自身の後方の領域の左下
    [SerializeField]
    private Vector2 buckAreaLeftBottom = default;
    // 自身の上側の領域の左上
    // この領域の右下はrightTop
    [SerializeField]
    private Vector2 upsideAreaLeftTop = default;
    // 自身の下側の領域の右下
    // この領域の左上はleftBottom
    [SerializeField]
    private Vector2 undersideAreaRightBottom = default;
    #endregion

    // 弾の配列配列
    private Rigidbody2D[] bulletRigidbodies;
    // キャラクターの配列
    private Character[] otherCharacters = default;

    #region 初期化
    // Start is called before the first frame update
    IEnumerator Start()
    {
        // コンポーネントを取得
        var enemy = GetComponent<Enemy>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        moveCheck = GetComponent<EnemyMoveCheck>();
        // レイヤーをセット
        bulletLayer = LayerMask.GetMask("Bullet");
        sliderLayer = LayerMask.GetMask("Slider");
        groundLayer = LayerMask.GetMask("Ground");
        characterLayer = LayerMask.GetMask("Player");
        ringLayer = LayerMask.GetMask("Ring");
        // コライダーの左下と右上を取得
        var boxCollider2D = GetComponent<BoxCollider2D>();
        GetBoxColliderQuadLeftBottomAndRightTop(boxCollider2D, out leftBottom, out rightTop);
        // 他のキャラクターの配列を初期化
        InitCharactersArray(enemy);
        // 処理順を確定させるために1フレーム待つ
        yield return null;
        // 弾のリジッドボディの配列を初期化
        InitBulletsArray();
    }
    /// <summary>
    /// 弾のリジッドボディの配列を初期化
    /// </summary>
    private void InitBulletsArray()
    {
        // 全ての弾のオブジェクトを取得
        var bulletObjects = GameObject.FindGameObjectsWithTag("Bullet");
        // 配列の要素数をbulletObjectsの要素数に変更
        bulletRigidbodies = new Rigidbody2D[bulletObjects.Length];
        for (int i = 0; i < bulletObjects.Length; i++)
        {
            // Rigidbodyを取得
            var bulletRigidbody = bulletObjects[i].GetComponent<Rigidbody2D>();
            // 配列に格納
            bulletRigidbodies[i] = bulletRigidbody;
        }
    }

    /// <summary>
    /// 自身以外のキャラクターの配列を初期化
    /// </summary>
    private void InitCharactersArray(Enemy enemy)
    {
        // 自身より前のPlayerNoのキャラクターを配列に格納
        for (var no = PLAYER_NO.PLAYER1; no < enemy.playerNO; no++)
        {
            // 配列に格納
            otherCharacters[(int)no] = SceneController.Instance.players[no];
        }
        // 自身を飛ばす
        for (var no = (enemy.playerNO + 1); no < (PLAYER_NO)GameManager.Instance.playerNumber; no++)
        {
            // 配列に格納
            otherCharacters[(int)(no - 1)] = SceneController.Instance.players[no];
        }
    }

    #endregion
    #region Rigidbody2Dのリスト
    #region 特定の範囲をチェック
    /// <summary>
    /// 上方向からの弾とブースト中の剣キャラクターをチェックして、リストに追加
    /// </summary>
    /// <param name="bulletRigidbodyList"></param>
    public void BulletAndCharacterCheck_Upside(ref List<Rigidbody2D> bulletRigidbodyList, ref List<Rigidbody2D> swordCharacterRigidbodyList)
    {
        // 領域内判定の領域の左上
        var upsideAreaLeftTop = this.upsideAreaLeftTop + (Vector2)transform.position;
        // 領域内判定の領域の右下
        var upsideAreaRightBottom = rightTop + (Vector2)transform.position;
        // 領域内の弾をすべてチェック
        var bulletObjects = Physics2D.OverlapAreaAll(upsideAreaLeftTop, upsideAreaRightBottom, bulletLayer);
        for (int i = 0; i < bulletObjects.Length; i++)
        {
            var bulletRigidbody = bulletObjects[i].GetComponent<Rigidbody2D>();
            // ベロシティが下向きなら
            if (bulletRigidbody.velocity.y < enemyRigidbody.velocity.y)
            {
                // リストに追加
                swordCharacterRigidbodyList.Add(bulletRigidbody);
            }
        } // for
        // 領域内のキャラクターをすべてチェック
        var characterObjects = Physics2D.OverlapAreaAll(upsideAreaLeftTop, upsideAreaRightBottom, characterLayer);
        for (int i = 0; i < characterObjects.Length; i++)
        {
            var character = characterObjects[i].GetComponent<Character>();
            // 剣キャラクターで、ブースト中なら
            if (character.charAttackType == GameManager.CHARATTACKTYPE.SWORD && character.IsBoost == true)
            {
                // 剣キャラクターのリジッドボディを取得
                var characterRigidbody = characterObjects[i].GetComponent<Rigidbody2D>();
                // ベロシティが下向きなら
                if (characterRigidbody.velocity.y < enemyRigidbody.velocity.y)
                {
                    // リストに追加
                    bulletRigidbodyList.Add(characterRigidbody);
                }
            } // if(剣キャラクターで、ブースト中なら)
        } // if(領域内のキャラクターをすべてチェック)
    } // 上方向からの弾とブースト中の剣キャラクターをチェックして、リストに追加

    /// <summary>
    /// 左方向からの弾とブースト中の剣キャラクターをチェックして、リストに追加
    /// </summary>
    /// <param name="bulletRigidbodyList"></param>
    public void BulletCheck_Leftside(ref List<Rigidbody2D> bulletRigidbodyList, ref List<Rigidbody2D> swordCharacterRigidbodyList)
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
            bulletRigidbodyList.Add(bulletRigidbody);
        }
        // 領域内のキャラクターをすべてチェック
        var characterObjects = Physics2D.OverlapAreaAll(leftBottom, rightTop, characterLayer);
        for (int i = 0; i < characterObjects.Length; i++)
        {
            var character = characterObjects[i].GetComponent<Character>();
            // 剣キャラクターで、ブースト中なら
            if (character.charAttackType == GameManager.CHARATTACKTYPE.SWORD && character.IsBoost == true)
            {
                // 剣キャラクターのリジッドボディを取得
                var characterRigidbody = characterObjects[i].GetComponent<Rigidbody2D>();
                // ベロシティが下向きなら
                if (characterRigidbody.velocity.y < enemyRigidbody.velocity.y)
                {
                    // リストに追加
                    swordCharacterRigidbodyList.Add(characterRigidbody);
                }
            } // if(剣キャラクターで、ブースト中なら)
        } // if(領域内のキャラクターをすべてチェック)

    } // BulletAndSwordBoostHitCheck

    /// <summary>
    /// 下方向からの弾とブースト中の剣キャラクターをチェックして、リストに追加
    /// </summary>
    /// <param name="bulletRigidbodyList"></param>
    public void BulletCheck_Downside(ref List<Rigidbody2D> bulletRigidbodyList, ref List<Rigidbody2D> swordCharacterRigidbodyList)
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
            if (bulletRigidbody.velocity.y > enemyRigidbody.velocity.y)
            {
                // リストに追加
                swordCharacterRigidbodyList.Add(bulletRigidbody);
            }
        }
        // 領域内のキャラクターをすべてチェック
        var characterObjects = Physics2D.OverlapAreaAll(undersideAreaLeftTop, undersideAreaRightBottom, characterLayer);
        for (int i = 0; i < characterObjects.Length; i++)
        {
            var character = characterObjects[i].GetComponent<Character>();
            // 剣キャラクターで、ブースト中なら
            if (character.charAttackType == GameManager.CHARATTACKTYPE.SWORD && character.IsBoost == true)
            {
                // 剣キャラクターのリジッドボディを取得
                var characterRigidbody = characterObjects[i].GetComponent<Rigidbody2D>();
                // ベロシティが下向きなら
                if (characterRigidbody.velocity.y < enemyRigidbody.velocity.y)
                {
                    // リストに追加
                    bulletRigidbodyList.Add(characterRigidbody);
                }
            } // if(剣キャラクターで、ブースト中なら)
        } // if(領域内のキャラクターをすべてチェック)
    } // BulletCheck_Downside
    #endregion
    #region 全要素で作成
    /// <summary>
    /// 画面内の弾のrigidbodyのリストを取得
    /// </summary>
    /// <returns></returns>
    public List<Rigidbody2D> GetBulletRigidbodyList()
    {
        // 画面内の弾のリジッドボディを追加するリスト
        var bulletRigidbodyList = new List<Rigidbody2D>();
        // リストの初期化
        for (int i = 0; i < bulletRigidbodies.Length; i++)
        {
            // 表示されているなら
            if (bulletRigidbodies[i].gameObject.activeInHierarchy == true)
            {
                // リストに追加
                bulletRigidbodyList.Add(bulletRigidbodies[i]);
            }
        }
        // 出来上がったリスト
        return bulletRigidbodyList;
    } // GetBulletRigidbodyList

    /// <summary>
    /// 当たる可能性のあるブースト中の剣キャラクターのリジッドボディのリストを取得
    /// </summary>
    /// <returns></returns>
    public List<Rigidbody2D> GetSwordBoostRigidbodyList()
    {
        // 当たるかもしれないブースト中の剣キャラクターのリジッドボディを追加するリスト
        var swordCharacterRigidbodyList = new List<Rigidbody2D>();
        // リストの初期化
        for (int i = 0; i < otherCharacters.Length; i++)
        {
            // ブースト中の剣キャラクターなら
            if (otherCharacters[i].charAttackType == GameManager.CHARATTACKTYPE.SWORD && otherCharacters[i].IsBoost == true)
            {
                // コンポーネントを取得
                var swordCharacterRigidbody = otherCharacters[i].GetComponent<Rigidbody2D>();
                // リストに追加
                swordCharacterRigidbodyList.Add(swordCharacterRigidbody);
            }
        }
        // 出来上がったリスト
        return swordCharacterRigidbodyList;
    }
    #endregion
    #endregion
    
    
    /// <summary>
    /// 自分の左上側にプレイヤーがいるかチェックする
    /// </summary>
    /// <returns></returns>
    public bool LeftTopCharacterCheck()
    {
        // 他のキャラクターをすべてチェック
        for (int i = 0; i < otherCharacters.Length; i++)
        {
            // 非表示なら
            if (otherCharacters[i].gameObject.activeInHierarchy == false)
            {
                continue;
            }
            // コライダーの取得
            var characterCollider = otherCharacters[i].GetComponent<BoxCollider2D>();
            // コライダーの情報からワールド座標上のコライダーの左上を取得
            Vector2 characterPositionLeftTop = GetColliderLeftTopPoint(characterCollider);
            // 自身の当たり判定の右下を計算
            var rightBottom = ((Vector2)transform.position + new Vector2(rightTop.x, leftBottom.y));
            // チェックしているキャラクターが自身の左側にいるかチェック
            if (characterPositionLeftTop.x < rightBottom.x)
            {
                // チェックしているキャラクターが自身の上側にいるかチェック
                if (characterPositionLeftTop.y > rightBottom.y)
                {
                    return true;
                } // if (チェックしているキャラクターが自身の上側にいるかチェック)
            }// if (チェックしているキャラクターが自身の左側にいるかチェック)
        } // for
        return false;
    } //  LeftTopCharacterCheck()

    #region 特定の相手と当たり判定
    /// <summary>
    /// 手すりを掴めるか
    /// </summary>
    /// <param name="thisPosition"></param>
    /// <returns></returns>
    public bool SliderHitCheck(Vector2 thisPosition)
    {
        return Physics2D.Raycast(thisPosition, Vector2.up, rightTop.y, sliderLayer);
    }

    /// <summary>
    /// 地面に接地するか
    /// </summary>
    /// <param name="thisPosition"></param>
    /// <returns></returns>
    public bool GroundCheck(Vector2 thisPosition)
    {
        return Physics2D.Raycast(thisPosition, Vector2.down, Mathf.Abs(leftBottom.y), groundLayer);
    }

    /// <summary>
    /// 移動後のポジションを求めて当たり判定を行う
    /// </summary>
    /// <param name="bulletRigidbodyList"></param>
    /// <param name="swordCharacterRigidbodyList"></param>
    /// <param name="bulletColliderRadiuses"></param>
    /// <param name="bulletPositions"></param>
    /// <param name="swordCharacterLeftBottoms"></param>
    /// <param name="swordCharacterRightTops"></param>
    /// <param name="swordCharacterPositions"></param>
    /// <param name="leftBottom"></param>
    /// <param name="rightTop"></param>
    /// <returns></returns>
    public bool HitCheckBulletAndBoostCharacter(List<Rigidbody2D> bulletRigidbodyList, List<Rigidbody2D> swordCharacterRigidbodyList, float[] bulletColliderRadiuses, Vector2[] bulletPositions, Vector2[] swordCharacterLeftBottoms, Vector2[] swordCharacterRightTops, Vector2[] swordCharacterPositions, Vector2 leftBottom, Vector2 rightTop)
    {
        // 全ての弾をチェック
        for (int i = 0; i < bulletRigidbodyList.Count; i++)
        {
            // 移動後のポジションを求める
            bulletPositions[i] += bulletRigidbodyList[i].velocity * Time.fixedDeltaTime;
            // 弾の中心
            var center = bulletPositions[i];
            // 弾と当たるかチェック
            if (HitCheckBoxAndCircle(leftBottom, rightTop, center, bulletColliderRadiuses[i]))
            {
                return true;
            } // if(弾と当たるかチェック)
        } // for(全ての弾をチェック)
        // ブースト中の剣キャラクターをすべてチェック
        for (int i = 0; i < swordCharacterPositions.Length; i++)
        {
            var swordCharacterLeftBottom = swordCharacterLeftBottoms[i] + swordCharacterPositions[i];
            swordCharacterPositions[i] += swordCharacterRigidbodyList[i].velocity * Time.fixedDeltaTime;
            var swordCharacterRightTop = swordCharacterRightTops[i] + swordCharacterPositions[i];
            // 剣キャラクターと当たるかチェック
            if (HitCheckBox(leftBottom, rightTop, swordCharacterLeftBottom, swordCharacterRightTop))
            {
                return true;
            } // if(剣キャラクターと当たるかチェック)
        }// for(ブースト中の剣キャラクターをすべてチェック)
        return false;
    }

    /// <summary>
    /// ジャンプした場合に手すりを掴めるか、リングを通れるかチェック
    /// </summary>
    /// <returns></returns>
    public bool AdvantageObjectCheck()
    {
        #region 自身
        // ジャンプした後の自身のポジション
        Vector2 thisPosition = transform.position;
        // ジャンプ時の移動量を算出
        Vector2 velocity = moveCheck.JumpVelocity();
        #endregion
        // 自身の移動ルートをチェック
        for (float fixedDeltaTime = Time.fixedDeltaTime; true; fixedDeltaTime += Time.fixedDeltaTime)
        {
            // 当たり判定の左下
            Vector2 leftBottom;
            // 当たり判定の右上
            Vector2 rightTop;
            // 当たり判定の領域を計算
            GetMovingQuadArea(ref thisPosition, velocity, out leftBottom, out rightTop);
            // 空中状態での加速処理
            moveCheck.AerialAcceleration(ref velocity);
            #region 終了確認
            // 手すりに掴めるか
            if (Physics2D.Raycast(thisPosition, Vector2.up, this.rightTop.y, sliderLayer))
            {
                return true;
            }
            // リングを通過できるか
            if (Physics2D.OverlapArea(leftBottom, rightTop, ringLayer))
            {
                return true;
            }
            // 落ちているなら
            if (velocity.y < 0)
            {
                // 地面に接地するか
                if (Physics2D.Raycast(thisPosition, Vector2.down, Mathf.Abs(this.leftBottom.y), groundLayer))
                {
                    return false;
                }
            } // if(地面との当たり判定)
            #endregion
        } // for (自身の移動ルートをチェック)
    } // AdvantageObjectCheck
    #endregion
    #region 当たり判定の公式
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
        if ((circleCenter.x > boxLeftBottom.x - circleRadis) && (circleCenter.x < boxRightTop.x + circleRadis) &&
            (circleCenter.y > boxLeftBottom.y - circleRadis) && (circleCenter.y < boxRightTop.y + circleRadis))
        {
            // 左端チェック
            if (circleCenter.x < boxLeftBottom.x)
            {
                // 左下端をチェック
                if (circleCenter.y < boxLeftBottom.y)
                {
                    // 距離をチェック
                    if (DistanceSqr(circleCenter, boxLeftBottom) >= circleRadis * circleRadis)
                    {
                        return false;
                    } // if 距離をチェック
                } // if左下端をチェック
                // 左上端をチェック
                else if (circleCenter.y > boxRightTop.y)
                {
                    // 距離チェック
                    if (DistanceSqr(circleCenter, new Vector2(boxLeftBottom.x, boxRightTop.y)) >= circleRadis * circleRadis)
                    {
                        return false;
                    } // if 距離チェック
                } // else if 左上端をチェック
            } // if 左端チェック
            // 右端チェック
            else if (circleCenter.x > boxRightTop.x)
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
    /// 矩形同士の当たり判定
    /// </summary>
    /// <param name="boxA_LeftBottom"></param>
    /// <param name="boxA_RightTop"></param>
    /// <param name="boxB_LeftBottom"></param>
    /// <param name="boxB_RightTop"></param>
    /// <returns></returns>
    private bool HitCheckBox(Vector2 boxA_LeftBottom, Vector2 boxA_RightTop, Vector2 boxB_LeftBottom, Vector2 boxB_RightTop)
    {
        #region エラーチェック
        // 左右逆なら
        if (boxA_LeftBottom.x > boxA_RightTop.x)
        {
            // スワップ
            var workX = boxA_LeftBottom.x;
            boxA_LeftBottom.x = boxA_RightTop.x;
            boxA_RightTop.x = workX;
        }
        if (boxB_LeftBottom.x > boxB_RightTop.x)
        {
            // スワップ
            var workX = boxB_LeftBottom.x;
            boxB_LeftBottom.x = boxB_RightTop.x;
            boxB_RightTop.x = workX;
        }
        // 上下逆なら
        if (boxA_LeftBottom.y > boxA_RightTop.y)
        {
            // スワップ
            var workY = boxA_LeftBottom.y;
            boxA_LeftBottom.y = boxA_RightTop.y;
            boxA_RightTop.y = workY;
        }
        if (boxB_LeftBottom.y > boxB_RightTop.y)
        {
            // スワップ
            var workY = boxB_LeftBottom.y;
            boxB_LeftBottom.y = boxB_RightTop.y;
            boxB_RightTop.y = workY;
        }
        #endregion

        return (boxA_RightTop.x >= boxB_LeftBottom.x && boxB_RightTop.x >= boxA_LeftBottom.x &&
            boxA_RightTop.y >= boxB_LeftBottom.y && boxB_RightTop.y >= boxA_LeftBottom.y);
    }
    #endregion
    #region 矩形の点
    /// <summary>
    /// 移動後のポジションでの当たり判定の領域を計算
    /// </summary>
    /// <param name="thisPosition"></param>
    /// <param name="velocity"></param>
    /// <param name="leftBottom"></param>
    /// <param name="rightTop"></param>
    public void GetMovingQuadArea(ref Vector2 thisPosition, Vector2 velocity, out Vector2 leftBottom, out Vector2 rightTop)
    {
        // 落ちているのなら
        if (velocity.y < 0)
        {
            // 前のフレームでの当たり判定の左上
            var leftTop = new Vector2(this.leftBottom.x, this.rightTop.y) + thisPosition;
            // 1フレーム後のポジションに移動
            thisPosition += (velocity * Time.fixedDeltaTime);
            // 新しいポジションでの当たり判定の右下
            var rightBottom = new Vector2(this.rightTop.x, this.leftBottom.y) + thisPosition;
            // セットする
            leftBottom = new Vector2(leftTop.x, rightBottom.y);
            rightTop = new Vector2(rightBottom.x, leftTop.y);
        } // if (落ちているのなら)
          // 落ちていないのなら
        else
        {
            // 前のフレームでの当たり判定の左下
            leftBottom = this.leftBottom + thisPosition;
            // 1フレーム後のポジションに移動
            thisPosition += (velocity * Time.fixedDeltaTime);
            // 新しいポジションでの当たり判定の右上
            rightTop = this.rightTop + thisPosition;
        } // else (落ちていないのなら)
    }

    /// <summary>
    /// 矩形の中心とサイズから左下と右上を取得
    /// </summary>
    /// <param name="center"></param>
    /// <param name="size"></param>
    /// <param name="leftBottom"></param>
    /// <param name="rightTop"></param>
    private void GetQuadLeftBottomAndRightTop(Vector2 center, Vector2 size, out Vector2 leftBottom, out Vector2 rightTop)
    {
        // 中心
        leftBottom = center;
        rightTop = center;
        // 左下にずらす
        leftBottom += -(size * 0.5f);
        // 右上にずらす
        rightTop += (size * 0.5f);
    }
    /// <summary>
    /// ボックスコライダーの情報を基に矩形の左下と右上のワールド座標を取得
    /// </summary>
    /// <param name="boxCollider2D"></param>
    /// <param name="leftBottom"></param>
    /// <param name="rightTop"></param>
    private void GetBoxColliderWorldPointLeftBottomAndRightTop(BoxCollider2D boxCollider2D, out Vector2 leftBottom, out Vector2 rightTop)
    {
        // コライダーの中心のワールド座標を求める
        var center = (Vector2)boxCollider2D.transform.position + boxCollider2D.offset;
        // 左下と右上のワールド座標を取得
        GetQuadLeftBottomAndRightTop(center, boxCollider2D.size, out leftBottom, out rightTop);
    }
    /// <summary>
    /// ボックスコライダーの情報を基に矩形の左下と右上を取得
    /// </summary>
    /// <param name="boxCollider2D"></param>
    /// <param name="leftBottom"></param>
    /// <param name="rightTop"></param>
    private void GetBoxColliderQuadLeftBottomAndRightTop(BoxCollider2D boxCollider2D, out Vector2 leftBottom, out Vector2 rightTop)
    {
        // コライダーのオフセットを求める
        var center = boxCollider2D.offset;
        // 左下と右上のワールド座標を取得
        GetQuadLeftBottomAndRightTop(center, boxCollider2D.size, out leftBottom, out rightTop);
    } // GetBoxColliderQuadLeftBottomAndRightTop

    /// <summary>
    /// ボックスコライダーの当たり判定の右上のワールド座標を取得
    /// </summary>
    /// <param name="boxCollider2D"></param>
    /// <returns></returns>
    private static Vector2 GetColliderLeftTopPoint(BoxCollider2D boxCollider2D)
    {
        // コライダーのオフセットを勘定
        var colliderPositionLeftTop = boxCollider2D.offset;
        // コライダーのポジションを勘定
        colliderPositionLeftTop += (Vector2)boxCollider2D.transform.position;
        // コライダーのサイズを勘定
        colliderPositionLeftTop.x += -(boxCollider2D.size.x * 0.5f);
        colliderPositionLeftTop.y += (boxCollider2D.size.y * 0.5f);
        // 戻り値
        return colliderPositionLeftTop;
    } // GetColliderLeftTopPoint
    #endregion
    #region コライダーからパラメータを取得
    /// <summary>
    /// コライダーの情報からコライダーの中心のワールド座標を取得
    /// </summary>
    /// <param name="boxCollider2D"></param>
    /// <returns></returns>
    private Vector2 GetColliderCenterPosition(BoxCollider2D boxCollider2D)
    {
        return (Vector2)boxCollider2D.transform.position + boxCollider2D.offset;
    }

    /// <summary>
    /// 剣キャラクターのコライダーの情報を取得
    /// </summary>
    /// <param name="swordCharacterRigidbodyList"></param>
    /// <param name="swordCharacterLeftBottoms"></param>
    /// <param name="swordCharacterRightTops"></param>
    /// <param name="swordCharacterPositions"></param>
    public void GetColliderParameter(List<Rigidbody2D> swordCharacterRigidbodyList, out Vector2[] swordCharacterLeftBottoms, out Vector2[] swordCharacterRightTops, out Vector2[] swordCharacterPositions)
    {
        // 剣キャラクターのコライダーの左下
        swordCharacterLeftBottoms = new Vector2[swordCharacterRigidbodyList.Count];
        // 剣キャラクターのコライダーの右上
        swordCharacterRightTops = new Vector2[swordCharacterRigidbodyList.Count];
        // 剣キャラクターのポジション
        swordCharacterPositions = new Vector2[swordCharacterRigidbodyList.Count];
        // 配列の初期化
        for (int i = 0; i < swordCharacterRigidbodyList.Count; i++)
        {
            // 剣キャラクターのコライダーを取得
            var swordCharacterCollider = swordCharacterRigidbodyList[i].GetComponent<BoxCollider2D>();
            // 剣キャラクターの当たり判定
            GetBoxColliderQuadLeftBottomAndRightTop
                (swordCharacterCollider, out swordCharacterLeftBottoms[i], out swordCharacterRightTops[i]);
            // 剣キャラクターのポジションをセット
            swordCharacterPositions[i] = GetColliderCenterPosition(swordCharacterCollider);
        }
    }

    /// <summary>
    /// 弾のコライダーの情報を取得
    /// </summary>
    /// <param name="bulletRigidbodyList"></param>
    /// <param name="bulletColliderRadiuses"></param>
    /// <param name="bulletPositions"></param>
    public void GetColliderParameter(List<Rigidbody2D> bulletRigidbodyList, out float[] bulletColliderRadiuses, out Vector2[] bulletPositions)
    {
        // 弾のコライダーの半径
        bulletColliderRadiuses = new float[bulletRigidbodyList.Count];
        // 弾のポジション
        bulletPositions = new Vector2[bulletRigidbodyList.Count];
        // 配列の初期化
        for (int i = 0; i < bulletRigidbodyList.Count; i++)
        {
            // 弾のコライダーを取得
            var bulletCollider = bulletRigidbodyList[i].GetComponent<CircleCollider2D>();
            // コライダーの半径をセット
            bulletColliderRadiuses[i] = bulletCollider.radius;
            // 弾のポジションをセット
            bulletPositions[i] = (Vector2)bulletRigidbodyList[i].transform.position + bulletCollider.offset;
        }
    }
    #endregion
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