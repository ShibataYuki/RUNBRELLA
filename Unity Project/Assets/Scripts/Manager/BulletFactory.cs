using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{

    // 弾のPrefab
    [SerializeField]
    GameObject bulletPrefab = null;
    // 作成する数
    [SerializeField]
    int bulletMax = 1;
    // 作成した弾のリスト
    List<GameObject> bulletObjects = new List<GameObject>();
    [SerializeField]
    float offsetX = 0;
    [SerializeField]
    float offsetY = 0;
    // 弾の親オブジェクト
    private GameObject bulletParent;


    // Start is called before the first frame update
    void Start()
    {
        // textからパラメータを読み込む
        // ReadTextParameter();
        bulletParent = GameObject.Find("Bullets").gameObject;
        CreateBullet();
    }


    /// <summary>
    /// textからパラメータを読み込む関数
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var textName = "Bullet";
        // テキストの中のデータをセットするディクショナリー
        Dictionary<string, float> bulletFactoryDictionary;
        SheetToDictionary.Instance.TextToDictionary(textName, out bulletFactoryDictionary);
        try
        {
            // ファイル読み込み
            offsetX = bulletFactoryDictionary["弾がプレイヤーを(0、0)として出てくる横(X)方向のオフセット"];
            offsetY = bulletFactoryDictionary["弾がプレイヤーを(0、0)として出てくる縦(Y)方向のオフセット"];
        }
        catch
        {
            Debug.Assert(false, nameof(ShockCamera) + "でエラーが発生しました");
        }

    }



    /// <summary>
    /// 弾を作成する関数
    /// </summary>
    void CreateBullet()
    {
        for(int i=0;i<bulletMax;i++)
        {
            // 弾をbulletMax個作成
            var bulletObj = Instantiate(bulletPrefab);
            // Bulletsの子オブジェクトにする
            bulletObj.transform.parent = bulletParent.transform;
            // 最初は消す
            bulletObj.SetActive(false);
            bulletObjects.Add(bulletObj);
        }
    }


    /// <summary>
    /// 弾の発射処理をする関数
    /// </summary>
    /// <param name="playerObj">弾を発射したプレイヤー</param>
    public void ShotBullet(GameObject playerObj)
    {
        for(int i=0;i<bulletMax;i++)
        {
            var character=playerObj.GetComponent<Character>();
            var playerAttack = playerObj.GetComponent<PlayerAttack>();
            // プールに弾があったら
            if (bulletObjects[i].activeInHierarchy == false)
            {
                var position = playerObj.transform.position;
                // 弾が出る位置をずらす
                if(character.IsRun == true)
                {
                    position.y -= playerAttack.offsetY;
                }
                else
                {
                    position.y -= (playerAttack.offsetY - 0.2f);
                }
                position.x += playerAttack.offsetX;
                // 撃ったプレイヤーの座標に合わせる
                bulletObjects[i].transform.position = position;
                var bullet = bulletObjects[i].GetComponent<Bullet>();
                // 弾のスピードを決定
                bullet.speed = playerAttack.speed;
                // 弾を撃つ方向を決める
                bullet.bulletDirection = Bullet.BulletDirection.MIDDLE;
                // 弾を撃ったプレイヤーのIDを記憶
                bullet.playerNo = character.playerNO;
                // 弾を表示
                bulletObjects[i].SetActive(true);
                // ショット関数を呼ぶ
                bullet.Shot();

                return;
                
            }
        }
    }

    /// <summary>
    /// 雨の時の弾の発射処理をする関数
    /// </summary>
    /// <param name="playerObj">弾を発射したプレイヤー</param>
    public void WhenRainShotBullet(GameObject playerObj)
    {
        for(int l=0;l<3;l++)
        {
            for (int i = 0; i < bulletMax; i++)
            {
                // プールから弾を見つけたかどうかのフラグ
                bool isFind = false;
                var character = playerObj.GetComponent<Character>();
                var playerAttack = playerObj.GetComponent<PlayerAttack>();
                // プールに弾があったら
                if (bulletObjects[i].activeInHierarchy == false)
                {
                    var position = playerObj.transform.position;
                    // 弾が出る位置をずらす
                    if (character.IsRun)
                    {
                        position.y -= playerAttack.offsetY; ;
                    }
                    else
                    {
                        position.y -= (playerAttack.offsetY - 0.2f);
                    }
                    position.x += playerAttack.offsetX;
                    // 撃ったプレイヤーの座標に合わせる
                    bulletObjects[i].transform.position = position;
                    var bullet = bulletObjects[i].GetComponent<Bullet>();
                    // 弾のスピードを決定
                    bullet.speed = playerAttack.speed;
                    // 弾を撃つ方向を決定
                    bullet.bulletDirection = (Bullet.BulletDirection)l;
                    // 弾を撃つ角度を決定
                    switch(bullet.bulletDirection)
                    {
                        case Bullet.BulletDirection.UP:
                            bullet.upVec = playerAttack.upVec;
                            break;
                        case Bullet.BulletDirection.DOWN:
                            bullet.downVec = playerAttack.downVec;
                            break;
                    }
                    // 雨フラグをONにする
                    bullet.isRain = true;
                    // 弾を撃ったプレイヤーのIDを記憶
                    bullet.playerNo = character.playerNO;
                    // 弾を表示
                    bulletObjects[i].SetActive(true);
                    // ショット関数を呼ぶ
                    bullet.Shot();
                    // フラグをON
                    isFind = true;
                }
                if(isFind)
                {
                    break;
                }
            }
        }
    }


    /// <summary>
    /// 撃った球をプールに戻す関数
    /// </summary>
    /// <param name="bulletObj">プールに戻す弾</param>
    public void ReturnBullet(GameObject bulletObj)
    {
        // 非表示にする
        bulletObj.SetActive(false);
        // 位置初期化
        bulletObj.transform.position = new Vector3(0, 0, 0);
        // 大きさを初期化
        bulletObj.transform.localScale = new Vector3(1, 1, 1);
        var bullet = bulletObj.GetComponent<Bullet>();
        // 雨天時フラグをOffにする
        bullet.isRain = false;
        // 移動量初期化
        bullet.GetComponent<Bullet>().nowMoveVecY = 0;
    }
}
