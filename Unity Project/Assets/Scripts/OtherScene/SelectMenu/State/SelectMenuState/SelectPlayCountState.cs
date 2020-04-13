using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class SelectPlayCountState : SelectMenuState
    {
        // このステートで使うコンポーネント
        SelectPlayCount selectPlayCount = null;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="selectPlayCount"></param>
        public SelectPlayCountState(SelectPlayCount selectPlayCount)
        {
            this.selectPlayCount = selectPlayCount;
        }

        /// <summary>
        /// ステート開始時の処理
        /// </summary>
        public void Entry(SelectMenuState beforeState)
        {
            // 何本先取か表示する
            selectPlayCount.PlayCountOpen(beforeState);
        }

        /// <summary>
        /// フレーム更新処理
        /// </summary>
        public void Do()
        {
            // 何本先取か選択する
            selectPlayCount.SelectPlayCountEntry();
        }

        /// <summary>
        /// ステート終了時の処理
        /// </summary>
        public void Exit(SelectMenuState nextState)
        {
            // 何本先取か非表示にする
            selectPlayCount.PlayCountHide(nextState);
        }
    }
}
