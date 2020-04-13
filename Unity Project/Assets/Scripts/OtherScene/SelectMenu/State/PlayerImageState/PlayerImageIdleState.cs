using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class PlayerImageIdleState : PlayerImageState
    {
        /// <summary>
        /// ステートの開始処理
        /// </summary>
        /// <param name="playerImage">操作するイメージのコンポーネント</param>
        public void Entry(PlayerImage playerImage)
        {
            // レクトトランスフォームの取得
            var rectTransform = playerImage._rectTransform;
            // 座標の計算
            var position = rectTransform.anchoredPosition;
            position = new Vector2(-960, -540 + 54);
            position.x += -(rectTransform.rect.size.x * 0.5f);
            position.y += rectTransform.rect.size.y * 0.5f;
            // 座標セット
            playerImage._rectTransform.anchoredPosition = position;
        }

        public void Do(PlayerImage playerImage)
        {

        }
    }
}