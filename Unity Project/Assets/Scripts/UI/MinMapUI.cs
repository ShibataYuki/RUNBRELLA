using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinMapUI : MonoBehaviour
{
    
    public enum SHOWMODE
    {
        ALLPLAYER,
        FIRSTPLAYER,
        CROWN,
    }

    // ミニプレイヤーのPrefab
    [SerializeField]
    private GameObject minPlayerPrefab = null;
    // ミニプレイヤーの親オブジェクト
    [SerializeField]
    private GameObject minPlayerParent = null;
    // スタート地点にあるミニ信号機
    [SerializeField]
    private GameObject minTrafficLight = null;
    // ゴール地点にあるミニゴールフラッグ
    [SerializeField]
    private GameObject minGoalFlag = null;
    // ミニプレイヤーのスプライト
    [SerializeField]
    private List<Sprite> minPlayerSprite = new List<Sprite>();
    // ステージ上にあるゴールフラッグ
    private GameObject GoalFlag;
    // 生成したミニプレイヤーのリスト
    private List<GameObject> minPlayers = new List<GameObject>();
    // ミニプレイヤーのマテリアルのリスト
    [SerializeField]
    private List<Material> minPlayerMaterials = new List<Material>();
    // ステージとミニマップの比率
    float distanceRatio = 0;
    // ミニプレイヤー死亡時のスプライトのリスト
    [SerializeField]
    private List<Sprite> minDeadPlayerSprites = new List<Sprite>();
    // プレイヤーが死亡したかどうか
    private List<bool> isDeads = new List<bool>();
    // 表示モード
    [SerializeField]
    private SHOWMODE showMode = SHOWMODE.ALLPLAYER;
    // ミニ王冠のプレファブ
    [SerializeField]
    private GameObject minCrownPrefab = null;
    // ミニ王冠のオブジェクト
    private GameObject minCrownObj;
    // 一位のプレイヤーのオブジェクト
    private GameObject minFirstPlayerObj;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // ゲーム中なら
        if(!SceneController.Instance.isEnd)
        {
            if(SceneController.Instance.isStart)
            {
                switch (showMode)
                {
                    case SHOWMODE.ALLPLAYER:
                        // ミニプレイヤーの座標を更新
                        UpdateMinPlayer();
                        break;
                    case SHOWMODE.CROWN:
                        // ミニ王冠の座標を更新
                        UpdateMinPlayer();
                        break;
                    case SHOWMODE.FIRSTPLAYER:
                        UpdateMinFirstPlayer();
                        break;
                }
            }
        }
    }


    /// <summary>
    /// 初期化処理をする関数
    /// </summary>
    public void Init()
    {
        switch(showMode)
        {
            case SHOWMODE.ALLPLAYER:
                CreateMinPlayer();
                break;
            case SHOWMODE.CROWN:
                CreateMinCrown();
                break;
            case SHOWMODE.FIRSTPLAYER:
                CreateMinFirstPlayer();
                break;
        }
        // ゴールフラッグを初期化
        GoalFlag = GameObject.Find("Flag").gameObject;
        // ステージでのスタートからゴールまでの距離
        float stageDistance = GoalFlag.transform.position.x;
        // ミニマップでのスタートからゴールまでの距離
        float minMapDistance = minGoalFlag.GetComponent<RectTransform>().position.x -
            minTrafficLight.GetComponent<RectTransform>().position.x;
        // ステージとミニマップの比率を計算
        distanceRatio = minMapDistance / stageDistance;
    }


    /// <summary>
    /// ミニプレイヤーを生成する関数
    /// </summary>
    private void CreateMinPlayer()
    {
        for(int i=0;i<GameManager.Instance.playerNumber;i++)
        {
            // ミニプレイヤーを生成
            minPlayerPrefab = Resources.Load<GameObject>("Prefabs/MinPlayerUI");
            var minPlayerObj = Instantiate(minPlayerPrefab);
            // 親オブジェクトを設定
            minPlayerObj.transform.SetParent(minPlayerParent.transform);
            // ミニプレイヤーのスプライトを設定
            var minPlayerImage = minPlayerObj.GetComponent<Image>();
            // KeyからValueを取得
            var value = (PLAYER_NO)i;
            minPlayerImage.sprite = minPlayerSprite
                [(int)SceneController.Instance.playerObjects[value].GetComponent<Character>().charType];
            // ミニプレイヤーのマテリアルを設定
            minPlayerImage.material = minPlayerMaterials[i];
            // ミニプレイヤーをリストに格納
            minPlayers.Add(minPlayerObj);
            // ミニプレイヤーの死亡フラグをリストに格納
            isDeads.Add(false);
            // ミニプレイヤーを初期位置へ移動
            minPlayerObj.GetComponent<RectTransform>().position = 
                minTrafficLight.GetComponent<RectTransform>().position;
            // ミニプレイヤーを透明にする
            var color = minPlayerImage.color;
            color.a = 0f;
            minPlayerImage.color = color;
        }
    }
    
    /// <summary>
    /// ミニ王冠を生成する関数
    /// </summary>
    private void CreateMinCrown()
    {
        // ミニプレイヤーを生成
        minCrownPrefab = Resources.Load<GameObject>("Prefabs/MinCrownUI");
        minCrownObj = Instantiate(minCrownPrefab);
        // 親オブジェクトを設定
        minCrownObj.transform.SetParent(minPlayerParent.transform);
        // ミニ王冠を初期位置へ移動
        minCrownObj.GetComponent<RectTransform>().position =
            minTrafficLight.GetComponent<RectTransform>().position;
    }

    /// <summary>
    /// ミニ一位を生成する関数
    /// </summary>
    private void CreateMinFirstPlayer()
    {
        // ミニプレイヤーを生成
        minPlayerPrefab = Resources.Load<GameObject>("Prefabs/MinPlayerUI");
        minFirstPlayerObj = Instantiate(minPlayerPrefab);
        // 親オブジェクトを設定
        minFirstPlayerObj.transform.SetParent(minPlayerParent.transform);
        // ミニプレイヤーのスプライトを設定
        var minPlayerImage = minFirstPlayerObj.GetComponent<Image>();
        minPlayerImage.sprite = 
            minPlayerSprite[(int)SceneController.Instance.firstRunkPlayer.GetComponent<Character>().charType];
        // ミニプレイヤーのマテリアルを設定
        minPlayerImage.material = SceneController.Instance.firstRunkPlayer.GetComponent<SpriteRenderer>().material;
        // ミニプレイヤーを初期位置へ移動
        minFirstPlayerObj.GetComponent<RectTransform>().position =
            minTrafficLight.GetComponent<RectTransform>().position;

    }

    /// <summary>
    /// プレイヤーの位置からミニプレイヤーの位置を求める関数
    /// </summary>
    private void UpdateMinPlayer()
    {
        for(int i=0;i<minPlayers.Count;i++)
        {
            var minPlayerImage = minPlayers[i].GetComponent<Image>();
            // 透明なら透明じゃなくする
            if (minPlayerImage.color.a <= 0)
            {
                var color = minPlayerImage.color;
                color.a = 1f;
                minPlayerImage.color = color;
            }
            // 死亡フラグチェック
            if (isDeads[i])
            {
                continue;
            }
            // KeyからValueを取得
            var value = (PLAYER_NO)i;
            // 生存チェック
            if (!SceneController.Instance.playerObjects[value].activeInHierarchy)
            {
                // ミニプレイヤーのスプライトを死亡用スプライトに切り替え
                minPlayers[i].GetComponent<Image>().sprite = minDeadPlayerSprites[i];
                // 死亡フラグをONにする
                isDeads[i] = true;
                continue;
            }
            // KeyからValueを取得
            var Value = GameManager.Instance.PlayerNoToControllerNo((PLAYER_NO)i);
            // プレイヤーの位置を変数に代入
            float playerPosX = SceneController.Instance.playerObjects[value].transform.position.x;
            // プレイヤーの位置からミニプレイヤーの位置を更新
            minPlayers[i].GetComponent<RectTransform>().position = 
                new Vector3(
                (playerPosX * distanceRatio) + minTrafficLight.GetComponent<RectTransform>().position.x, 
                minPlayers[i].GetComponent<RectTransform>().position.y, 
                minPlayers[i].GetComponent<RectTransform>().position.z);
        }
    }

    /// <summary>
    /// 一位のプレイヤーの位置からミニ王冠の位置を求める関数
    /// </summary>
    private void UpdateMinCrown()
    {
        float playerPosX = 0;
        if(SceneController.Instance.firstRunkPlayer!=null)
        {
            // 一位のプレイヤーの位置を変数に代入
            playerPosX = SceneController.Instance.firstRunkPlayer.transform.position.x;
        }
        // 一位のプレイヤーの位置からミニプレイヤーの位置を更新
        minCrownObj.GetComponent<RectTransform>().position =
            new Vector3(
            (playerPosX * distanceRatio) + minTrafficLight.GetComponent<RectTransform>().position.x,
            minCrownObj.GetComponent<RectTransform>().position.y,
            minCrownObj.GetComponent<RectTransform>().position.z);
    }


    /// <summary>
    /// 一位のプレイヤーかあミニ一位プレイヤーの位置を求める関数
    /// </summary>
    private void UpdateMinFirstPlayer()
    {
        float playerPosX = 0;
        var minFirstPlayerImage = minFirstPlayerObj.GetComponent<Image>();
        // 透明なら戻す
        if(minFirstPlayerImage.color.a<=0)
        {
            minFirstPlayerImage.color = new Color(minFirstPlayerImage.color.r, minFirstPlayerImage.color.g, minFirstPlayerImage.color.b, 1);
        }
        if (SceneController.Instance.firstRunkPlayer != null)
        {
            // 一位のプレイヤーのスプライトとマテリアルを設定
            minFirstPlayerImage.sprite = 
                minPlayerSprite[(int)SceneController.Instance.firstRunkPlayer.GetComponent<Character>().charType];
            minFirstPlayerImage.material = SceneController.Instance.firstRunkPlayer.GetComponent<SpriteRenderer>().material;
            // 一位のプレイヤーの位置を変数に代入
            playerPosX = SceneController.Instance.firstRunkPlayer.transform.position.x;
        }

        // 一位のプレイヤーの位置からミニプレイヤーの位置を更新
        minFirstPlayerObj.GetComponent<RectTransform>().position =
            new Vector3(
            (playerPosX * distanceRatio) + minTrafficLight.GetComponent<RectTransform>().position.x,
            minFirstPlayerObj.GetComponent<RectTransform>().position.y,
            minFirstPlayerObj.GetComponent<RectTransform>().position.z);
    }
}
