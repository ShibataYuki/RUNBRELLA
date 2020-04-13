using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<int> selectedChar = new List<int>();
    // プレイヤーの人数
    public int playerNumber = 0;
    // キャラクターの種類
    public List<CHARTYPE> charType = new List<CHARTYPE>();
    // キャラクターの武器の種類
    public List<CHARATTACKTYPE> charAttackType = new List<CHARATTACKTYPE>();
    // プレイヤーのIDを格納するリスト
    public List<int> playerIDs = new List<int>();
    // 前回のプレイヤー順のID
    public List<int> playerRanks = new List<int>();
    // 各プレイヤーの勝利数
    public List<int> playerWins = new List<int>();
    // 選ばれるステージ
    public List<GameObject> ChooseStages = new List<GameObject>();
    // 選ばれたステージ
    public List<GameObject> ChoosedStages = new List<GameObject>();

}
