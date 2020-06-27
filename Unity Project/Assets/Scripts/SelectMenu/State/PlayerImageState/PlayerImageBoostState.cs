using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class PlayerImageBoostState : PlayerImageState
    {
        // 1秒間に移動する移動量（グリッド数）
        private float speed = 1280;
        // キャラクターを選択した後に地面の上を走るプレイヤーの画像のコンポーネント
        PlayerImage playerImage;

        /// <summary>
        /// フレーム更新を行う前に行う初期化処理
        /// </summary>
        private void Start()
        {
            // コンポーネントの取得
            playerImage = GetComponent<PlayerImage>();
            // テキストから読み込んだスピードをメンバー変数にセットする
            SetSpeed();
        }

        /// <summary>
        /// テキストから読み込んだスピードをメンバー変数にセットする
        /// </summary>
        public void SetSpeed()
        {
            try
            {
                // テキストデータからパラメータを取り出しディクショナリーにセット
                SheetToDictionary.Instance.TextToDictionary(SceneController.Instance.textName,
                    out var speedDictionary);
                try
                {
                    speed = speedDictionary["ブースト中の1秒あたりに移動するグリッド数"];
                }
                catch
                {
                    Debug.Assert(false, nameof(PlayerImageBoostState) + "でエラーが発生しました");
                }
            }
            catch
            {
                Debug.Assert(false, nameof(SheetToDictionary.TextToDictionary) + "から" +
                    "Charaselectのディクショナリーを取得できませんでした。");
            }
        }

        /// <summary>
        /// ステート開始処理
        /// </summary>
        public override void Entry()
        {
            // エフェクトの開始
            playerImage._particleSystem.Play();
        }

        /// <summary>
        /// フレーム更新処理
        /// </summary>
        public override void Do()
        {
            // ポジションを求める
            var position = transform.position;
            // 移動後のポジションを求める
            position.x += speed * Time.deltaTime;
            // 新しいポジションをセット
            transform.position = position;
            // 画面に映らなくなったら
            if (playerImage.IsScreen == false)
            {
                // 画面外のステートに変更
                playerImage.Goal();
            }
        }
    }
}