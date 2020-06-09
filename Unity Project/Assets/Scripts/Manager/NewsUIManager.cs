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
    [System.Serializable]
    struct NewsUIInfo
    {
        public NEWSMODE newsMode;
        public GameObject character;
    }
    // ニュース演出を実行するリスト
    [SerializeField]
    private List<NewsUIInfo> newsList = new List<NewsUIInfo>();
    GameObject targetNewsUIObj = null;
    NewsUIEntryState entryState;
    int showNewsUICount = 0;
    int moveNewsUICount = 0;

    // Start is called before the first frame update
    void Start()
    {
        idleState = gameObject.GetComponent<NewsUIIdleState>();
        entryState = gameObject.GetComponent<NewsUIEntryState>();
        for (int i = 0; i < 5; i++)
        {
            var newsUI = newsUIs[i].GetComponent<NewsUI>();
            // StateをIdleにする
            ChangeNewsUIState(idleState, newsUI.ID);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 表示するニュース演出があるなら
        if(newsList.Count>0)
        {
            List<NewsUI> useNewsUIs = new List<NewsUI>();
            // 今いくつのNewsUIが表示されているのかチェック
            for (int l = 0; l < newsUIs.Count; l++)
            {
                // 今表示しているNewsUIを格納するリスト
                NewsUI newsUI = newsUIs[l].GetComponent<NewsUI>();
                if (newsUI.nowState != (INewsUIState)idleState)
                {
                    showNewsUICount++;
                    useNewsUIs.Add(newsUI);
                }
                if(newsUI.isMoveingUnder)
                {
                    moveNewsUICount++;
                }
            }
            // ニュース演出が3つ以上出ていないかつ一つも動いていないなら
            if (showNewsUICount < 3&&moveNewsUICount==0)
            {
                // 今表示しているNewsUiを下に下げる
                for (int i = 0; i < useNewsUIs.Count; i++)
                {
                    useNewsUIs[i].MoveUnderInit();
                }
                // ニュース演出を表示
                ShowNewsUI(newsList[0].newsMode, newsList[0].character);
                // 表示したニュース演出の情報はリストから削除
                newsList.Remove(newsList[0]);
            }
            // チェックし終わるごとにカウントをリセット
            showNewsUICount = 0;
            moveNewsUICount = 0;
            useNewsUIs.Clear();

        }
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

    }


    /// <summary>
    /// 表示するニュース演出の情報をニュース演出表示用リストに追加するメソッド
    /// </summary>
    /// <param name="newsMode"></param>
    /// <param name="character"></param>
    public void EntryNewsUI(NEWSMODE newsMode, GameObject character=null)
    {
        NewsUIInfo newsUIInfo;
        newsUIInfo.newsMode = newsMode;
        newsUIInfo.character = character;
        newsList.Add(newsUIInfo);
    }

    /// <summary>
    /// ニュース演出をする際にどのNewsUIを使うのか
    /// を決定するメソッド
    /// </summary>
    /// <param name="newsMode"></param>
    /// <param name="character">プレイヤーが脱落したりゴールしたときに使うキャラクター情報</param>
    /// <returns></returns>
    private void ShowNewsUI(NEWSMODE newsMode, GameObject character)
    {
        {
            //GameObject targetNewsUIObj = null;
            //NewsUIEntryState entryState = gameObject.GetComponent<NewsUIEntryState>();
            //int showNewsUICount = 0;
            //while (true)
            //{
            //    List<NewsUI> useNewsUIs = new List<NewsUI>();
            //    var playerNo = PLAYER_NO.PLAYER1;
            //    if (character!=null)
            //    {
            //        playerNo = character.GetComponent<Character>().playerNO;
            //    }
            //    // 今いくつのNewsUIが表示されているのかチェック
            //    for (int l = 0; l < newsUIs.Count; l++)
            //    {
            //        // 今表示しているNewsUIを格納するリスト
            //        NewsUI newsUI = newsUIs[l].GetComponent<NewsUI>();
            //        if (newsUI.nowState != (INewsUIState)idleState)
            //        {
            //            showNewsUICount++;
            //            useNewsUIs.Add(newsUI);
            //        }
            //    }
            //    if (showNewsUICount < 3)
            //    {
            //        if(character!=null)
            //        {
            //            Debug.Log(playerNo + "が移動しようとしています");
            //        }
            //        else
            //        {
            //            Debug.Log(newsMode + "の移動をしようとしています");
            //        }
            //        // 今表示しているNewsUiを下に下げる
            //        for (int i=0;i<useNewsUIs.Count;i++)
            //        {
            //            Debug.Log(useNewsUIs[i].ID + "番目のニュース演出が移動します");
            //            useNewsUIs[i].MoveUnderInit();
            //        }
            //        if (character != null)
            //        {
            //            Debug.Log(playerNo + "が移動");
            //        }
            //        else
            //        {
            //            Debug.Log(newsMode + "の移動をしようとしています");
            //        }
            //        break;
            //    }
            //    // チェックし終わるごとにカウントをリセット
            //    showNewsUICount = 0;
            //    useNewsUIs.Clear();
            //    yield return null;
            //}
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
        }
            
        // エラーチェック
        if (targetNewsUIObj == null)
        {
            Debug.Log("使えるNewsUIがありません");
            return;
        }
        var targetNewsUI = targetNewsUIObj.GetComponent<NewsUI>();
        var targetNewsUIEntry = targetNewsUIObj.GetComponent<NewsUIEntry>();
        // 表示するニュース演出の種類によって呼び出す関数を変える
        switch (newsMode)
        {
            case NEWSMODE.WIN:
            case NEWSMODE.GOAL:
            case NEWSMODE.DEAD:
                // どのプレイヤーが死んだかも伝える
                targetNewsUIEntry.playerNo = character.GetComponent<Character>().playerNO;
                targetNewsUIEntry.newsMode = newsMode;
                break;
            case NEWSMODE.RAIN:
            case NEWSMODE.START:
                targetNewsUIEntry.newsMode = newsMode;
                break;
        }
        // EntryStateにチェンジ
        ChangeNewsUIState(entryState, targetNewsUI.ID);
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
