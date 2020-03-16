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
    // ジャンプの音源
    [SerializeField]
    private AudioClip jumpSE = null;
    // SEのボリューム
    [SerializeField]
    private float SEVolume = 5;

    // 読み込むファイルのファイル名
    private readonly string fileName = nameof(PlayerJump) + "Data";

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        rigidbody = GetComponent<Rigidbody2D>();
        // ファイルの読み込み
        //jump.y = TextManager.Instance.GetValue(fileName, nameof(jump));
        //SEVolume = TextManager.Instance.GetValue(fileName, nameof(SEVolume));
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    public void Jump()
    {
        rigidbody.AddForce(jump, ForceMode2D.Impulse);
        // SEの再生
        AudioManager.Instance.PlaySE(jumpSE, SEVolume);
    }
}
