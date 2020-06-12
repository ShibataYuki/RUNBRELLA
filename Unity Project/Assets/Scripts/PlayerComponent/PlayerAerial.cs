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
        // ファイル名
        string fileName = nameof(PlayerAerial) + "Data" + character.Type;
        // テキストの読み込み
        // decaySpeed = TextManager.Instance.GetValue_float(fileName, nameof(decaySpeed));
        aerialGravityScale = TextManager.Instance.GetValue_float(fileName, nameof(aerialGravityScale));
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
        SheetToDictionary.Instance.TextToDictionary(textName, out var textDataDic);
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
    /// 空中状態へのメイン処理   
    /// </summary>
    public void Aerial()
    {
        // 加速度の蓄積
        move.AddAcceleration();
        // 速度の増加
        move.SpeedChange();
        /*保留
        // x方向への速度変化
        ChangeVelocityXToBase();
         */
    }

    /*保留     
    /// <summary>
    /// 速度調整
    /// </summary>
    void ChangeVelocityXToBase()
    {
        // 速度が最高速度以下なら加速
        if (rigidbody.velocity.x < player.MaxSpeed)
        {
            SpeedUpToMaxSpeed();
        }
        // 最高速度以上なら減速
        else if (rigidbody.velocity.x > player.MaxSpeed)
        {
            SetMaxVelocity();
        }
        // ちょうどなら何もしない
    }

    /// <summary>
    /// 加速処理
    /// </summary>
    void SpeedUpToMaxSpeed()
    {
        // 加速処理
        rigidbody.AddForce(new Vector2(player.BaseAddSpeed, 0), ForceMode2D.Force);
        var velocity = rigidbody.velocity;
        // 速度が基本速度を下回っていたら基本速度に戻す
        if (velocity.x < player.BaseSpeed)
        {
            velocity.x = player.BaseSpeed;
        }
        // 速度が最高速度を上回っていたら最高速度に戻す
        if (velocity.x > player.MaxSpeed)
        {
            velocity.x = player.MaxSpeed;
        }

        rigidbody.velocity = velocity;
    }

    /// <summary>
    /// 減速処理
    /// </summary>
    void SetMaxVelocity()
    {
        // Velocityを最高速度に戻す
        Vector2 afterVelocity = rigidbody.velocity;
        rigidbody.velocity = new Vector2(player.MaxSpeed, afterVelocity.y);
    }
    */

    /// <summary>
    /// 保留　上昇気流に乗れることを演出で示す
    /// </summary>
    //public void EffectOn()
    //{
    //    var color = updraftEffect.color;
    //    color.a = 1.0f;
    //    updraftEffect.color = color;
    //}
          
    ///// <summary>
    ///// 保留　上昇気流に乗れないことを演出で示す
    ///// </summary>
    //public void EffectOff()
    //{
    //    var color = updraftEffect.color;
    //    color.a = 0.0f;
    //    updraftEffect.color = color;
    //}
    // 保留
    //public void UpdraftCheck()
    //{
    //    // 当たり判定の領域の現在位置を計算
    //    var workLeftBottom = leftBottom + (Vector2)transform.position;
    //    var workRightTop   = rightTop   + (Vector2)transform.position;
    //    // 上昇気流内にいるかチェック
    //    bool isHit = Physics2D.OverlapArea(workLeftBottom, workRightTop, updraftLayer);
    //    if(isHit == true)
    //    {
    //        // エフェクトをONにする
    //        EffectOn();
    //    }
    //    else
    //    {
    //        // エフェクトをOFFにする
    //        EffectOff();
    //    }
    //}
}
