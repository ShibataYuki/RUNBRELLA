using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    // ジャンプ力
    [SerializeField]
    private Vector2 jump = new Vector2(0.0f, 4.0f);
    // 必要なコンポーネント
    new Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    public void Jump()
    {
        rigidbody.AddForce(jump, ForceMode2D.Impulse);
    }
}
