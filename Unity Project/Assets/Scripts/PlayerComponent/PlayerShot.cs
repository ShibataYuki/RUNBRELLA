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
    public int nowBulletCount = 6;
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
    // 弾発射のSE
    [SerializeField]
    private AudioClip shotSE = null;
    // SEのボリューム
    [SerializeField]
    private float SEVolume = 1f;

    // 必要なコンポーネント
    Animator animator;
    int shotID = Animator.StringToHash("Shot");

    // Start is called before the first frame update
    void Start()
    {
        bulletFactory = GameObject.Find("BulletFactory").GetComponent<BulletFactory>();
        animator = GetComponent<Animator>();
        var player = GetComponent<Player>();
        // 読み込むファイルのファイル名
        string fileName = nameof(PlayerShot) + "Data" + player.Type;
        // テキストの読み込み
        bulletCount = TextManager.Instance.GetValue_int(fileName, nameof(bulletCount));
        defaultBulletChargeTime = TextManager.Instance.GetValue_float(fileName, nameof(defaultBulletChargeTime));
        rainBulletChargeTime = TextManager.Instance.GetValue_float(fileName, nameof(rainBulletChargeTime));
        SEVolume = TextManager.Instance.GetValue_float(fileName, nameof(SEVolume));
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
            // SEの再生
            AudioManager.Instance.PlaySE(shotSE, SEVolume);
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
                nowBulletCount = bulletCount;
            }
        }
    }

}
