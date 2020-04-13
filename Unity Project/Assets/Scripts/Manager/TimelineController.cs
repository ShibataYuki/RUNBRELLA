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
     
    /// <summary>
    /// タイムライン開始処理
    /// </summary>
    public IEnumerator StartRaceTimeline()
    {
        // プレイアブルディレクターに使用するタイムラインをセット
        SetTimeLineToDirector("Timeline/StartRace");
        // トラックにオブジェクトをバインド
        BindObject();
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
    /// トラックへのオブジェクトのバインド処理
    /// </summary>
    private void BindObject()
    {
        for (int i = 0; i < GameManager.Instance.playerRanks.Count; i++)
        {
            var PlayerID = GameManager.Instance.playerRanks[i];
            var playerAnimator = SceneController.Instance.playerObjects[PlayerID].GetComponent<Animator>();
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
}
