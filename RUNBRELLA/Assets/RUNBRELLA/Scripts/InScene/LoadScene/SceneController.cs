using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoadScene
{
    public class SceneController : MonoBehaviour
    {        
        // データ読み込みのエラーを通知するポップアップ
        GameObject errorPop = null;
        private void Start()
        {
            // 非アクティブ状態のオブジェクト検索のために親オブジェクトを取得
            GameObject canvas = GameObject.Find("Canvas");
            // エラーポップのFind
            errorPop = canvas.transform.Find("ErrorPop").gameObject;            
        }
        // Update is called once per frame
        void Update()
        {
            // csvの読み込みエラーポップが出ていたらキー入力待機
            if (errorPop.activeInHierarchy)
            {
                // キーボード「Enter」かゲームパットのボタン入力でゲームを終了させる
                if (Input.GetKeyDown(KeyCode.Return) || GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                    UnityEngine.Application.Quit();
#endif
                }
                return;
            }

            // スプレッドシートからのデータ読み込み完了を待ってシーン移行
            if (SheetToDictionary.Instance.IsCompletedSheetToText)
            {

                //インデックスに応じた終了処理を行う
                SceneManager.LoadScene("Title");

            }
        }
    }
}

