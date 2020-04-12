using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class PlayerImageRunState : PlayerImageState
    {
        private float speed = 6;
        private readonly int runID = Animator.StringToHash("Velociity");

        public void Entry(PlayerImage playerImage)
        {
            var animator = playerImage._animator;
            animator.SetFloat(runID, speed);
        }

        public void Do(PlayerImage playerImage)
        {
            var rectTransform = playerImage._rectTransform;
            var position = rectTransform.position;
            position.x += speed * Time.deltaTime;
            rectTransform.position = position;
            // 右端まで行ったのなら
            if (position.x >= (960 + rectTransform.rect.size.x * 0.5f))
            {
                // ステートの変更
                playerImage.ChangeState(playerImage._playerImageManager._goalState);
            }
        }
    }
}