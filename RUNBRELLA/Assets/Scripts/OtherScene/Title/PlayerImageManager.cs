using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
    public class PlayerImageManager : MonoBehaviour
    {
        // プレイヤーの画像の配列
        private PlayerImage[] playerImages;
        // 出現間隔の時間
        [SerializeField]
        private float waitTime = 10.0f;

        // Start is called before the first frame update
        void Start()
        {
            // プレイヤーの画像の配列の初期化
            SetPlayerImages();
            // 一定時間経過したらプレイヤーを走り状態に変更
            StartCoroutine(PlayerImageFuctry());
        }

        /// <summary>
        /// プレイヤーの画像の配列の初期化
        /// </summary>
        private void SetPlayerImages()
        {
            // プレイヤーの画像のオブジェクトの配列を取得
            var playerImageObjects = GameObject.FindGameObjectsWithTag("PlayerImage");
            // 配列の要素数を変更
            playerImages = new PlayerImage[playerImageObjects.Length];
            // オブジェクトの配列からコンポーネントを取得
            for (int i = 0; i < playerImageObjects.Length; i++)
            {
                // オブジェクトからコンポーネントを取得
                var workPlayerImage = playerImageObjects[i].GetComponent<PlayerImage>();
                // 待機状態に変更
                workPlayerImage.IdleStart();
                // 配列にセット
                playerImages[i] = workPlayerImage;
            }
        } // InitPlayerImages

        /// <summary>
        /// 待機状態のプレイヤーのイメージのコンポーネントを取得
        /// </summary>
        /// <returns></returns>
        private PlayerImage GetPlayerImage()
        {
            foreach (var playerImage in playerImages)
            {
                // 画面外でないなら
                if (playerImage.IsIdle == false)
                {
                    // 取得しない
                    continue;
                }
                // 画面外のプレイヤーイメージを渡す
                return playerImage;
            }

            Debug.Log("PlayerImage" + null);
            return null;
        }

        // 一定時間ごとにプレイヤーを生成

        private IEnumerator PlayerImageFuctry()
        {
            // 無限ループ
            while(true)
            {
                // 一定時間待つ
                yield return new WaitForSeconds(waitTime);
                // プレイヤーのイメージを走り状態に変更
                CreatePlayerImage();
            }
        }

        /// <summary>
        /// プレイヤーイメージを走り状態に変更
        /// </summary>
        private void CreatePlayerImage()
        {
            // 画面外で待機しているプレイヤーの参照を取得
            var playerImage = GetPlayerImage();
            // どのキャラを選択しているかをランダムに選択
            var selectCharaNumber = Random.Range(0, 2);
            // アニメーターコントローラーを取得
            var animatorController = Resources.Load<RuntimeAnimatorController>
                (string.Format("PlayerImageAnimator/PlayerImage{0}", selectCharaNumber == 0 ? 'A' : 'B'));
            // アニメーターコントローラーをセット
            playerImage._animator.runtimeAnimatorController = animatorController;
            // 走るステートに変更
            playerImage.RunStart();
        }

    }
}