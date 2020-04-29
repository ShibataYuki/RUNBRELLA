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

        InputManager inputManager;

        private void Start()
        {
            inputManager = GetComponent<InputManager>();
            // 読み込むテキストの名前
            var fileName = string.Format("{0}Data", nameof(PlayerEntry));
            shutterSpeed = TextManager.Instance.GetValue_float(fileName, nameof(shutterSpeed));
        }

        /// <summary>
        /// 新たな参加者がいないかチェックするメソッド
        /// </summary>
        public void EntryCheck()
        {
            for (var controllerNo = CONTROLLER_NO.CONTROLLER1; controllerNo <= CONTROLLER_NO.CONTROLLER4; controllerNo++)
            {
                // 参加していない場合
                if (SceneController.Instance.IsAccess[controllerNo] == false)
                {
                    // 参加表明をしたなら
                    if (inputManager.AnyKeyIn((GamePad.Index)(controllerNo)))
                    {
                        // 参加処理
                        Participate(controllerNo);
                    }

                    #region キーボード入力
                    else if (Input.GetKeyDown(inputManager.EntryKeyCodes[(int) controllerNo]))
                    {
                        // 参加処理
                        Participate(controllerNo);
                        SceneController.Instance.IsKeyBoard = true;
                        return;
                    }
                    #endregion
                } // if
            } // for
        } // EntryCheck

        /// <summary>
        /// 参加処理
        /// </summary>
        /// <param name="controllerNo">ジョイスティックのNo</param>
        void Participate(CONTROLLER_NO controllerNo)
        {
            // ゲームマネージャーにセット
            GameManager.Instance.playerAndControllerDictionary.Add((PLAYER_NO)SceneController.Instance.PlayerNumber, controllerNo);
            // シーンコントローラーセット
            SceneController.Instance.IsAccess[controllerNo] = true;
            SceneController.Instance.IsSubmits.Add(controllerNo, false);
            SceneController.Instance.PlayerNumber++;
            // 選択したプレイヤーの表示
            var selectPlayerImage = GameObject.Find(string.Format("Canvas/SelectPlayerImage{0}", SceneController.Instance.PlayerNumber));
            // 選択したプレイヤーのコンポーネントの取得
            var selectCharacter = selectPlayerImage.GetComponent<SelectCharacter>();
            // ディクショナリーに追加
            SceneController.Instance._selectCharacterManager.SelectCharacters.Add(controllerNo, selectCharacter);
            // キャラクター選択に戻す
            SceneController.Instance.ChangeState(SceneController.Instance._selectCharacterState);
            // 最初のキャラを変更
            for(int i = 1; i < SceneController.Instance.PlayerNumber; i++)
            {
                selectCharacter.IndexUp();
            }
            // シャッターを開く
            StartCoroutine(ShutterOpen(controllerNo));
            // SEの再生
            SceneController.Instance.PlayEnterSE();
        } // Participate

        /// <summary>
        /// シャッターを開くメソッド
        /// </summary>
        /// <param name="controllerNo">ジョイスティックの番号</param>
        /// <returns></returns>
        IEnumerator ShutterOpen(CONTROLLER_NO controllerNo)
        {
            // 移動中のフラグを立てる
            SceneController.Instance._selectCharacterManager.SelectCharacters[controllerNo].IsMove = true;
            // 開くシャッターのスクロールバーの参照を取得
            var selectPlayerImage = SceneController.Instance._selectCharacterManager.SelectCharacters[controllerNo].gameObject;
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
                    SceneController.Instance._selectCharacterManager.SelectCharacters[controllerNo].IsMove = false;
                    // コルーチンの終了
                    yield break;
                }
                // 次のフレームまで待つ
                yield return null;
            } // while
        } // IEnumerator
    } // class
} // namespace
