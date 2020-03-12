using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    /// <summary>
    /// 状態遷移
    /// </summary>
    enum Mode
    {
        IDlE,
        INIT,
        MOVE_FORWARD,
        FOLLOW,
        MOVE_BACK,
        DEATH,
        RELEASE,
    }   

    // 移動スピード
    [SerializeField]
    float moveSpeed = 1f;
    // カメラの中央に留まる時間（秒）
    [SerializeField]
    float keepCameraCenterTime = 4f;
    [SerializeField]
    // スイッチモードの状態
    Mode mode = Mode.IDlE;
    // 雨のエフェクト
    public ParticleSystem rainDrop;
    // スプライトレンダラー
    SpriteRenderer spriteRenderer; 
    // コルーチン
    IEnumerator delayChangestate = null;  
    IEnumerator moveForward = null;
    IEnumerator moveBack = null;     

    // Start is called before the first frame update
    void Start()
    {
        rainDrop = transform.Find("RainDrop").GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();        
    }
  
    private void LateUpdate()
    {
        Do();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {            
            var player = collision.gameObject.GetComponent<Player>();
            //プレイヤーの雨フラグON
            player.IsRain = true;            
        }        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<Player>();
            //プレイヤーの雨フラグOFF
            player.IsRain = false;
        }        
    }

    /// <summary>
    /// モードによって処理を変える処理
    /// </summary>
    void Do()
    {
        switch(mode)
        {
            case Mode.IDlE:
                {
                    // キー入力があったらモード移行
                    if (InputManager.Instance.CallRainKeyIn())
                    {
                        ChangeMode(Mode.INIT);
                    }
                    break;
                }
            case Mode.INIT:
                {
                    // 雲セット
                    SetCloud();                    
                    break;
                }

            case Mode.MOVE_FORWARD:
                {
                    if(moveForward == null)
                    {
                        // 前方への移動
                        moveForward = MoveForward();
                        StartCoroutine(moveForward);
                    }

                    break;
                }
            case Mode.FOLLOW:
                {
                    // カメラ中央にとどまる
                    FollowCamera();
                    if(delayChangestate == null)
                    {
                        // 時間をおいてモード移行
                        delayChangestate = DelayChangeMode(keepCameraCenterTime, Mode.MOVE_BACK);
                        StartCoroutine(delayChangestate);
                    }
                    
                    break;
                }
            case Mode.MOVE_BACK:
                {
                    if (moveBack == null)
                    {
                        // 後方への移動
                        moveBack = MoveBack();
                        StartCoroutine(moveBack);
                    }
                    
                    break;
                }
            case Mode.RELEASE:
                {
                    // 移動終了後処理
                    Reset();
                    break;
                }
        }
    }

    /// <summary>
    /// モード移行処理
    /// </summary>
    /// <param name="mode"></param>
    void ChangeMode(Mode mode)
    {
        this.mode = mode;
    }

    /// <summary>
    /// 遅延後モード移行処理
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    IEnumerator DelayChangeMode(float delay, Mode mode)
    {
        yield return new WaitForSeconds(delay);
        delayChangestate = null;
        ChangeMode(mode);
        yield break;
    }

    /// <summary>
    /// 雨雲を画面の左端に呼び出します
    /// </summary>
    void SetCloud()
    {
        // エフェクトの再生
        rainDrop.Play();
        //雲を初期位置へセット
        {
            // スプライトのサイズの半分（offSet用）
            float spriteHurfWhith = GetComponent<SpriteRenderer>().bounds.size.x / 2;
            float spriteHurfHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
            // オフセット
            Vector3 offSet = new Vector3(spriteHurfWhith, spriteHurfHeight, 0);
            // 雲の初期位置
            Vector2 startPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 1)) - offSet;
            // 初期位置へセット
            transform.position = startPos;
        }       
        // モード移行
        ChangeMode(Mode.MOVE_FORWARD);        
    }

    

    /// <summary>
    /// 雲が前方に移動する処理です
    /// </summary>
    IEnumerator MoveForward()
    {
        // 雲の総移動距離
        float cloudMoveVlue = 0;
        // スプライトのサイズの半分（offSet用）
        float spriteHurfWhith = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float spriteHurfHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        // オフセット
        Vector3 offSet = new Vector3(spriteHurfWhith, spriteHurfHeight, 0);        
        // 雲の中心のｘ座標
        float cloudPosX = 0;
        
        while (true)
        {
            // 移動後座標
            Vector3 posMoved = Camera.main.ViewportToWorldPoint(new Vector3(cloudMoveVlue, 1f, 1f)) - offSet;
            // カメラの中心のｘ座標
            float CameraCenterPosX = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 1f)).x;
            // 画面の左側から右に向けて移動
            transform.position = posMoved;
            // 次フレーム用値更新
            {
                // 雲の現在地更新
                cloudPosX = transform.position.x;
                // 総移動距離更新
                cloudMoveVlue += moveSpeed * Time.deltaTime;
            }           
            // カメラの中央に到達したら追従モードに
            if (cloudPosX >= CameraCenterPosX)
            {
                break;
            }

            yield return null;
        }
        // モード移行
        ChangeMode(Mode.FOLLOW);
        yield break;

    }


    IEnumerator MoveBack()
    {
        // 雲の総移動処理
        float cloudMoveVlue = 0;
        // スプライトのサイズの半分（offSet用）
        float spriteHurfWhith = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float spriteHurfHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        // オフセット
        Vector3 offSet = new Vector3(0, spriteHurfHeight, 0);
        // 雲の中心のｘ座標
        float cloudPosX = 0;
        
        while (true)
        {
            // 移動後座標
            Vector3 posMoved = Camera.main.ViewportToWorldPoint(new Vector3(0.5f - cloudMoveVlue, 1f, 1f)) - offSet;
            // 移動終了ｘ座標
            float moveEndPosX = Camera.main.ViewportToWorldPoint(new Vector3(0, 1f, 1f)).x - spriteHurfWhith;
            // 画面の中央から左に向けて移動
            transform.position = posMoved;
            // 次フレーム用値更新
            {
                cloudPosX = transform.position.x;
                cloudMoveVlue += moveSpeed * Time.deltaTime;
            }            
            // 移動終了ｘ座標に到達したら追従モードに
            if (cloudPosX <= moveEndPosX)
            {
                break;
            }

            yield return null;
        }

        // モード移行
        ChangeMode(Mode.RELEASE);
        yield break;

    }

    /// <summary>
    /// カメラの座標を追従します
    /// </summary>
    void FollowCamera()
    {
        // スプライトのサイズの半分（offSet用）        
        float spriteHurfHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        // オフセット
        Vector3 offSet = new Vector3(0, spriteHurfHeight, 0);
        // カメラ中央座標
        Vector3 cameraCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 1f)) - offSet;
        // 位置の代入
        transform.position = cameraCenter;
    }

    /// <summary>
    /// 終了後処理
    /// </summary>
    void Reset()
    {
        // コルーチン変数リセット
        moveForward         = null;
        moveBack            = null;
        // エフェクト停止
        rainDrop.Stop();
        // モード移行
        ChangeMode(Mode.IDlE);
    }

}
