using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDown : MonoBehaviour
{
    [SerializeField]
    private Character character = default;
    [SerializeField]
    private PlayerAttack playerAttack = default;
    // 空中状態
    private PlayerAerial playerAerial = default;
    // 移動クラス
    PlayerMove move;
    // ダウン時に速度がどれだけ減るか
    float downSpeed = 0;
    // 現在の時間
    public float nowTime = 0;
    // ダウン時にボタンを押したときに１フレームごとに減る時間の値
    [SerializeField]
    public float addTime = 0.05f;
    // ダメージを受けたときのSE
    [SerializeField]
    private AudioClip damageSE = null;
    // SEのボリューム
    private float damageSEVolume = 1f;
    // 必要なコンポーネント
    PlayerCharge playerCharge;


    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        playerCharge = GetComponent<PlayerCharge>();
        playerAerial = GetComponent<PlayerAerial>();
        move = GetComponent<PlayerMove>();
        character = GetComponent<Character>();
        ReadTextParameter();
        // 読み込むファイルのファイル名
        var fileName = nameof(PlayerDown) + "Data" + character.Type;
        // テキストの読み込み
        addTime = TextManager.Instance.GetValue_float(fileName, nameof(addTime));
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
        downSpeed = textDataDic["ダウン中の速度"];        
    }

    public void StartDown()
    {        
        // SEの再生
        AudioManager.Instance.PlaySE(damageSE, damageSEVolume);
        // 加速度の蓄積をリセット
        move.ResetAcceleration();  
        // プレイヤーの移動ベクトルを最低速度にする
        Rigidbody2D rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(downSpeed, 0);
        // プレイヤーを遅くする
        //SceneController.Instance.playerEntityData.playerRuns[ID].SetSpeed(SceneController.Instance.playerEntityData.playerRuns[ID].downSpeed);
        playerAttack.NowBulletCount -= playerCharge.chargeCount;
        // チャージをリセット
        playerCharge.ChargeReset();
        // 重力加速度を変更
        rigidbody2d.gravityScale = playerAerial.aerialGravityScale;
        // 角度を初期化
        transform.localRotation = Quaternion.identity;

    }

    public void EndDown()
    {       
        // 被弾フラグを解除
        playerAttack.IsHit = false;
    }

    /// <summary>
    /// 時間測定関数
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool TimeCounter(float time)
    {
        nowTime += Time.deltaTime;
        if (nowTime >= time)
        {
            nowTime = 0;
            return true;
        }
        return false;
    }

}
