using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainIcon : MonoBehaviour
{
    enum Mode
    {
        IDLE,           // 待機状態
        INIT,           // 初期化状態
        MOVE_TO_CENTER, // 中心へ移動する状態
        KEEP_CENTER,    // 中心にとどまる状態
        MOVE_TO_END,    // 最終地点に移動する状態       
    }

    // レクトトランスフォーム
    RectTransform rectTransform;
    // 明滅する速度
    [SerializeField]
    float changeAlphaSpeed = 1;
    // 最大倍率
    [SerializeField]
    float maxScaleMagnification = 5;
    // 最小倍率
    [SerializeField]
    float minScaleMagnification = 0;
    // 中央にとどまる時間
    [SerializeField]
    float keepCenterTime = 1.5f;
    // 移動にかかる時間
    [SerializeField]
    float MoveToBigTime = 3;
    [SerializeField]
    float MoveToSmallTime = 3;
    // 明滅を開始する雨の強さのボーダー(%)
    [SerializeField,Range(0,100)]
    float flickBorder = 30;
    // 終了地点とするオブジェクト
    GameObject IconEnd = null;
    // 雨オブジェクト
    Rain rain;
    // SwitchCase切替用変数
    [SerializeField]
    Mode mode;
    // 移動ベクトル
    Vector3 moveVec;
    // 移動先ポジション
    Vector3 targetPos;
    // 移動距離（計算用）
    float moveDistance = 0;   
    // 最終的に収まるポジション
    Vector3 endPos;
    Vector3 baseScale;
    // イメージ
    Image image;    
    // 明滅コルーチン
    public IEnumerator flick = null;
    // アイコンを生成するクラス
    RainIconFactory rainIconFactory;

    private void Start()
    {
        // テキスト読み込み
        SheetToDictionary.Instance.TextToDictionary("Rain", out var textDataDic);
        // データ代入
        maxScaleMagnification = textDataDic["雨の看板が大きくなる最大値(通常の何倍か)"];
        minScaleMagnification = textDataDic["雨の看板が小さくなる最小値(通常の何倍か)"];
        keepCenterTime = textDataDic["看板が画面の真ん中にとどまる時間(秒)"];
        MoveToBigTime = textDataDic["看板が最大の大きさになるまでの時間(秒)"];
        MoveToSmallTime = textDataDic["看板が最小の大きさになるまでの時間(秒)"];
        flickBorder = textDataDic["雨がどのくらいの強さを下回ったら看板が点滅しだすか(0%～100%)"];
        changeAlphaSpeed = textDataDic["看板が点滅するスピード"];
        rain = rain = GameObject.Find("Rain").GetComponent<Rain>();
        rainIconFactory = transform.parent.gameObject.GetComponent<RainIconFactory>();
        IconEnd = transform.parent.Find("IconEndPos").gameObject;
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        endPos = IconEnd.GetComponent<RectTransform>().position;
        baseScale = rectTransform.localScale;
       

    }
    private void OnEnable()
    {                     
        mode = Mode.INIT;       
    }
    private void OnDisable()
    {
        // 色を戻す
        var color = image.color;
        image.color = Color.white;
        rectTransform.localScale = Vector3.one;
        // コルーチン停止
        StopAllCoroutines();
        // 明滅コルーチン解放
        flick = null;
    }
    // Update is called once per frame
    void Update()
    {
        // 雨が弱まるモード＆雨の強さが基準以下なら明滅コルーチン停止
        if (rain.rainPercentage <= flickBorder && (rain.mode == Rain.RainMode.DECREASE))
        {
            // 一度だけ呼ばれるように制限
            if (flick == null)
            {
                flick = Flick();
                StartCoroutine(flick);
            }
        }
        else
        {
            // 一度だけ呼ばれるように制限
            if (flick != null)
            {
                // 明滅コルーチン停止
                StopCoroutine(flick);
                // 明滅コルーチン解放
                flick = null;
                // 色を戻す
                var color = image.color;
                image.color = Color.white;
            }
        }

        // モードに応じた処理
        Do_SwtichMode();
    }

    void Do_SwtichMode()
    {
        switch(mode)
        {
            case Mode.IDLE:
                {
                    break;
                }
            case Mode.INIT:
                {
                    // 移動目標を画面中央に変更
                    //var centerPos = new Vector3(960,540,0);
                    var centerPos = (endPos + rectTransform.position) / 2;
                    ChangeTargetPos(centerPos);                   
                    // 初期のサイズを1として最大サイズ指定倍のサイズを計算
                    targetScale = (Vector2)baseScale * maxScaleMagnification;
                    // 目標サイズと現在のサイズの差分を計算
                    changeValue = targetScale - (Vector2)baseScale;
                    // モード切替
                    StartCoroutine(ChangeMode( Mode.MOVE_TO_CENTER));                           
                    break;
                }
            case Mode.MOVE_TO_CENTER:
                {
                    // 中心に移動する処理            
                    var moveCompleate = MoveToTarget();
                    var scaleCompleate = ChangeScaleToTarget();
                    if(moveCompleate && scaleCompleate) // 中央に移動が完了したら
                    {
                        // 現在の移動距離リセット
                        nowDistance = 0;
                        // 移動目標を最終地点に変更
                        ChangeTargetPos(endPos);
                        // 初期のサイズを1として最大サイズ指定倍のサイズを計算
                        targetScale = (Vector2)baseScale * minScaleMagnification;
                        // 目標サイズと現在のサイズの差分を計算
                        changeValue = targetScale - (Vector2)rectTransform.localScale;
                        // モード切替
                        StartCoroutine(ChangeMode(Mode.KEEP_CENTER));         
                    }
                    break;
                }
            case Mode.KEEP_CENTER:
                {
                    // 指定秒数経過後モード切替
                    StartCoroutine(ChangeMode(Mode.MOVE_TO_END,keepCenterTime));
                    break;
                }
            case Mode.MOVE_TO_END:
                {
                    // 目標へ移動する処理
                    var moveCompleate = MoveToEnd();
                    var scaleCompleate = ChangeScaleToTarget();
                    if (moveCompleate && scaleCompleate) // 移動が完了したら
                    {
                        // 現在の移動距離リセット
                        nowDistance = 0;
                        // モード切替
                        StartCoroutine(ChangeMode(Mode.IDLE));                        
                        // ファクトリーが保持しているアイコンを切り替えてもらう
                        rainIconFactory.KeepObjChange(gameObject);
                    }
                    break;
                }                          
        }
    }
      
    /// <summary>
    /// モード切替処理
    /// </summary>
    /// <param name="mode">切り替え先のモード</param>
    /// <param name="delay">遅延時間（指定しなければ０）</param>
    /// <returns></returns>
    IEnumerator ChangeMode(Mode mode,float delay = 0)
    {
        // いったん待機状態に対比
        this.mode = Mode.IDLE;
        // 時間経過を待つ
        yield return new WaitForSeconds(delay);
        // モード切替
        this.mode = mode;
    }

    /// <summary>
    /// 移動先のポジションを変更
    /// </summary>
    /// <param name="targetPos">移動先のポジション</param>
    void ChangeTargetPos(Vector3 targetPos)
    {
        // 目標座標変更
        this.targetPos = targetPos;
        // 移動ベクトル作成
        MakeMoveVec();
    }

    /// <summary>
    /// 移動ベクトル作成
    /// </summary>
    void MakeMoveVec()
    {
        // 移動ベクトル作成
        moveVec = targetPos - rectTransform.position;
        // ノーマライズ
        moveVec.Normalize();
        // 長さを計算
        moveDistance = Vector3.Distance(targetPos, rectTransform.position);
    }
    
    float nowDistance = 0; //「MoveToTarget用変数」
    /// <summary>
    /// 目標への移動処理
    /// </summary>
    /// <returns></returns>
    bool MoveToTarget()
    {
        // 移動距離計算
        // １秒あたりの移動距離を計算
        // Time.deltaTimeをかけて１フレーム当たりの移動量に変換
        var movePerSecond = moveDistance / MoveToBigTime;
        var movePerFrame = movePerSecond * Time.deltaTime;
        // 移動処理
        rectTransform.position += (moveVec * movePerFrame);
        // 現在の移動距離加算
        nowDistance += movePerFrame;
        
        // 目標移動距離に現在の移動距離が到達したら位置修正、終了報告
        if(moveDistance <= nowDistance)
        {
            // 位置補正
            rectTransform.position = targetPos;
            // 現在の移動距離リセット
            nowDistance = 0;
            // 完了報告
            return true;
        }
        return false;
    }



    /// <summary>
    /// 目標への移動処理
    /// </summary>
    /// <returns></returns>
    bool MoveToEnd()
    {
        // 移動距離計算
        // １秒あたりの移動距離を計算
        // Time.deltaTimeをかけて１フレーム当たりの移動量に変換
        var movePerSecond = moveDistance / MoveToSmallTime;
        var movePerFrame = movePerSecond * Time.deltaTime;
        // 移動処理
        rectTransform.position += (moveVec * movePerFrame);
        // 現在の移動距離加算
        nowDistance += movePerFrame;

        // 目標移動距離に現在の移動距離が到達したら位置修正、終了報告
        if (moveDistance <= nowDistance)
        {
            // 位置補正
            rectTransform.position = targetPos;
            
            // 完了報告
            return true;
        }
        return false;
    }


    /// <summary>
    /// 目標サイズへ近づけていく処理
    /// </summary>
    /// <returns></returns>
    // 目標サイズ
    Vector2 targetScale = new Vector2();
    // 変化したい量
    Vector2 changeValue = new Vector2();
    bool ChangeScaleToTarget()
    {
        // 加算するべきか
        bool isAdding = false;
        // 変化したい量
        if(changeValue.x > 0)
        {
            isAdding = true;
        }
        
        // 大きくするか小さくするかの判断
        switch(isAdding)
        {
            case true:
                {
                    var time = mode == Mode.MOVE_TO_CENTER ? MoveToBigTime : MoveToSmallTime;
                    // 1秒あたりの変化量
                    var addPerSecond = changeValue / time;
                    // 1フレーム当たりの変化量
                    var addPerFrame = addPerSecond * Time.deltaTime;
                    // サイズの変化
                    rectTransform.localScale += (Vector3)addPerFrame;
                    // 目標のサイズになったらその大きさに固定
                    if (rectTransform.localScale.x >= targetScale.x)
                    {
                        rectTransform.localScale = targetScale;
                        // 完了報告
                        return true;
                    }
                    break;
                }
            case false:
                {
                    var time = mode == Mode.MOVE_TO_CENTER ? MoveToBigTime : MoveToSmallTime;
                    // 1秒あたりの変化量
                    var addPerSecond = changeValue / time;
                    // 1フレーム当たりの変化量
                    var addPerFrame = addPerSecond * Time.deltaTime;
                    // サイズの変化
                    rectTransform.localScale += (Vector3)addPerFrame;
                    // 目標のサイズになったらその大きさに固定
                    if (rectTransform.localScale.x <= targetScale.x)
                    {
                        rectTransform.localScale = targetScale;
                        // 完了報告
                        return true;
                    }
                    break;
                }
        }       
        return false;
    }

    /// <summary>
    /// 明滅コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator Flick()
    {        
        // 加算処理フラグ
        bool isAdding = false;              
        var workColor = new Color(0, 0, 0, changeAlphaSpeed);
        
        while(true)
        {
            // α値の加算減算
            if (isAdding)
            {
                image.color += workColor * Time.deltaTime;
            }
            else
            {
                image.color -= workColor * Time.deltaTime;
            }

            // α値が上限加減に到達するたびに処理フラグ切り替え
            if (image.color.a >= 1)
            {
                var color = image.color;
                image.color = new Color(color.r, color.g, color.b, 1);
                isAdding = false;
            }
            if (image.color.a <= 0)
            {
                var color = image.color;
                image.color = new Color(color.r, color.g, color.b, 0);
                isAdding = true;
            }
           
            yield return null;
        }
               
    }

}
