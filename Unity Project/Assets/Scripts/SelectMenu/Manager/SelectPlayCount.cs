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
        // 何本先取かを表示するテキストのアニメーション
        private Animator Animator;
        private AgreeCheck agreeCheck;
        // 何本先取かを表示する為のアニメーションのID
        private readonly int playCountID = Animator.StringToHash("PlayCount");

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
            Animator = playCountObject.transform.Find("PlayCountFrame/CountText").gameObject.GetComponent<Animator>();
            // テキストを非表示にする
            PlayCountHide();
        }

        private void Update()
        {
            // テキストの更新
            Animator.SetInteger(playCountID, raceNumber);
        }

        /// <summary>
        /// テキストの非表示
        /// </summary>
        public void PlayCountHide()
        {
            playCountObject.SetActive(false);
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
                    if (SceneController.Instance.IsAgreeCheck == true)
                    {
                        // 何本先取か選択する
                        SelectCount(controllerNo);
                        // ステートが変化したなら
                        if (SceneController.Instance.IsAgreeCheck == false)
                        {
                            return;
                        }
                    }
                    // Bボタンを押したなら
                    if (GamePad.GetButtonDown(GamePad.Button.B, (GamePad.Index)controllerNo) || Input.GetKeyDown(inputManager.CancelKeyCodes[(int)controllerNo - 1]) == true)
                    {
                        // キャラクター選択に戻る
                        SceneController.Instance.Cancel(controllerNo);
                        return;
                    } // if
                } // if
            } // for
        } // SelectPlayCountEntry

        /// <summary>
        /// 何本先取か決める個別の入力
        /// </summary>
        /// <returns></returns>
        private void SelectCount(CONTROLLER_NO controllNo)
        {
            // 左右に押し倒されたかチェック
            if (inputManager.RightShoulderKeyDown((GamePad.Index)controllNo))
            {
                raceNumber++;
                // SE再生
                SceneController.Instance.PlayChoiseSE();
            }
            if (inputManager.LeftShoulderKeyDown((GamePad.Index)controllNo))
            {
                raceNumber--;
                // SE再生
                SceneController.Instance.PlayChoiseSE();
            }
            #region キーボード入力
            if (inputManager.RightShoulderKeyDown((GamePad.Index.Any)) == false &&
                (Input.GetKeyDown(inputManager.RightKeyCodes[(int)controllNo - 1])) &&
                SceneController.Instance.IsKeyBoard == false)
            {
                raceNumber++;
                // SE再生
                SceneController.Instance.PlayChoiseSE();
                SceneController.Instance.IsKeyBoard = true;
            }
            if (inputManager.LeftShoulderKeyDown((GamePad.Index.Any)) == false &&
                Input.GetKeyDown(inputManager.LeftKeyCodes[(int)controllNo - 1]) &&
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