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
            for (int ID = 1; ID <= SceneController.Instance.MaxPlayerNumber; ID++)
            {
                // 参加者なら
                if (SceneController.Instance.IsAccess[ID] == true)
                {
                    // 何本先取か選択する
                    if(SceneController.Instance._state == SceneController.Instance._selectPlayCountState)
                    {
                        // 何本先取か選択する
                        SelectCount(ID);
                        // ステートが変化したなら
                        if(SceneController.Instance._state != SceneController.Instance._selectPlayCountState)
                        {
                            return;
                        }
                    }
                    // Bボタンを押したなら
                    if (GamePad.GetButtonDown(GamePad.Button.B, (GamePad.Index)ID))
                    {
                        // キャラクター選択に戻る
                        SceneController.Instance.Cancel(ID);
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

        } // SelectPlayCountEntry


        /// <summary>
        /// 何本先取か決める個別の入力
        /// </summary>
        /// <returns></returns>
        private void SelectCount(int ID)
        {
            // 左右に押し倒されたかチェック
            if(inputManager.RightShoulderKeyDown((GamePad.Index)ID))
            {
                raceNumber++;
            }
            if(inputManager.LeftShoulderKeyDown((GamePad.Index)ID))
            {
                raceNumber--;
            }
            // １～３の間に収める
            raceNumber = Mathf.Clamp(raceNumber, 1, 3);
        } // SelectCount

    } // class
} // namesace
