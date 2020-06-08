using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlide : MonoBehaviour
{
    // 自身のリジットボディ
    Rigidbody2D rigidbody2d;
    Character character;
    PlayerMove move;
    // 保留
    //PlayerRun playerRun;
    // 速度減衰値
    //float decaySpeed;    
    // 加える力    
    //private float grideAddSpeed;
    //[SerializeField]
    //private float maxSpeed = 0;

    // 走っている状態の速度を基準としてその何パーセントの速さにするか
    [SerializeField]
    float eagingSpeedPercent = 0;
    // 毎フレーム前フレームの落下速度の何パーセントの速さにするか
    [SerializeField]
    float easingVelocityYPercent = 0;
    // 滑空状態での重力加速度
    [SerializeField]
    private float glideGravityScale = 0;
    // ホップを1度だけに制御するフラグ
    public bool CanHop { get;  set; } = true;
    // ホップの力
    private float HopPower = 0;
    // 前方に地面があるかチェックするコンポーネント
    private PlayerAerial playerAerial = null;
    // 傘を開いた時のSE
    [SerializeField]
    private AudioClip openSE = null;
    // SEのボリューム
    private float SEVolume = 0.5f;


    private void Start()
    {                
        // 変数の初期化
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
        playerAerial = GetComponent<PlayerAerial>();
        move = GetComponent<PlayerMove>();
        // テキストの読み込み       
        // データ代入
        ReadTextParameter();
        // 百分率を倍率に変換
        easingVelocityYPercent /= 100;
        eagingSpeedPercent /= 100;
        // 保留
        //playerRun = GetComponent<PlayerRun>();      
        //grideAddSpeed = player.BaseAddSpeed * eagingSpeedPercent;
        //decaySpeed = player.BaseAddSpeed * (1 - eagingSpeedPercent);
        // 読み込むファイルのファイル名
        string fileName = nameof(PlayerGlide) + "Data" + character.Type;
       
        // decaySpeed = TextManager.Instance.GetValue_float(fileName, nameof(decaySpeed));        
        SEVolume = TextManager.Instance.GetValue_float(fileName, nameof(SEVolume));
    }

    /// <summary>
    /// テキストからパラメータを取得
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var textName = "";
        var charaCommonTextName = "Chara_Common";
        switch (character.charType)
        {
            case GameManager.CHARTYPE.PlayerA:
                textName = "Chara_A";
                break;
            case GameManager.CHARTYPE.PlayerB:
                textName = "Chara_B";
                break;
        }
        Dictionary<string, float> charaCommonDictionary;
        // テキストの中のデータをセットするディクショナリー        
        SheetToDictionary.Instance.TextToDictionary(textName, out var textDataDic);
        SheetToDictionary.Instance.TextToDictionary(charaCommonTextName, out charaCommonDictionary);
        // 保留
        //maxSpeed  = textDataDic["滑空中の最高速度"];
        eagingSpeedPercent = textDataDic["滑空中の速度が走り状態の何パーセントの速度になるか(%)"];
        easingVelocityYPercent = textDataDic["滑空中の落下速度が通常の何パーセントになるか(%)"];
        glideGravityScale = charaCommonDictionary["滑空状態の場合における重力加速度の倍率"];
        HopPower = charaCommonDictionary["傘を開いた際のホップの強さ"];
    }

    /// <summary>
    /// 滑空の開始処理
    /// </summary>
    public void StartGlide()
    {
        // SEの再生
        AudioManager.Instance.PlaySE(openSE, SEVolume);
        // 重力を滑空用の重力に変更
        rigidbody2d.gravityScale = glideGravityScale;
        // 角度を初期化
        transform.localRotation = Quaternion.identity;
        // まだホップしていないならホップ
        if(CanHop)
        {
            Hop();
            CanHop = false;
        }
    }

    /// <summary>
    /// 滑空中の処理
    /// </summary>
    public void Gride()
    {
        // 加速度の蓄積
        move.AddAcceleration();
        // 速度の増加
        move.SpeedChange();
        // x方向への速度緩和
        EasingVelocityX();
        // 保留
        //ChangeVelocityX();
        // y方向への速度緩和
        EasingVelocityY();
    }

    /// <summary>
    /// 落下を和らげる処理
    /// </summary>
    void EasingVelocityY()
    {
        var velocity = rigidbody2d.velocity;
        // 落下速度軽減処理
        Vector2 easingVelocity = new Vector2(velocity.x, velocity.y * easingVelocityYPercent);
        rigidbody2d.velocity = easingVelocity;
    }

    /// <summary>
    /// X方向への速度緩和
    /// </summary>
    void EasingVelocityX()
    {
        var velocity = rigidbody2d.velocity;
        // 速度軽減処理
        Vector2 easingVelocity = new Vector2(velocity.x * eagingSpeedPercent, velocity.y);
        rigidbody2d.velocity = easingVelocity;
    }

    /// <summary>
    /// 上にホップする処理
    /// </summary>
    private void Hop()
    {
        rigidbody2d.AddForce(new Vector2(0, HopPower), ForceMode2D.Impulse);
    }

    // 保留
    /// <summary>
    /// 速度調整
    /// </summary>
    //void ChangeVelocityX()
    //{

    //    // 速度が最高速度以下なら加速
    //    if (rigidbody2d.velocity.x < maxSpeed)
    //    {
    //        SpeedUpToMaxSpeed();
    //    }
    //    // 最高速度以上なら減速
    //    else if (rigidbody2d.velocity.x >= maxSpeed)
    //    {
    //        SpeedDownToMaxSpeed();
    //    }
    //    // ちょうどなら何もしない
    //}

    // 保留
    /// <summary>
    /// 加速処理
    /// </summary>
    //void SpeedUpToMaxSpeed()
    //{
    //    // 加速処理
    //    rigidbody2d.AddForce(new Vector2(grideAddSpeed, 0), ForceMode2D.Force);
    //    var velocity = rigidbody2d.velocity;
    //    // 速度が基本速度を下回っていたら基本速度に戻す
    //    if (player.BaseSpeed > velocity.x)
    //    {
    //        velocity.x = player.BaseSpeed;
    //    }
    //    // 速度が最高速度を上回っていたら最高速度に戻す
    //    if (maxSpeed < velocity.x)
    //    {
    //        velocity.x = maxSpeed;
    //    }

    //    rigidbody2d.velocity = velocity;
    //}

    // 保留
    /// <summary>
    /// 減速処理
    /// </summary>
    //void SpeedDownToMaxSpeed()
    //{
    //    //// 減速前の速度
    //    //var beforeVelocity = rigidbody2d.velocity;
    //    //// 減速後の速度
    //    Vector2 afterVelocity;
    //    //// 減速処理
    //    rigidbody2d.AddForce(new Vector2(-decaySpeed, 0), ForceMode2D.Force);
    //    afterVelocity = rigidbody2d.velocity;
    //    // 減速後の速度が最高速度を下回っていたら最高速度に戻す
    //    if (afterVelocity.x < maxSpeed)
    //    {
    //        rigidbody2d.velocity = new Vector2(maxSpeed, afterVelocity.y);
    //    }
    //    // そうでなければ減速後処理を適応
    //    else
    //    {
    //        rigidbody2d.velocity = afterVelocity;
    //    }
    //}

    /// <summary>
    /// 滑空の終了処理
    /// </summary>
    public void EndGlide()
    {
        if(rigidbody2d.velocity.y > 0)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
        }
    }
}
