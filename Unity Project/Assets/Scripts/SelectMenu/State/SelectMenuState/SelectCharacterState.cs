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
        /// フレーム更新処理の前に行う初期化処理
        /// </summary>
        private void Awake()
        {
            selectCharacterManager = GetComponent<SelectCharacterManager>();
        }

        public override void Entry()
        {
        }

        /// <summary>
        /// フレーム更新処理
        /// </summary>
        public override void Do()
        {
            // キャラクター選択時のフレーム更新処理
            selectCharacterManager.SelectCharacter();
        }

        public override void Exit()
        {
        }
    }
}