using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDropItemFactory : MonoBehaviour
{
    // 作成するPrefab
    [SerializeField]
    private GameObject rainDropItemPrefab = null;
    // 作成する数
    [SerializeField]
    private int ItemMax = 0;
    // 作成したアイテムのリスト
    [SerializeField]
    private List<GameObject> rainDropItems = new List<GameObject>();
    // 親オブジェクト
    GameObject cloud;
    // アイテムの重力
    [SerializeField]
    private float gravity = 0;
    // アイテムが出る確率(毎フレーム抽選)
    [SerializeField]
    private int frameMax = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        cloud = GameObject.Find("Cloud");
        Create();
    }

    // Update is called once per frame
    void Update()
    {
    }


    /// <summary>
    /// アイテムを作成する関数
    /// </summary>
    void Create()
    {
        for(int i=0;i<ItemMax;i++)
        {
            // 作成
            var rainDropItem = Instantiate(rainDropItemPrefab);
            // 最初は非表示
            rainDropItem.SetActive(false);
            // Cloudの子オブジェクトにする
            rainDropItem.transform.parent = cloud.transform;
            // position初期化
            rainDropItem.transform.localPosition = new Vector3(2.6f, 0, 0);
            // 重力を設定
            rainDropItem.GetComponent<Rigidbody2D>().gravityScale = gravity;
            // リストに追加
            rainDropItems.Add(rainDropItem);
        }
    }


    /// <summary>
    /// アイテムを落とす関数
    /// </summary>
    void Drop()
    {
        for(int i=0;i<ItemMax;i++)
        {
            // 既に使用しているなら使用しない
            if(rainDropItems[i].activeInHierarchy)
            {
                continue;
            }
            // 子オブジェクトから抜ける
            rainDropItems[i].transform.parent = null;
            // 表示する
            rainDropItems[i].SetActive(true);
            return;
        }
        return;
    }


    /// <summary>
    /// アイテムを出すかどうかを選ぶ関数
    /// </summary>
    public void ChooseDrop()
    {
        // 乱数を生成
        var chooseNum = Random.Range(0, frameMax + 1);
        if(chooseNum==0)
        {
            Drop();
        }
    }


    /// <summary>
    /// プールに戻す処理をする関数
    /// </summary>
    /// <param name="item"></param>
    public void ReturnItem(GameObject item)
    {
        // 非表示にする
        item.SetActive(false);
        // Cloudの子オブジェクトにもどす
        item.transform.parent = cloud.transform;
        // 位置を初期化
        item.transform.localPosition = new Vector3(2.6f, 0, 0);
    }
}
