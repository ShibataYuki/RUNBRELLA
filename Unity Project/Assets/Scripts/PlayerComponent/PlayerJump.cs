using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    // ジャンプ力
    [SerializeField]
    private Vector2 jump = new Vector2(0.0f, 4.0f);
    // 必要なコンポーネント
    Rigidbody2D playerRigidbody;
    // ジャンプの音源
    [SerializeField]
    private AudioClip jumpSE = null;
    // SEのボリューム
    [SerializeField]
    private float SEVolume = 5;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        playerRigidbody = GetComponent<Rigidbody2D>();
        var character = GetComponent<Character>();
        // 読み込むファイルのファイル名
        ReadTextParameter(character);
    }

    /// <summary>
    /// テキストからパラメータを読み込む
    /// </summary>
    private void ReadTextParameter(Character character)
    {
        // 読み込むテキストの名前
        var textName = "";
        switch (character.charType)
        {
            case GameManager.CHARTYPE.PlayerA:
                textName = "Chara_A";
                break;
            case GameManager.CHARTYPE.PlayerB:
                textName = "Chara_B";
                break;
        }
        try
        {
            // テキストの中のデータをセットするディクショナリー
            SheetToDictionary.Instance.TextToDictionary(textName, out var jumpDictionary);

            try
            {
                // ファイル読み込み
                jump.y = jumpDictionary["ジャンプ力(重力を考えなかった場合に1秒間に上に移動する移動量)"];
                SEVolume = jumpDictionary["ジャンプ時のSEのボリューム"];
            }
            catch
            {
                Debug.Assert(false, nameof(PlayerJump) + "でエラーが発生しました");
            }
        }
        catch
        {
            Debug.Assert(false, nameof(SheetToDictionary.TextToDictionary) + "から"
                + textName + "の読み込みに失敗しました。");
        }
    }



    /// <summary>
    /// ジャンプ処理
    /// </summary>
    public void Jump()
    {
        playerRigidbody.AddForce(jump, ForceMode2D.Impulse);
        // SEの再生
        AudioManager.Instance.PlaySE(jumpSE, SEVolume);
    }
}
