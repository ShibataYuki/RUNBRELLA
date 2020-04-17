using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class AgreeCheckState : SelectMenuState
    {
        // このステートで使うコンポーネント
        AgreeCheck agreeCheck = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="agreeCheck"></param>
        public AgreeCheckState(AgreeCheck agreeCheck)
        {
            this.agreeCheck = agreeCheck;
        }

        /// <summary>
        /// ステート開始時の処理
        /// </summary>
        public void Entry()
        {
            agreeCheck.AgreeCheckEntry();
        }

        /// <summary>
        /// フレーム更新処理
        /// </summary>
        public void Do()
        {
            agreeCheck.SubmitCheck();
        }

        /// <summary>
        /// ステート終了時の処理
        /// </summary>
        public void Exit()
        {
            agreeCheck.AgreeCheckExit();
        }
    }

}
