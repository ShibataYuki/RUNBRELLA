using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    // プレイアブルディレクター
    PlayableDirector director;

    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    /// <summary>
    /// 第一レース用タイムラインの開始処理
    /// </summary>
    public void FirstRace_Start()
    {
        SetTimeLineToDirector("Timeline/StartTimeline");
        BindObject();
        director.Play();
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
    /// トラックへのオブジェクトのバインド処理
    /// </summary>
    private void BindObject()
    {
        for (int i = 0; i < GameManager.Instance.playerRanks.Count; i++)
        {
            var playerRankNo = GameManager.Instance.playerRanks[i];
            var playerAnimator = SceneController.Instance.playerObjects[playerRankNo].GetComponent<Animator>();
            // トラックを全検索して条件に当てはまるオブジェクトをバインドします
            foreach (var track in director.playableAsset.outputs)
            {
                string playerRank = "No" + (playerRankNo) + "Player";
                if (track.streamName == playerRank)
                {
                    director.SetGenericBinding(track.sourceObject, playerAnimator);
                    break;
                }
            }
        }
    }
}
