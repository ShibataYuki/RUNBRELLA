using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    // 移動速度
    [SerializeField]
    float speed = 6;
    // 速度減衰値
    [SerializeField]
    float decaySpeed = 0.05f;
    public float Speed { get { return speed; } set { speed = value; } }
    Player player;
    Rigidbody2D rigidbody2d;

    private void Start()
    {
        // rigidbodyをセット
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }


    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    public void Run()
    {
        //rigidbody2d = transform.GetComponent<Rigidbody2D>();
        // 移動ベクトル
        Vector2 moveVec;
        if(player.VelocityXStorage <= speed)
        {            
            moveVec = Vector2.right * speed;
            rigidbody2d.velocity = new Vector2(moveVec.x, rigidbody2d.velocity.y);
        }
        else
        {           
            rigidbody2d.velocity = new Vector2 (player.VelocityXStorage,0);
            player.VelocityXStorage -= decaySpeed;
        }
        
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
