using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updraft : MonoBehaviour
{
    // 上昇気流の強さ
    [SerializeField]
    float upPower = 0f;

    private void Start()
    {
        // テキストからデータ読み込み
        Dictionary<string, float> textDataDic;
        textDataDic = SheetToDictionary.TextNameToData["Stage"];
       
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

            var character = collision.gameObject.GetComponent<Character>();
            var rigidBody = character.GetComponent<Rigidbody2D>();

            if(character.IsGlide)
            {
                rigidBody.AddForce(new Vector2(0, upPower));
            }
        }

    }

    /// <summary>
    /// 離れた時の処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var character = collision.gameObject.GetComponent<Character>();
            var rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

            if(character.IsGlide)
            {
                var workVelocity = rigidBody.velocity;
                rigidBody.velocity = new Vector2(workVelocity.x, workVelocity.y);
            }

        }
    }

}
