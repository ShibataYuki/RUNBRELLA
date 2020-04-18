using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashesFactory : MonoBehaviour
{    
    // どれだけ雨を準備するか
    [SerializeField]
    int howManyInstantiate = 80;
    // 雨が降りだすまでの遅延時間
    [SerializeField]
    float playDelay = 2f;
    List<ParticleSystem> splashesParticleList = new List<ParticleSystem>();
    // レイヤーマスクを地面に設定
    LayerMask layerMask;    
    // 何フレームに一度しぶきを発生させるかを決める基準(小数点以下切り上げ)
    float flameParSplash = 0.1f;
    // 何フレームに一度しぶきを発生させるかを決める基準の増加量
    [SerializeField]
    float addFlameParSplash = 0.004f;
    // レイの長さ
    float rayLength = 0;   
    // 雨
    VerticalRain rain;
    // 雨のモード
    VerticalRain.RainMode rainMode;
    // 雨を降らせるコルーチン
    IEnumerator playEffect = null;
   

    // Start is called before the first frame update
    void Start()
    {
        // レイヤーマスクセット
        layerMask = LayerMask.GetMask(new string[] { "Ground" ,"Player"});
        // 子オブジェクトとしてエフェクトを作成
        MakeSplashes();
        // レイの長さを求める
        rayLength = GetRayLength();
        rain = GameObject.Find("Main Camera/Rain").GetComponent<VerticalRain>();
    }

    // Update is called once per frame
    void Update()
    {

        // 雨のモード
        rainMode = rain.mode;

        // 雨の状態によって色を変える
        switch (rainMode)
        {
            // 雨が強くなっているとき
            case VerticalRain.RainMode.INCREASE:
                {
                   if(playEffect != null) { return; }
                    playEffect = PlayEffect(playDelay);
                    StartCoroutine(playEffect);
                    break;
                }
            case VerticalRain.RainMode.DECREASE:
                {
                    // エフェクトの発生ペースを遅くする
                    flameParSplash += addFlameParSplash;
                    break;
                }
            default:
                {
                    if (playEffect != null)
                    {
                        StopCoroutine(playEffect);
                        flameParSplash = 0.1f;
                        playEffect = null;
                    }
                    break;
                }
        }
      
    }

    /// <summary>
    /// エフェクトの発生総処理
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator PlayEffect(float delay = 0)
    {                
        yield return new WaitForSeconds(delay);
        while(true)
        {
            // レイを飛ばしてエフェクトの再生位置を決める
            var effectPos = GetEffectPos();
            // 作動中でないエフェクトを選ぶ
            var effect = GetIsStoopedEffect();
            // 選んだエフェクトを再生位置に移動
            MoveParticleToPoint(effect, effectPos);
            // エフェクトを再生
            StartEffect(effect);
            // エフェクトの発生ペースをコントロール
            for(int i = 0; i < flameParSplash; i++)
            {
                yield return null;
            }
        }        
    }

    /// <summary>
    /// エフェクト再生処理
    /// </summary>
    /// <param name="effect"></param>
    void StartEffect(ParticleSystem effect)
    {
        if (effect == null)
        {
            Debug.Assert(effect != null, "再生するエフェクトがありません！");
            return;
        }

        effect.Play();
    }

    /// <summary>
    /// 水しぶきのエフェクトを子オブジェクトとして作成
    /// </summary>
    private void MakeSplashes()
    {
        // ロード
        var splashPrefab = LoadSplash();

        for (int i = 0; i < howManyInstantiate; i++)
        {
            // 実体生成
            var splashInstance = InstantiateSplash(splashPrefab);
            // 子オブジェクトにする
            SetChild(splashInstance);
            // リストに詰める
            SetList(splashInstance);
        }
    }

    /// <summary>
    /// カメラの高さからレイの長さを求める
    /// </summary>
    /// <returns></returns>
    private float GetRayLength()
    {
        // 画面の一番上の座標
        Vector2 topPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        // 画面の一番下の座標
        Vector2 bottomPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        // 画面の一番上の座標から 画面の一番下の座標までの長さを
        // レイの長さにする
        float rayLength = topPos.y - bottomPos.y;
        return rayLength;
    }

    /// <summary>
    /// エフェクトの発生位置を決める処理
    /// </summary>
    /// <returns></returns>
    private Vector2 GetEffectPos()
    {
        // ヒットしたものの情報を格納する変数
        RaycastHit2D RayHit;
        var randomRayStartPos = GetRandomRayStartPos();
        // プレイヤーの上の方向から下方向に向けてレイを飛ばして当たり判定                                        
        RayHit = Physics2D.Raycast(randomRayStartPos,   // 発射位置
                                Vector2.down,   // 発射方向
                                rayLength,      // 長さ
                                layerMask);     // どのレイヤーに当たるか
        if(RayHit == false)
        {
            RayHit = Physics2D.Raycast(randomRayStartPos,   // 発射位置
                               Vector2.up,   // 発射方向
                               rayLength,      // 長さ
                               layerMask);     // どのレイヤーに当たるか
        }

        // レイがレイヤーマスクに設定したオブジェクトに当たった位置
        var effectPos = RayHit.point;
        return effectPos;
    }   

    /// <summary>
    /// エフェクトを発生ポイントまで移動させる処理
    /// </summary>
    /// <param name="point"></param>
    private void MoveParticleToPoint(ParticleSystem effect, Vector2 point)
    {        
        if (effect == null) { return; }

        effect.transform.position = point;
    }

    /// <summary>
    /// 画面上のランダムな位置を返す処理
    /// </summary>
    /// <returns></returns>
    private Vector2 GetRandomRayStartPos()
    {
        // カメラ上のランダムな位置
        var randamViePos_X = Random.Range(0.0f, 1.0f);
        var randamViePos_Y = Random.Range(0.0f, 1.0f);
        // ワールド座標に変換
        var randamWorldPos = (Vector2)Camera.main.ViewportToWorldPoint(new Vector3(randamViePos_X, randamViePos_Y,0));
        return randamWorldPos;
    }


    /// <summary>
    /// リストから再生中でないエフェクトを選ぶ
    /// </summary>
    private ParticleSystem GetIsStoopedEffect()
    {
        foreach(ParticleSystem effect in splashesParticleList)
        {
            // 再生中でないエフェクトを選ぶ
            if(effect.isStopped)
            {               
                return effect;
            }
        }
        Debug.Assert(false, "しぶきエフェクトが弾切れです");
        return null;
    }

    /// <summary>
    /// ロード
    /// </summary>
    /// <returns></returns>
    private GameObject LoadSplash()
    {
        var splashPrefab = Resources.Load<GameObject>("Prefabs/Effects/SplasheEffect");
        return splashPrefab;
    }

    /// <summary>
    /// 実体生成
    /// </summary>
    /// <param name="splashPrefab">ロードしたプレファブ</param>
    /// <returns></returns>
    private GameObject InstantiateSplash(GameObject splashPrefab)
    {
        var rotate = splashPrefab.transform.rotation;
        var splash = Instantiate(splashPrefab, new Vector3(0, 0, -15), rotate);
        return splash;
    }

    /// <summary>
    /// 子オブジェクトにセット
    /// </summary>
    /// <param name="splash"></param>
    private void SetChild(GameObject splash)
    {
        splash.transform.SetParent(transform);
    }

    /// <summary>
    /// リストにパーティクルシステムの参照をセット
    /// </summary>
    /// <param name="splash"></param>
    private void SetList(GameObject splash)
    {
        var splashParticle = splash.GetComponent<ParticleSystem>();
        splashesParticleList.Add(splashParticle);
    }

}
