using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using GamepadInput;

public class ResultUI : MonoBehaviour
{
    // プレイヤーのリザルトUIのコイン用UI
    List<List<GameObject>> coinUIs = new List<List<GameObject>>();
    // コイン用UIのポジション
    List<List<Vector3>> coinUIsPos = new List<List<Vector3>>();
    // リザルトUIのリスト
    List<GameObject> resultUIs = new List<GameObject>();
    // ゴールコイン
    GameObject goalCoinObj;
    // ゴールコインのアニメーター
    public Animator goalCoinAnimator;
    // ゴールコインのSprite
    [SerializeField]
    Sprite goalCoinSprite = null;
    // コインがはまる座標
    public Vector3 targetPos;
    // リザルトのメインコルーチン
    public Coroutine resultCoroutine;
    // リザルトのコルーチンが終了したかどうか
    public bool isEnd = false;
    // プレイヤーナンバーのスプライト
    public List<Sprite> playerNoSprites = new List<Sprite>();
    // PlayerResultUIのスプライト
    public List<Sprite> playerResultUISprites = new List<Sprite>();
    public AudioClip StampSE = default;

    // Start is called before the first frame update
    void Start()
    {
        goalCoinObj = GameObject.Find("GoalCoinUI");
        Init();
    }

    public void Init()
    {
        for(int i = 0; i < resultUIs.Count; i++)
        {
            var resultUIObj = resultUIs[i];
            // リザルトUIのプレイヤーネームのスプライトを設定
            var playerNameImage = resultUIObj.transform.Find("PlayerName").gameObject.GetComponent<Image>();
            playerNameImage.sprite = playerNoSprites[i];
            // 非表示にする
            resultUIObj.SetActive(false);
            // プレイヤーリザルトのスプライトを設定
            var playerNo = (PLAYER_NO)i;
            var charType = SceneController.Instance.playerObjects[playerNo].GetComponent<Player>().charType;
            resultUIObj.GetComponent<Image>().sprite = playerResultUISprites[(int)charType];
            // PlayerCoinUIを作成
            CreatePlayerCoinUI(resultUIObj, i);
            // playerCoinUIを格納するリストを作成
            List<GameObject> coinUI = new List<GameObject>();
            // レース数回ループ
            for (int loop = 0; loop < GameManager.Instance.RaceNumber; loop++)
            {
                // コイン用UIをリストに格納
                GameObject coinUIObj =
                    resultUIObj.transform.Find("PlayerCoinUI" + i.ToString() + "_" + loop.ToString()).gameObject;
                coinUI.Add(coinUIObj);
            }
            coinUIs.Add(coinUI);
            // 各プレイヤーの勝ち数によってコイン用UIのスプライトを変更
            if (GameManager.Instance.playerWins.Count > 0)
            {
                for (int l = 0; l < GameManager.Instance.playerWins[(PLAYER_NO)i]; l++)
                {
                    // 色を黒から白に戻す
                    var coinImage = coinUIs[i][l].GetComponent<Image>();
                    coinImage.color = new Color(255, 255, 255, 255);
                    coinImage.sprite = goalCoinSprite;
                }
            }

        }
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
            // リストに追加
            resultUIs.Add(resultUIObj);
            // UIManagerの子オブジェクトに変更
            resultUIObj.transform.SetParent(GameObject.Find("UIManager").transform);
            // 座標を変更
            Vector3 resultUIPos = new Vector3(resultOffsetX, resultUIObj.transform.position.y, 0);
            resultUIObj.transform.localPosition = resultUIPos;
            // オフセットをずらす
            resultOffsetX += (width + offsetX);
        }
    }


    /// <summary>
    ///　プレイヤーのコインはまる用UIを作成する関数
    /// </summary>
    /// <param name="resultObj"></param>
    /// <param name="num"></param>
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
            playerCoinObj.transform.SetParent(resultObj.transform);
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
        return coinUIsPos[ID][GameManager.Instance.playerWins[(PLAYER_NO)ID]];
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
        // コイン回転アニメーション開始
        goalCoinAnimator.SetBool("isStart", true);
        // ValueからKeyを取得
        var playerNo = SceneController.Instance.goalRunkOrder[0].GetComponent<Character>().playerNO;
        // どのコイン用UIのポジションに移動するか決定
        targetPos = ChoosePos((int)playerNo);
        // 画面中央に出るまで拡大＆移動
        yield return StartCoroutine(goalCoin.OnMove(goalCoin.showPos,goalCoin.startMoveSpeed,goalCoin.startCoinSizeMax));
        // 表示用アニメーション開始
        goalCoinAnimator.SetBool("isShow", true);
        float showTime = 0;
        while(showTime < 1f)
        {
            showTime += Time.deltaTime;
            if(GamePad.GetButtonDown(GamePad.Button.A,GamePad.Index.Any)||Input.GetKeyDown(KeyCode.Return))
            {
                goalCoin.GetComponent<GoalCoin>().End();
            }
            yield return null;
        }
        // 回転アニメーション開始
        goalCoinAnimator.SetBool("isShow", false);
        // 勝ったプレイヤーのコイン用UIに移動
        yield return StartCoroutine(goalCoin.OnMove(targetPos,goalCoin.endMoveSpeed,goalCoin.endCoinSizeMini));
        // SE再生
        AudioManager.Instance.PlaySE(StampSE, 0.5f);
        // 回転アニメーション終了
        goalCoinAnimator.SetBool("isStart", false);
        // 終了フラグをONにする
        isEnd = true;
        yield return null;
    }
}
