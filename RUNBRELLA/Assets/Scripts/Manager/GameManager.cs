using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PLAYER_NO
{
    PLAYER1,
    PLAYER2,
    PLAYER3,
    PLAYER4,
}

public enum CONTROLLER_NO
{
    CONTROLLER1=1,
    CONTROLLER2=2,
    CONTROLLER3=3,
    CONTROLLER4=4,
}

public enum SLECT_MAP_MODE
{
    RANDOM,
    SELECT,
}


public class GameManager : MonoBehaviour
{

    public enum CHARTYPE
    {
        PlayerA,
        PlayerB,
        PlayerC,
        PlayerD,
    }

    public enum CHARATTACKTYPE
    {
        GUN,
        SWORD,
    }

    public struct PlayerResultInfo
    {
        public PLAYER_NO playerNo;
        public CHARTYPE charType;
    }

    #region シングルトンインスタンス

    // プロジェクト開始時にオブジェクトがなくても呼び出される
    [RuntimeInitializeOnLoadMethod]
    private static void CreateInstance()
    {
        // Resourcesからプレハブを読み込む
        var prefab = Resources.Load<GameObject>("GameManager");
        // プレハブの生成
        Instantiate(prefab);
    }

    /// <summary>
    /// このクラスのインスタンスを取得するプロパティーです。
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static GameManager instance = null;

    /// <summary>
    /// Start()の実行より先行して処理したい内容を記述します。
    /// </summary>
    void Awake()
    {
        // 初回作成時
        if (instance == null)
        {
            instance = this;
            // シーンをまたいで削除されないように設定
            DontDestroyOnLoad(gameObject);
            // セーブデータを読み込む
            //Load();
        }
        // 2個目以降の作成時
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    // 現在のレース数
    public int nowRaceNumber = 0;
    // プレイするレース数
    public int RaceNumber = 0;
    // 選んだキャラクター
    public Dictionary<PLAYER_NO, CHARTYPE> selectedChar = new Dictionary<PLAYER_NO, CHARTYPE>();
    // プレイヤーの人数
    public int playerNumber = 0;
    // キャラクターの種類
    public List<CHARTYPE> charType = new List<CHARTYPE>();
    // キャラクターの武器の種類
    public List<CHARATTACKTYPE> charAttackType = new List<CHARATTACKTYPE>();
    // プレイヤーナンバーをKeyにコントローラナンバーをValueにするディクショナリー
    public Dictionary<PLAYER_NO, CONTROLLER_NO> playerAndControllerDictionary = new Dictionary<PLAYER_NO, CONTROLLER_NO>();
    // 前回のプレイヤー順のID
    public List<PlayerResultInfo> playerResultInfos = new List<PlayerResultInfo>();
    // 各プレイヤーの勝利数
    public Dictionary<PLAYER_NO, int> playerWins = new Dictionary<PLAYER_NO, int>();
    // 選ばれるステージ
    public List<GameObject> ChooseStages = new List<GameObject>();
    // 選ばれたステージ
    public List<GameObject> ChoosedStages = new List<GameObject>();
    // 一位のキャラタイプ
    public CHARTYPE firstCharType;
    // プレイヤーのアウトラインのリスト
    public List<Material> playerOutlines = new List<Material>();
    // マップを選ぶモード
    public SLECT_MAP_MODE selectMapMode;
    // 選べるマップのリスト
    public List<GameObject> canChooseStage_Ishibashi = new List<GameObject>();
    public List<GameObject> canChooseStage_Mishima = new List<GameObject>();
    public List<GameObject> canChooseStage_Osio = new List<GameObject>();
    // 選ばれたステージ
    public GameObject choosedStage;
    
    private void Start()
    {
#if UNITY_EDITOR
        StartCoroutine(SheetToDictionary.Instance.SheetToText());
#endif
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }
    }

    

    /// <summary>
    /// プレイヤーナンバーをKeyにコントローラナンバーをValueにするディクショナリーの
    /// ValueからKeyを返す関数
    /// </summary>
    /// <param name="controllerNo">Value</param>
    /// <returns></returns>
    public PLAYER_NO ContorllerNoToPlayerNo( CONTROLLER_NO controllerNo)
    {
        // keyからvalueを取得
        var pair = playerAndControllerDictionary.FirstOrDefault(c => c.Value == controllerNo);
        var key = pair.Key;
        return key;
    }

    /// <summary>
    /// プレイヤーナンバーをKeyにコントローラナンバーをValueにするディクショナリーの
    /// KeyからValueを返す関数    /// </summary>
    /// <param name="playerNo"></param>
    /// <returns></returns>
    public CONTROLLER_NO PlayerNoToControllerNo(PLAYER_NO playerNo)
    {
        var value = playerAndControllerDictionary[playerNo];
        return value;
    }

}   
    
