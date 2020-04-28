using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NEWSMODE
{
    START,
    GOAL,
    WIN,
    DEAD,
    RAIN,
}

public class NewsUIManager: MonoBehaviour
{
    
    // 作成したNewsUIのリスト
    private List<GameObject> newsUIs = new List<GameObject>();
    // NewsUIの親オブジェクト
    [SerializeField]
    private GameObject newsUIParent = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// NewsUIを作成する関数
    /// </summary>
    public void Create()
    {
        // リソースからロード
        GameObject newsUIPrefab = Resources.Load<GameObject>("Prefabs/NewsUI");
        // 5個作成
        for(int i=0;i<5;i++)
        {
            // 作成
            GameObject newsUIObj = Instantiate(newsUIPrefab);
            // 親オブジェクトを設定
            newsUIObj.transform.SetParent(newsUIParent.transform);
            // 位置初期化
            newsUIObj.GetComponent<RectTransform>().localPosition = new Vector3(1338f, 486, 0);
            newsUIs.Add(newsUIObj);
        }
    }


    public void ShowNewsUI(NEWSMODE newsMode,GameObject player=null)
    {
        GameObject targetNewsUIObj = null;
        // どのNewsUIを使うか決める
        for(int i=0;i<newsUIs.Count;i++)
        {
            NewsUI newsUI = newsUIs[i].GetComponent<NewsUI>();
            // 使っていないなら
            if(!newsUI.isShowing)
            {
                targetNewsUIObj = newsUIs[i];
                break;
            }
        }
        // エラーチェック
        if(targetNewsUIObj==null)
        {
            Debug.Log("使えるNewsUIがありません");
            return;
        }
        var targetNewsUI = targetNewsUIObj.GetComponent<NewsUI>();
        // 表示するニュース演出の種類によって呼び出す関数を変える
        switch(newsMode)
        {
            case NEWSMODE.DEAD:
                targetNewsUI.ShowDeadPlyaerNews(player);
                break;
            case NEWSMODE.GOAL:
                targetNewsUI.ShowGoalNews();
                break;
            case NEWSMODE.RAIN:
                targetNewsUI.ShowRainNews();
                break;
            case NEWSMODE.START:
                targetNewsUI.ShowStartNews();
                break;
            case NEWSMODE.WIN:
                targetNewsUI.ShowWinNews();
                break;
        }
    }

}
