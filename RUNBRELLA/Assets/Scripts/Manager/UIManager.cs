using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FADEMODE
{
    FADEIN,
    FADEOUT,
}

public class UIManager : MonoBehaviour
{

    #region シングルトン
    // シングルトン
    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // 複数個作成しないようにする
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        // リザルトUI作成
        resultUI.CreateResultUI();
        // newsUI作成
        newsUIManager.Create();
        // ミニリザルトUI作成
        minResultUI.CreateMinPlayerResultUI();
    }

    #endregion

    [SerializeField]
    List<GameObject> countdowns = new List<GameObject>();
    // プレイヤーカラー
    [SerializeField]
    public List<Color> playerColors = new List<Color>();
    public ResultUI resultUI = null;
    // ミニマップ
    public MinMapUI minMapUI = null;
    // ミニリザルトUI
    public MinResultUI minResultUI = null;
    // ニュースマネージャー
    public NewsUIManager newsUIManager = null;
    // フェードアウトするまでの時間
    [SerializeField]
    private float fadeOutTime = 0;
    // フェードインするまでの時間
    [SerializeField]
    private float fadeInTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        // 保留　初期化処理
        //for(int i=0;i<countdowns.Count;i++)
        //{
        //    countdowns[i].SetActive(false);
        //}

        // GoalCoinUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 保留 ゲームスタート時のカウントダウンをする関数
    /// </summary>
    /// <returns></returns>
    //public IEnumerator StartCountdown()
    //{
    //    for(int i=0; i<countdowns.Count;i++)
    //    {
    //        if(i==3)
    //        {
    //            AudioManager.Instance.PlaySE(start_and_endSE, 1f);
    //        }
    //        countdowns[i].SetActive(true);
    //        yield return new WaitForSeconds(1);
    //        countdowns[i].SetActive(false);
    //    }
    //}


    public IEnumerator StartFade(FADEMODE fadeMode)
    {
        var fadeObj = GameObject.Find("Fade").gameObject;
        var fadeImage = fadeObj.GetComponent<Image>();
        var fadeColor = fadeImage.color;
        var startFadeColorAlpha = 0f;
        var endFadeColorAlpha = 0f;
        var fadeTime = 0f;
        switch(fadeMode)
        {
            case FADEMODE.FADEIN:
                // Fadeを暗くする
                fadeColor.a = 1f;
                // 開始時の透明度
                startFadeColorAlpha = 1f;
                // 終了時の透明度
                endFadeColorAlpha = 0f;
                // フェードインし終わるまでの時間を設定
                fadeTime = fadeInTime;
                break;
            case FADEMODE.FADEOUT:
                // Fadeを明るくする
                fadeColor.a = 0f;
                // 開始時の透明度
                startFadeColorAlpha = 0f;
                // 終了時の透明度
                endFadeColorAlpha = 1f;
                // フェードアウトし終わるまでの時間を設定
                fadeTime = fadeOutTime;
                break;
        }
        fadeImage.color = fadeColor;
        // プラスとマイナスどちらへ変化するか
        var whereChangeValue = endFadeColorAlpha - startFadeColorAlpha;
        // 一秒ごとの透明度の変化量を計算
        var changeAlphaPer1SecondSpeed = whereChangeValue * (1 / fadeTime);
        while (true)
        {
            // 1フレームごとの変化量を計算
            var changeAlphaPer1FrameSpeed = changeAlphaPer1SecondSpeed * Time.deltaTime;
            // Fadeの透明度を変更
            fadeColor = fadeImage.color;
            fadeColor.a += changeAlphaPer1FrameSpeed;
            fadeImage.color = fadeColor;
            // 規定数値まで行ったら終了
            switch (fadeMode)
            { 
                case FADEMODE.FADEIN:
                if (fadeColor.a < endFadeColorAlpha)
                {
                    fadeColor.a = endFadeColorAlpha;
                    fadeImage.color = fadeColor;
                    yield break;
                }
                break;
            case FADEMODE.FADEOUT:
                if (fadeColor.a > endFadeColorAlpha)
                {
                    fadeColor.a = endFadeColorAlpha;
                    fadeImage.color = fadeColor;
                    yield break;
                }
                break;
            }
            yield return null;
        }
    }

}
