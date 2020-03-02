using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{

    BulletFactory bulletFactory;
    private int bulletCount=3;
    public int nowBulletCount = 0;
    [SerializeField]
    private float bulletChargeTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        bulletFactory = GameObject.Find("BulletFactory").GetComponent<BulletFactory>();
    }

    // Update is called once per frame
    void Update()
    {
        ChargeBullet();
    }


    /// <summary>
    /// 弾を発射する関数です
    /// </summary>
    /// <param name="position"></param>
    public void Shot(Vector2 position)
    {
        if(nowBulletCount>0)
        {
            nowBulletCount--;
            bulletFactory.ShotBullet(position);
        }
    }


    public void ChargeBullet()
    {
        // 一定時間経過で弾をチャージ
        if(SceneManager.Instance.TimeCounter(bulletChargeTime))
        {
            nowBulletCount++;
            if(nowBulletCount>bulletCount)
            {
                nowBulletCount = 3;
            }
        }
    }

}
