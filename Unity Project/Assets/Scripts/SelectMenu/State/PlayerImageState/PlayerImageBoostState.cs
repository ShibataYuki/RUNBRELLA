using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class PlayerImageBoostState : PlayerImageState
    {
        // 1秒間に移動する移動量（グリッド数）
        private float speed = 1280;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlayerImageBoostState()
        {
            // テキスト読み込みを行うファイル名
            var fileName = string.Format("{0}Data", nameof(PlayerImageBoostState));
            // 読み込んだスピードをセット
            speed = TextManager.Instance.GetValue_float(fileName, nameof(speed));
        }

        /// <summary>
        /// ステート開始処理
        /// </summary>
        /// <param name="playerImage"></param>
        public void Entry(PlayerImage playerImage)
        {
            // エフェクトの開始
            playerImage._particleSystem.Play();
        }

        /// <summary>
        /// フレーム更新処理
        /// </summary>
        /// <param name="playerImage">操作するプレイヤーのコンポーネント</param>
        public void Do(PlayerImage playerImage)
        {
            // ポジションを求める
            var transform = playerImage.transform;
            var position = transform.position;
            // 移動後のポジションを求める
            position.x += speed * Time.deltaTime;
            // 新しいポジションをセット
            playerImage.transform.position = position;
            // 画面に映らなくなったら
            if (playerImage.IsScreen == false)
            {
                // ステートの変更
                playerImage.ChangeState(playerImage._playerImageManager.GoalState);
            }
        }
    }
}