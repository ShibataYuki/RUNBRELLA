using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSordCollider : MonoBehaviour
{
    
    BulletFactory bulletFactory;

    // Start is called before the first frame update
    void Start()
    {
        bulletFactory = GameObject.Find("BulletFactory").GetComponent<BulletFactory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// 衝突判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーの弾と当たったとき
        if(collision.tag=="Bullet")
        {
            var playerAttack = transform.parent.GetComponent<PlayerAttack>();
            if(playerAttack.isGuardBullet)
            {
                var bullet = collision.GetComponent<Bullet>();
                // 弾を破壊する
                bullet.IsShoting = false;
                bulletFactory.ReturnBullet(collision.gameObject);
            }
        }
    }
}
