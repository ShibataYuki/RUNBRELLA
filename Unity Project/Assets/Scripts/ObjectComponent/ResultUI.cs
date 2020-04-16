using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    // プレイヤーのリザルトUIのコイン用UI
    List<List<GameObject>> coinUIs = new List<List<GameObject>>();
    // コイン用UIのポジション
    List<List<Vector3>> coinUIsPos = new List<List<Vector3>>();
    // リザルトUIのリスト
    List<GameObject> resultUIs = new List<GameObject>();
    // メインカメラ
    Camera camera;
    // ゴールコイン
    GameObject goalCoinObj;
    // ゴールコインのアニメーター
    [SerializeField]
    Animator goalCoinAnimator = null;
    // ゴールコインのSprite
    [SerializeField]
    Sprite goalCoinSprite = null;


    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        goalCoinObj = GameObject.Find("GoalCoinUI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// ResultUIを生成する関数
    /// </summary>
    public void CreateResultUI()
    {
        // UIを作成
        var resultUIPrefab = Resources.Load<GameObject>("Prefabs/PlayerResultUI");
        // 作成するUIのWidth
        float width = resultUIPrefab.GetComponent<RectTransform>().sizeDelta.x;
        // プレイヤーの数によってオフセットを決める
        float offsetX = (Screen.width - width * GameManager.Instance.playerNumber) 
            / (GameManager.Instance.playerNumber + 1f);
        float resultOffsetX = (-Screen.width / 2f) + ((width / 2f) + offsetX);
        // プレイヤーの数だけ作成
        for(int i=0;i<GameManager.Instance.playerNumber;i++)
        {
            var resultUIObj = Instantiate(resultUIPrefab);
            // リザルトUIのテキストを設定
            var resultUIText = resultUIObj.transform.Find("PlayerName/Text").gameObject.GetComponent<Text>();
            resultUIText.text = "Player" + (i + 1).ToString();
            // リストに追加
            resultUIs.Add(resultUIObj);
            // 非表示にする
            resultUIObj.SetActive(false);
            // UIManagerの子オブジェクトに変更
            resultUIObj.transform.SetParent(GameObject.Find("UIManager").transform);
            // PlayerCoinUIを作成
            CreatePlayerCoinUI(resultUIObj,i);
            // playerCoinUIを格納するリストを作成
            List<GameObject> coinUI = new List<GameObject>();
            // レース数回ループ
            for(int loop=0;loop<GameManager.Instance.RaceNumber;loop++)
            {
                // コイン用UIをリストに格納
                GameObject coinUIObj = 
                    resultUIObj.transform.Find("PlayerCoinUI" + i.ToString()+"_"+loop.ToString()).gameObject;
                coinUI.Add(coinUIObj);
            }
            coinUIs.Add(coinUI);
            // 各プレイヤーの勝ち数によってコイン用UIのスプライトを変更
            if(GameManager.Instance.playerWins.Count>0)
            {
                for (int l = 0; l < GameManager.Instance.playerWins[i]; l++)
                {
                    coinUIs[i][l].GetComponent<Image>().sprite = goalCoinSprite;
                }
            }
            // 座標を変更
            Vector3 resultUIPos = new Vector3(resultOffsetX, resultUIObj.transform.position.y, 0);
            resultUIObj.transform.localPosition = resultUIPos;
            // オフセットをずらす
            resultOffsetX += (width + offsetX);
        }
    }


    private void CreatePlayerCoinUI(GameObject resultObj,int num)
    {
        // リソースからロード
        var playerCoinPrefab = Resources.Load<GameObject>("Prefabs/PlayerCoinUI");
        // 作成するUIのwidth
        float playerCoinUIWidth = playerCoinPrefab.GetComponent<RectTransform>().sizeDelta.x;
        // リザルトUIのwidth
        float resultUIWidth = resultObj.GetComponent<RectTransform>().sizeDelta.x;
        // プレイヤーの数によってPlayerCoinUIのオフセットを決める
        float OffsetX = (resultUIWidth - playerCoinUIWidth * GameManager.Instance.RaceNumber)
            / (GameManager.Instance.RaceNumber + 1);
        float playerCoinOffset = (-resultUIWidth / 2f) + (playerCoinUIWidth / 2f) + OffsetX;
        // プレイヤーの数だけ作成
        for(int i=0;i<GameManager.Instance.RaceNumber;i++)
        {
            // 生成
            var playerCoinObj = Instantiate(playerCoinPrefab);
            // resultUIの子オブジェクトにする
            playerCoinObj.transform.parent = resultObj.transform;
            // 座標を変更
            Vector3 pos = new Vector3(playerCoinOffset, playerCoinObj.transform.position.y, 0);
            playerCoinObj.transform.localPosition = pos;
            // オフセットを変更
            playerCoinOffset += (playerCoinUIWidth + OffsetX);
            // 名前を変更
            playerCoinObj.name = "PlayerCoinUI" + num.ToString() + "_" + i.ToString();
        }
    }

    /// <summary>
    /// リザルトUIのコイン用UIのポジションをセットする関数
    /// </summary>
    public void SetPosition()
    {
        for(int i=0;i<coinUIs.Count;i++)
        {
            var coinUI = coinUIs[i];
            List<Vector3> coinUIPos = new List<Vector3>();
            for(int l=0;l<coinUI.Count;l++)
            {
                // コイン用UIのポジションを格納
                coinUIPos.Add(coinUI[l].GetComponent<RectTransform>().localPosition + resultUIs[i].GetComponent<RectTransform>().localPosition);
            }
            coinUIsPos.Add(coinUIPos);
        }
    }

    /// <summary>
    /// リザルトUIを表示する関数
    /// </summary>
    public void ShowResultUI()
    {
        for(int i=0;i<resultUIs.Count;i++)
        {
            resultUIs[i].SetActive(true);
        }
    }

    /// <summary>
    /// どのコイン用UIのポジションにするか選ぶ関数
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Vector3 ChoosePos(int ID)
    {
        return coinUIsPos[ID - 1][GameManager.Instance.playerWins[ID - 1]];
    }

    /// <summary>
    /// リザルト中のコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnResultUI()
    {
        var goalCoin = goalCoinObj.GetComponent<GoalCoin>();
        // コイン用UIのポジションをセット
        SetPosition();
        // どのコイン用UIのポジションにするか選ぶ
        var playerID = SceneController.Instance.goalRunkOrder[0].GetComponent<Player>().ID;
        // コイン回転アニメーション開始
        goalCoinAnimator.SetBool("isStart", true);
        // 画面中央に出るまで拡大＆移動
        yield return StartCoroutine(goalCoin.OnMove(goalCoin.showPos,goalCoin.startMoveSpeed,goalCoin.startCoinSizeMax));
        // 表示用アニメーション開始
        goalCoinAnimator.SetBool("isShow", true);
        yield return new WaitForSeconds(1f);
        // どのコイン用UIのポジションに移動するか決定
        var targetPos = ChoosePos(SceneController.Instance.playerNumbers[playerID]);
        // 回転アニメーション開始
        goalCoinAnimator.SetBool("isShow", false);
        // 勝ったプレイヤーのコイン用UIに移動
        yield return StartCoroutine(goalCoin.OnMove(targetPos,goalCoin.endMoveSpeed,goalCoin.endCoinSizeMini));
        // 回転アニメーション終了
        goalCoinAnimator.SetBool("isStart", false);
        // ゴールコインを画面外へ
        // ゴール用UIにスプライトを設定
        yield break;
    }
}
