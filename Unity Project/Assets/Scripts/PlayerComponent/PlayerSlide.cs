using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{   
    // 通常の速度
    [SerializeField]
    float speed = 0;   
    [SerializeField]
    public float catchSliderTime = 0f;
    float rayLengthOffset = 0.25f;
    // ヒットしたものの情報を格納する変数
    public RaycastHit2D RayHit { get; set; }
    // 自身のコライダー
    BoxCollider2D boxCollider;   
    //「Player」コンポーネント
    Character character;
    // 移動クラス
    PlayerMove move;
    // どのレイヤーのものとヒットさせるか
    LayerMask layerMask;   
    // 自身のリジットボディ
    Rigidbody2D rigidbody2d;
    // 地面との当たり判定を行うコンポーネント
    private HitChecker hitChecker;
    //// 保留　掴めることを示すスプライト
    //private SpriteRenderer catchEffect = null;
    // スライド中の軌跡の親オブジェクト
    private GameObject slideTrails;
    // SEを再生するAudioSource
    AudioSource audioSource = null;
    // 保存するvelocityのx
    float velocityX;
    // 手すりモードを終わるとき、どの程度y軸方向の慣性を残すか(%)
    [SerializeField]
    float slideInertiaYPercent = 0;

    // 角度によって水平の手すりに対してどの程度の速度に変化をするか(%)
    [SerializeField]
    float maxSpeedPersentByRot = 0;
    [SerializeField]
    float minSpeedPersentByRot = 0;
    private SlideState slideState;
    // Start is called before the first frame update
    void Start()
    {
        // 変数の初期化
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        character = GetComponent<Character>();
        audioSource = GetComponent<AudioSource>();
        hitChecker = GetComponent<HitChecker>();
        move = GetComponent<PlayerMove>();
        slideState = GetComponent<SlideState>();       
        // レイヤーマスクを「Slider」に設定
        layerMask = LayerMask.GetMask(new string[] {"Slider"});       
        // 保留　子オブジェクトのコンポーネントを探す
        //catchEffect = transform.Find("B").gameObject.GetComponent<SpriteRenderer>();
        // 保留　演出を切る
        //EffectOff();
        // スライド中の軌跡の親オブジェクト
        slideTrails = transform.Find("SlideTrails").gameObject;
        // テキストファイル読み込み＆データ代入
        ReadTextParameterByCharaType();
        // テキストの中のデータをセットするディクショナリー        
        SheetToDictionary.Instance.TextToDictionary("Chara_Common", out var textDataDic);
        catchSliderTime = textDataDic["手すりをつかむボタンを押してからつかむ判定が出ている継続時間(秒)"];
    }


    /// <summary>
    /// テキストからパラメータを取得
    /// </summary>
    private void ReadTextParameterByCharaType()
    {
        // 読み込むテキストの名前
        var textName = "";
        switch (character.charType)
        {
            case GameManager.CHARTYPE.PlayerA:
                textName = "Chara_A";
                break;
            case GameManager.CHARTYPE.PlayerB:
                textName = "Chara_B";
                break;
        }
        // テキストの中のデータをセットするディクショナリー        
        SheetToDictionary.Instance.TextToDictionary(textName, out var textDataDic);
        speed = textDataDic["手すりの右方向への速度の秒速"];
        slideInertiaYPercent = textDataDic["手すりを離れた後にY軸方向への慣性を何パーセント残すか(%)"];
        maxSpeedPersentByRot = textDataDic["傾きが90度の時に水平な手すりの右方向への速度の何パーセントにするか"];
        minSpeedPersentByRot = textDataDic["傾きが-90度の時に水平な手すりの右方向への速度の何パーセントにするか"];        
    }
      
    /// <summary>
    /// 滑走の開始処理
    /// </summary>
    public void StartSlide()
    {
        // 現在のvelocityを保存
        velocityX = rigidbody2d.velocity.x;
        rigidbody2d.velocity = Vector2.zero;
        // プレイヤーの高さを手すりの高さに調整
        AdjustHight();
        // 重力を０に変更
        rigidbody2d.gravityScale = 0;
        // サイズの変更
        var size = boxCollider.size;
        size.y -= 0.05f;
        boxCollider.size = size;
        // オフセットの変更
        var offset = boxCollider.offset;
        offset.y += 0.05f;
        boxCollider.offset = offset;
        slideTrails.SetActive(true);
        // SEのループ再生
        audioSource.Play();
    }

    /// <summary>
    /// スライダーとの当たり判定をする
    /// </summary>
    /// <returns></returns>
    public void SlideCheck()
    {        
        Vector2 playerTopPos;
        Vector2 playerBottomPos;
        float colliderHurfX = boxCollider.size.x / 2;
        float colliderHurfY = boxCollider.size.y / 2;
        float rayLength;
        playerTopPos = new Vector2(transform.position.x + colliderHurfX, transform.position.y + colliderHurfY + rayLengthOffset + boxCollider.offset.y);
        playerBottomPos = new Vector2(transform.position.x + colliderHurfX, transform.position.y - colliderHurfY + boxCollider.offset.y);
        rayLength = playerTopPos.y - playerBottomPos.y;        
        Debug.DrawLine(playerTopPos, playerBottomPos, Color.blue);
        // プレイヤーの上の方向から下方向に向けてレイを飛ばして当たり判定                                        
        RayHit = Physics2D.Raycast(playerTopPos,   // 発射位置
                                Vector2.down,   // 発射方向
                                rayLength,      // 長さ
                                layerMask);     // どのレイヤーに当たるか
    }

    /// <summary>
    /// 保留　手すりを掴めることを演出で示す
    /// </summary>
    //public void EffectOn()
    //{
    //    var color = Color.yellow;
    //    color.a = 1.0f;
    //    catchEffect.color = color;
    //}

    /// <summary>
    /// 保留　手すりをしばらく掴めないことを演出で示す
    /// </summary>
    //public void EffectOff()
    //{
    //    var color = catchEffect.color;
    //    color.a = 0.0f;
    //    catchEffect.color = color;
    //}

    /// <summary>
    /// プレイヤーを手すりの高さに調整する関数
    /// </summary>
    public void AdjustHight()
    {
        
        if (RayHit == true)
        {
            var hitY = new Vector2(gameObject.transform.position.x, RayHit.point.y);
            rigidbody2d.position = hitY;
        }
        
    }

    /// <summary>
    /// プレイヤーのvelocityを手すりのright方向に変換する関数
    /// </summary>
    public void Slide()
    {
        // 加速度の蓄積
        move.AddAcceleration();
        // 高さの調整
        AdjustHight();
        
        if(RayHit == true)
        {            
            Vector2 workVelocity;
            // 角度による速度変化
            var addSpeedByRot = AddSpeedByRotate();
            var workSpeed = this.speed + addSpeedByRot;
            // ベクトルによってはx方向とy方向に力が分散してしまうため
            // x方向の力の大きさをを無理やりspeedに戻す処理
            float workX = 1 / RayHit.transform.right.x;
            workVelocity.x = workSpeed * RayHit.transform.right.x * workX;
            workVelocity.y = workSpeed * RayHit.transform.right.y * workX;
            rigidbody2d.velocity = workVelocity;

            // 元の処理
            //rigidbody2d.velocity = workSpeed * hitObject.transform.right;

        }
        if (RayHit == true)
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.right,  RayHit.collider.gameObject.transform.right);
        }

        //else if(Hit.collider.gameObject.tag == "Converter")
        //{
        //    Debug.Log("コンバーター");
        //    GameObject converter = Hit.collider.gameObject;
        //    Vector2 converterPos = Hit.collider.gameObject.transform.position;
        //    Vector2 hit2converterBottom
        //        = Hit.point - new Vector2(converterPos.x, converterPos.y - converter.GetComponent<CircleCollider2D>().radius * converter.transform.localScale.x);
        //    Debug.DrawLine(Hit.point, new Vector2(converterPos.x, converterPos.y - converter.GetComponent<CircleCollider2D>().radius * converter.transform.localScale.x), Color.red);

        //    Vector2 point = new Vector2(Hit.point.x, converterPos.y - converter.GetComponent<CircleCollider2D>().radius * converter.transform.localScale.y *2);
        //    Debug.DrawLine(converter.transform.position, point, Color.blue);
        //    Vector2 Converter2point = point - (Vector2)converter.transform.position;

        //    transform.rotation = Quaternion.FromToRotation(Vector2.up, hit2converterBottom);
        //}

    }

    /// <summary>
    /// 手すりの傾いている方向と傾きの大きさによって速度を加算する処理
    /// </summary>
    /// <returns></returns>
    float AddSpeedByRotate()
    {
        // 現在のローテーション
        var rotation = transform.localEulerAngles;
        // ローテンションのZを0度から90度のオイラー角に変換
        var rotZ_0_90 = ConvertRotTo90Steps(rotation);
        
        if(rotation.z >= 0 && rotation.z <= 180)
        {
            // 水平な手すりと比較して最小でどの程度の速度にするか
            // をパーセントから倍率に変換
            var minSpeedMagnification = minSpeedPersentByRot / 100f;
            // 最遅の速度を計算
            var minSpeed = this.speed * minSpeedMagnification;
            // 1度当たりの速度の減少値計算
            var AddSpeedPerAngle = (minSpeed - this.speed) / 90f;
            // 1度当たりの速度の減少値*手すりの角度 = この手すりの速度の減少値
            return AddSpeedPerAngle * rotZ_0_90;
        }
        else if (rotation.z > 180 && rotation.z < 360)
        {   // 水平な手すりと比較して最大でどの程度の速度にするか
            // をパーセントから倍率に変換
            var maxSpeedMagnification = maxSpeedPersentByRot / 100f;
            // 最速の速度を計算
            var maxSpeed = this.speed * maxSpeedMagnification;
            // 1度当たりの速度の増加値計算
            var AddSpeedPerAngle =  (maxSpeed - this.speed) / 90f;
            // 1度当たりの速度の増加値*手すりの角度 = この手すりの速度の増加値
            return AddSpeedPerAngle * rotZ_0_90;
        }
        return 0;
    }

    /// <summary>
    /// z軸のローテンションを0～90度のオイラー角に変換する処理
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
    float ConvertRotTo90Steps(Vector3 rotation)
    {
        var rot_Z = rotation.z;
        // 0 ～ 180度なら 0 ～ 90度に変換
        if(rot_Z >= 0 && rot_Z <= 180)
        {
            return Mathf.Clamp(rot_Z, 0, 90);
        }
        // 181 ～ 359度なら 90 ～ 0度に変換 
        else if(rot_Z >180 && rot_Z < 360)
        {
            rot_Z = 360 - rot_Z;
            return Mathf.Clamp(rot_Z, 0, 90);
        }
        Debug.Assert(false, "手すりの角度変換に失敗しました");
        return 0;
    }

    /// <summary>
    /// 滑走の終了処理
    /// </summary>
    public void EndSlide()
    {
        
        rigidbody2d.gravityScale = 1;
        // AfterSlideに移しました。
        //transform.rotation = Quaternion.FromToRotation(transform.right, Vector2.zero);
        // サイズの変更
        var size = boxCollider.size;
        size.y += 0.05f;
        boxCollider.size = size;
        // オフセットの変更
        var offset = boxCollider.offset;
        offset.y -= 0.05f;
        boxCollider.offset = offset;
        // 速度変更
        //ResetVelocityX();
        // SEの停止
        audioSource.Stop();
        // 滑走時エフェクトOFF
        slideTrails.SetActive(false);
        // 接地判定を行う
        hitChecker.GroundCheck();
    }

    /// <summary>
    /// 速度をリセット
    /// x方向の速度を手すりに摑まる前の速度へ
    /// </summary>
    void ResetVelocityX()
    {        
        Vector2 velocity;
        // スライド終了時に残るy軸方向の慣性を％から倍率に変換        
        velocity.x = velocityX;
        velocity.y = rigidbody2d.velocity.y;
        rigidbody2d.velocity = velocity;
    }

    /// <summary>
    /// 手すりから離れる際にy方向の慣性を制限する処理(高く飛びすぎることを防ぐため)
    /// </summary>
    public void LimitInertiaY()
    {
        Vector2 velocity;
        var slideInertiaY = slideInertiaYPercent / 100;
        velocity.x = rigidbody2d.velocity.x;
        velocity.y = rigidbody2d.velocity.y * slideInertiaY;
        rigidbody2d.velocity = velocity;
    }

    // 現在稼働中の「RayTimer」コルーチン
    IEnumerator RanRayTimer = null;
    /// <summary>
    /// 「RayTimer」を開始する処理
    /// </summary>
    /// <param name="time"></param>
    /// <param name="controllerNo"></param>
    public void RayTimerStart(float time)
    {
        // すでに動作中なら終了
        if (RanRayTimer != null)
        {
            StopCoroutine(RanRayTimer);
        }
        // 最新版コルーチンセット
        RanRayTimer = RayTimer(time);
        // コルーチンスタート
        StartCoroutine(RanRayTimer);
    }

    // レイの照射時間
    private float rayDuration = 0;    
    
    /// <summary>
    /// 手すりに摑まれる時間管理処理
    /// </summary>
    /// <param name="time"></param>
    /// <param name="controllerNo"></param>
    /// <returns></returns>
    private IEnumerator RayTimer(float time)
    {
                
        // タイマーセット
        rayDuration = time;

        while(true)
        {
            // 手すりヒット確認
            SlideCheck();
            // 手すりにヒットしていたら滑走状態へ移行
            if (RayHit)
            {
                // ステート移行処理
                character.SlideStart();           
                yield break;
            }

            // タイマー減少
            rayDuration -= Time.deltaTime;
            var timeOver = rayDuration <= 0;

            // 指定時間経過したら終了
            if (timeOver)
            {               
                yield break;
            }

            yield return null;
        }
       
    }

}
