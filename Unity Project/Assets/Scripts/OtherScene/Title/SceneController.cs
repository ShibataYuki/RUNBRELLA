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
            //Credit,
            Exit,
        }
        // 現在選んでいる次の項目
        private SelectIndex selectIndex = SelectIndex.SelectMenu;
        private ButtonManager buttonManager;

        // Start is called before the first frame update
        void Start()
        {
            buttonManager = GetComponent<ButtonManager>();
        }


        // Update is called once per frame
        void Update()
        {
            // 左右入力をチェック
            bool leftKeyIn, rightKeyIn;
            HorizontalKeyCheck(out leftKeyIn, out rightKeyIn);
            // キー入力に応じてインデックスを変更する
            ChangeIndex(leftKeyIn, rightKeyIn);
            // インデックスに応じてスケールを変更する
            buttonManager.ChangeScale(selectIndex);
            // 終了処理を行うかチェック
            if(EnterCheck())
            {
                // インデックスに応じた終了処理を行う
                Enter();
            }
        }


        /// <summary>
        /// 左右入力をチェック
        /// </summary>
        /// <param name="leftKeyIn">左のキーが押されたかどうか</param>
        /// <param name="rightKeyIn">右のキーが押されたかどうか</param>
        private void HorizontalKeyCheck(out bool leftKeyIn, out bool rightKeyIn)
        {
            // Lボタンが押されたかどうか
            leftKeyIn = GamePad.GetButtonDown(GamePad.Button.LeftShoulder, GamePad.Index.Any);
            // Rボタンがが押されたかどうか
            rightKeyIn = GamePad.GetButtonDown(GamePad.Button.RightShoulder, GamePad.Index.Any);
            #region キーボード入力
            // キーボード入力で左が押されたかチェック
            leftKeyIn |= Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
            // キーボード入力で右が押されたかチェック
            rightKeyIn |= Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
            #endregion
        }

        /// <summary>
        /// キー入力に応じてインデックスを変更する
        /// </summary>
        /// <param name="leftKeyIn">左のキーが押されたかどうか</param>
        /// <param name="rightKeyIn">>右のキーが押されたかどうか</param>
        private void ChangeIndex(bool leftKeyIn, bool rightKeyIn)
        {
            // 左だけ押されてたら
            if (leftKeyIn == true)
            {
                selectIndex--;
            }
            // 右だけ押されてたら
            if (rightKeyIn == true)
            {
                selectIndex++;
            }
            // 範囲内に収める
            selectIndex = (SelectIndex)Mathf.Clamp((int)selectIndex, (int)SelectIndex.SelectMenu, (int)SelectIndex.Exit);
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
                //case SelectIndex.Credit:
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
