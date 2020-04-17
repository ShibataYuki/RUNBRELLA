using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class SelectCharacterState : SelectMenuState
    {
        // このステートで使うコンポーネント
        private SelectCharacterManager selectCharacterManager = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="selectCharacterManagerr"></param>
        public SelectCharacterState(SelectCharacterManager selectCharacterManager)
        {
            this.selectCharacterManager = selectCharacterManager;
        }

        public void Entry()
        {
        }

        /// <summary>
        /// フレーム更新処理
        /// </summary>
        public void Do()
        {
            // キャラクター選択時のフレーム更新処理
            selectCharacterManager.SelectCharacter();
        }

        public void Exit()
        {
        }
    }
}

