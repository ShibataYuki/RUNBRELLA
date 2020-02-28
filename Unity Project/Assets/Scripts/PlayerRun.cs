using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    // 移動速度
    [SerializeField]
    float speed;

    Rigidbody2D rigidbody2d;

    private void Start()
    {
        // rigidbodyをセット
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
    }


    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    public void Run()
    {
        //rigidbody2d = transform.GetComponent<Rigidbody2D>();
        // 移動ベクトル
        Vector2 moveVec;
        moveVec = Vector2.right * speed;
        rigidbody2d.velocity = new Vector2(moveVec.x, rigidbody2d.velocity.y);
    }


    /// <summary>
    /// プレイヤーの移動速度をセットする関数
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

}
