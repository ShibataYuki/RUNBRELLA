using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    if (Input.GetButtonDown(string.Format("Player{0}Submit", ID)))
                    {
                        // キャラクター決定処理
                        SceneController.Instance.IsSubmits[ID] = true;
                        selectPlayCount.KeyFlag.Add(ID, true);
                    } // if
                } // if
				// キャラクター選択が完了していて
                else if (SceneController.Instance.IsSubmits[ID] == true)
                {
					// Bボタンを押したなら
                    if (Input.GetButtonDown(string.Format("Player{0}Cancel", ID)))
                    {
						// キャラクター選択をやり直す
                        SceneController.Instance.Cancel(ID);
                        continue;
                    }
                } // else if
            } // for

            // 全員の入力が終わったかチェック
            SubmitCheck();
        } // SelectCharacter

        /// <summary>
        /// 全員の入力が終わったかチェックするメソッド
        /// </summary>
        private void SubmitCheck()
        {
            // プレイヤーがいなければ
            if (SceneController.Instance.PlayerNumber <= 0)
            {
                // 入力を待つ
                SceneController.Instance._state = SceneController.State.SelectCharacter;
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
                    SceneController.Instance._state = SceneController.State.SelectCharacter;
                    return;
                } // if
            } // for

            // 入力の完了
            SceneController.Instance._state = SceneController.State.SelectPlayCount;
            // テキストの表示
            selectPlayCount.PlayCountOpen();
        } // SubmitCheck

    } // class
} // namespace
