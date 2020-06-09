using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEffectFactory : MonoBehaviour
{

    // 作成するゲームオブジェクト
    [SerializeField]
    GameObject ringEffectPrefab = null;
    // 作成する数
    [SerializeField]
    int ringEffectMax = 0;
    // リングを通ったときにでるエフェクトの数
    [SerializeField]
    int ringEffectCount = 0;
    // 作成したリングエフェクトの親
    [SerializeField]
    GameObject ringEffectParent = null;
    // 作成したリングエフェクトのリスト
    private List<GameObject> ringEffects = new List<GameObject>();
    // エフェクトのフラグ
    [SerializeField]
    public bool isTextMesh = false;
    [SerializeField]
    public bool isTracking = false;

    private void Awake()
    {
        // 作成
        CreateRingEffect();
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    /// <summary>
    /// リングエフェクトを作成する関数
    /// </summary>
    private void CreateRingEffect()
    {
        if(!isTracking)
        {
            return;
        }
        for(int i=0;i<ringEffectMax;i++)
        {
            // リングエフェクトを作成
            var ringEffect = Instantiate(ringEffectPrefab);
            // 最初は消す
            ringEffect.SetActive(false);
            // 親オブジェクトを設定
            ringEffect.transform.parent = ringEffectParent.transform;
            ringEffect.GetComponent<RingEffect>().id = i;
            // リストに格納
            ringEffects.Add(ringEffect);
        }
    }


    /// <summary>
    /// エフェクトを開始する関数
    /// </summary>
    /// <param name="playerNo">リングを通ったプレイヤーのID</param>
    /// <param name="ringObj">通ったリング</param>
    public void ShowRingEffect(PLAYER_NO playerNo,GameObject ringObj)
    {
        // 使うエフェクトの数だけループ
        for(int l=0;l<ringEffectCount;l++)
        {
            // 使っていないエフェクトの数だけループ
            for (int i = 0; i < ringEffectMax; i++)
            {
                // エフェクトをみつけたかどうかの関数
                bool isFound = false;
                // まだ使ってないエフェクトがあるなら
                if (!ringEffects[i].activeInHierarchy)
                {
                    // エフェクトを表示
                    ringEffects[i].SetActive(true);
                    // エフェクトを発射
                    StartCoroutine(ringEffects[i].GetComponent<RingEffect>().MoveImpulse(playerNo, ringObj));
                    // 見つけたかどうかのフラグをONにする
                    isFound = true;
                }
                if (isFound)
                {
                    break;
                }
            }
        }
    }


    /// <summary>
    /// リングエフェクトをプールに戻す関数
    /// </summary>
    /// <param name="ringEffectObj">プールに戻すリングエフェクト</param>
    public void ReturnRingEffect(GameObject ringEffectObj)
    {
        // 表示をOFF
        ringEffectObj.SetActive(false);
        // 位置初期化
        ringEffectObj.transform.position = new Vector3(0, 0, 0);
    }

}
