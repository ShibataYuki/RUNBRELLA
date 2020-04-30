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

    #region シングルトン
    // シングルトン
    private static NewsUIManager instance;
    public static NewsUIManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // 複数個作成しないようにする
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    #endregion

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
            newsUIObj.GetComponent<RectTransform>().localPosition = new Vector3(-1338f, 486, 0);
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
            // 使っているなら下にずらす
            else
            {
                float targetPosX = newsUIs[i].GetComponent<RectTransform>().localPosition.x;
                float targetPosY = newsUIs[i].GetComponent<RectTransform>().localPosition.y -
                    newsUIs[i].GetComponent<RectTransform>().sizeDelta.y;
                float targetPosZ = newsUIs[i].GetComponent<RectTransform>().localPosition.z;
                Vector3 pos = new Vector3(targetPosX, targetPosY, targetPosZ);
                StartCoroutine(newsUI.OnMove(0.5f, pos));
                newsUI.targetPos.y = newsUIs[i].GetComponent<RectTransform>().localPosition.y - 
                    newsUIs[i].GetComponent<RectTransform>().sizeDelta.y;
                
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

    public void ChangeNewsUIState(INewsUIState state, int ID)
    {
        NewsUI newsUI = newsUIs[ID].GetComponent<NewsUI>();
        if (newsUI.nowState != null)
        {
            // 現在のステートの終了処理を呼ぶ
            newsUI.nowState.Exit(ID);
        }
        // ステートを変更する
        newsUI.nowState = state;
        // 変更後の開始処理を呼ぶ
        newsUI.nowState.Entry(ID);

    }


}
