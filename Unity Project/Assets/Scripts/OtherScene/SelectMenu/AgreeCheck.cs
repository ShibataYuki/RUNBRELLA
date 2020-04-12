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

        // 拡大縮小を行うコンポーネント
        private ScalingAnimation scalingAnimation;

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
            IsAgreeOpen();
            selectPlayCount.PlayCountOpen();
            // 同意していない状態に変更する
            isAgree = IsAgree.Disagree;
        }

        // Start is called before the first frame update
        void Start()
        {
            // コンポーネントの取得
            scalingAnimation = GetComponent<ScalingAnimation>();
            selectPlayCount = GetComponent<SelectPlayCount>();
            inputManager = GetComponent<InputManager>();

            // 参照の取得
            var playCountObject = GameObject.Find("Canvas/PlayCount");
            isAgreeObject = playCountObject.transform.Find("AgreeCheck").gameObject;

            // 配列にセット
            for (var i = IsAgree.Agree; i <= IsAgree.Disagree; i++)
            {
                // 子オブジェクトから文字列探索
                isAgreeObjects[i] = isAgreeObject.transform.Find(i.ToString()).gameObject;
            }
        }

        /// <summary>
        /// 選択中のアイコンを拡大縮小する
        /// </summary>
        public void Scaling()
        {
            for(var i = IsAgree.Agree; i <=  IsAgree.Disagree; i++)
            {
                // 選択中なら
                if(i == isAgree)
                {
                    // ゲームオブジェクトをセット
                    scalingAnimation.SetScalingObject(isAgreeObjects[i]);
                }
                // 選択中でないのなら
                else
                {
                    // 元の大きさに変更する
                    isAgreeObjects[i].transform.localScale = Vector3.one;
                } // else
            } // for
        } // update

        /// <summary>
        /// ルールについて同意するかチェック
        /// </summary>
        public void SubmitCheck()
        {
            for(int ID = 1; ID <= SceneController.Instance.MaxPlayerNumber; ID ++)
            {
                if(SceneController.Instance.IsAccess[ID] == true)
                {
                    // 何本先取か選択する
                    SubmitCheck(ID);
                    // キャンセル
                    if (inputManager.CancelKeyDown((GamepadInput.GamePad.Index)ID) || Input.GetKeyDown(inputManager.CancelKeyCodes[ID]))
                    {
                        SceneController.Instance.Cancel(ID);

                        return;
                    }
                    // 何本先取かを選択
                    else if (inputManager.XKeyDown((GamepadInput.GamePad.Index)ID) || Input.GetKeyDown(inputManager.MenuKeyCodes[ID]))
                    {
                        SceneController.Instance.ChangeState(SceneController.Instance._selectPlayCountState);
                        return;
                    }
                }
            }

            // 決定ボタンを押したなら
            if (inputManager.SubmitKeyDown(GamepadInput.GamePad.Index.Any))
            {
                // 同意しているなら
                if (isAgree == IsAgree.Agree)
                {
                    // ステージに遷移
                    SceneController.Instance.GameStart();
                    return;
                } // if
                // 同意していないなら
                else
                {
                    // 何本先取か選択する
                    SceneController.Instance.ChangeState(SceneController.Instance._selectPlayCountState);
                    return;
                } // else
            } // if
            #region キーボード入力
            else if (Input.GetKeyDown(KeyCode.Return) && SceneController.Instance.IsKeyBoard == false)
            {
                SceneController.Instance.IsKeyBoard = true;
                // 同意しているなら
                if (isAgree == IsAgree.Agree)
                {
                    // ステージに遷移
                    SceneController.Instance.GameStart();
                    return;
                } // if
                // 同意していないなら
                else
                {
                    // 何本先取か選択する
                    SceneController.Instance.ChangeState(SceneController.Instance._selectPlayCountState);
                    return;
                } // else
            } // if

            #endregion
        }

        /// <summary>
        /// ルールについて同意するかチェック
        /// </summary>
        /// <param name="ID">ジョイスティックの番号</param>
        public void SubmitCheck(int ID)
        {
            // 左右にスティックを押し倒したかチェック
            if (inputManager.RightShoulderKeyDown((GamepadInput.GamePad.Index)ID))
            {
                isAgree++;
            }
            if (inputManager.LeftShoulderKeyDown((GamepadInput.GamePad.Index)ID))
            {
                isAgree--;
            }
            #region キーボード入力
            if (inputManager.RightShoulderKeyDown((GamepadInput.GamePad.Index.Any)) == false &&
                Input.GetKeyDown(inputManager.RightKeyCodes[ID]) &&
                SceneController.Instance.IsKeyBoard == false)
            {
                isAgree++;
                SceneController.Instance.IsKeyBoard = true;
            }
            if (inputManager.LeftShoulderKeyDown((GamepadInput.GamePad.Index.Any)) == false && 
                (Input.GetKeyDown(inputManager.LeftKeyCodes[ID])) && 
                SceneController.Instance.IsKeyBoard == false)
            {
                isAgree--;
                SceneController.Instance.IsKeyBoard = true;
            }
            #endregion
            // Agree と Disagree の間に収める
            isAgree = (IsAgree)Mathf.Clamp((int)isAgree, (int)IsAgree.Agree, (int)IsAgree.Disagree);
        } // SubmitCheck 
    } // class
} // namespace

