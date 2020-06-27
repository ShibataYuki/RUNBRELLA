using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SelectMenu
{
    public class Charaselect : MonoBehaviour
    {
        // 画像のコンポーネント
        Image image;
        // 消えてから表示されるまでの時間
        float showTime = 3.0f;
        // 現れてから消えるまでの時間
        float hideTime = 3.0f;

        // ステート
        enum State
        {
            Show,
            Hide,
        }
        // 現在のステート
        State state = State.Show;

        // コルーチン
        IEnumerator corutine;

        // Start is called before the first frame update
        void Start()
        {
            // コンポーネントの取得
            image = GetComponent<Image>();
            // 表示時間と非表示時間をセット
            SetTime();
            // シートの読み込みが終わり次第もう一回パラメータをセットしなおす
            StartCoroutine(RoadSheetCheck());
        }

        /// <summary>
        /// テキストから表示時間と非表示時間を読み込んでメンバー変数にセット
        /// </summary>
        void SetTime()
        {
            try
            {
                // ディクショナリーを取得
                SheetToDictionary.Instance.TextToDictionary(SceneController.Instance.textName, out var charSelectDictionary);
                try
                {
                    showTime = charSelectDictionary["プレイヤーのNoが表示されるまでの秒数"];
                    hideTime = charSelectDictionary["プレイヤーのNoが表示されている秒数"];
                }
                catch
                {
                    Debug.Assert(false, nameof(Charaselect) + "でエラーが発生しました");
                }
            }
            catch
            {
                Debug.Assert(false, nameof(SheetToDictionary.TextToDictionary) + "から" +
                SceneController.Instance.textName+ "のディクショナリーを取得できませんでした。");
            }
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
                    // パラメータをテキストから読み込んで、時間を変更
                    SetTime();
                    yield break;
                }
                // 1フレーム待機する
                yield return null;
            }
        }


        // Update is called once per frame
        void Update()
        {
            // コルーチンがNUllなら
            if (corutine == null)
            {
                // ステートに応じたコルーチンをセット
                switch (state)
                {
                    case State.Show:
                        corutine = ShowImage();
                        break;
                    case State.Hide:
                        corutine = HideImage();
                        break;
                }
                // コルーチンの開始
                StartCoroutine(corutine);
            }
        }

        /// <summary>
        /// 一定時間後に画像を不透明にする
        /// </summary>
        /// <returns></returns>
        IEnumerator ShowImage()
        {
            // 透明にする
            var color = image.color;
            color.a = 0.0f;
            image.color = color;
            // 一定時間待機する
            yield return new WaitForSeconds(showTime);
            // 不透明にする
            color.a = 1.0f;
            image.color = color;
            // ステートの変更
            state = State.Hide;
            corutine = null;
        } // ShowImage

        /// <summary>
        /// 一定時間後に画像を透明にする
        /// </summary>
        /// <returns></returns>
        IEnumerator HideImage()
        {
            // 不透明にする
            var color = image.color;
            color.a = 1.0f;
            image.color = color;
            // 一定時間待機する
            yield return new WaitForSeconds(hideTime);
            // 透明にする
            color.a = 0.0f;
            image.color = color;
            // ステートの変更
            state = State.Show;
            corutine = null;
        } // HideImage

        /// <summary>
        /// 画像を非表示にする
        /// </summary>
        public void Close()
        {
            // ステートを変更
            state = State.Show;
            // 透明にする
            var color = image.color;
            color.a = 0.0f;
            image.color = color;
            // コルーチンが入っているなら
            if (corutine != null)
            {
                // コルーチンの終了
                StopCoroutine(corutine);
                // コルーチンを削除
                corutine = null;
            }
            // 非表示にする
            gameObject.SetActive(false);
        } // Close
    } // class
}