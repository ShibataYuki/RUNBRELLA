using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public List<GameObject> newsUIs = new List<GameObject>();
    // NewsUIの親オブジェクト
    [SerializeField]
    private GameObject newsUIParent = null;
    NewsUIIdleState idleState;

    // Start is called before the first frame update
    void Start()
    {
        idleState = gameObject.GetComponent<NewsUIIdleState>();
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
            var newsUI = newsUIObj.GetComponent<NewsUI>();
            // IDを設定
            newsUI.ID = i;
            // 親オブジェクトを設定
            newsUIObj.transform.SetParent(newsUIParent.transform);
            // 位置初期化
            newsUIObj.GetComponent<RectTransform>().localPosition = new Vector3(-1338f, 486, 0);
            newsUIs.Add(newsUIObj);
        }
        for(int i=0;i<5;i++)
        {
            var newsUI = newsUIs[i].GetComponent<NewsUI>();
            // StateをIdleにする
            ChangeNewsUIState(idleState, newsUI.ID);
        }

    }


    public IEnumerator ShowNewsUI(NEWSMODE newsMode, GameObject player = null)
    {
        GameObject targetNewsUIObj = null;
        NewsUIEntryState entryState = gameObject.GetComponent<NewsUIEntryState>();
        int showNewsUICount = 0;
        while (true)
        {
            // 今いくつのNewsUIが表示されているのかチェック
            for (int i = 0; i < newsUIs.Count; i++)
            {
                NewsUI newsUI = newsUIs[i].GetComponent<NewsUI>();
                if (newsUI.nowState != (INewsUIState)idleState)
                {
                    showNewsUICount++;
                }
            }
            if (showNewsUICount < 3)
            {
                break;
            }
            showNewsUICount = 0;
            yield return null;
        }
        // どのNewsUIを使うか決める
        for (int i = 0; i < newsUIs.Count; i++)
        {
            NewsUI newsUI = newsUIs[i].GetComponent<NewsUI>();
            // 使っていないなら
            if (newsUI.nowState == (INewsUIState)idleState)
            {
                targetNewsUIObj = newsUIs[i];
                break;
            }
            // 使っているなら下にずらす
            else
            {
                newsUI.MoveUnderInit();
            }
        }
            
        // エラーチェック
        if (targetNewsUIObj == null)
        {
            Debug.Log("使えるNewsUIがありません");
            yield break;
        }
        var targetNewsUI = targetNewsUIObj.GetComponent<NewsUI>();
        var targetNewsUIEntry = targetNewsUIObj.GetComponent<NewsUIEntry>();
        // 表示するニュース演出の種類によって呼び出す関数を変える
        switch (newsMode)
        {
            case NEWSMODE.DEAD:
                // DEADの場合はどのプレイヤーが死んだかも伝える
                targetNewsUIEntry.playerNo = player.GetComponent<Player>().playerNO;
                targetNewsUIEntry.newsMode = newsMode;
                break;
            case NEWSMODE.GOAL:
            case NEWSMODE.RAIN:
            case NEWSMODE.START:
            case NEWSMODE.WIN:
                targetNewsUIEntry.newsMode = newsMode;
                break;
        }
        // EntryStateにチェンジ
        ChangeNewsUIState(entryState, targetNewsUI.ID);
    }


    //public void ShowNewsUI(NEWSMODE newsMode,GameObject player=null)
    //{
    //    GameObject targetNewsUIObj = null;
    //    NewsUIEntryState entryState = gameObject.GetComponent<NewsUIEntryState>();
    //    // どのNewsUIを使うか決める
    //    for(int i=0;i<newsUIs.Count;i++)
    //    {
    //        NewsUI newsUI = newsUIs[i].GetComponent<NewsUI>();
    //        // 使っていないなら
    //        if(newsUI.nowState==(INewsUIState)idleState)
    //        {
    //            targetNewsUIObj = newsUIs[i];
    //            break;
    //        }
    //        // 使っているなら下にずらす
    //        else
    //        {
    //            newsUI.MoveUnderInit();
    //        }
    //    }
    //    // エラーチェック
    //    if(targetNewsUIObj==null)
    //    {
    //        Debug.Log("使えるNewsUIがありません");
    //        return;
    //    }
    //    var targetNewsUI = targetNewsUIObj.GetComponent<NewsUI>();
    //    var targetNewsUIEntry = targetNewsUIObj.GetComponent<NewsUIEntry>();
    //    // 表示するニュース演出の種類によって呼び出す関数を変える
    //    switch(newsMode)
    //    {
    //        case NEWSMODE.DEAD:
    //            // DEADの場合はどのプレイヤーが死んだかも伝える
    //            targetNewsUIEntry.playerNo = player.GetComponent<Player>().playerNO;
    //            targetNewsUIEntry.newsMode = newsMode;
    //            break;
    //        case NEWSMODE.GOAL:
    //        case NEWSMODE.RAIN:
    //        case NEWSMODE.START:
    //        case NEWSMODE.WIN:
    //            targetNewsUIEntry.newsMode = newsMode;
    //            break;
    //    }
    //    // EntryStateにチェンジ
    //    ChangeNewsUIState(entryState, targetNewsUI.ID);
    //}

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
