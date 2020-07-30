using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BuindObject : MonoBehaviour
{
    // プレイアブルディレクター
    PlayableDirector director;

    // Start is called before the first frame update
    void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }
  
    /// <summary>
    /// レースの順位によってプレイヤーをトラックにバインドする処理
    /// </summary>
    public void BindPlayer()
    {
        for (int i = 0; i < GameManager.Instance.playerResultInfos.Count; i++)
        {
            var PlayerNo = GameManager.Instance.playerResultInfos[i].playerNo;
            var playerAnimator = SceneController.Instance.playerObjects[PlayerNo].GetComponent<Animator>();
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


    /// <summary>
    /// 与えられたGameObjectを指定の名前のトラックにバインドする処理
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="trackName"></param>
    public void BindObj(GameObject obj, string trackName)
    {
        // トラックを全検索して条件に当てはまるオブジェクトをバインドします
        foreach (var track in director.playableAsset.outputs)
        {
            
            if (track.streamName == trackName)
            {
                director.SetGenericBinding(track.sourceObject, obj);
                break;
            }
        }
    }
    
}
