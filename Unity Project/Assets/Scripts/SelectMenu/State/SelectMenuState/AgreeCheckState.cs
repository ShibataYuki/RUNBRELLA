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
        private void Start()
        {
            agreeCheck = GetComponent<AgreeCheck>();
            selectStages = GameObject.Find("SelectStagesUI").GetComponent<SelectStages>();
        }

        /// <summary>
        /// ステート開始時の処理
        /// </summary>
        public override void Entry()
        {
            agreeCheck.AgreeCheckEntry();
            if (GameManager.Instance.selectMapMode == SLECT_MAP_MODE.SELECT)
            {
                selectStages.Show();
            }
        }

        /// <summary>
        /// フレーム更新処理
        /// </summary>
        public override void Do()
        {
            agreeCheck.SubmitCheck();
        }

        /// <summary>
        /// ステート終了時の処理
        /// </summary>
        public override void Exit()
        {
            if (GameManager.Instance.selectMapMode == SLECT_MAP_MODE.SELECT)
            {
                selectStages.Hide();
            }
            agreeCheck.AgreeCheckExit();
        }
    }
}
