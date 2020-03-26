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
    }

    // Update is called once per frame
    void Update()
    {
        // ゲージアニメーションのパラメーターをセットする
        // BulletCount アニメーターのパラメーター
        gaugeAnimator.SetInteger("BulletCount", playerAttack.NowBulletCount);
    }
}
