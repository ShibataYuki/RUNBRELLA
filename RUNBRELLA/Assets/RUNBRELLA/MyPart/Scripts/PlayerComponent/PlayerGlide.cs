using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlide : MonoBehaviour
{
    // 自身のリジットボディ
    Rigidbody2D rigidbody2d;
    Character character;
    PlayerMove move;    

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
        // テキストの中のデータをセットするディクショナリー        
        Dictionary<string, float> textDataDic;
        textDataDic = SheetToDictionary.TextNameToData[textName];
        Dictionary<string, float> charaCommonDictionary;
        charaCommonDictionary = SheetToDictionary.TextNameToData[charaCommonTextName];             
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
    public void Glide()
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
        // Y方向の慣性をリセット
        if(rigidbody2d.velocity.y <= 0)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
        }        
        rigidbody2d.AddForce(new Vector2(0, HopPower), ForceMode2D.Impulse);
    }
    
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
