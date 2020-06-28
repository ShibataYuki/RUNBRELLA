using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class SelectCharacterState : SelectMenuState
    {
        // このステートで使うコンポーネント
        private SelectCharacterManager selectCharacterManager = null;
        // キャラクター選択用のガイドのオブジェクト
        GameObject guideObject;

        /// <summary>
        /// フレーム更新処理の前に行う初期化処理
        /// </summary>
        private void Awake()
        {
            // コンポーネントを取得
            selectCharacterManager = GetComponent<SelectCharacterManager>();
            // ガイドのオブジェクトを取得
            guideObject = GameObject.Find("Canvas/Guide");
        }

        public override void Entry()
        {
            // ガイドのオブジェクトを表示
            guideObject.SetActive(true);
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
            // ガイドのオブジェクトを非表示にする
            guideObject.SetActive(false);

        }
    }
}