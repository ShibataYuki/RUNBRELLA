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
        if (collision.tag == "Player" && 
            player.state == PlayerStateManager.Instance.playerGlideState)
        {
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, upPower));
        }

    }
   
}
