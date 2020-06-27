using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // 1秒間にビューポートのサイズの何倍スクロールするか
    [SerializeField]
    float scrollSpeed = -1;
    //差分
    float Difference;
    Vector3 cameraRectMin;
    // ワールド座標においてのビューポートのサイズ
    private float viewPortSize;


    // Start is called before the first frame update
    void Start()
    {
        //カメラの範囲を取得
        //カメラの左下の座標
        cameraRectMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z));
        // ワールド座標においてのビューポートのサイズ
        viewPortSize = (Camera.main.transform.position.x - (Camera.main.ViewportToWorldPoint(Vector3.zero)).x) * 2;
        // スピードをセット
        SetScrollSpeed();
        // シートの読み込みが終わり次第もう一回パラメータをセットしなおす
        StartCoroutine(RoadSheetCheck());
    }

    /// <summary>
    /// シートの読み込みをチェックして、完了したらパラメータを変更する
    /// </summary>
    /// <returns></returns>
    IEnumerator RoadSheetCheck()
    {
        // シートからの読み込みが完了しているのなら
        if (SheetToDictionary.Instance.IsCompletedSheetToText == true)
        {
            // コルーチンを終了
            yield break;
        }
        while (true)
        {
            // スプレッドシートの読み込みが完了したのなら
            if (SheetToDictionary.Instance.IsCompletedSheetToText == true)
            {
                // パラメータをテキストから読み込んで、speedを変更
                SetScrollSpeed();
                yield break;
            }
            // 1フレーム待機する
            yield return null;
        }
    }

    /// <summary>
    /// テキストからスクロールスピードをセット
    /// </summary>
    void SetScrollSpeed()
    {
        // 読み込むテキストの名前
        var textName = "Title";
        try
        {
            // テキストの中のデータをセットするディクショナリー
            SheetToDictionary.Instance.TextToDictionary(textName, out var moveDictionary);
            try
            {
                // スクロールスピードをセット
                scrollSpeed = 1 / moveDictionary["背景が1スクロールするのにかかる秒数"];
            }
            catch
            {
                Debug.Assert(false, nameof(Background) + "でエラーが発生しました");
            }
        }
        catch
        {
            Debug.Assert(false, nameof(SheetToDictionary.TextToDictionary) + "から"
                + textName + "の読み込みに失敗しました。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }

    void Move()
    {
        // 値の変更
        var position = transform.position;
        position.x -= (viewPortSize * scrollSpeed * Time.deltaTime);   //X軸方向にスクロール
        transform.position = position;

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
