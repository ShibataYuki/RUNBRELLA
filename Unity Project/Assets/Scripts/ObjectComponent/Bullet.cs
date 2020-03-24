using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // 弾を打っているかのフラグ
    public bool IsShoting { get; set; } = false;
    // 画面外かどうかのフラグ
    bool isScreen = true;
    Rigidbody2D rigidbody2d;
    BulletFactory bulletFactory;
    // 通常の弾の速さ
    public float defaultSpeed = 5;
    // 雨が降っているときの弾の速さ
    public float rainSpeed = 8;
    // 現在の弾の速さ
    public float nowSpeed = 0;
    new Renderer renderer;
    // 地面のレイヤー
    [SerializeField]
    LayerMask playerlayer = 0;
    // プレイヤーのレイヤー
    [SerializeField]
    LayerMask groundlayer = 0;
    // 弾を撃ったプレイヤーのID
    public int ID;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        renderer = GetComponent<Renderer>();
        bulletFactory = GameObject.Find("BulletFactory").GetComponent<BulletFactory>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isScreen)
        {
            // 画面外ならプールに戻す
            IsShoting = false;
            bulletFactory.ReturnBullet(gameObject);
        }
        isScreen = false;
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
        Vector2 moveVec;
        moveVec.x = nowSpeed;
        moveVec.y = 0;
        rigidbody2d.velocity = moveVec;
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
