using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
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
    public Dictionary<CONTROLLER_NO, GameObject> playerObjects = new Dictionary<CONTROLLER_NO, GameObject>();
    // プレイヤーコンポーネントの実体を格納しているPlayerEntityData
    public PlayerEntityData playerEntityData;
    // ゲームがスタートしているかどうか
    public bool isStart;
    // 誰かがゴールしているか
    public bool isEnd;
    [SerializeField]
    AudioClip stageBGM = null;
    public int deadPlayerCount = 0;
    public int goalPlayerCount = 0;
    [SerializeField]
    // ゴールしたプレイヤーのリスト
    public List<GameObject> goalRunkOrder = new List<GameObject>();
    // 残り一人のプレイヤー
    GameObject survivorObj;
    // ゴールしたときに出るコイン
    [SerializeField]
    private GameObject goalCoinObj = null;
    // メインカメラ
    Camera mainCamera;
    GameObject flag;
    // 旗に触れたかどうか
    public bool isTouchFlag = false;
    // スタート時の音
    [SerializeField]
    private AudioClip startAudioClip = null;
    // 一位のプレイヤー
    public GameObject firstRunkPlayer;
    // 一位のプレイヤーのx座標
    public float firstRunkPlayerPosX = 0;
    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        StartCoroutine(Ready());
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        flag = GameObject.Find("Flag").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region コルーチン
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
        // newsUI作成
        UIManager.Instance.newsUIManager.Create();
        // ミニマップ初期化
        UIManager.Instance.minMapUI.Init();
        // 登場演出開始
        yield return StartCoroutine(TimelineController.Instance.StartRaceTimeline());
        // カウントダウン用SE再生
        for (int i = 0; i < GameManager.Instance.playerNumber; i++)
        {
            var value = GameManager.Instance.PlayerNoToControllerNo((PLAYER_NO)i);
            // Run状態にチェンジ
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState,value);
            // 各プレイヤーの移動をタイムラインからスクリプトでの記述に移行
            playerObjects[value].GetComponent<Animator>().applyRootMotion = false;
            // プレイヤーが画面外に出たかどうかのコンポーネントを追加
            playerObjects[value].AddComponent<PlayerCheckScreen>();
        }
        AudioManager.Instance.PlaySE(startAudioClip, 1f);
        AudioManager.Instance.PlayBGM(stageBGM, true, 0.1f);
        // スタートニュース演出開始
        UIManager.Instance.newsUIManager.ShowNewsUI(NEWSMODE.START);
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
            // リロード処理
            if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder,GamePad.Index.Any) || Input.GetKeyDown(KeyCode.R))
            {
                ResetStage();
                GameManager.Instance.nowRaceNumber = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnityEngine.Application.Quit();
            }
            // 一位をチェック
            CheckFirstRunkPlayer();
            // 参加プレイヤーが一人の場合はチェックしない
            if (GameManager.Instance.playerNumber >= 2)
            {
                // 残り一人になったら終了
                if (CheckSurvivor() == 1)
                {
                    // 残り一人をプレイヤーの順位順のリストに格納
                    goalRunkOrder.Insert(0, survivorObj);
                    // 全滅勝利時用ニュース演出開始
                    UIManager.Instance.newsUIManager.ShowNewsUI(NEWSMODE.WIN);
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
        // 全員がゴールするまで待機
        while(true)
        {
            if (GameManager.Instance.playerNumber <= goalRunkOrder.Count)
            {
                break;
            }
            yield return null;
        }
        // エンドフラグをONにする
        isEnd = true;
        // ゴールフラッグの位置まで行っていたならゴールコインの位置をゴールフラッグの位置にする
        if (isTouchFlag)
        {
            goalCoinObj.transform.position = mainCamera.WorldToScreenPoint(flag.transform.position);
        }
        // リザルトUIを表示
        UIManager.Instance.resultUI.ShowResultUI();
        // ゴールコインを表示
        goalCoinObj.SetActive(true);
        // リザルトコルーチン開始
        UIManager.Instance.resultUI.resultCoroutine = 
            StartCoroutine(UIManager.Instance.resultUI.OnResultUI());
        while (true)
        {
            if(!UIManager.Instance.resultUI.isEnd)
            {
                yield return null;
                continue;
                
            }
            // リロード処理
            if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder,GamePad.Index.Any) || Input.GetKeyDown(KeyCode.R))
            {
                ResetStage();
                GameManager.Instance.nowRaceNumber = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                yield break;
            }
            yield return null;
            // 進行検知
            if (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any) || Input.GetKeyDown(KeyCode.Return))
            {
                // 各プレイヤーの勝ち数を更新
                GameManager.Instance.playerWins[goalRunkOrder[0].GetComponent<Player>().playerNO] += 1;
                // もしいずれかのプレイヤーが規定回数の勝ち数になったらゲーム終了
                if (GameManager.Instance.playerWins[goalRunkOrder[0].GetComponent<Player>().playerNO]
                    >= GameManager.Instance.RaceNumber)
                {
                    // 勝者のキャラタイプを記録
                    GameManager.Instance.firstCharType = goalRunkOrder[0].GetComponent<Player>().charType;
                    // 勝者のプレイヤー番号
                    GameManager.Instance.firstPlayerNumber = (int)goalRunkOrder[0].GetComponent<Player>().playerNO + 1;
                    // 開放処理
                    ReleaseStage();
                    // 最終リザルトを更新
                    SceneManager.LoadScene("Result");
                    yield break;
                }
                // レース数を進める
                GameManager.Instance.nowRaceNumber++;
                // レースの結果をゲームマネージャーに格納
                for(int i=0;i<GameManager.Instance.playerNumber;i++)
                {
                    GameManager.Instance.playerRanks[i] = goalRunkOrder[i].GetComponent<Player>().playerNO;
                }
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            yield return null;
        }
    }

    #endregion

    #region 関数
    /// <summary>
    /// ステージを作成する関数
    /// </summary>
    void CreateStage()
    {
        // 乱数を生成
        int random = UnityEngine.Random.Range(0, GameManager.Instance.ChooseStages.Count);
        // GameManagerに登録されているステージを読み込み
        Instantiate(GameManager.Instance.ChooseStages[random]);
        // 使ったステージは使用済みのリストへ移動
        // 追加
        GameManager.Instance.ChoosedStages.Add(GameManager.Instance.ChooseStages[random]);
        // 削除
        GameManager.Instance.ChooseStages.Remove(GameManager.Instance.ChooseStages[random]);
    }


    /// <summary>
    /// プレイヤー作成する関数
    /// </summary>
    void CreatePlayer()
    {
        for (int i = 0; i < GameManager.Instance.playerNumber; i++)
        {
            // プレイヤーを作成
            GameObject playerPrefab;
            playerPrefab = Resources.Load<GameObject>("Prefabs/"+GameManager.Instance.charType[i].ToString());
            var playerObj = Instantiate(playerPrefab,new Vector3(-20,0,0), Quaternion.identity);
            // keyからValueを獲得
            var controllerNo = GameManager.Instance.PlayerNoToControllerNo((PLAYER_NO)i);
            // PlayersにプレイヤーのIDとGameObjectを格納
            playerObjects.Add(controllerNo, playerObj);
            // プレイヤーのスクリプト
            var player = playerObj.GetComponent<Player>();
            // プレイヤーナンバーを設定
            player.playerNO = (PLAYER_NO)i;
            // プレイヤーのコントローラナンバーを設定
            player.controllerNo = controllerNo;
            // プレイヤーの種類を設定
            player.charType = GameManager.Instance.charType[i];
            // プレイヤーの攻撃手段の種類を設定
            player.charAttackType = GameManager.Instance.charAttackType[i];
            // プレイヤーのアウトラインを設定
            Renderer playerRenderer = playerObj.GetComponent<SpriteRenderer>();
            playerRenderer.material = GameManager.Instance.playerOutlines[(int)player.playerNO];
            // プレイヤーのタイプをセット
            player.Type = player.charAttackType.ToString();
            // Stateを初期化
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerIdelState, player.controllerNo);
        }
        playerEntityData = new PlayerEntityData(GameManager.Instance.playerNumber);
        var firstControllerNo = GameManager.Instance.PlayerNoToControllerNo(GameManager.Instance.playerRanks[0]);
        // スタート時に一位のプレイヤーを格納
        firstRunkPlayer = playerObjects[firstControllerNo];
    }


    /// <summary>
    /// プレイヤーが何人生きているかを返す関数
    /// </summary>
    int CheckSurvivor()
    {
        int survivor = 0;
        GameObject obj = null;
        // プレイヤーの生存チェック
        for(int i=0;i<GameManager.Instance.playerNumber;i++)
        {
            var controllerNo = GameManager.Instance.PlayerNoToControllerNo((PLAYER_NO)i);
            if (playerObjects[controllerNo].activeInHierarchy)
            {
                obj = playerObjects[controllerNo];
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
    /// 一位のプレイヤーを調べる関数
    /// </summary>
    void CheckFirstRunkPlayer()
    {
        // 未設定なら0を代入
        if (firstRunkPlayer == null)
        {
            firstRunkPlayerPosX = 0;
        }
        // 一位のプレイヤーを更新
        for(int i=0;i<GameManager.Instance.playerNumber;i++)
        {
            var controllerNo = GameManager.Instance.PlayerNoToControllerNo((PLAYER_NO)i);
            // 死亡済みなら無視
            if(!playerObjects[controllerNo].activeInHierarchy)
            {
                continue;
            }
            if(playerObjects[controllerNo].transform.position.x>firstRunkPlayerPosX)
            {
                firstRunkPlayer = playerObjects[controllerNo];
                firstRunkPlayerPosX = firstRunkPlayer.transform.position.x;
            }
        }
    }

    /// <summary>
    /// エンドコルーチンを開始
    /// </summary>
    public void StartEnd(GameObject playerObject)
    {
        // すべてのコルーチンを停止
        StopAllCoroutines();
        // ゲーム中フラグをOFFにする
        isStart = false;
        // ゴールコインを一番手前のUIにする
        goalCoinObj.transform.SetSiblingIndex(goalCoinObj.transform.childCount - 1);
        // ゴールしたプレイヤーの状態をRunにチェンジ
        var player = playerObject.GetComponent<Player>();
        PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, player.controllerNo);
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


    /// <summary>
    /// リセット用開放処理
    /// </summary>
    private void ResetStage()
    {
        // 各プレイヤーの勝ち数をリセット
        for(int i=0;i<GameManager.Instance.playerWins.Count;i++)
        {
            GameManager.Instance.playerWins[(PLAYER_NO)i] = 0;
        }
        // 選ばれたステージを戻す
        for (int i = 0; i < GameManager.Instance.ChoosedStages.Count; i++)
        {
            // ステージを戻す
            GameManager.Instance.ChooseStages.Add(GameManager.Instance.ChoosedStages[i]);
        }
        // 使用済みステージを消去
        GameManager.Instance.ChoosedStages.Clear();
    }

    /// <summary>
    /// ゲーム終了時の開放処理
    /// </summary>
    private void ReleaseStage()
    {
        // 各プレイヤーの勝ち数をリセット
        GameManager.Instance.playerWins.Clear();
        // 前回の順位をリセット
        GameManager.Instance.playerRanks.Clear();
        // 選ばれたステージを戻す
        for(int i=0;i<GameManager.Instance.ChoosedStages.Count;i++)
        {
            // ステージを戻す
            GameManager.Instance.ChooseStages.Add(GameManager.Instance.ChoosedStages[i]);
        }
        // 使用済みステージを消去
        GameManager.Instance.ChoosedStages.Clear();
    }

    #endregion
}
