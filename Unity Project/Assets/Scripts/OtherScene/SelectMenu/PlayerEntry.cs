using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamepadInput;

namespace SelectMenu
{
    public class PlayerEntry : MonoBehaviour
    {
        // シャッターの開くスピード
        private float shutterSpeed = 2;

        /// <summary>
        /// 新たな参加者がいないかチェックするメソッド
        /// </summary>
        public void EntryCheck()
        {
            for (int ID = 1; ID < SceneController.Instance.MaxPlayerNumber; ID++)
            {
                // 参加していない場合
                if (SceneController.Instance.IsAccess[ID] == false)
                {
                    // 参加表明をしたなら
                    if (GamePad.GetButtonDown(GamePad.Button.A,(GamePad.Index)ID))
                    {
                        // 参加処理
                        Participate(ID);
                    }
                } // if
            } // for
        } // EntryCheck

        /// <summary>
        /// 参加処理
        /// </summary>
        /// <param name="ID">ジョイスティックのID</param>
        void Participate(int ID)
        {
            // ゲームマネージャーにセット
            GameManager.Instance.playerIDs.Add(ID);
            // シーンコントローラーセット
            SceneController.Instance.IsAccess[ID] = true;
            SceneController.Instance.IsSubmits.Add(ID, false);
            SceneController.Instance.PlayerNumber++;
            // 
            var selectPlayerImage = GameObject.Find(string.Format("Canvas/SelectPlayerImage{0}", SceneController.Instance.PlayerNumber));
            // 
            var selectCharacter = selectPlayerImage.GetComponent<SelectCharacter>();
            // ディクショナリーに追加
            SceneController.Instance._selectCharacterManager.SelectCharacters.Add(ID, selectCharacter);
            // キャラクター選択に戻す
            SceneController.Instance._state = SceneController.State.SelectCharacter;
            // シャッターを開く
            StartCoroutine(ShutterOpen(ID));
        } // Participate

        /// <summary>
        /// シャッターを開くメソッド
        /// </summary>
        /// <param name="ID">ジョイスティックの番号</param>
        /// <returns></returns>
        IEnumerator ShutterOpen(int ID)
        {
            // 移動中のフラグを立てる
            SceneController.Instance._selectCharacterManager.SelectCharacters[ID].IsMove = true;
            // 開くシャッターのスクロールバーの参照を取得
            var selectPlayerImage = SceneController.Instance._selectCharacterManager.SelectCharacters[ID].gameObject;
            var scrollView = selectPlayerImage.transform.Find("Scroll View Shutter").gameObject;
            var scrollRectScript = scrollView.GetComponent<ScrollRect>();
            // スクロール量の割合
            var value = scrollRectScript.verticalNormalizedPosition;

            while (true)
            {
                // 値の変更
                value -= shutterSpeed * Time.deltaTime;
                // 0～1の間に収めるように変更
                value = Mathf.Clamp(value, 0.0f, 1.0f);
                // スクロールビューに値をセット
                scrollRectScript.verticalNormalizedPosition = value;
                // シャッターが上がったら
                if(value <= 0.0f)
                {
                    // 移動中のフラグをオフにする
                    SceneController.Instance._selectCharacterManager.SelectCharacters[ID].IsMove = false;
                    // コルーチンの終了
                    yield break;
                }
                // 次のフレームまで待つ
                yield return null;
            } // while
        } // IEnumerator
    } // class
} // namespace
