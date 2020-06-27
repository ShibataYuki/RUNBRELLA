using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;

namespace Manual
{
    public class SceneController : MonoBehaviour
    {
        // UIを管理するマネージャー
        private UIManager uiManager;

        private void Start()
        {
            var uiObject = GameObject.Find("Canvas");
            uiManager = uiObject.GetComponent<UIManager>();
        }

        // Update is called once per frame
        void Update()
        {
            KeyCheck();
        }

        /// <summary>
        /// キー入力のチェック
        /// </summary>
        void KeyCheck()
        {
            // キー入力をチェック
            bool isLeftKey, isRightKey;
            KeyCheck(out isLeftKey, out isRightKey);
            // スクロール処理
            Scroll(isLeftKey, isRightKey);
        }

        /// <summary>
        /// スクロール処理
        /// </summary>
        /// <param name="isLeftKey">左のキー入力</param>
        /// <param name="isRightKey">右のキー入力</param>
        private void Scroll(bool isLeftKey, bool isRightKey)
        {
            // 右だけ、左だけの入力を受けたかチェック
            if (isLeftKey != isRightKey)
            {
                // 右の入力があったら
                if (isRightKey == true)
                {
                    // 右へのスクロールが出来るかチェック
                    if (uiManager.ScrollRightCheck() == false)
                    {
                        // キャラ選択画面に遷移
                        SceneManager.LoadScene("SelectMenu");
                    }
                }
                // 左の入力があったら
                else if (isLeftKey == true)
                {
                    // 左へのスクロールが出来るかチェック
                    if(uiManager.ScrollLeftCheck() == false)
                    {
                        // タイトル画面に戻る
                        SceneManager.LoadScene("Title");
                    }
                }
            }
        }

        /// <summary>
        /// キー入力をチェック
        /// </summary>
        /// <param name="isLeftKey"></param>
        /// <param name="isRightKey"></param>
        private static void KeyCheck(out bool isLeftKey, out bool isRightKey)
        {
            // 左へのキー入力があるかチェックした結果
            isLeftKey = (Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.LeftArrow));
            // 右へのキー入力があるかチェックした結果
            isRightKey = (Input.GetKeyDown(KeyCode.D) ||
                Input.GetKeyDown(KeyCode.RightArrow));
            // ゲームパッドのLボタンの入力をチェック
            isLeftKey |= GamePad.GetButtonDown
                (GamePad.Button.LeftShoulder, GamePad.Index.Any);
            // ゲームパッドのRボタンの入力をチェック
            isRightKey |= GamePad.GetButtonDown
                (GamePad.Button.RightShoulder, GamePad.Index.Any);
        }
    }
}