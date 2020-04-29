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
        bulletParent = GameObject.Find("Bullets").gameObject;
        CreateBullet();
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
            var player=playerObj.GetComponent<Player>();
            // プールに弾があったら
            if (bulletObjects[i].activeInHierarchy == false)
            {
                var position = playerObj.transform.position;
                // 弾が出る位置をずらす
                if(player.state==PlayerStateManager.Instance.playerRunState)
                {
                    position.y -= offsetY;
                }
                else
                {
                    position.y -= (offsetY - 0.2f);
                }
                position.x += offsetX;
                // 撃ったプレイヤーの座標に合わせる
                bulletObjects[i].transform.position = position;
                var bullet = bulletObjects[i].GetComponent<Bullet>();
                // 弾を撃つ方向を決める
                bullet.bulletDirection = Bullet.BulletDirection.MIDDLE;
                // 弾を撃ったプレイヤーのIDを記憶
                bullet.controllerNo = player.controllerNo;
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
                var player = playerObj.GetComponent<Player>();
                // プールに弾があったら
                if (bulletObjects[i].activeInHierarchy == false)
                {
                    var position = playerObj.transform.position;
                    // 弾が出る位置をずらす
                    if (player.state == PlayerStateManager.Instance.playerRunState)
                    {
                        position.y -= offsetY;
                    }
                    else
                    {
                        position.y -= (offsetY - 0.2f);
                    }
                    position.x += offsetX;
                    // 撃ったプレイヤーの座標に合わせる
                    bulletObjects[i].transform.position = position;
                    var bullet = bulletObjects[i].GetComponent<Bullet>();
                    // 弾を撃つ方向を決定
                    bullet.bulletDirection = (Bullet.BulletDirection)l;
                    // 弾を撃ったプレイヤーのIDを記憶
                    bullet.controllerNo = player.controllerNo;
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
    /// <param name="bullet">プールに戻す弾</param>
    public void ReturnBullet(GameObject bullet)
    {
        // 非表示にする
        bullet.SetActive(false);
        // 位置初期化
        bullet.transform.position = new Vector3(0, 0, 0);
        // 移動量初期化
        bullet.GetComponent<Bullet>().nowMoveVecY = 0;
    }
}
