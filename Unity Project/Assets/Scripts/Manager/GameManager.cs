using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

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
    [SerializeField]
    public int nowRaceNumber = 0;
    // プレイするレース数
    [SerializeField]
    public int RaceNumber = 5;
    // ゲームのステージ
    [SerializeField]
    public GameObject[] stages = new GameObject[5];
    [SerializeField]
    // 選んだキャラクター
    public List<int> selectedChar = new List<int>();
    // プレイヤーの人数
    [SerializeField]
    public int playerNumber = 0;


}
