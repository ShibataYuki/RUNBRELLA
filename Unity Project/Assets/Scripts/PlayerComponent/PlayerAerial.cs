using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 空中状態のときにスピードをチェックして止まらないようにする
/// </summary>
public class PlayerAerial : MonoBehaviour
{
    // 加える力
    [SerializeField]
    float addSpeed = 0.3f;

    // リジッドボディのコンポーネント
    private new Rigidbody2D rigidbody;
    // 速度減衰値
    [SerializeField]
    float decaySpeed = 0.05f;
    Player player;

    // 上昇気流内にいることを示すスプライト
    private SpriteRenderer updraftEffect = null;
    // 上昇気流のレイヤー
    private LayerMask updraftLayer = 0;
    // 上昇気流内にいるかチェックする領域
    private Vector2 leftBottom = Vector2.zero;
    private Vector2 rightTop = Vector2.zero;

    [SerializeField]
    float aerialGravityScale = 3;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        // コンポーネントの取得
        rigidbody = GetComponent<Rigidbody2D>();
        updraftEffect = transform.Find("A").gameObject.GetComponent<SpriteRenderer>();
        // 演出は最初、切っておく
        EffectOff();
        // 上昇気流レイヤーの取得
        updraftLayer = LayerMask.GetMask("Updraft");
        // 当たり判定の領域の計算
        var collider = GetComponent<BoxCollider2D>();
        leftBottom = collider.offset;
        rightTop = collider.offset;
        leftBottom += -(collider.size * 0.5f);
        rightTop   += (collider.size * 0.5f);
        // ファイル名
        string fileName = nameof(PlayerAerial) + "Data" + player.Type;
        // テキストの読み込み
        decaySpeed = TextManager.Instance.GetValue_float(fileName, nameof(decaySpeed));
        aerialGravityScale = TextManager.Instance.GetValue_float(fileName, nameof(aerialGravityScale));
    }

/// <summary>
/// 開始時処理
/// </summary>
public void StartAerial()
    {
        player.Rigidbody.gravityScale = aerialGravityScale;
    }

    /// <summary>
    /// 終了時処理
    /// </summary>
    public void EndAerial()
    {
        player.Rigidbody.gravityScale = 1;
    }

    /// <summary>
    /// 最低速度を下回っているかチェックして
    /// 遅かったら最低速度に変更
    /// </summary>
    public void SpeedCheck()
    {

        rigidbody.AddForce(new Vector2(addSpeed, 0), ForceMode2D.Force);

        // 速度の取得
        var velocity = rigidbody.velocity;
        if(velocity.x>player.MaxSpeed)
        {
            velocity.x = player.MaxSpeed;
        }
        if (velocity.x < player.BaseSpeed)
        {
            velocity.x = player.BaseSpeed ;
        }

        rigidbody.velocity = velocity;

        // 速度の制限処理
        //velocity.x -= decaySpeed;
        //if (velocity.x < player.BaseSpeed)
        //{
        //    velocity.x = player.BaseSpeed;
        //}

        //rigidbody.velocity = velocity;        

    }

    /// <summary>
    /// 上昇気流に乗れることを演出で示す
    /// </summary>
    public void EffectOn()
    {
        var color = updraftEffect.color;
        color.a = 1.0f;
        updraftEffect.color = color;
    }
          
    /// <summary>
    /// 上昇気流に乗れないことを演出で示す
    /// </summary>
    public void EffectOff()
    {
        var color = updraftEffect.color;
        color.a = 0.0f;
        updraftEffect.color = color;
    }

    public void UpdraftCheck()
    {
        // 当たり判定の領域の現在位置を計算
        var workLeftBottom = leftBottom + (Vector2)transform.position;
        var workRightTop   = rightTop   + (Vector2)transform.position;
        // 上昇気流内にいるかチェック
        bool isHit = Physics2D.OverlapArea(workLeftBottom, workRightTop, updraftLayer);
        if(isHit == true)
        {
            // エフェクトをONにする
            EffectOn();
        }
        else
        {
            // エフェクトをOFFにする
            EffectOff();
        }
    }
}
