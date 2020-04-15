using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    [SerializeField]
    float scrollSpeed = -1;
    //差分
    float Difference;
    Vector3 cameraRectMin;


    // Start is called before the first frame update
    void Start()
    {
        //カメラの範囲を取得
        //カメラの左下の座標
        cameraRectMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z));

    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }

    void Move()
    {
        transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);   //X軸方向にスクロール

        //カメラの左端から完全に出たら、右端に瞬間移動
        if (transform.position.x < (cameraRectMin.x - Camera.main.transform.position.x) * 2)
        {
            //差分を求めている(右にずれないように)
            Difference = transform.position.x - (cameraRectMin.x - Camera.main.transform.position.x) * 2;
            //瞬間移動
            transform.position = new Vector2(((Camera.main.transform.position.x - cameraRectMin.x) * 2)+ Difference-0.01f, transform.position.y);
        }


    }

}
