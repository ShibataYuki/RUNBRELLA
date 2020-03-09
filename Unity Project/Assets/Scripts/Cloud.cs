using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    enum Mode
    {
        IDlE,
        INIT,
        MOVE,
        Follow,
        Deth,
    }
    // 雲の移動速度
    [SerializeField]
    float moveSpeed = 10f;
    Mode mode = Mode.IDlE;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ChangeMode(Mode.INIT);
        }
    }

    private void LateUpdate()
    {
        Do();

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
                    break;
                }

            case Mode.INIT:
                {
                    SetCloud();                    
                    break;
                }

            case Mode.MOVE:
                {
                    MoveCloud();
                    break;
                }
            case Mode.Follow:
                {
                    FollowCamera();
                    break;
                }


        }
    }
    


    /// <summary>
    /// 雨雲を画面の左端に呼び出します
    /// </summary>
    void SetCloud()
    {        
        // スプライトのサイズの半分（offSet用）
        float spriteHurfWhith = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float spriteHurfHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        // オフセット
        Vector3 offSet = new Vector3(spriteHurfWhith, spriteHurfHeight, 0);

        // 位置の代入
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0,1,1)) - offSet;
        ChangeMode(Mode.MOVE);
    }


    void ChangeMode(Mode mode)
    {
        this.mode = mode;
    }

    /// <summary>
    /// 雲が移動する処理です
    /// </summary>
    void MoveCloud()
    {
        
        // 右方向への移動
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // カメラの中心のｘ座標
        float CameraCenterPosX = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.8f, 1f)).x;
        // 雲の中心のｘ座標
        float cloudPosX = transform.position.x;

        // カメラの中央に当たちしたら追従モードに
        if(cloudPosX >= CameraCenterPosX)
        {
            ChangeMode(Mode.Follow);
        }
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

        // 位置の代入
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 1f)) - offSet;
    }

}
