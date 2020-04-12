using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

namespace SelectMenu
{
    public class SelectCharacterManager : MonoBehaviour
    {
        // 参加するプレイヤーのキャラ選択画面のディクショナリー
        private Dictionary<int, SelectCharacter> selectCharacters = new Dictionary<int, SelectCharacter>();
        public Dictionary<int, SelectCharacter> SelectCharacters
        { get { return selectCharacters; } set { selectCharacters = value; } }

        private SelectPlayCount selectPlayCount;

        // Start is called before the first frame update
        void Start()
        {
            selectPlayCount = GetComponent<SelectPlayCount>();
        }

        /// <summary>
        /// キャラクター選択ステートのフレーム更新処理
        /// </summary>
        public void SelectCharacter()
        {
            for (int ID = 1; ID <= SceneController.Instance.MaxPlayerNumber; ID++)
            {
                // 参加してなければ
                if (SceneController.Instance.IsAccess[ID] == false)
                {
                    continue;
                }
                // キャラクター選択中なら
                if (SceneController.Instance.IsAccess[ID] == true && SceneController.Instance.IsSubmits[ID] == false)
                {
                    selectCharacters[ID].MoveCheck(ID);

                    // 決定ボタンをしたなら
                    if (GamePad.GetButtonDown(GamePad.Button.A, (GamePad.Index)ID))
                    {
                        // キャラクター決定処理
                        Submit(ID);
                    } // if
                    #region キーボード入力
                    if(GamePad.GetButton(GamePad.Button.A, GamePad.Index.Any) == false)
                    {
                        if (Input.GetKeyDown(KeyCode.Return) == true && SceneController.Instance.IsKeyBoard == false)
                        {
                            // キャラクター決定処理
                            Submit(ID);
                            SceneController.Instance.IsKeyBoard = true;
                        }
                    }
                    #endregion
                } // if
                  // キャラクター選択が完了していて
                else if (SceneController.Instance.IsSubmits[ID] == true)
                {
                    // Bボタンを押したなら
                    if (GamePad.GetButtonDown(GamePad.Button.B, (GamePad.Index)ID))
                    {
						// キャラクター選択をやり直す
                        SceneController.Instance.Cancel(ID);
                        continue;
                    }
                } // else if

            } // for

            // 誰かがXボタンを押したなら
            if (GamePad.GetButtonDown(GamePad.Button.X, GamePad.Index.Any))
            {
                // ステートを何本先取か選択するステートに変更
                SceneController.Instance.ChangeState(SceneController.Instance._selectPlayCountState);
                return;
            }

            #region キーボード入力
            else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                // ステートを何本先取か選択するステートに変更
                SceneController.Instance.ChangeState(SceneController.Instance._selectPlayCountState);
                SceneController.Instance.IsKeyBoard = true;
                return;
            }

            #endregion
            // 全員の入力が終わったかチェック
            SubmitCheck();
        } // SelectCharacter

        /// <summary>
        /// キャラクター決定処理
        /// </summary>
        /// <param name="ID"></param>
        private void Submit(int ID)
        {
            selectCharacters[ID].Submit();
            SceneController.Instance.IsSubmits[ID] = true;
        }

        /// <summary>
        /// 全員の入力が終わったかチェックするメソッド
        /// </summary>
        private void SubmitCheck()
        {
            // プレイヤーがいなければ
            if (SceneController.Instance.PlayerNumber <= 0)
            {
                // 入力を待つ
                return;
            } // if
            // 全員のキャラ選択が終わったかチェック
            for (int ID = 1; ID <= SceneController.Instance.MaxPlayerNumber; ID++)
            {
                // 参加していない場合
                if (SceneController.Instance.IsAccess[ID] == false)
                {
                    // チェックしない
                    continue;
                } // if

                // 一人でも入力中の人がいれば
                if (SceneController.Instance.IsSubmits[ID] == false)
                {
                    // 入力を待つ
                    return;
                } // if
            } // for

            // 入力の完了
            SceneController.Instance.ChangeState(SceneController.Instance._agreeCheckState);
        } // SubmitCheck

    } // class
} // namespace
