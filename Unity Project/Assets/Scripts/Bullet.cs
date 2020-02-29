using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // 弾を打っているかのフラグ
    bool isShoting = false;
    Rigidbody2D rigidbody2d;
    BulletFactory bulletFactory;
    // 弾の速さ
    [SerializeField]
    float speed = 5;
    new Renderer renderer;

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
        if(renderer.isVisible)
        {
            // 画面外ならプールに戻す
            isShoting = false;
            bulletFactory.ReturnBullet(gameObject);
        }
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
        if(collision.tag=="Player"||collision.tag=="Ground")
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
        moveVec.x = speed;
        moveVec.y = 0;
        rigidbody2d.velocity = moveVec;
    }

}
