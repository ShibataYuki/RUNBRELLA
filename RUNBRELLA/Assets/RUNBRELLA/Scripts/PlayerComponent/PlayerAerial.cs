using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 空中状態のときにスピードをチェックして止まらないようにする
/// </summary>
public class PlayerAerial : MonoBehaviour
{    
    // リジッドボディのコンポーネント
    private Rigidbody2D playerRigidbody;
    Character character;
    // 移動クラス
    PlayerMove move;
    // 上昇気流のレイヤー
    private LayerMask updraftLayer = 0;
    // 上昇気流内にいるかチェックする領域
    private Vector2 leftBottom = Vector2.zero;
    private Vector2 rightTop = Vector2.zero;

    public float aerialGravityScale = 0;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();
        // コンポーネントの取得
        playerRigidbody = GetComponent<Rigidbody2D>();
        move = GetComponent<PlayerMove>();
        // 保留　演出は最初、切っておく
        // EffectOff();
        // 上昇気流レイヤーの取得
        updraftLayer = LayerMask.GetMask("Updraft");
        // 当たり判定の領域の計算
        var collider = GetComponent<BoxCollider2D>();
        leftBottom = collider.offset;
        rightTop = collider.offset;
        leftBottom += -(collider.size * 0.5f);
        rightTop   += (collider.size * 0.5f);        
        ReadTextParameter();
       
    }

    /// <summary>
    /// テキストからパラメータを取得
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var textName = "";
        switch (character.charType)
        {
            case GameManager.CHARTYPE.PlayerA:
                textName = "Chara_A";
                break;
            case GameManager.CHARTYPE.PlayerB:
                textName = "Chara_B";
                break;
        }
        // テキストの中のデータをセットするディクショナリー        
        Dictionary<string, float> textDataDic;
        textDataDic = SheetToDictionary.TextNameToData[textName];
        aerialGravityScale = textDataDic["空中状態の場合における重力加速度の倍率"];

    }

    /// <summary>
    /// 開始時処理
    /// </summary>
    public void StartAerial()
    {
        // 重力加速度を変更
        character.Rigidbody.gravityScale = aerialGravityScale;
        // 角度を初期化
        transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// 終了時処理
    /// </summary>
    public void EndAerial()
    {
        character.Rigidbody.gravityScale = 1;
    }

    /// <summary>
    /// 空中状態のメイン処理   
    /// </summary>
    public void Aerial()
    {
        // 加速度の蓄積
        move.AddAcceleration();
        // 速度の増加
        move.SpeedChange();       
    }
   
}
