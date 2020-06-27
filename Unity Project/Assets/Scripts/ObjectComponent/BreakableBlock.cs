using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{

    // ブロックのHP
    [SerializeField]
    int HP = 2;
    // プレイヤーを押し戻す力の大きさ
    [SerializeField]
    Vector2 pushBackPower = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 衝突相手がプレイヤーの弾ならHPへらす
        if(collision.gameObject.tag=="Bullet")
        {
            HP--;
            // もしHPが0になったらブロックを消す
            if (HP<=0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤーとあたったら押し戻す処理
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, collision.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(pushBackPower, ForceMode2D.Impulse);
        }
    }

}
