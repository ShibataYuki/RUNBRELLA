using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BuindCharactor : MonoBehaviour
{
    // プレイアブルディレクター
    PlayableDirector director = null;
    GameObject charactors = null;

    // Start is called before the first frame update
    void Awake()
    {
        director = GetComponent<PlayableDirector>();
        charactors = GameObject.Find("Charactors");
    }

    private void Start()
    {
        BindPlayer();
    }

    /// <summary>
    /// レースの順位によってプレイヤーをトラックにバインドする処理
    /// </summary>
    public void BindPlayer()
    {
        for (int i = 0; i < GameManager.Instance.playerResultInfos.Count; i++)
        {
            var PlayerNo = GameManager.Instance.playerResultInfos[i].playerNo;
            var playerAnimator = charactors.GetComponent<Charactors>().charactorAnimatorList[i];           
            // トラックを全検索して条件に当てはまるオブジェクトをバインドします
            foreach (var track in director.playableAsset.outputs)
            {                
                string trackName = "No" + (i + 1) + "Charactor";
                if (track.streamName == trackName)
                {
                    director.SetGenericBinding(track.sourceObject, playerAnimator);
                    break;
                }
            }
        }
    }
    
}
