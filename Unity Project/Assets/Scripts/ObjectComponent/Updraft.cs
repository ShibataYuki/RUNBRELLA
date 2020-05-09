using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updraft : MonoBehaviour
{

    [SerializeField]
    float upPower = 0f;

    private void Start()
    {
        SheetToDictionary.Instance.TextToDictionary("Stage", out var textDataDic);
        upPower = textDataDic["上昇気流の強さ"];
    }

    /// <summary>
    /// 接触時処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 接触したオブジェクトがプレイヤーで滑空状態なら
        // 上方向に力を加える
        
        if (collision.tag == "Player" )
        {
            var player = collision.gameObject.GetComponent<Player>();
            var rigidBody = player.GetComponent<Rigidbody2D>();

            if(player.state == PlayerStateManager.Instance.playerGlideState)
            {
                rigidBody.AddForce(new Vector2(0, upPower));
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<Player>();
            var rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

            if(player.state == PlayerStateManager.Instance.playerGlideState)
            {
                var workVelocity = rigidBody.velocity;
                rigidBody.velocity = new Vector2(workVelocity.x, workVelocity.y);
            }

        }
    }

}
