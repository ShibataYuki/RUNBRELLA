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
        // 同意しているかどうかを表すオブジェクト
        private GameObject isAgreeObject;
        // 入力を管理するマネージャー
        private InputManager inputManager;

        // 同意する/同意しない を表すオブジェクトのディクショナリー
        private Dictionary<IsAgree, GameObject> isAgreeObjects = new Dictionary<IsAgree, GameObject>();

        // 何本先取かの表示/非表示を切り替えるコンポーネント
        private SelectPlayCount selectPlayCount = null;

        /// <summary>
        /// 同意しているかどうかを表すオブジェクトを非表示にする
        /// </summary>
        public void IsAgreeHide()
        {
            isAgreeObject.SetActive(false);
        }

        /// <summary>
        /// ステート終了時に行うメソッド
        /// </summary>
        public void AgreeCheckExit()
        {
            IsAgreeHide();
            selectPlayCount.PlayCountHide();
        }

        /// <summary>
        ///同意しているかどうかを表すオブジェクトを表示する
        /// </summary>
        public void IsAgreeOpen()
        {
            isAgreeObject.SetActive(true);
        }

        /// <summary>
        /// ステート開始時に行うメソッド
        /// </summary>
        public void AgreeCheckEntry()
        {
            selectPlayCount.PlayCountOpen();
            IsAgreeOpen();
            // 同意していない状態に変更する
            isAgree = IsAgree.Disagree;
        }

        // Start is called before the first frame update
        void Start()
        {
            // コンポーネントの取得
            selectPlayCount = GetComponent<SelectPlayCount>();
            inputManager = GetComponent<InputManager>();

            // 参照の取得
            var canvas = GameObject.Find("Canvas");
            isAgreeObject = canvas.transform.Find("PlayCount/AgreeCheck").gameObject;

            // 配列にセット
            for (var i = IsAgree.Agree; i <= IsAgree.Disagree; i++)
            {
                // 子オブジェクトから文字列探索
                isAgreeObjects.Add(i,isAgreeObject.transform.Find(i.ToString()).gameObject);
            }

            IsAgreeHide();
        }

        /// <summary>
        /// ルールについて同意するかチェック
        /// </summary>
        public void SubmitCheck()
        {
            selectPlayCount.SelectPlayCountEntry();

            for(var controllerNo = CONTROLLER_NO.CONTROLLER1; controllerNo <= CONTROLLER_NO.CONTROLLER4; controllerNo ++)
            {
                if(SceneController.Instance.IsAccess[controllerNo] == true)
                {
                    // キャンセル
                    if (inputManager.CancelKeyDown((GamepadInput.GamePad.Index)controllerNo) || Input.GetKeyDown(inputManager.CancelKeyCodes[(int)controllerNo]))
                    {
                        SceneController.Instance.Cancel(controllerNo);
                        return;
                    }
                }
            }

            // 決定ボタンを押したなら
            if (inputManager.StartKeyDown(GamepadInput.GamePad.Index.Any))
            {
                // SEの再生
                SceneController.Instance.PlayEnterSE();
                // ステートの変更
                SceneController.Instance.ChangeState(SceneController.Instance._selectMenuEndState);
                    return;
            } // if
            #region キーボード入力
            else if (Input.GetKeyDown(KeyCode.Return) && SceneController.Instance.IsKeyBoard == false)
            {
                SceneController.Instance.IsKeyBoard = true;
                // SEの再生
                SceneController.Instance.PlayEnterSE();
                // ステートの変更
                SceneController.Instance.ChangeState(SceneController.Instance._selectMenuEndState);
                return;
            } // if
            #endregion
        }
    } // class
} // namespace

