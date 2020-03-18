using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{

    // 移動速度   
    float speed = 0;

    // 通常の速度
    [SerializeField]
    float nomalSpeed = 10;
    // 雨を受けているときのスピード
    [SerializeField]
    float rainSpeed = 15;

    // ヒットしたものの情報を格納する変数
    public RaycastHit2D RayHit { get; set; }
    GameObject hitObject = null;
    // 自身のコライダー
    BoxCollider2D boxCollider;  
    //「Player」コンポーネント
    Player player;
    // どのレイヤーのものとヒットさせるか
    LayerMask layerMask;   
    // 自身のリジットボディ
    Rigidbody2D rigidbody2d;   
    // 掴めることを示すスプライト
    private SpriteRenderer catchEffect = null;

    // 掴めそうなときにアルファ値にかける倍率
    [SerializeField]
    private float aScale = 0.5f;
    // 何フレーム先の予測までチェックするか
    [SerializeField]
    private int checkCount = 10;
    [SerializeField]
    private bool isColliderHit = false;
    public bool IsColliderHit { get { return isColliderHit; } set { isColliderHit = value; } }

    private readonly string fileName = nameof(PlayerSlide) + "Data";

    // 保存するvelocityのx
    float velocityX;
    // Start is called before the first frame update
    void Start()
    {
        // 変数の初期化
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GetComponent<Player>();
        // レイヤーマスクを「Slider」に設定
        layerMask = LayerMask.GetMask(new string[] {"Slider"});       
        // 子オブジェクトのコンポーネントを探す
        catchEffect = transform.Find("B").gameObject.GetComponent<SpriteRenderer>();
        // テキストの読み込み
        //nomalSpeed = TextManager.Instance.GetValue(fileName, nameof(nomalSpeed));
        //rainSpeed = TextManager.Instance.GetValue(fileName, nameof(rainSpeed));
        //aScale = TextManager.Instance.GetValue(fileName, nameof(aScale));
        //checkCount = (int)TextManager.Instance.GetValue(fileName, nameof(checkCount));
        // 演出を切る
        EffectOff();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {        
        if(collision.tag == "Slider")
        {
            IsColliderHit = true;
            hitObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Slider")
        {
            IsColliderHit = false ;
            hitObject = null;
        }
    }

    void SpeedChange()
    {
        if(player.IsRain)
        {
            speed = rainSpeed;
        }
        else
        {
            speed = nomalSpeed;
        }
        
    }

    /// <summary>
    /// 滑走の開始処理
    /// </summary>
    public void StartSlide()
    {
        // 現在のvelocityを保存
        velocityX = rigidbody2d.velocity.x;
        rigidbody2d.velocity = Vector2.zero;
        // プレイヤーの高さを手すりの高さに調整
        AdjustHight();
        // 重力を０に変更
        rigidbody2d.gravityScale = 0;
        // サイズの変更
        var size = boxCollider.size;
        size.y -= 0.05f;
        boxCollider.size = size;
        // オフセットの変更
        var offset = boxCollider.offset;
        offset.y += 0.05f;
        boxCollider.offset = offset;
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
        Debug.DrawLine(playerTopPos, playerBottomPos, Color.white);
        // プレイヤーの上の方向から下方向に向けてレイを飛ばして当たり判定                                        
        RayHit = Physics2D.Raycast(playerTopPos,   // 発射位置
                                Vector2.down,   // 発射方向
                                rayLength,      // 長さ
                                layerMask);     // どのレイヤーに当たるか
    }

    /// <summary>
    /// もうすぐ手すりを掴める位置に到達するかをチェックするメソッド
    /// </summary>
    /// <returns></returns>
    public void SliderCheckSoon()
    {
        Vector2 playerTopPos; // レイの発射位置
        Vector2 playerBottomPos; // レイの飛ばした先のポイント
        float rayLength; // レイの長さ
        playerTopPos = new Vector2(transform.position.x, transform.position.y + (boxCollider.size.y / 1.5f) + boxCollider.offset.y);
        playerBottomPos = new Vector2(transform.position.x, transform.position.y - (boxCollider.size.y / 1.5f) + boxCollider.offset.y);
        rayLength = playerTopPos.y - playerBottomPos.y;
        // checkCount フレーム内に手すりを掴めそうかチェックする
        for (int i = checkCount; i > 0; i--)
        {
            // (checkCount - i + 1)フレーム後の予測位置からレイのポイントを生成
            playerTopPos += (rigidbody2d.velocity * Time.deltaTime);
            playerTopPos.y += -(rigidbody2d.gravityScale * Time.deltaTime * 9.8f);
            playerBottomPos += (rigidbody2d.velocity * Time.deltaTime);
            playerBottomPos.y += -(rigidbody2d.gravityScale * Time.deltaTime * 9.8f);

            Debug.DrawLine(playerTopPos, playerBottomPos, Color.white);

            // プレイヤーの上の方向から下方向に向けてレイを飛ばして当たり判定                                        
            bool hit = Physics2D.Raycast(playerTopPos,   // 発射位置
                                    Vector2.down,   // 発射方向
                                    rayLength,      // 長さ
                                    layerMask);     // どのレイヤーに当たるか
            if (hit == true)
            {
                // ループ回数に応じたアルファ値でエフェクトを表示
                EffectLittle((float)i / checkCount * aScale);
                return;
            }
        }
        // エフェクトを非表示にする
        EffectOff();
    }

    /// <summary>
    /// 手すりを掴めることを演出で示す
    /// </summary>
    public void EffectOn()
    {
        var color = Color.yellow;
        color.a = 1.0f;
        catchEffect.color = color;
    }

    /// <summary>
    /// 手すりを掴めそうなことを演出で示す
    /// </summary>
    /// <param name="a">手すりとの距離に応じたセットするアルファ値</param>
    public void EffectLittle(float a)
    {
        var color = Color.green;
        color.a = a;
        catchEffect.color = color;
    }

    /// <summary>
    /// 手すりをしばらく掴めないことを演出で示す
    /// </summary>
    public void EffectOff()
    {
        var color = catchEffect.color;
        color.a = 0.0f;
        catchEffect.color = color;
    }

    /// <summary>
    /// プレイヤーを手すりの高さに調整する関数
    /// </summary>
    public void AdjustHight()
    {        
        if(RayHit == true)
        {
            var hitY = new Vector2(RayHit.point.x, RayHit.point.y);
            rigidbody2d.position = hitY;
        }
        
    }

    /// <summary>
    /// プレイヤーのvelocityを手すりのright方向に変換する関数
    /// </summary>
    public void Slide()
    {
        
        SpeedChange();
        AdjustHight();
        
        if(IsColliderHit == true)
        {
            // ベクトルによってはx方向とy方向に力が分散してしまうため
            // x方向の力の大きさをを無理やりspeedに戻す処理
            Vector2 workVelocity;
            float workX = 1 / hitObject.transform.right.x;
            workVelocity.x = speed * hitObject.transform.right.x * workX;
            workVelocity.y = speed * hitObject.transform.right.y * workX;
            rigidbody2d.velocity = workVelocity;
            // 元の処理
            // rigidbody2d.velocity = speed * hitObject.transform.right;

        }
        if (RayHit == true)
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.right,  RayHit.collider.gameObject.transform.right);
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
        // サイズの変更
        var size = boxCollider.size;
        size.y += 0.05f;
        boxCollider.size = size;
        // オフセットの変更
        var offset = boxCollider.offset;
        offset.y -= 0.05f;
        boxCollider.offset = offset;

        // 保存したvelocityに戻す
        Vector2 velocity;
        velocity.x = velocityX;
        velocity.y = rigidbody2d.velocity.y;
        rigidbody2d.velocity = velocity;

    }
}
