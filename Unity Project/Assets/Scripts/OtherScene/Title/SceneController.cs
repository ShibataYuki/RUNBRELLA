using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;


namespace Title
{
    public class SceneController : MonoBehaviour
    {
        public enum SelectIndex
        {
            SelectMenu,
            Manual,
            Exit,
            Credit,
            MAX,
        }
        // 現在選んでいる次の項目
        private SelectIndex selectIndex = 0;
        private ButtonManager buttonManager;
        private InputManager inputManager;
        // Start is called before the first frame update
        void Start()
        {
            buttonManager = GetComponent<ButtonManager>();
            inputManager = GetComponent<InputManager>();
        }


        // Update is called once per frame
        void Update()
        {
            // 上下のキー入力をチェック
            bool upKeyIn, downKeyIn;
            VerticalKeyCheck(out upKeyIn, out downKeyIn);
            // キー入力に応じてインデックスを変更する
            ChangeIndex(upKeyIn, downKeyIn);
            // インデックスに応じて画像を変更する
            buttonManager.SetAnimation(selectIndex);
            // 終了処理を行うかチェック
            if(EnterCheck())
            {
                // インデックスに応じた終了処理を行う
                Enter();
            }
        }

        /// <summary>
        /// 上下のキー入力をチェック
        /// </summary>
        /// <param name="upKeyIn">上のキーが押されたかどうか</param>
        /// <param name="downKeyIn">下のキーが押されたかどうか</param>
        private void VerticalKeyCheck(out bool upKeyIn, out bool downKeyIn)
        {
            // 上ボタンが押されたかどうか
            upKeyIn = inputManager.UpKeyCheck(GamePad.Index.Any);
            // 下ボタンがが押されたかどうか
            downKeyIn = inputManager.DownKeyCheck(GamePad.Index.Any);
            #region キーボード入力
            // キーボード入力で上が押されたかチェック
            upKeyIn |= Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
            // キーボード入力で下が押されたかチェック
            downKeyIn |= Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
            #endregion
        }

        /// <summary>
        /// キー入力に応じてインデックスを変更する
        /// </summary>
        /// <param name="upKeyIn">上のキーが押されたかどうか</param>
        /// <param name="downKeyIn">>下のキーが押されたかどうか</param>
        private void ChangeIndex(bool upKeyIn, bool downKeyIn)
        {
            // 上だけ押されてたら
            if (upKeyIn == true)
            {
                selectIndex--;
            }
            // 下だけ押されてたら
            if (downKeyIn == true)
            {
                selectIndex++;
            }
            // 範囲内に収める
            if(selectIndex < 0)
            {
                selectIndex = SelectIndex.MAX - 1;
            }
            else if(selectIndex >= SelectIndex.MAX)
            {
                selectIndex = 0;
            }
        }

        /// <summary>
        /// 終了処理を行うかチェック
        /// </summary>
        private bool EnterCheck()
        {
            return (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any) || Input.GetKeyDown(KeyCode.Return));
        }

        /// <summary>
        /// インデックスに応じた終了処理を行う
        /// </summary>
        private void Enter()
        {
            switch(selectIndex)
            {
                case SelectIndex.SelectMenu:
                case SelectIndex.Manual:
                case SelectIndex.Credit:
                    SceneManager.LoadScene(selectIndex.ToString());
                    break;
                case SelectIndex.Exit:
                    Exit();
                    break;
            }
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        private void Exit()
        {
            // 終了処理
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
        }
    }
}
