using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{

    #region シングルトン
    // シングルトン
    private static TimelineController instance;
    public static TimelineController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // 複数個作成しないようにする
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    // プレイアブルディレクター
    PlayableDirector director;

    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
        
    }

    public IEnumerator StartRaceTimeline()
    {
        bool isFirstRace = GameManager.Instance.nowRaceNumber == 0;
        IEnumerator startRace = null;
        if(isFirstRace)
        {
            startRace = Start_FirstRace();
        }
        else
        {
            startRace = Start_NextRace();
        }
        yield return StartCoroutine(startRace);
        yield break;
    }

    /// <summary>
    /// 第１レース用タイムライン開始処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start_FirstRace()
    {
        // プレイアブルディレクターに使用するタイムラインをセット
        SetTimeLineToDirector("Timeline/FirstRace");
        // トラックにオブジェクトをバインド
        BindObject_FirstRase();
        // タイムラインの再生
        director.Play();
        // タイムラインの再生終了待機
        while (IsTimelinePlaying()) { yield return null; }
        // アニメーターコントローラーをセット
        //（最初からアニメーターコントローラーをセットしているとタイムラインでの操作と競合する？ため
        //  タイムライン再生後にセットする  ）
        SetAnimationController();
        yield break;
    }

    /// <summary>
    /// 第２レース以降用タイムライン開始処理
    /// </summary>
    private IEnumerator Start_NextRace()
    {
        // プレイアブルディレクターに使用するタイムラインをセット
        SetTimeLineToDirector("Timeline/NextRace");
        // トラックにオブジェクトをバインド
        BindObject_NextRase();
        // タイムラインの再生
        director.Play();
        // タイムラインの再生終了待機
        while (IsTimelinePlaying()) { yield return null; }
        // アニメーターコントローラーをセット
        //（最初からアニメーターコントローラーをセットしているとタイムラインのアニメーションクリップと競合する？ため
        //  タイムライン再生後にセットする  ）
        SetAnimationController();
        yield break;
    }

    /// <summary>
    /// タイムラインの再生中かを判断する処理
    /// </summary>
    /// <returns></returns>
    private bool IsTimelinePlaying()
    {
        if (director.state == PlayState.Playing) return true;
        else                                     return false;
    }
    /// <summary>
    /// PlayableDirectorのTaimlineをセットする処理
    /// </summary>
    /// <param name="pass">リソース下のタイムラインのパス</param>
    private void SetTimeLineToDirector(string pass)
    {
        // タイムラインをリソースからロード
        var timeline = Resources.Load<PlayableAsset>(pass);
        Debug.Assert(timeline != null, "passが間違っています");
        // タイムラインのセット
        director.playableAsset = timeline;
    }

    /// <summary>
    /// 各キャラクターにアニメーターコントローラーをセットします
    /// </summary>
    public void SetAnimationController()
    {
        Dictionary<int,Player> players = SceneController.Instance.playerEntityData.players;

        foreach(var player in players.Values)
        {
            var animator = player.GetComponent<Animator>();
            RuntimeAnimatorController animatorController = null;
            var playerType = player.charType;
            var pass = "PlayerAnimator/" + playerType.ToString();
            animatorController = (RuntimeAnimatorController)Resources.Load(pass);
            animator.runtimeAnimatorController = animatorController;
        }
    }


    /// <summary>
    /// 1レース用トラックへのオブジェクトのバインド処理
    /// </summary>
    private void BindObject_FirstRase()
    {
        List<int> randomList = RandomList();
        for (int i = 0; i < randomList.Count; i++)
        {
            var playerNo = randomList[0];
            var playerAnimator = SceneController.Instance.playerObjects[playerNo].GetComponent<Animator>();
            // トラックを全検索して条件に当てはまるオブジェクトをバインドします
            foreach (var track in director.playableAsset.outputs)
            {
                string trackName = "No" + (i+1) + "Player";
                if (track.streamName == trackName)
                {
                    director.SetGenericBinding(track.sourceObject, playerAnimator);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 2レース目以降用トラックへのオブジェクトのバインド処理
    /// </summary>
    private void BindObject_NextRase()
    {
        for (int i = 0; i < GameManager.Instance.playerRanks.Count; i++)
        {
            var playerRankNo = GameManager.Instance.playerRanks[i];
            var playerAnimator = SceneController.Instance.playerObjects[playerRankNo].GetComponent<Animator>();
            // トラックを全検索して条件に当てはまるオブジェクトをバインドします
            foreach (var track in director.playableAsset.outputs)
            {
                string trackName = "No" + (playerRankNo) + "Player";
                if (track.streamName == trackName)
                {
                    director.SetGenericBinding(track.sourceObject, playerAnimator);
                    break;
                }
            }
        }
    }



    private List<int> RandomList()
    {
        List<int> originList = new List<int>();
        List<int> randomList = new List<int>();
        // 元となる数列
        for (int i = 0; i < GameManager.Instance.playerNumber; i++)
        {
            originList.Add(i);
        }
        // ランダムな並びの数列生成(List)
        for (int i = 0;i< GameManager.Instance.playerNumber; i++)
        {
            int randomNo = Random.Range(0, originList.Count);
            randomList.Add(originList[randomNo] + 1);
            originList.RemoveAt(randomNo);            
        }
        return randomList;
    }

}
