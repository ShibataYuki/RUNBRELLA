using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
    public class PlayerImageGoalState : SelectMenu.PlayerImageState
    {
        // 待機状態に戻るために必要
        private PlayerImage playerImage;

        private void Start()
        {
            // コンポーネントを取得
            playerImage = GetComponent<PlayerImage>();
        }

        public override void Entry() {
            // 待機状態に変更
            playerImage.IdleStart();
        }

        public override void Do() { }
    }
}