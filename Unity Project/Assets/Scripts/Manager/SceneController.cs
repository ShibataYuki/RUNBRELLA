using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;

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
    // プレイヤーコンポーネントの実体を格納しているPlayerEntityData
    public PlayerEntityData playerEntityData;
    // プレイヤーのナンバーが格納されたディクショナリー
    public Dictionary<int, int> playerNumbers = new Dictionary<int, int>();
    // ゲームがスタートしているかどうか
    public bool isStart;
    // 誰かがゴールしているか
    public bool isEnd;
    [SerializeField]
    AudioClip stageBGM = null;
    [SerializeField]
    AudioClip countDownSE = null;
    public int deadPlayerCount = 0;
    public int goalPlayerCount = 0;
    [SerializeField]
    // ゴールしたプレイヤーのリスト
    public List<GameObject> goalRunkOrder = new List<GameObject>();
    // 残り一人のプレイヤー
    GameObject survivorObj;
    // ゴールしたときに出るコイン
    [SerializeField]
    GameObject goalCoinObj = null;
    // メインカメラ
    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Ready());
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
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
        // リザルトUI作成
        UIManager.Instance.resultUI.CreateResultUI();
        //yield return new WaitForSeconds(1);

        StartCoroutine(TimelineController.Instance.StartRaceTimeline());

        // カウントダウン用SE再生
        AudioManager.Instance.PlaySE(countDownSE, 1f);
        yield return StartCoroutine(UIManager.Instance.StartCountdown());
        for (int i = 1; i <= GameManager.Instance.playerNumber; i++)
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
            if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder,GamePad.Index.Any) || Input.GetKeyDown(KeyCode.R))
            {
                GameManager.Instance.nowRaceNumber = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnityEngine.Application.Quit();
            }

            // 参加プレイヤーが一人の場合はチェックしない
            if (GameManager.Instance.playerNumber >= 2)
            {
                // 残り一人になったら終了
                if (CheckSurvivor() == 1)
                {
                    // 残り一人をプレイヤーの順位順のリストに格納
                    goalRunkOrder.Insert(0, survivorObj);
                    // 終了処理開始
                    StartEnd(survivorObj);
                    yield break;
                }

            }
            yield return null;
        }
    }


    /// <summary>
    /// ゲーム終了時の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator End()
    {
        // エンドフラグをONにする
        isEnd = true;
        // リザルトUIを表示
        UIManager.Instance.resultUI.ShowResultUI();
        // ゴールコインを表示
        goalCoinObj.SetActive(true);
        // リザルトコルーチン開始
        yield return StartCoroutine(UIManager.Instance.resultUI.OnResultUI());
        while (true)
        {
            if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder,GamePad.Index.Any) || Input.GetKeyDown(KeyCode.R))
            {
                GameManager.Instance.nowRaceNumber = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                yield break;
            }
            if (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any) || Input.GetKeyDown(KeyCode.Return))
            {
                // 各プレイヤーの勝ち数を更新
                GameManager.Instance.playerWins[playerNumbers[goalRunkOrder[0].GetComponent<Player>().ID] - 1] += 1;
                // もしいずれかのプレイヤーが規定回数の勝ち数になったらゲーム終了
                if (GameManager.Instance.playerWins[playerNumbers[goalRunkOrder[0].GetComponent<Player>().ID] - 1]
                    >= GameManager.Instance.RaceNumber)
                {
                    SceneManager.LoadScene("Result");
                    yield break;
                }
                // レース数を進める
                GameManager.Instance.nowRaceNumber++;
                // レースの結果をゲームマネージャーに格納
                for(int i=0;i<GameManager.Instance.playerNumber;i++)
                {
                    GameManager.Instance.playerRanks[i] = goalRunkOrder[i].GetComponent<Player>().ID;
                }
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
        for (int ID = 1; ID <= GameManager.Instance.playerNumber; ID++)
        {
            // プレイヤーを作成
            GameObject playerPrefab;
            playerPrefab = Resources.Load<GameObject>("Prefabs/"+GameManager.Instance.charType[ID - 1].ToString());
            var playerObj = Instantiate(playerPrefab);
            // PlayersにプレイヤーのIDとGameObjectを格納
            playerObjects.Add(GameManager.Instance.playerIDs[ID - 1], playerObj);
            // playerNumbersにプレイヤーのIDをKeyにプレイヤーナンバーを格納
            playerNumbers.Add(GameManager.Instance.playerIDs[ID - 1], ID);
            // プレイヤーのスクリプト
            var player = playerObj.GetComponent<Player>();
            // プレイヤーのID設定
            player.ID = GameManager.Instance.playerIDs[ID - 1];
            // プレイヤーの種類を設定
            player.charType = GameManager.Instance.charType[ID - 1];
            // プレイヤーの攻撃手段の種類を設定
            player.charAttackType = GameManager.Instance.charAttackType[ID - 1];
            // プレイヤーのタイプをセット
            player.Type = player.charAttackType.ToString();
            // プレイヤーのIDをプレイヤーの子オブジェクトに渡す
            playerObjects[player.ID].transform.Find("PlayerInformation").GetComponent<PlayerInformation>().playerID = player.ID;
            // Stateを初期化
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerIdelState, player.ID);
        }
        playerEntityData = new PlayerEntityData(GameManager.Instance.playerNumber);
    }


    /// <summary>
    /// プレイヤーが何人生きているかを返す関数
    /// </summary>
    int CheckSurvivor()
    {
        int survivor = 0;
        GameObject obj = null;
        // プレイヤーの生存チェック
        for(int i=1;i<=GameManager.Instance.playerNumber;i++)
        {
            if(playerObjects[i].activeInHierarchy)
            {
                obj = playerObjects[i];
                survivor++;
            }
        }
        // 生存者が一人の場合は変数に格納
        if(survivor==1)
        {
            survivorObj = obj;
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
        // ゴールコインの位置を一位のプレイヤーの位置にする
        goalCoinObj.transform.position = mainCamera.WorldToScreenPoint(playerObject.transform.position);
        // ゴールコインを一番手前のUIにする
        goalCoinObj.transform.SetSiblingIndex(goalCoinObj.transform.childCount - 1);
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
