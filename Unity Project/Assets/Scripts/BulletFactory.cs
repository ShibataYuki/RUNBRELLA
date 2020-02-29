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
    public void ShotBullet(Vector2 position)
    {
        for(int i=0;i<bulletMax;i++)
        {
            // プールに弾があったら
            if(bulletObjects[i].activeInHierarchy==false)
            {
                // 撃ったプレイヤーの座標に合わせる
                bulletObjects[i].transform.position = position;
                // 弾を表示
                bulletObjects[i].SetActive(true);
                // ショット関数を呼ぶ
                bulletObjects[i].GetComponent<Bullet>().Shot();
                
            }
        }
    }


    public void ReturnBullet(GameObject bullet)
    {
        // 非表示にする
        bullet.SetActive(false);
        // 位置初期化
        bullet.transform.position = new Vector3(0, 0, 0);
    }
}
