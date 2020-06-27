using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

namespace SelectMenu
{
    public class SelectCharacterManager : MonoBehaviour
    {
        // 参加するプレイヤーのキャラ選択画面のディクショナリー
        private Dictionary<CONTROLLER_NO, SelectCharacter> selectCharacters = new Dictionary<CONTROLLER_NO, SelectCharacter>();
        public Dictionary<CONTROLLER_NO, SelectCharacter> SelectCharacters
        { get { return selectCharacters; } set { selectCharacters = value; } }

        // 必要なコンポーネント
        private SelectPlayCount selectPlayCount;

        private PlayerImageManager playerImageManager = null;

        private InputManager inputManager;
        // プレイヤーの画像のマネージャー
        private PlayerImageManager imageManager;

        // Start is called before the first frame update
        void Start()
        {
            // コンポーネントを取得
            selectPlayCount = GetComponent<SelectPlayCount>();
            playerImageManager = GetComponent<PlayerImageManager>();
            inputManager = GetComponent<InputManager>();
            imageManager = GetComponent<PlayerImageManager>();
        }

        /// <summary>
        /// キャラクター選択ステートのフレーム更新処理
        /// </summary>
        public void SelectCharacter()
        {
            for (var controllerNo = CONTROLLER_NO.CONTROLLER1; controllerNo <= CONTROLLER_NO.CONTROLLER4; controllerNo++)
            {
                // 参加してなければ
                if (SceneController.Instance.IsAccess[controllerNo] == false)
                {
                    continue;
                }
                // キャラクター選択中なら
                if (SceneController.Instance.IsAccess[controllerNo] == true && SceneController.Instance.IsSubmits[controllerNo] == false)
                {
                    selectCharacters[controllerNo].MoveCheck(controllerNo);

                    // 決定ボタンをしたなら
                    if (GamePad.GetButtonDown(GamePad.Button.A, (GamePad.Index)controllerNo))
                    {
                        // キャラクター決定処理
                        Submit(controllerNo);
                    } // if
                    // Bボタンを押したなら
                    if (GamePad.GetButtonDown(GamePad.Button.B, (GamePad.Index)controllerNo))
                    {
                        // キャンセルフラグをONにする
                        inputManager.IsChanselDictionary[controllerNo] = true;
                    }
                    #region キーボード入力
                    else if(GamePad.GetButton(GamePad.Button.A, GamePad.Index.Any) == false)
                    {
                        if (Input.GetKeyDown(KeyCode.Return) == true && SceneController.Instance.IsKeyBoard == false)
                        {
                            // キャラクター決定処理
                            Submit(controllerNo);
                            SceneController.Instance.IsKeyBoard = true;
                        }
                    }
                    #endregion
                } // if
                // キャラクター選択が完了していて
                else if (SceneController.Instance.IsSubmits[controllerNo] == true)
                {
                    // Bボタンを押したなら
                    if (GamePad.GetButtonDown(GamePad.Button.B, (GamePad.Index)controllerNo) || Input.GetKeyDown(inputManager.CancelKeyCodes[(int)controllerNo - 1]) == true)
                    {
						// キャラクター選択をやり直す
                        SceneController.Instance.Cancel(controllerNo);
                        continue;
                    }
                } // else if

            } // for

            // 全員の入力が終わったかチェック
            SubmitCheck();
        } // SelectCharacter

        /// <summary>
        /// キャラクター決定処理
        /// </summary>
        /// <param name="controllerNo"></param>
        private void Submit(CONTROLLER_NO controllerNo)
        {
            selectCharacters[controllerNo].Submit();
            SceneController.Instance.IsSubmits[controllerNo] = true;
            // SE再生
            SceneController.Instance.PlayEnterSE();
            // 選択したキャラクターのボイスを再生
            //var selectCharaIndex = SelectCharacters[controllerNo].SelectCharacterNumber;
            //var selectVoice = SceneController.Instance._characterMessages[selectCharaIndex].audioClip;
            //var audioSorce = GetComponent<AudioSource>();
            //audioSorce.PlayOneShot(selectVoice, 1f);
            imageManager.PlayerImageEntry(controllerNo);
        }

        /// <summary>
        /// 全員の入力が終わったかチェックするメソッド
        /// </summary>
        private void SubmitCheck()
        {
            // プレイヤーが2人いなければ
            if (SceneController.Instance.PlayerNumber < 2)
            {
                // 入力を待つ
                return;
            } // if
            // 全員のキャラ選択が終わったかチェック
            for (var controllerNo = CONTROLLER_NO.CONTROLLER1; controllerNo <= CONTROLLER_NO.CONTROLLER4; controllerNo++)
            {
                // 参加していない場合
                if (SceneController.Instance.IsAccess[controllerNo] == false)
                {
                    // チェックしない
                    continue;
                } // if

                // 一人でも入力中の人がいれば
                if (SceneController.Instance.IsSubmits[controllerNo] == false)
                {
                    // 入力を待つ
                    return;
                } // if
            } // for
            // 入力の完了
            SceneController.Instance.AgreeCheckStart();
        } // SubmitCheck

    } // class
} // namespace
