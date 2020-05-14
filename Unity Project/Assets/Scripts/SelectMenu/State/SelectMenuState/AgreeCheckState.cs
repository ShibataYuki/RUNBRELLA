using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class AgreeCheckState : SelectMenuState
    {
        // このステートで使うコンポーネント
        AgreeCheck agreeCheck = null;
        // ステージ選択のボタン
        SelectStages selectStages;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="agreeCheck"></param>
        public AgreeCheckState(AgreeCheck agreeCheck)
        {
            this.agreeCheck = agreeCheck;
            selectStages = GameObject.Find("SelectStagesUI").GetComponent<SelectStages>();
        }

        /// <summary>
        /// ステート開始時の処理
        /// </summary>
        public void Entry()
        {
            agreeCheck.AgreeCheckEntry();
            selectStages.Show();
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
            selectStages.Hide();
            agreeCheck.AgreeCheckExit();
        }
    }

}
