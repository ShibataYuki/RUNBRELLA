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
        public void Entry(SelectMenuState beforeState)
        {
            agreeCheck.AgreeCheckEntry(beforeState);
        }

        /// <summary>
        /// フレーム更新処理
        /// </summary>
        public void Do()
        {
            agreeCheck.SubmitCheck();
            agreeCheck.Scaling();
        }

        /// <summary>
        /// ステート終了時の処理
        /// </summary>
        public void Exit(SelectMenuState nextState)
        {
            agreeCheck.AgreeCheckExit(nextState);
        }
    }

}
