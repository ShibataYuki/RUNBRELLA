using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{

    // 移動速度  
    float speed = 10;

    // ヒットしたものの情報を格納する変数
    public RaycastHit2D Hit { get; set; }
    // 自身のコライダー
    BoxCollider2D boxCollider;  
    //「Player」コンポーネント
    Player player;
    // どのレイヤーのものとヒットさせるか
    LayerMask layerMask;   
    // 自身のリジットボディ
    Rigidbody2D rigidbody2d;
    SpriteRenderer sprite;
    // 掴めることを示すエフェクト
    private GameObject catchEffect = null;

    // Start is called before the first frame update
    void Start()
    {
        // 変数の初期化
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GetComponent<Player>();
        // レイヤーマスクを「Slider」に設定
        layerMask = LayerMask.GetMask(new string[] {"Slider"});
        sprite = GetComponent<SpriteRenderer>();
        // ゲームオブジェクトを探す
        catchEffect = transform.Find("ExclamationMark").gameObject;
        // 演出を切る
        EffectOff();
    }

    /// <summary>
    /// 滑走の開始処理
    /// </summary>
    public void StartSlide()
    {
        // プレイヤーの高さを手すりの高さに調整
        AdjustHight();
        // 重力を０に変更
        rigidbody2d.gravityScale = 0;
        // デバッグ用色の変更
        sprite.color = Color.yellow;
    }

    /// <summary>
    /// スライダーとの当たり判定をする
    /// </summary>
    /// <returns></returns>
    public void SlideCheck()
    {        
        Vector2 playerTopPos;
        Vector2 playerBottomPos;
        float rayLength;
        playerTopPos = new Vector2(transform.position.x , transform.position.y + (boxCollider.size.y / 1.5f) + boxCollider.offset.y);
        playerBottomPos = new Vector2(transform.position.x , transform.position.y - (boxCollider.size.y / 1.5f) + boxCollider.offset.y);
        rayLength = playerTopPos.y - playerBottomPos.y;        
        //Debug.DrawLine(playerTopPos, playerBottomPos, Color.white);
        // プレイヤーの上の方向から下方向に向けてレイを飛ばして当たり判定                                        
        Hit = Physics2D.Raycast(playerTopPos,   // 発射位置
                                Vector2.down,   // 発射方向
                                rayLength,      // 長さ
                                layerMask);     // どのレイヤーに当たるか
    }

    /// <summary>
    /// 手すりを掴めることを演出で示す
    /// </summary>
    public void EffectOn()
    {
        catchEffect.SetActive(true);
    }

    /// <summary>
    /// 手すりを掴めないことを演出で示す
    /// </summary>
    public void EffectOff()
    {
        catchEffect.SetActive(false);
    }

    /// <summary>
    /// プレイヤーを手すりの高さに調整する関数
    /// </summary>
    public void AdjustHight()
    {        
        var hitY = new Vector2(Hit.point.x  , Hit.point.y );
        rigidbody2d.position = hitY;
    }

    /// <summary>
    /// プレイヤーのvelocityを手すりのright方向に変換する関数
    /// </summary>
    public void Slide()
    {
        AdjustHight();
        rigidbody2d.velocity = Hit.collider.gameObject.transform.right * speed;
        if(Hit.collider.gameObject.tag == "Slider")
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.right, Hit.collider.gameObject.transform.right);
        }
        //else if(Hit.collider.gameObject.tag == "Converter")
        //{
        //    Debug.Log("コンバーター");
        //    GameObject converter = Hit.collider.gameObject;
        //    Vector2 converterPos = Hit.collider.gameObject.transform.position;
        //    Vector2 hit2converterBottom
        //        = Hit.point - new Vector2(converterPos.x, converterPos.y - converter.GetComponent<CircleCollider2D>().radius * converter.transform.localScale.x);
        //    Debug.DrawLine(Hit.point, new Vector2(converterPos.x, converterPos.y - converter.GetComponent<CircleCollider2D>().radius * converter.transform.localScale.x), Color.red);

        //    Vector2 point = new Vector2(Hit.point.x, converterPos.y - converter.GetComponent<CircleCollider2D>().radius * converter.transform.localScale.y *2);
        //    Debug.DrawLine(converter.transform.position, point, Color.blue);
        //    Vector2 Converter2point = point - (Vector2)converter.transform.position;

        //    transform.rotation = Quaternion.FromToRotation(Vector2.up, hit2converterBottom);
        //}

    }

    /// <summary>
    /// プレイヤーの滑走速度をセットする関数
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }


    /// <summary>
    /// 滑走の終了処理
    /// </summary>
    public void EndSlide()
    {        
        rigidbody2d.gravityScale = 1;
        transform.rotation = Quaternion.FromToRotation(transform.right, Vector2.zero);

    }
}
