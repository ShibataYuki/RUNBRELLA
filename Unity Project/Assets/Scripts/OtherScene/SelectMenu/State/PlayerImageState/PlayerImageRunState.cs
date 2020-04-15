using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class PlayerImageRunState : PlayerImageState
    {
        // 1秒間に移動する移動量（ピクセル数）
        private float speed = 216;
        // 走るアニメーションを行うためのパラメータ用のID
        private readonly int runID = Animator.StringToHash("Velocity");

        public PlayerImageRunState()
        {
            // テキスト読み込みを行うファイル名
            var fileName = string.Format("{0}Data", nameof(PlayerImageRunState));
            // 読み込んだスピードをセット
            speed = TextManager.Instance.GetValue_float(fileName, nameof(speed));
        }

        /// <summary>
        /// ステート開始時の処理
        /// </summary>
        /// <param name="playerImage">走り始めるプレイヤーのコンポーネント</param>
        public void Entry(PlayerImage playerImage)
        {
            // アニメーターの参照を取得
            var animator = playerImage._animator;
            // アニメーターにパラメータをセット
            animator.SetFloat(runID, speed);
        }

        /// <summary>
        /// フレーム更新処理
        /// </summary>
        /// <param name="playerImage">操作するプレイヤーのコンポーネント</param>
        public void Do(PlayerImage playerImage)
        {
            // アンカーのポジションを求める
            var rectTransform = playerImage._rectTransform;
            var position = rectTransform.anchoredPosition;
            // 移動後のポジションを求める
            position.x += speed * Time.deltaTime;
            // 新しいポジションをセット
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