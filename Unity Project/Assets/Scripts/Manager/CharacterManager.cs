using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの情報
/// </summary>
struct CharacterData
{
    // キャラクター
    public Character character;
    // リジッドボディ
    public Rigidbody2D rigidbody;
    // 当たり判定の左下
    public Vector2 leftBottom;
    // 当たり判定の右上
    public Vector2 rightTop;
    // 現在射線上にいるか
    public bool isShotLine;
    // 走り状態の場合に射線上にいるか
    // public bool isShotLineWhenRun;
    // 空中状態の場合に射線上にいるか
    // public bool isShotLineWhenAerial;
    // 滑空状態の場合に射線上にいるか
    // public bool isShotLineWhenGlide;
    // ジャンプした場合に手すりを掴めるか
    // public bool isSliderLineWhenJump;
    // ジャンプした場合にリングをいくつ通るか
    // public int ringCountWhenJump;

    public CharacterData(Character character,Rigidbody2D rigidbody,
        Vector2 leftBottom,Vector2 rightTop,bool isShotLine)
    {
        this.character = character;
        this.rigidbody = rigidbody;
        this.leftBottom = leftBottom;
        this.rightTop = rightTop;
        this.isShotLine = isShotLine;
    }
}

public class CharacterManager : MonoBehaviour
{
    // キャラクターの情報のディクショナリ－
    Dictionary<PLAYER_NO, CharacterData> characterDataDictionary = new Dictionary<PLAYER_NO, CharacterData>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(var characterDictionaryData in SceneController.Instance.players)
        {
            var characterData = new CharacterData();
            var character = characterDictionaryData.Value;
            characterData.character = character;
            var rigidbody = character.GetComponent<Rigidbody2D>();
            characterData.rigidbody = rigidbody;
            var boxCollider = character.GetComponent<BoxCollider2D>();
            GetBoxColliderQuadLeftBottomAndRightTop(boxCollider, out characterData.leftBottom, out characterData.rightTop);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region public
    /// <summary>
    /// 現在射線上にいるか
    /// </summary>
    /// <param name="playerNo"></param>
    /// <returns></returns>
    public bool IsShotLine(PLAYER_NO playerNo)
    {
        return characterDataDictionary[playerNo].isShotLine;
    }

    /// <summary>
    /// 攻撃の射線上に他プレイヤーがいるか
    /// </summary>
    /// <param name="playerNo"></param>
    /// <returns></returns>
    public bool IsShotLineHit(PLAYER_NO playerNo)
    {
        return characterDataDictionary[playerNo].isShotLine;
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
    public void GetMovingQuadArea(ref Vector2 thisPosition, Vector2 velocity, PLAYER_NO playerNo, out Vector2 leftBottom, out Vector2 rightTop)
    {
        // ディクショナリ－のplayerNo番目の当たり判定の四隅
        leftBottom = characterDataDictionary[playerNo].leftBottom;
        rightTop = characterDataDictionary[playerNo].rightTop;

        // 落ちているのなら
        if (velocity.y < 0)
        {
            // 前のフレームでの当たり判定の左上
            var leftTop = new Vector2(leftBottom.x, rightTop.y) + thisPosition;
            // 1フレーム後のポジションに移動
            thisPosition += (velocity * Time.fixedDeltaTime);
            // 新しいポジションでの当たり判定の右下
            var rightBottom = new Vector2(rightTop.x, leftBottom.y) + thisPosition;
            // セットする
            leftBottom = new Vector2(leftTop.x, rightBottom.y);
            rightTop = new Vector2(rightBottom.x, leftTop.y);
        } // if (落ちているのなら)
        // 落ちていないのなら
        else
        {
            // 前のフレームでの当たり判定の左下
            leftBottom = leftBottom + thisPosition;
            // 1フレーム後のポジションに移動
            thisPosition += (velocity * Time.fixedDeltaTime);
            // 新しいポジションでの当たり判定の右上
            rightTop = rightTop + thisPosition;
        } // else (落ちていないのなら)
    } // GetMovingQuadArea

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
    #endregion
}