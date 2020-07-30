using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{

    bool isHit = false;
    // ゴール時のボイスのリスト
    [SerializeField]
    private List<AudioClip> charaAVoices = new List<AudioClip>();
    [SerializeField]
    private List<AudioClip> charaBVoices = new List<AudioClip>();
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 衝突相手がプレイヤーなら
        if(collision.tag=="Player")
        {
            // プレイヤーを順位順のリストに格納
            SceneController.Instance.InsertGoalPlayer(collision.gameObject);
            // ゴールしたプレイヤーの状態をRunにチェンジ
            var character = collision.gameObject.GetComponent<Character>();
            character.AfterGoalStart();
            // 終了処理
            if (!isHit&&SceneController.Instance.isStart)
            {
                // ゴール時用ニュース演出開始
                UIManager.Instance.newsUIManager.EntryNewsUI(NEWSMODE.GOAL,collision.gameObject);
                // ゴール用紙吹雪の演出
                var poopers = Camera.main.transform.Find("Poppers").GetComponent<Poppers>();
                poopers.PlayPoperEffect();
                // 終了処理開始
                SceneController.Instance.StartEnd(collision.gameObject);
                // 旗に触れたフラグをONにする
                SceneController.Instance.isTouchFlag = true;
            }
            isHit = true;
        }
    }


    IEnumerator PlayVoice(Character character)
    {
        yield return new WaitForSeconds(1f);
        // どのボイスを使用するか選択
        var selectVoiceIndex = Random.Range(0, charaAVoices.Count);
        if (character.charType == GameManager.CHARTYPE.PlayerA)
        {
            AudioManager.Instance.PlaySE(charaAVoices[selectVoiceIndex], 1.0f);
        }
        else
        {
            AudioManager.Instance.PlaySE(charaBVoices[selectVoiceIndex], 1.0f);
        }
    }

}
