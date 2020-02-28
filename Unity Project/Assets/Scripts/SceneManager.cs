using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    #region シングルトン
    // シングルトン
    private static SceneManager instance;
    public static SceneManager Instance
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
    int playerCount = 0;

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
        CreatePlayer();
        yield return new WaitForSeconds(1);
        for (int i = 1; i <= playerCount; i++)
        {
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerRunState, i);
        }
        yield break;
    }


    /// <summary>
    /// ゲーム中の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator OnGame()
    {

        yield break;
    }


    /// <summary>
    /// ゲーム終了時の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator End()
    {
        yield break;
    }


    /// <summary>
    /// プレイヤー作成処理
    /// </summary>
    void CreatePlayer()
    {
        for (int ID = 1; ID <= playerCount; ID++)
        {
            // プレイヤーを作成
            var playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
            var player = Instantiate(playerPrefab);
            // PlayersにプレイヤーのIDとGameObjectを格納
            playerObjects.Add(ID, player);
            // プレイヤーのID設定
            playerObjects[ID].GetComponent<Player>().ID = ID;
            // Stateを初期化
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.Instance.playerIdelState, ID);
        }
        playerEntityData = new PlayerEntityData(playerCount);
    }

}
