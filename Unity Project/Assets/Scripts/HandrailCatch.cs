using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandrailCatch : MonoBehaviour
{
    // 傘の柄
    private GameObject grip = null;
    // グリップの判定
    private Vector2 gripPoint;
    // グリップの半径
    private float gripRadius;

    // 手すりのレイヤー
    [SerializeField]
    private LayerMask handrail = 0;
    // 手すりの上の空間のレイヤー
    [SerializeField]
    private LayerMask handrailArea = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 傘の柄を探す
        grip = transform.Find("Grip").gameObject;
        // コライダーを取得
        var gripCollider = grip.GetComponent<CircleCollider2D>();
        // 当たり判定の領域を計算
        gripPoint  = gripCollider.offset;
        gripRadius = gripCollider.radius;
        // グリップを非表示にする
        grip.SetActive(false);
    }

    /// <summary>
    /// 手すりの上にいるか、また傘の柄が手すりにかかってないかチェック
    /// </summary>
    /// <returns></returns>
    public bool HandrailCheck()
    {
        // 現在の手すりの位置
        var nowGripPoint = gripPoint + (Vector2)transform.position;
        // 手すりの上にいるか、チェックする
        if (Physics2D.OverlapCircle(nowGripPoint, gripRadius, handrail))
        {
            // 手すりにかかってないか、チェックする
            if (!Physics2D.OverlapCircle(nowGripPoint, gripRadius, handrailArea))
            {
                return true;
            }
        }
        return false;
    }
}
