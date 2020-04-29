using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamepadInput;

namespace SelectMenu
{
    public class SelectPlayCount : MonoBehaviour
    {
        // 何本先取か
        private int raceNumber = 3;
        public int RaceNumber { get { return raceNumber; } }
        // 入力をチェックするコンポーネント
        private InputManager inputManager;
		// 何本先取かを表示するオブジェクト
        private GameObject playCountObject;
		// 何本先取かを表示するテキスト
        private Text playCountText;
        private AgreeCheck agreeCheck;

        // Start is called before the first frame update
        void Start()
        {
            // 前回の値を取得
            raceNumber = GameManager.Instance.RaceNumber;
            // 自分のgameObjectからコンポーネントを取得
            inputManager = GetComponent<InputManager>();
            agreeCheck = GetComponent<AgreeCheck>();
			// 参照の取得
            playCountObject = GameObject.Find("Canvas/PlayCount");
			// Textからコンポーネントを取得
            playCountText = playCountObject.transform.Find("Text").gameObject.GetComponent<Text>();
			// テキストを非表示にする
            PlayCountHide();
        }

        private void Update()
        {
            // テキストの更新
            playCountText.text = string.Format("{0}本先取", raceNumber);
        }

        /// <summary>
        /// テキストの非表示
        /// </summary>
        public void PlayCountHide()
        {
            playCountObject.SetActive(false);
            agreeCheck.IsAgreeHide();
        }

        /// <summary>
        /// テキストの表示
        /// </summary>
        public void PlayCountOpen()
        {
            playCountObject.SetActive(true);
        }

        /// <summary>
        /// 何本先取か入力する人をチェックするメソッド
        /// </summary>
        public void SelectPlayCountEntry()
        {
            for (var controllerNo = CONTROLLER_NO.CONTROLLER1; controllerNo <= CONTROLLER_NO.CONTROLLER4; controllerNo++)
            {
                // 参加者なら
                if (SceneController.Instance.IsAccess[controllerNo] == true)
                {
                    // 何本先取か選択する
                    if(SceneController.Instance._state == SceneController.Instance._agreeCheckState)
                    {
                        // 何本先取か選択する
                        SelectCount(controllerNo);
                        // ステートが変化したなら
                        if(SceneController.Instance._state != SceneController.Instance._agreeCheckState)
                        {
                            return;
                        }
                    }
                    // Bボタンを押したなら
                    if (GamePad.GetButtonDown(GamePad.Button.B, (GamePad.Index)controllerNo) || Input.GetKeyDown(inputManager.CancelKeyCodes[(int)controllerNo]) == true)
                    {
                        // キャラクター選択に戻る
                        SceneController.Instance.Cancel(controllerNo);
                        return;
                    } // if
                } // if
            } // for

            // 誰かがXボタンを押したら
            if (GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Any))
            {
                // ステートを変更
                SceneController.Instance.ChangeState(SceneController.Instance._selectCharacterState);
                return;
            } // if
            #region キーボード入力
            else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                // ステートを変更
                SceneController.Instance.ChangeState(SceneController.Instance._selectCharacterState);
                // SEの再生
                SceneController.Instance.PlayEnterSE();
                SceneController.Instance.IsKeyBoard = true;
                return;
            }
            #endregion
        } // SelectPlayCountEntry


            /// <summary>
            /// 何本先取か決める個別の入力
            /// </summary>
            /// <returns></returns>
            private void SelectCount(CONTROLLER_NO controllNo)
        {
            // 左右に押し倒されたかチェック
            if(inputManager.RightShoulderKeyDown((GamePad.Index)controllNo))
            {
                raceNumber++;
                // SE再生
                SceneController.Instance.PlayChoiseSE();
            }
            if(inputManager.LeftShoulderKeyDown((GamePad.Index)controllNo))
            {
                raceNumber--;
                // SE再生
                SceneController.Instance.PlayChoiseSE();
            }
            #region キーボード入力
            if (inputManager.RightShoulderKeyDown((GamePad.Index.Any)) == false &&
                (Input.GetKeyDown(inputManager.RightKeyCodes[(int)controllNo])) && 
                SceneController.Instance.IsKeyBoard == false)
            {
                raceNumber++;
                // SE再生
                SceneController.Instance.PlayChoiseSE();
                SceneController.Instance.IsKeyBoard = true;
            }
            if (inputManager.LeftShoulderKeyDown((GamePad.Index.Any)) == false && 
                Input.GetKeyDown(inputManager.LeftKeyCodes[(int)controllNo]) &&
                SceneController.Instance.IsKeyBoard == false)
            {
                SceneController.Instance.IsKeyBoard = true;
                // SE再生
                SceneController.Instance.PlayChoiseSE();
                raceNumber--;
            }
            #endregion
            // １～３の間に収める
            raceNumber = Mathf.Clamp(raceNumber, 1, 3);
        } // SelectCount

    } // class
} // namesace
