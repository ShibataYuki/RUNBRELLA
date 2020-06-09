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
        // ステージ作成
        CreateStage();
        // プレイヤー作成
        CreatePlayer();
    }


    #endregion


    // プレイヤーのGameObjectを格納するディクショナリー
    public Dictionary<PLAYER_NO, GameObject> playerObjects = new Dictionary<PLAYER_NO, GameObject>();
    // 各プレイヤーのプレイヤーコンポーネント
    public Dictionary<PLAYER_NO, Character> players = new Dictionary<PLAYER_NO, Character>();
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

    // プレイヤーのリスポーン地点
    private readonly int playerOffsetX = -25;
    private readonly int playerOffsetY = 20;

    // プレイヤーがリスポーンする地面からのオフセット
    [SerializeField]
    private float playerRespawnOffsetY = 0;
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
        // ミニマップ初期化
        UIManager.Instance.minMapUI.Init();
        // フェードイン
        yield return StartCoroutine(UIManager.Instance.StartFade(FADEMODE.FADEIN));
        // カメラ移動コルーチン開始
        yield return StartCoroutine(CameraManager.Instance.MoveCameraProduction());
        //for (int i = 0; i < GameManager.Instance.playerNumber; i++)
        //{
        //    // 重力を戻す
        //    var controllerNo = GameManager.Instance.PlayerNoToControllerNo((PLAYER_NO)i);
        //    playerObjects[controllerNo].GetComponent<Rigidbody2D>().gravityScale = 1;           
        //}
        // 登場演出開始
        yield return StartCoroutine(TimelineController.Instance.StartRaceTimeline());
        // ミニリザルトUIを非表示にする
        UIManager.Instance.minResultUI.HideMinResultUI();
        // カウントダウン用SE再生
        for (int i = 0; i < GameManager.Instance.playerNumber; i++)
        {
            var playerNo = (PLAYER_NO)i;
            // Velocityいったん0に戻す
            playerObjects[playerNo].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            // Run状態にチェンジ
            players[playerNo].RunStart();
            // 各プレイヤーの移動をタイムラインからスクリプトでの記述に移行
            playerObjects[playerNo].GetComponent<Animator>().applyRootMotion = false;
            // プレイヤーが画面外に出たかどうかのコンポーネントを追加
            playerObjects[playerNo].AddComponent<PlayerCheckScreen>();
        }
        AudioManager.Instance.PlaySE(startAudioClip, 1f);
        AudioManager.Instance.PlayBGM(stageBGM, true, 0.1f);
        // スタートニュース演出開始
        UIManager.Instance.newsUIManager.EntryNewsUI(NEWSMODE.START);
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
#if UNITY_EDITOR
            // リロード処理
            if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder,GamePad.Index.Any) || Input.GetKeyDown(KeyCode.R))
            {
                ResetStage();
                GameManager.Instance.nowRaceNumber = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
#endif
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
                    UIManager.Instance.newsUIManager.EntryNewsUI(NEWSMODE.WIN,survivorObj);
                    // ゴール用紙吹雪の演出
                    var poopers = Camera.main.transform.Find("Poppers").GetComponent<Poppers>();
                    poopers.PlayPoperEffect();
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
            if (!UIManager.Instance.resultUI.isEnd)
            {
                yield return null;
                continue;
            }
            else
            {
                break;
            }
        }
        // 同フレーム内でキー入力しないように１フレームまつ
        yield return null;
        while (true)
        { 
            // リロード処理
            if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder,GamePad.Index.Any) || Input.GetKeyDown(KeyCode.R))
            {
                ResetStage();
                GameManager.Instance.nowRaceNumber = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                yield break;
            }
            // 進行検知
            if (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any) || Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("キー入力終わった");
                // 各プレイヤーの勝ち数を更新
                GameManager.Instance.playerWins[goalRunkOrder[0].GetComponent<Character>().playerNO] += 1;
                // レースの結果をゲームマネージャーに格納
                for (int i = 0; i < GameManager.Instance.playerNumber; i++)
                {
                    var playerinfo = GameManager.Instance.playerResultInfos[i];
                    playerinfo.playerNo = goalRunkOrder[i].GetComponent<Character>().playerNO;
                    playerinfo.charType = goalRunkOrder[i].GetComponent<Character>().charType;
                    GameManager.Instance.playerResultInfos[i] = playerinfo;
                }
                // もしいずれかのプレイヤーが規定回数の勝ち数になったらゲーム終了
                if (GameManager.Instance.playerWins[goalRunkOrder[0].GetComponent<Character>().playerNO]
                    >= GameManager.Instance.RaceNumber)
                {
                    // 勝者のキャラタイプを記録
                    GameManager.Instance.firstCharType = goalRunkOrder[0].GetComponent<Character>().charType;
                    // 勝者のプレイヤー番号
                    // GameManager.Instance.playerResultInfos[0].playerNo = (int)goalRunkOrder[0].GetComponent<Character>().playerNO + 1;
                    // 開放処理
                    ReleaseStage();
                    // 最終リザルトを更新
                    SceneManager.LoadScene("Result");
                    yield break;
                }
                // レース数を進める
                GameManager.Instance.nowRaceNumber++;
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
        if(GameManager.Instance.selectMapMode==SLECT_MAP_MODE.RANDOM)
        {
            // 乱数を生成
            int selectindex = UnityEngine.Random.Range(0, GameManager.Instance.ChooseStages.Count);
            // GameManagerに登録されているステージを読み込み
            Instantiate(GameManager.Instance.ChooseStages[selectindex]);
            // ステージ数が最長バトル数以上あるなら使い終わったステージは使わない
            if (GameManager.Instance.ChooseStages.Count+GameManager.Instance.ChoosedStages.Count>=
                (GameManager.Instance.playerNumber*(GameManager.Instance.RaceNumber-1)+1))
            {
                // 使ったステージは使用済みのリストへ移動
                // 追加
                GameManager.Instance.ChoosedStages.Add(GameManager.Instance.ChooseStages[selectindex]);
                // 削除
                GameManager.Instance.ChooseStages.Remove(GameManager.Instance.ChooseStages[selectindex]);
            }
        }
        else
        {
            Instantiate(GameManager.Instance.choosedStage);
        }
    }


    /// <summary>
    /// プレイヤー作成する関数
    /// </summary>
    void CreatePlayer()
    {

        var respawnPos = CheckPlayerRespawn();

        for (int i = 0; i < GameManager.Instance.playerNumber; i++)
        {
            // プレイヤーを作成
            GameObject playerPrefab;
            playerPrefab = Resources.Load<GameObject>("Prefabs/"+GameManager.Instance.charType[i].ToString());
            var playerObj = Instantiate(playerPrefab,respawnPos, Quaternion.identity);
            // keyからValueを獲得
            var controllerNo = GameManager.Instance.PlayerNoToControllerNo((PLAYER_NO)i);
            // PlayersにプレイヤーのIDとGameObjectを格納
            playerObjects.Add((PLAYER_NO)i, playerObj);
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
            var character = playerObj.GetComponent<Character>();
            var playerNo = character.playerNO;
            players.Add(playerNo, character);
            // Timelineで動かすため作成時は重力を0にする
            //var playerRigidBody = playerObj.GetComponent<Rigidbody2D>();
            //playerRigidBody.gravityScale = 0;
            //Stateを初期化
            player.IdleStart();
        }
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
            var playerNo = (PLAYER_NO)i;
            if (playerObjects[playerNo].activeInHierarchy)
            {
                obj = playerObjects[playerNo];
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
            var playerNo = (PLAYER_NO)i;
            // 死亡済みなら無視
            if(!playerObjects[playerNo].activeInHierarchy)
            {
                continue;
            }
            if(playerObjects[playerNo].transform.position.x>firstRunkPlayerPosX)
            {
                firstRunkPlayer = playerObjects[playerNo];
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
        // 終了処理コルーチンを開始
        StartCoroutine(End());
    }


    /// <summary>
    /// 死んだプレイヤーをリストに格納する関数
    /// </summary>
    public void InsertDeadPlayer(GameObject character)
    {
        // 同じプレイヤーがいるなら格納しない
        for(int i=0;i<goalRunkOrder.Count;i++)
        {
            if(goalRunkOrder[i]==character)
            {
                return;
            }
        }
        goalRunkOrder.Insert(goalRunkOrder.Count - deadPlayerCount, character);
        // 死んだプレイヤーのカウントを増やす
        deadPlayerCount++;
    }


    /// <summary>
    /// ゴールしたプレイヤーをリストに格納する関数
    /// </summary>
    public void InsertGoalPlayer(GameObject character)
    {
        // 同じプレイヤーがいるなら格納しない
        for (int i = 0; i < goalRunkOrder.Count; i++)
        {
            if (goalRunkOrder[i] == character)
            {
                return;
            }
        }
        goalRunkOrder.Insert(goalPlayerCount, character);
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
        // 選ばれたステージを戻す
        for(int i=0;i<GameManager.Instance.ChoosedStages.Count;i++)
        {
            // ステージを戻す
            GameManager.Instance.ChooseStages.Add(GameManager.Instance.ChoosedStages[i]);
        }
        // 使用済みステージを消去
        GameManager.Instance.ChoosedStages.Clear();
    }


    private Vector3 CheckPlayerRespawn()
    {
        // ステージへとばすRayを作成
        Ray ray = new Ray(new Vector3(playerOffsetX, playerOffsetY, 0), Vector3.down);
        // rayがヒットしたコライダーの情報
        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 50.0f);
        // rayが衝突したかどうか
        if (hit)
        {
            var respawnPos = new Vector3(playerOffsetX, hit.point.y + playerRespawnOffsetY, 0);
            return respawnPos;
        }
        else
        {
            Debug.Log("リスポーンする地面が見つかりませんでした");
        }
        return Vector3.zero;

    }

    #endregion
}
