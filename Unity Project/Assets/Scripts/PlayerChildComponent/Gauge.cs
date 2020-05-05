using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    PlayerAttack playerAttack;
    Animator gaugeAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //このスクリプトがついてるオブジェクトのコンポーネント(部品)を取得
        gaugeAnimator = GetComponent<Animator>();
        //親オブジェクトのコンポーネントを取得
        playerAttack = transform.parent.gameObject.GetComponent<PlayerAttack>();
        var playerObject = transform.parent.gameObject;
        var player = playerObject.GetComponent<Player>();
        // プレイヤー毎に異なるソーティングレイヤーをゲージとマスクにセットする
        SetSortingLayer(player);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneController.Instance.isStart == true)
        {
            // ゲージアニメーションのパラメーターをセットする
            // BulletCount アニメーターのパラメーター
            gaugeAnimator.SetInteger("BulletCount", playerAttack.NowBulletCount);
        }
    }

    /// <summary>
    /// プレイヤー毎に異なるソーティングレイヤーをゲージとマスクにセットする
    /// </summary>
    /// <param name="player">ナンバーを持っているプレイヤーのスクリプト</param>
    private void SetSortingLayer(Player player)
    {
        // セットするソーティングレイヤーのID
        int ID = (int)player.playerNO + 1;
        var sortingLayerID = SortingLayer.NameToID(string.Format("Player{0}", ID));
        // 各ゲージにソーティングレイヤーをセット
        SetSortingLayer(sortingLayerID);
    }

    /// <summary>
    /// 各ゲージにソーティングレイヤーをセット
    /// </summary>
    /// <param name="sortingLayerID">セットするソーティングレイヤーのID</param>
    private void SetSortingLayer(int sortingLayerID)
    {
        for (int i = 1; i <= 5; i++)
        {
            // ゲージのカバーのゲームオブジェクトの参照を取得
            var gaugeCover = transform.Find(string.Format("Gauge_Cover{0}", i)).gameObject;
            // ゲージのオブジェクトからマスクのコンポーネントを取得
            var spriteMask = gaugeCover.GetComponent<SpriteMask>();
            // マスクにソーティングレイヤーをセットする
            spriteMask.backSortingLayerID = sortingLayerID;
            spriteMask.frontSortingLayerID = sortingLayerID;
            // ゲージの画像のオブジェクトの参照を取得
            var gaugeCoverSprite = gaugeCover.transform.Find("GaugeCoverSprite").gameObject;
            // ゲージの画像のオブジェクトのコンポーネントを取得
            var spriteRenderer = gaugeCoverSprite.GetComponent<SpriteRenderer>();
            // スプライトレンダラーにソーティングレイヤーをセットする
            spriteRenderer.sortingLayerID = sortingLayerID;
        }
    }
}
