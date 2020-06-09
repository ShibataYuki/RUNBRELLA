using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainIconFactory : MonoBehaviour
{
    // 画面右上に保持しているオブジェクト
    GameObject keepObj = null;
    // レインアイコンを保持するリスト
    List<GameObject> rainIconObjList = new List<GameObject>();
    // アイコンをいくつプールしておくか
    [SerializeField]
    int HowManyRainIcon = 2;

    private void Awake()
    {
        // レインアイコン作成
        CreateRainIcon();
    }

    // Start is called before the first frame update
    void Start()
    {        
    }

    /// <summary>
    /// アイコンを始動させる処理
    /// </summary>
    /// <param name="cloudSwitchPos">雨の看板の位置</param>
    public void StartRainIcon(Vector3 cloudSwitchPos)
    {
        // 使用中でないアイコンを選ぶ
        var icon = ChooseNotActive();
        // アイコンをスタート位置に移動
        SetStartPos(cloudSwitchPos, icon);
        // アイコンをアクティブ化
        icon.SetActive(true);
    }

    /// <summary>
    /// 画面右上に保持するオブジェクトを変更
    /// </summary>
    /// <param name="target"></param>
    public void KeepObjChange(GameObject target)
    {
        // 保持オブジェクトを非アクティブ化
        KeepObjSetFalse();
        // 保持オブジェクト入れ替え
        keepObj = target;
    }

    /// <summary>
    /// 画面右上に保持しているオブジェクトを非アクティブに変更
    /// </summary>
    public void KeepObjSetFalse()
    {
        // すでにあるオブジェクトは非アクティブに変更
        if (keepObj != null)
        {
            keepObj.SetActive(false);
            keepObj = null;
        }
    }
    /// <summary>
    /// 子オブジェクトに雨アイコンを作成、リストに追加
    /// </summary>
    void CreateRainIcon()
    {
        // リソースからロード 
        GameObject rainIcon_Loaded = Resources.Load<GameObject>("Prefabs/RainIcon");
        for(int i = 0; i < HowManyRainIcon;i++)
        {
            // 実体作成
            var rainIcon = Instantiate(rainIcon_Loaded);
            // 子オブジェクトにセット
            rainIcon.transform.SetParent(transform);
            // リストに追加
            rainIconObjList.Add(rainIcon);
        }
        
    }

    /// <summary>
    /// 呼び出された看板の位置に移動する処理
    /// </summary>
    /// <param name="cloudSwichPos"></param>
    void SetStartPos(Vector3 cloudSwichPos, GameObject rainIcon)
    {
        var rectTransform = rainIcon.GetComponent<RectTransform>();
        // 看板の位置をスクリーン座標に変換
        var startPoint_Screen = Camera.main.WorldToScreenPoint(cloudSwichPos);
        // スタートポジションにセット
        rectTransform.position = startPoint_Screen;
    }   

    /// <summary>
    /// 使用中でないアイコンを選択
    /// </summary>
    /// <returns></returns>
    GameObject ChooseNotActive()
    {
        // リストの中から使用中でないものを検索して返す
        foreach(var icon in rainIconObjList)
        {
            if(icon.activeInHierarchy == false)
            {                
                return icon;
            }
        }

        Debug.Assert(false,"雨アイコンのプールが０です");
        return null;
    }
       
}
