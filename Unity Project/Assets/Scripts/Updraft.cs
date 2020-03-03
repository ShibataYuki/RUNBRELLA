using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updraft : MonoBehaviour
{

    [SerializeField]
    float upPower = 10.0f;

    /// <summary>
    /// 接触時処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 接触したオブジェクトがプレイヤーで滑空状態なら
        // 上方向に力を加える
        var player = collision.gameObject.GetComponent<Player>();
        var rigidBody = player.GetComponent<Rigidbody2D>();
        if (collision.tag == "Player" &&
            player.state == PlayerStateManager.Instance.playerGlideState)
        {
            rigidBody.AddForce(new Vector2(0, upPower));
        }

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    var player = collision.gameObject.GetComponent<Player>();
    //    var rigidBody = player.GetComponent<Rigidbody2D>();
    //    if (collision.tag == "Player" &&
    //        player.state == PlayerStateManager.Instance.playerGlideState)
    //    {
    //        rigidBody.AddForce(new Vector2(0, upPower),ForceMode2D.Impulse);
    //    }
    //}

}
