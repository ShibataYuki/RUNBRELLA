using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BuindObject : MonoBehaviour
{
    // プレイアブルディレクター
    PlayableDirector director;

    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();

    }
  
    /// <summary>
    /// レースの順位によってプレイヤーをトラックにバインドする処理
    /// </summary>
    public void BindPlayer()
    {
        for (int i = 0; i < GameManager.Instance.playerRanks.Count; i++)
        {
            var PlayerNo = GameManager.Instance.playerRanks[i];
            var PlayerControllerNo = GameManager.Instance.PlayerNoToControllerNo(PlayerNo);
            var playerAnimator = SceneController.Instance.playerObjects[PlayerControllerNo].GetComponent<Animator>();
            // トラックを全検索して条件に当てはまるオブジェクトをバインドします
            foreach (var track in director.playableAsset.outputs)
            {
                string trackName = "No" + (i + 1) + "Player";
                if (track.streamName == trackName)
                {
                    director.SetGenericBinding(track.sourceObject, playerAnimator);
                    break;
                }
            }
        }
    }


    
}
