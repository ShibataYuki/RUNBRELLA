using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 空中状態のときにスピードをチェックして止まらないようにする
/// </summary>
public class PlayerAeriaSpeedCheck : MonoBehaviour
{
    // リジッドボディのコンポーネント
    private new Rigidbody2D rigidbody;
    // 最低速度
    [SerializeField]
    private float minSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 最低速度を下回っているかチェックして
    /// 遅かったら最低速度に変更
    /// </summary>
    public void SpeedCheck()
    {
        // 速度の取得
        var velocity = rigidbody.velocity;
        // 横方向の移動量が最低速度以下なら
        if (Mathf.Abs(velocity.x) < Mathf.Abs(minSpeed))
        {
            // 横方向の移動量を最低速度に変更
            velocity.x = minSpeed;
            rigidbody.velocity = velocity;
        }
    }
}
