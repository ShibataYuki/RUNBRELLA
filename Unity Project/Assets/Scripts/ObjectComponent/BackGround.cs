using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    // 前フレームのカメラのポジション
    private Vector3 beforeCameraPos;
    // カメラの移動に対しての背景の移動量
    [SerializeField]
    private float backGroundMoveScale = 0.5f;


    // Start is called before the first frame update
    void Start()
    {

        // カメラのポジションをセット
        beforeCameraPos = Camera.main.transform.position;
        // ファイル読み込み
        SetMoveScale();
    }

    /// <summary>
    /// テキストからパラメータを読み込んでセットする
    /// </summary>
    void SetMoveScale()
    {
        var textName = "Stage";
        try
        {
            // テキストの中のデータをセットするディクショナリー
            SheetToDictionary.Instance.TextToDictionary(textName, out var moveDictionary);
            try
            {
                // オブジェクトの名前に応じて異なるパラメータを読み込み
                switch (name)
                {
                    case "Cityscape_Front":
                        backGroundMoveScale = moveDictionary["手前側の背景のスクロール量"];
                        break;
                    case "Cityscape_Back":
                        backGroundMoveScale = moveDictionary["奥側の背景のスクロール量"];
                        break;
                }
            }
            catch
            {
                Debug.Assert(false, nameof(BackGround) + "でエラーが発生しました");
            }

        }
        catch
        {
            Debug.Assert(false, nameof(SheetToDictionary.TextToDictionary) + "から"
                + textName + "の読み込みに失敗しました。");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 現在のカメラのポジション
        var cameraPos = Camera.main.transform.position;
        // 1フレームでのカメラの移動量
        var cameraMove = (cameraPos - beforeCameraPos);
        // カメラの移動に応じて背景を移動
        transform.position += (cameraMove * backGroundMoveScale);
        // カメラのポジションをセット
        beforeCameraPos = cameraPos;
    }
}
