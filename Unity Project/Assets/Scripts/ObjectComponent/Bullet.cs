using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // 弾を打っているかのフラグ
    bool isShoting = false;
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
    // 撃ったプレイヤーのコライダー
    public Collider2D playerCollider2D;

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
            isShoting = false;
            bulletFactory.ReturnBullet(gameObject);
        }
        isScreen = false;
    }

    private void FixedUpdate()
    {
        if (isShoting)
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
        // プレイヤーか地面と当たったらプールに戻す
        if(collision.gameObject.tag=="Player")
        {
            // 衝突相手が弾を撃ったプレイヤー自身なら被弾しない
            if(collision==playerCollider2D)
            {
                return;
            }
            // プレイヤーと当たった場合は被弾フラグをONにする
            var player = collision.gameObject.GetComponent<Player>();
            player.IsHitBullet = true;
            isShoting = false;
            bulletFactory.ReturnBullet(gameObject);
        }
        if(collision.gameObject.tag=="Ground"||collision.gameObject.tag=="BreakableBlock")
        {
            isShoting = false;
            bulletFactory.ReturnBullet(gameObject);
        }
    }

    /// <summary>
    /// 発射フラグを
    /// </summary>
    public void Shot()
    {
        // shotフラグをtrueにする
        if(isShoting==false)
        {
            isShoting = true;
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
