using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{

    // リザルトのアニメーション
    [SerializeField]
    private Animator animator = null;
    // プレイヤーの順位
    [SerializeField]
    private Text[] playerNumberTexts = new Text[4];
    // プレイヤーのアイコン         
    [SerializeField]
    private Image[] playerIconImages = new Image[4];
    // プレイヤーのアイコンのスプライト
    [SerializeField]
    private Sprite[] playerIconSprits = new Sprite[4];
    // リザルトアニソンメドレーが終わったかどうか
    bool isEndAnimation = false;

    // Start is called before the first frame update
    void Start()
    {
    }


// Update is called once per frame
void Update()
    {
        
    }


    /// <summary>
    /// リザルト終了アニメーションを開始する関数
    /// </summary>
    public void StartEndResultAnimation()
    {
        animator.SetBool("isStartCutOut", true);
    }


    /// <summary>
    /// リザルトコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnResult()
    {
        // プレイヤーの順位とスプライトを設定
        for(int i=0;i<SceneController.Instance.playerCount;i++)
        {
            // プレイヤーの順位を設定
            playerNumberTexts[i].text = "Player" +
                SceneController.Instance.goalRunkOrder[i].GetComponent<Player>().ID.ToString();
            // プレイヤーのスプライトを設定
            playerIconImages[i].sprite = playerIconSprits[i];
        }
        // アニメーション開始
        animator.SetBool("isStartCutIn", true);
        yield break;
    }

}
