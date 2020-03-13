using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{

    BulletFactory bulletFactory;
    [SerializeField]
    Player player = null;
    // 弾の最大所持数
    [SerializeField]
    public int BulletCount
    {
        get { return bulletCount; }
    }
    [SerializeField]
    private int bulletCount=3;
    // 現在の弾の所持数
    public int nowBulletCount = 0;
    // 通常の弾のリロード時間
    public float DefaultBulletChargeTime
    {
        get { return defaultBulletChargeTime; }
    }
    [SerializeField]
    private float defaultBulletChargeTime = 3;
    // 雨が降っているときの弾のリロード時間
    public float RainBulletChargeTime
    {
        get { return rainBulletChargeTime; }
    }
    [SerializeField]
    private float rainBulletChargeTime = 1;
    // 今の弾のリロード時間
    [SerializeField]
    private float nowBulletChargeTime;
    // 現在の経過時間
    float nowTime = 0;

    // 必要なコンポーネント
    Animator animator;
    int shotID = Animator.StringToHash("Shot");

    // Start is called before the first frame update
    void Start()
    {
        bulletFactory = GameObject.Find("BulletFactory").GetComponent<BulletFactory>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 雨が降っているならチャージ時間を短くする
        if(player.IsRain)
        {
            if(nowBulletChargeTime!=rainBulletChargeTime)
            {
                nowTime = 0;
                nowBulletChargeTime = rainBulletChargeTime;
            }
        }
        else
        {
            if(nowBulletChargeTime!=defaultBulletChargeTime)
            {
                nowTime = 0;
                nowBulletChargeTime = defaultBulletChargeTime;
            }
        }

        // ゲームが開始したらチャージ開始
        if(SceneController.Instance.isStart)
        {
            // 弾をチャージ
            ChargeBullet();
        }
    }


    /// <summary>
    /// 弾を発射する関数です
    /// </summary>
    /// <param name="position"></param>
    public void Shot(Vector2 position,int ID)
    {
        if(nowBulletCount>0)
        {
            nowBulletCount--;
            bulletFactory.ShotBullet(position,ID);
            // アニメーターにセット
            animator.SetTrigger(shotID);
        }
    }


    /// <summary>
    /// 弾をチャージする時間
    /// </summary>
    public void ChargeBullet()
    {
        nowTime += Time.deltaTime;
        if (nowTime >= nowBulletChargeTime)
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
