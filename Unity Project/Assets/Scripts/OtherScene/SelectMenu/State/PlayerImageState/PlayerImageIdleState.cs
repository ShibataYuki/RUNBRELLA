using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class PlayerImageIdleState : PlayerImageState
    {
        private LayerMask groundLayer = LayerMask.GetMask("Ground");

        private Vector3 initPosition;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initPosition">初期化のポジション</param>
        public PlayerImageIdleState(Vector3 initPosition)
        {
            this.initPosition = initPosition;
        }

        /// <summary>
        /// ステートの開始処理
        /// </summary>
        /// <param name="playerImage">操作するイメージのコンポーネント</param>
        public void Entry(PlayerImage playerImage)
        {
            // 座標セット
            playerImage.transform.position = initPosition;
        }

        public void Do(PlayerImage playerImage)
        {

        }
    }
}