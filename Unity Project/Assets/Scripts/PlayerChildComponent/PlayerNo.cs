using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNo : MonoBehaviour
{
    // プレイヤーのNOを示す画像
    [SerializeField]
    Sprite[] playerNoSprite = new Sprite[4];
    // 画像を表示するスプライトレンダラー
    SpriteRenderer spriteRenderer;
    // ゲージのオブジェクト
    GameObject gaugeObject;

    // Start is called before the first frame update
    void Start()
    {
        // スプライトレンダラーのコンポーネントを取得
        spriteRenderer = GetComponent<SpriteRenderer>();
        // プレイヤーのNoの画像をセット
        SetPlayerNoSprite();
    }
    /// <summary>
    /// プレイヤーのNoの画像をセット
    /// </summary>
    void SetPlayerNoSprite()
    {
        // プレイヤーのNoを持つプレイヤーのコンポーネントを取得
        var player = transform.parent.gameObject.GetComponent<Player>();
        // 画像をセット
        spriteRenderer.sprite = playerNoSprite[(int)player.playerNO];
        // ゲージのオブジェクトを非表示に変更する
        gaugeObject = transform.parent.Find("Gauge").gameObject;
        gaugeObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // レース中なら
        if(SceneController.Instance.isStart == true)
        {
            // ゲージのオブジェクトを表示する
            gaugeObject.SetActive(true);
            // プレイヤーのナンバーを表示する
            gameObject.SetActive(false);
        }
    }
}