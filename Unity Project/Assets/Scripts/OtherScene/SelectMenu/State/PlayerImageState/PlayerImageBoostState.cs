using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class PlayerImageBoostState : PlayerImageState
    {
        private float speed = 960;

        public void Entry(PlayerImage playerImage)
        {
            var animator = playerImage._animator;
        }

        public void Do(PlayerImage playerImage)
        {
            var rectTransform = playerImage._rectTransform;
            var position = rectTransform.anchoredPosition;
            position.x += speed * Time.deltaTime;
            rectTransform.anchoredPosition = position;
            // 右端まで行ったのなら
            if (position.x >= (960 + rectTransform.rect.size.x * 0.5f))
            {
                // ステートの変更
                playerImage.ChangeState(playerImage._playerImageManager.GoalState);
            }
        }
    }
}