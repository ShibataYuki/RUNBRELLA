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

    // Start is called before the first frame update
    void Start()
    {
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
            // 最初は消す
            bulletObj.SetActive(false);
            bulletObjects.Add(bulletObj);
        }
    }


    /// <summary>
    /// 弾を発射させる関数
    /// </summary>
    /// <param name="position"></param>
    public void ShotBullet(Vector2 position,int ID)
    {
        for(int i=0;i<bulletMax;i++)
        {
            // プールに弾があったら
            if (bulletObjects[i].activeInHierarchy == false)
            {
                // 弾の速さを設定
                if(SceneController.Instance.playerObjects[ID].GetComponent<Player>().IsRain)
                {
                    bulletObjects[i].GetComponent<Bullet>().nowSpeed = bulletObjects[i].GetComponent<Bullet>().rainSpeed;
                }
                else
                {
                    bulletObjects[i].GetComponent<Bullet>().nowSpeed = bulletObjects[i].GetComponent<Bullet>().defaultSpeed;
                }
                // 弾が出る位置をずらす
                position.y -= 0.5f;
                // 撃ったプレイヤーの座標に合わせる
                bulletObjects[i].transform.position = position;
                // 撃ったプレイヤーのコライダーを登録
                bulletObjects[i].GetComponent<Bullet>().playerCollider2D = SceneController.Instance.playerObjects[ID].GetComponent<Collider2D>();
                // 弾を表示
                bulletObjects[i].SetActive(true);
                // ショット関数を呼ぶ
                bulletObjects[i].GetComponent<Bullet>().Shot();

                return;
                
            }
        }
    }


    public void ReturnBullet(GameObject bullet)
    {
        // 非表示にする
        bullet.SetActive(false);
        // 位置初期化
        bullet.transform.position = new Vector3(0, 0, 0);
        // 弾の速さ初期化
        bullet.GetComponent<Bullet>().nowSpeed = 0;
    }
}
