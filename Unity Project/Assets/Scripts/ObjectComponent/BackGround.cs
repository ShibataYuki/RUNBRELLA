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

    private readonly string fileName = nameof(BackGround) + "Data";

    // Start is called before the first frame update
    void Start()
    {
        // カメラのポジションをセット
        beforeCameraPos = Camera.main.transform.position;
        backGroundMoveScale = TextManager.Instance.GetValue(fileName, nameof(backGroundMoveScale));
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
