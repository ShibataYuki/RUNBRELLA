using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // プレイヤークラス
    Character character = null;
    // リジッドボディクラス
    Rigidbody2D rigidBody = null; 
    // 現在の加速度
    float nowAcceleration = 0;
    // 一秒間に増加する速度
    [SerializeField]
    float addVelocityXPersecond = 1.5f;
    // 最大速度
    [SerializeField]
    public float MaxVelocityX { get; private set; } = 0;
    // 加速度の最大蓄積値
    [SerializeField]
    float maxAcceleration = 0;
    // 最低速度
    [SerializeField]
    public float MinVelocityX { get; private set; } = 0;

    private void Awake()
    {
        character = GetComponent<Character>();
    }
    void Start()
    {        
        // 参照取得
        rigidBody = GetComponent<Rigidbody2D>();
        // テキストからデータ読み込み
        ReadTextParameter();
        maxAcceleration = MaxVelocityX - MinVelocityX;
    }

    #region private関数
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
        addVelocityXPersecond = textDataDic["1秒間に増加する速度"];        
        MaxVelocityX = textDataDic["最高速度の秒速"];
        MinVelocityX = textDataDic["最低速度の秒速"];
    }
    #endregion
    #region public関数

    /// <summary>
    /// 加速度の蓄積を0に戻す
    /// </summary>
    public void ResetAcceleration()
    {
        nowAcceleration = 0;
    }

    /// <summary>
    /// 加速度の蓄積
    /// </summary>
    public void AddAcceleration()
    {
        // 1秒に蓄積する加速度を1フレーム分に換算
        var addPerFrame = addVelocityXPersecond * Time.fixedDeltaTime;
        // 加速度の蓄積
        nowAcceleration += addPerFrame;
        // 0 ～　加速度の最大値　に範囲制限
        nowAcceleration = Mathf.Clamp(nowAcceleration, 0, maxAcceleration);
    }

    public void MinusAcceleration()
    {
        // 1秒に蓄積する加速度を1フレーム分に換算
        var addPerFrame = addVelocityXPersecond * Time.fixedDeltaTime;
        // 加速度の蓄積
        nowAcceleration -= addPerFrame;
        // 0 ～　加速度の最大値　に範囲制限
        nowAcceleration = Mathf.Clamp(nowAcceleration, 0, maxAcceleration);
    }

    /// <summary>
    /// x方向の速度を増加させる
    /// </summary>
    public void SpeedChange()
    {                       
        // 新しいX方向への速度(基本の速度＋現在蓄積されている加速度)
        float nextVelocityX = MinVelocityX +  nowAcceleration;        
        // X方向速度を　最低値　～　最高値　に範囲制限
        nextVelocityX = Mathf.Clamp(nextVelocityX, MinVelocityX, MaxVelocityX);
        // 速度セット
        var nextVelocity = new Vector2(nextVelocityX, rigidBody.velocity.y);
        rigidBody.velocity = nextVelocity;        
    }
    #endregion

}
