using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletDirection
    {
        UP,
        MIDDLE,
        DOWN,
    }

    // 弾を打っているかのフラグ
    public bool IsShoting { get; set; } = false;
    // 画面外かどうかのフラグ
    bool isScreen = true;
    Rigidbody2D rigidbody2d;
    BulletFactory bulletFactory;
    // 通常の弾の速さ
    public float speed = 0;
    Renderer bulletRenderer;
    // 地面のレイヤー
    [SerializeField]
    LayerMask playerlayer = 0;
    // プレイヤーのレイヤー
    [SerializeField]
    LayerMask groundlayer = 0;
    // 弾を撃ったプレイヤー
    public Character character;
    // 弾の進む方向
    public BulletDirection bulletDirection;
    // 上方向のベクトル
    public Vector2 upVec = new Vector2(0, 0);
    // 下方向のベクトル
    public Vector2 downVec = new Vector2(0, 0);
    // 移動量
    public float nowMoveVecY = 0;
    // 消えるまでの移動量
    public float targetMoveVecY = 0;

    // 弾の最大サイズ
    public float bulletSizeMax=0;
    // 一秒ごとの拡大率
    public float addBulletSize = 0;
    // 雨が降っているかどうか
    public bool isRain = false;
    

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        rigidbody2d = GetComponent<Rigidbody2D>();
        bulletRenderer = GetComponent<Renderer>();
        bulletFactory = GameObject.Find("BulletFactory").GetComponent<BulletFactory>();
        ReadTextParameter();
    }

    /// <summary>
    /// textからパラメータを読み込む関数
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var playerATextName = "Chara_A";
        var playerBTextName = "Chara_B";
        // テキストの中のデータをセットするディクショナリー
        Dictionary<string, float> charaABulletDictionary;
        Dictionary<string, float> charaBBulletDictionary;
        SheetToDictionary.Instance.TextToDictionary(playerATextName, out charaABulletDictionary);
        SheetToDictionary.Instance.TextToDictionary(playerBTextName, out charaBBulletDictionary);
        try
        {
            if(character.charType==GameManager.CHARTYPE.PlayerA)
            {
                // ファイル読み込み
                speed = charaABulletDictionary["弾のスピード"];
                upVec.x = charaABulletDictionary["雨時の弾の3方向のうちの上方向の弾の角度の横方向(X方向)"];
                upVec.y = charaABulletDictionary["雨時の弾の3方向のうちの上方向の弾の角度の縦方向(Y方向)"];
                downVec.x = charaABulletDictionary["雨時の弾の3方向のうちの下方向の弾の角度の横方向(X方向)"];
                downVec.y = charaABulletDictionary["雨時の弾の3方向のうちの下方向の弾の角度の縦方向(Y方向)"];
                targetMoveVecY = charaABulletDictionary["雨時の弾の上下方向の弾が消えるまでの距離"];
            }
            else
            {
                // ファイル読み込み
                speed = charaBBulletDictionary["弾のスピード"];
                upVec.x = charaBBulletDictionary["雨時の弾の3方向のうちの上方向の弾の角度の横方向(X方向)"];
                upVec.y = charaBBulletDictionary["雨時の弾の3方向のうちの上方向の弾の角度の縦方向(Y方向)"];
                downVec.x = charaBBulletDictionary["雨時の弾の3方向のうちの下方向の弾の角度の横方向(X方向)"];
                downVec.y = charaBBulletDictionary["雨時の弾の3方向のうちの下方向の弾の角度の縦方向(Y方向)"];
                targetMoveVecY = charaBBulletDictionary["雨時の弾の上下方向の弾が消えるまでの距離"];

            }
        }
        catch
        {
            Debug.Assert(false, nameof(NewsUIExit) + "でエラーが発生しました");
        }

    }


    // Update is called once per frame
    void Update()
    {
        // 画面内かどうか
        if(!isScreen)
        {
            // 画面外ならプールに戻す
            IsShoting = false;
            bulletFactory.ReturnBullet(gameObject);
        }
        isScreen = false;

        //一定以上移動したかどうか
        if(nowMoveVecY>=targetMoveVecY&&!(bulletDirection==BulletDirection.MIDDLE))
        {
            // 一定以上移動したならプールに戻す
            IsShoting = false;
            bulletFactory.ReturnBullet(gameObject);
        }

        // 最大サイズでないかつ雨が降っているなら拡大
        if(gameObject.transform.localScale.x<=bulletSizeMax&&isRain)
        {
            // 1フレームごとの拡大率
            float ratePer1Frame = addBulletSize * Time.deltaTime;
            Vector3 scale = gameObject.transform.localScale * ratePer1Frame + gameObject.transform.localScale;
            // 拡大
            gameObject.transform.localScale = scale;
        }
    }

    private void FixedUpdate()
    {
        if (IsShoting)
        {
            Move();
        }
    }
    /// <summary>
    /// 衝突時
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Ground"||collision.gameObject.tag=="BreakableBlock")
        {
            IsShoting = false;
            bulletFactory.ReturnBullet(gameObject);
        }
    }

    /// <summary>
    /// 発射フラグを
    /// </summary>
    public void Shot()
    {
        // shotフラグをtrueにする
        if(IsShoting==false)
        {
            IsShoting = true;
        }
    }


    /// <summary>
    /// 弾を移動させる関数
    /// </summary>
    private void Move()
    {
        Vector2 moveVec = new Vector2(0, 0);
        // 進む方向を決定
        if(bulletDirection==BulletDirection.UP)
        {
            moveVec = upVec;
        }
        else if(bulletDirection==BulletDirection.MIDDLE)
        {
            moveVec = new Vector2(1, 0);
        }
        else if(bulletDirection==BulletDirection.DOWN)
        {
            moveVec = downVec;
        }
        Vector2 workVelocity = moveVec.normalized * speed;
        // 真ん中以外の弾なら移動量を保存
        if(!(bulletDirection==BulletDirection.MIDDLE))
        {
            nowMoveVecY += Mathf.Abs(workVelocity.y);
        }
        // 決めた方向へ進む
        rigidbody2d.velocity = workVelocity;
    }


    /// <summary>
    /// メインカメラ内にいるか判定する関数
    /// </summary>
    private void OnWillRenderObject()
    {
        if(Camera.current.name=="Main Camera")
        {
            isScreen = true;
        }
    }

}
