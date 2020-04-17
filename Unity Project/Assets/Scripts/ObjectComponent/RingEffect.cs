using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEffect : MonoBehaviour
{

    // エフェクトがでてからプレイヤーのUIに行くまでの時間
    [SerializeField]
    float waitTime = 0;
    // とばす力
    [SerializeField]
    float powar = 0;
    // 追尾スピード
    [SerializeField]
    float trackingSpeed = 0;
    // プレイヤーに到着するまでのフレーム数
    [SerializeField]
    float frameCount = 0;
    // rigidbody
    [SerializeField]
    Rigidbody2D rigidbody2d = null;
    // ファクトリー
    RingEffectFactory ringEffectFactory;
    public int id;
    [SerializeField]
    int loopCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        ringEffectFactory = GameObject.Find("RingEffectFactory").GetComponent<RingEffectFactory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// リングエフェクトの初期化処理をするコルーチン
    /// </summary>
    /// <param name="ID">リングを通ったプレイヤーのID</param>
    /// <param name="ringObj">通ったリング</param>
    public IEnumerator MoveImpulse(int ID,GameObject ringObj)
    {
        // リングエフェクトの位置をリングと同じ座標にする
        transform.position = ringObj.transform.position;
        // 乱数で飛ぶ方向を決定
        float moveVecX = Random.Range(-1f, 1f);
        // 飛ぶ方向
        Vector2 moveVec = new Vector2(moveVecX, 1);
        // ノーマライズ
        moveVec.Normalize();
        // 速さを設定
        moveVec *= powar;
        // とばす
        // rigidbody2d.AddForce(moveVec, ForceMode2D.Impulse);
        // 規定秒数待機
        yield return new WaitForSeconds(waitTime);
        // リングを通ったプレイヤーのゲージに向けて移動
        StartCoroutine(MoveTracking(ID));
    }


    /// <summary>
    /// プレイヤーのゲージを追尾するコルーチン
    /// </summary>
    /// <param name="ID">追尾するプレイヤーのID</param>
    /// <returns></returns>
    private IEnumerator MoveTracking(int ID)
    {
        // velocityを0にする
        rigidbody2d.velocity = Vector3.zero;
        // 現在のフレーム数
        // float nowFrame = frameCount;
        while(true)
        {
            // 今のRingEffectの座標とリングを通ったプレイヤーのゲージの座標からベクトルを生成
            Vector3 gaugePos = SceneController.Instance.playerObjects[ID].transform.Find("Gauge").transform.position;
            Vector3 moveVec = gaugePos - transform.position;
            Vector3 workVec = new Vector3(0, 0, 0);
            // ノーマライズ
            moveVec.Normalize();
            float workX = Mathf.Abs(1 / moveVec.x);
            workVec.x = trackingSpeed * moveVec.x * workX;
            workVec.y = trackingSpeed * moveVec.y * workX;
            transform.position += workVec * Time.deltaTime;
            // プレイヤーのゲージへ行ったなら終了
            float distance = Vector3.Distance(gaugePos, transform.position);
            // 一定距離まで近づいたら
            if (distance <= 0.5f)
            {
                ringEffectFactory.ReturnRingEffect(gameObject);

                yield break;
            }
            yield return null;
            // 到着するまでのフレーム数から今回のフレームの移動量を計算する
            //Vector3 gaugePos = SceneController.Instance.playerObjects[ID].transform.Find("Gauge").transform.position;
            //Vector3 moveVec = (gaugePos - transform.position) / nowFrame;
            // 移動
            //transform.position += moveVec;
            //nowFrame--;
            //if(nowFrame<=0)
            //{
            //    ringEffectFactory.ReturnRingEffect(gameObject);
            //    yield break;
            //}
        }
    }
}
