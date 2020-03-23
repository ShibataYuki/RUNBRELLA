using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region シングルトン
    // シングルトン
    private static SceneController instance;
    public static SceneController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // 複数個作成しないようにする
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }


    }


    #endregion


    // プレイヤーのGameObjectを格納するディクショナリー
    public Dictionary<int, GameObject> playerObjects = new Dictionary<int, GameObject>();
    // 各プレイヤーのコンポーネントの実体が格納されたディクショナリー
    public PlayerEntityData playerEntityData;

    // プレイヤーの人数
    [SerializeField]
    public int playerCount = 0;
    // ゲームがスタートしているかどうか
    public bool isStart;
    // 誰かがゴールしているか
    public bool isGoal;
    // 終了していいか
    public bool isEnd = false;
    [SerializeField]
    AudioClip stageBGM = null;
    [SerializeField]
    AudioClip countDownSE = null;
    public int deadPlayerCount = 0;
    public int goalPlayerCount = 0;
    [SerializeField]
    // ゴールしたプレイヤーの配列
    public List<GameObject> goalRunkOrder = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Ready());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// ゲーム開始前の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Ready()
    {
        // プレイヤー作成
        CreatePlayer();
        // ステージ作成
        CreateStage();
        yield return new WaitForSeconds(1);

        // カウントダウン用SE再生
        AudioManager.Instance.PlaySE(countDownSE, 1f);
        yield return StartCoroutine(UIManager.Instance.StartCountdown());
        for (int i = 1; i <= playerCount; i++)
        {
            // Run状態にチェンジ
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, i);
            // プレイヤーが画面外に出たかどうかのコンポーネントを追加
            playerObjects[i].AddComponent<PlayerCheckScreen>();
        }
        AudioManager.Instance.PlayBGM(stageBGM, true, 0.2f);
        // ゲーム開始フラグをtrueにする
        isStart = true;
        StartCoroutine(OnGame());
        yield break;
    }


    /// <summary>
    /// ゲーム中の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator OnGame()
    {
        while(true)
        {
            if(Input.GetButtonDown("player1_Restart") || Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnityEngine.Application.Quit();
            }

            // 残り一人になったら終了
            //if(CheckSurvivor()==1)
            //{
            //    StartEnd();
            //    yield break;
            //}

            yield return null;
        }
    }


    /// <summary>
    /// ゲーム終了時の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator End()
    {
        // リザルトシーンを読み込む
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // ゴールフラグをON
        isGoal = true;
        while (true)
        {
            // すべてのプレイヤーが画面外に行ったかチェック
            if (CheckSurvivor() < 1)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(UIManager.Instance.resultUI.OnResult());
        while(true)
        {
            // キー入力をがあったらリザルトの終了アニメーションを開始
            if(Input.GetButtonDown("player1_jump"))
            {
                UIManager.Instance.resultUI.StartEndResultAnimation();
            }
            if(isEnd)
            {
                // 現在のステージを進める
                GameManager.Instance.nowRaceNumber++;
                // 3ステージ目ならゲーム終了
                if(GameManager.Instance.nowRaceNumber>=GameManager.Instance.RaceNumber)
                {
                    UnityEngine.Application.Quit();
                }
                // 次のステージへ
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            yield return null;
        }

    }


    /// <summary>
    /// ステージを作成する関数
    /// </summary>
    void CreateStage()
    {
        // GameManagerに登録されているステージを読み込み
        Instantiate(GameManager.Instance.stages[GameManager.Instance.nowRaceNumber]);
    }


    /// <summary>
    /// プレイヤー作成する関数
    /// </summary>
    void CreatePlayer()
    {
        for (int ID = 1; ID <= playerCount; ID++)
        {
            // プレイヤーを作成
            GameObject playerPrefab;
            playerPrefab = Resources.Load<GameObject>("Prefabs/"+GameManager.Instance.charType[ID - 1].ToString());
            var player = Instantiate(playerPrefab);
            // PlayersにプレイヤーのIDとGameObjectを格納
            playerObjects.Add(ID, player);
            // プレイヤーのID設定
            playerObjects[ID].GetComponent<Player>().ID = ID;
            // プレイヤーの種類を設定
            player.GetComponent<Player>().charType = GameManager.Instance.charType[ID - 1];
            // プレイヤーの攻撃手段の種類を設定
            player.GetComponent<Player>().charAttackType = GameManager.Instance.charAttackType[ID - 1];
            // プレイヤーのIDをプレイヤーの子オブジェクトに渡す
            playerObjects[ID].transform.Find("PlayerInformation").GetComponent<PlayerInformation>().playerID = ID;
            // Stateを初期化
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerIdelState, ID);
        }
        playerEntityData = new PlayerEntityData(playerCount);
    }


    /// <summary>
    /// プレイヤーが何人生きているかを返す関数
    /// </summary>
    int CheckSurvivor()
    {
        int survivor = 0;

        // プレイヤーの生存チェック
        for(int i=1;i<=playerCount;i++)
        {
            if(playerObjects[i].activeInHierarchy)
            {
                survivor++;
            }
        }

        return survivor;
    }


    /// <summary>
    /// エンドコルーチンを開始
    /// </summary>
    public void StartEnd(GameObject playerObject)
    {
        // すべてのコルーチンを停止
        StopAllCoroutines();
        // ゴールしたプレイヤーの状態をRunにチェンジ
        var player = playerObject.GetComponent<Player>();
        PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, player.ID);
        // 終了処理コルーチンを開始
        StartCoroutine(End());
    }


    /// <summary>
    /// 死んだプレイヤーをリストに格納する関数
    /// </summary>
    public void InsertDeadPlayer(GameObject player)
    {
        // 同じプレイヤーがいるなら格納しない
        for(int i=0;i<goalRunkOrder.Count;i++)
        {
            if(goalRunkOrder[i]==player)
            {
                return;
            }
        }

        goalRunkOrder.Insert(goalRunkOrder.Count - deadPlayerCount, player);
        // 死んだプレイヤーのカウントを増やす
        deadPlayerCount++;
    }


    /// <summary>
    /// ゴールしたプレイヤーをリストに格納する関数
    /// </summary>
    public void InsertGoalPlayer(GameObject player)
    {
        // 同じプレイヤーがいるなら格納しない
        for (int i = 0; i < goalRunkOrder.Count; i++)
        {
            if (goalRunkOrder[i] == player)
            {
                return;
            }
        }
        goalRunkOrder.Insert(goalPlayerCount, player);
        // ゴールカウントを増やす
        goalPlayerCount++;
    }


}
