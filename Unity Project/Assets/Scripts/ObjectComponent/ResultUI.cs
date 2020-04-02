using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    // プレイヤーのリザルトUIのコイン用UI
    List<List<GameObject>> coinUIs = new List<List<GameObject>>();
    // ResultUIのPrefabs
    [SerializeField]
    GameObject resultUIPrefab = null;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// ResultUIを生成する関数
    /// </summary>
    public void CreateResultUI()
    {
        // プレイヤーの数によってオフセット
        float offsetX = (Screen.width - 384 * GameManager.Instance.playerNumber) 
            / (GameManager.Instance.playerNumber + 1);
        float resultOffsetX = -Screen.width / 2 + offsetX;
        // プレイヤーの数だけ作成
        for(int i=1;i<=GameManager.Instance.playerNumber;i++)
        {
            // UIを作成
            var resultUIObj = Resources.Load<GameObject>("Prefabs/PlayerResultUI");
            // 非表示にする
            resultUIObj.SetActive(false);
            // 座標を変更
            Vector3 resultUIPos = new Vector3(resultOffsetX, resultUIObj.transform.position.y, 0);
            resultUIObj.transform.position = resultUIPos;
            // オフセットをずらす
            resultOffsetX += (384 + offsetX);
            List<GameObject> coinUI = new List<GameObject>();
            for(int loop=1;loop<=3;loop++)
            {
                // コイン用UIをリストに格納
                GameObject coinUIObj = resultUIObj.transform.Find("Player_Coin" + loop.ToString()).gameObject;
                coinUI.Add(coinUIObj);
            }
            coinUIs.Add(coinUI);
        }
    }

    /// <summary>
    /// リザルトUIのコイン用UIのポジションをセットする関数
    /// </summary>
    public void SetPosition()
    {
    }


}
