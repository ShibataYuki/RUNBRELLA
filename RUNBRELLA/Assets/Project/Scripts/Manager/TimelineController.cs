using ResultScene;
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
    PlayableDirector director = null;
    BuindObject buindObject = null;
    

    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
        buindObject = GetComponent<BuindObject>();
        
    }
     
    /// <summary>
    /// タイムライン開始処理
    /// </summary>
    public IEnumerator StartRaceTimeline()
    {
        //// プレイアブルディレクターに使用するタイムラインをセット
        //SetTimeLineToDirector("Timeline/StartRace");
        // プレイヤーをトラックにバインド
        buindObject.BindPlayer();       
        // タイムラインの再生
        director.Play();
        // ブーストエフェクトの再生
        StartBoostEffect();
        // タイムラインの再生終了待機
        while (IsTimelinePlaying()) { yield return null; }
        // ブーストエフェクトの停止
        StopBoostEffect();
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
        Dictionary<PLAYER_NO,Character> players = SceneController.Instance.players;

        foreach(var character in players.Values)
        {          
            var animator = character.GetComponent<Animator>();
            RuntimeAnimatorController animatorController = null;
            var playerType = character.charType;
            // キャラタイプによってロードするアニメーターコントローラを変更
            var pass = "PlayerAnimator/" + playerType.ToString();
            animatorController = (RuntimeAnimatorController)Resources.Load(pass);
            animator.runtimeAnimatorController = animatorController;            
        }
    }


    /// <summary>
    ///  1位以外のプレイヤーのブーストエフェクトを再生する
    /// </summary>
    public void StartBoostEffect()
    {
        Dictionary<PLAYER_NO, Character> players = SceneController.Instance.players;

        foreach (var character in players.Values)
        {
            var isTop = character.playerNO == GameManager.Instance.playerResultInfos[0].playerNo;
            // 1位の場合再生しない
            if (isTop) { continue; }

            character.PlayEffect(character.boostEffect);
        }
    }

    /// <summary>
    /// 1位以外のプレイヤーのブーストエフェクトを停止する
    /// </summary>
    public void StopBoostEffect()
    {
        Dictionary<PLAYER_NO, Character> players = SceneController.Instance.players;

        foreach (var character in players.Values)
        {
            var isTop = character.playerNO == GameManager.Instance.playerResultInfos[0].playerNo;
            // 1位の場合処理しない
            if (isTop) { continue; }

            character.StopEffect(character.boostEffect);
        }
    }


    /// <summary>
    /// トラックへのオブジェクトのバインド処理
    /// </summary>
    //private void BindTrack()
    //{
    //    for (int i = 0; i < GameManager.Instance.playerRanks.Count; i++)
    //    {
    //        var PlayerNo = GameManager.Instance.playerRanks[i];
    //        var PlayerControllerNo = GameManager.Instance.PlayerNoToControllerNo(PlayerNo);
    //        var playerAnimator = SceneController.Instance.playerObjects[PlayerControllerNo].GetComponent<Animator>();
    //         トラックを全検索して条件に当てはまるオブジェクトをバインドします
    //        foreach (var track in director.playableAsset.outputs)
    //        {
    //            string trackName = "No" + (i+1) + "Player";
    //            if (track.streamName == trackName)
    //            {
    //                director.SetGenericBinding(track.sourceObject, playerAnimator);
    //                break;
    //            }
    //        }
    //    }
    //}

    


}
