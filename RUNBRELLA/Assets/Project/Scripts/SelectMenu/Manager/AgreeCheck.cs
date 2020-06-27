using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class AgreeCheck : MonoBehaviour
    {
        // 同意しているか
        public enum IsAgree
        {
            Agree,    // 同意する
            Disagree, // 同意しない
        }
        // 同意しているか
        private IsAgree isAgree = IsAgree.Disagree;
        public IsAgree _isAgree { get { return isAgree; } }
        // 入力を管理するマネージャー
        private InputManager inputManager;

        // 同意する/同意しない を表すオブジェクトのディクショナリー
        private Dictionary<IsAgree, GameObject> isAgreeObjects = new Dictionary<IsAgree, GameObject>();

        // 何本先取かの表示/非表示を切り替えるコンポーネント
        private SelectPlayCount selectPlayCount = null;

        /// <summary>
        /// ステート終了時に行うメソッド
        /// </summary>
        public void AgreeCheckExit()
        {
            selectPlayCount.PlayCountHide();
        }

        /// <summary>
        /// ステート開始時に行うメソッド
        /// </summary>
        public void AgreeCheckEntry()
        {
            selectPlayCount.PlayCountOpen();
            // 同意していない状態に変更する
            isAgree = IsAgree.Disagree;
        }

        // Start is called before the first frame update
        void Start()
        {
            // コンポーネントの取得
            selectPlayCount = GetComponent<SelectPlayCount>();
            inputManager = GetComponent<InputManager>();
        }

        /// <summary>
        /// ルールについて同意するかチェック
        /// </summary>
        public void SubmitCheck()
        {
            // 何本先取か変更する
            selectPlayCount.SelectPlayCountEntry();

            for(var controllerNo = CONTROLLER_NO.CONTROLLER1; controllerNo <= CONTROLLER_NO.CONTROLLER4; controllerNo ++)
            {
                if(SceneController.Instance.IsAccess[controllerNo] == true)
                {
                    // キャンセル
                    if (inputManager.CancelKeyDown((GamepadInput.GamePad.Index)controllerNo) || Input.GetKeyDown(inputManager.CancelKeyCodes[(int)controllerNo - 1]))
                    {
                        SceneController.Instance.Cancel(controllerNo);
                        return;
                    }
                }
            }

            // 決定ボタンを押したなら
            if (inputManager.SubmitKeyDown(GamepadInput.GamePad.Index.Any))
            {
                // SEの再生
                SceneController.Instance.PlayEnterSE();
                // ステートの変更
                SceneController.Instance.EndStateStart();
                    return;
            } // if
            #region キーボード入力
            else if (Input.GetKeyDown(KeyCode.Return) && SceneController.Instance.IsKeyBoard == false)
            {
                SceneController.Instance.IsKeyBoard = true;
                // SEの再生
                SceneController.Instance.PlayEnterSE();
                // ステートの変更
                SceneController.Instance.EndStateStart();
                return;
            } // if
            #endregion
        }
    } // class
} // namespace

