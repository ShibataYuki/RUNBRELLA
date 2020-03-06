using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{

    BulletFactory bulletFactory;
    // 弾の最大所持数
    [SerializeField]
    private int bulletCount=3;
    // 現在の弾の所持数
    public int nowBulletCount = 0;
    // 弾のリロード時間
    [SerializeField]
    private float bulletChargeTime = 3;
    // 現在の経過時間
    float nowTime = 0;

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


    /// <summary>
    /// 弾をチャージする時間
    /// </summary>
    public void ChargeBullet()
    {
        nowTime += Time.deltaTime;
        Debug.Log(nowTime);
        if (nowTime >= bulletChargeTime)
        {
            nowTime = 0;
            nowBulletCount++;
            if (nowBulletCount > bulletCount)
            {
                nowBulletCount = 3;
            }
        }
    }

}
