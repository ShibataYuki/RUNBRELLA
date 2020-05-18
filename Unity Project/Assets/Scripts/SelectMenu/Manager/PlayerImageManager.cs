using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SelectMenu
{
    public class PlayerImageManager : MonoBehaviour
    {
        // プレイヤーの画像のコンポーネントのリスト
       PlayerImage[] playerImages = new PlayerImage[4];
        // キャラ選択が終了したプレイヤーの画像のコンポーネントのディクショナリー
        Dictionary<CONTROLLER_NO, PlayerImage> entryPlayerImages = new Dictionary<CONTROLLER_NO, PlayerImage>();
        // キャラ選択を管理するマネージャー
        private SelectCharacterManager selectCharacterManager;

        // Start is called before the first frame update
        void Start()
        {
            // コンポーネントの取得
            selectCharacterManager = GetComponent<SelectCharacterManager>();
            // プレイヤーの画像のプレファブを配列にセット
            SetPlayerImage();
        }


        /// <summary>
        /// プレイヤーのイメージの待機時のポジション
        /// </summary>
        /// <returns></returns>
        private Vector3 PlayerInitPosition()
        {
            #region プレイヤーのイメージのポジションの初期位置の右上
            var position = Camera.main.ViewportToWorldPoint(Vector2.zero);
            var center = Camera.main.transform.position;
            // 地面のチェックを行う
            var collider = Physics2D.OverlapArea(position, center, LayerMask.GetMask("Ground"));
            if (collider != null)
            {
                // コライダー
                var boxCollider2D = collider.GetComponent<BoxCollider2D>();
                // 領域
                var size = boxCollider2D.size;
                // 差分
                var offset = boxCollider2D.offset;
                // 当たり判定の領域の左上
                var colliderLeftTop = collider.transform.position;
                colliderLeftTop += (Vector3)offset;
                colliderLeftTop.x -= size.x * 0.5f;
                colliderLeftTop.y += size.y * 0.5f;
                if (position.x > colliderLeftTop.x)
                {
                    position.x = colliderLeftTop.x;
                }

                if (position.y < colliderLeftTop.y)
                {
                    position.y = colliderLeftTop.y;
                }
            }
            #endregion
            #region プレイヤーのイメージのポジションとの差分を求める
            var playerImageOffset = Vector2.zero;
            foreach(var playerImage in playerImages)
            {
                // コライダー領域
                var boxCollider = playerImage.GetComponent<BoxCollider2D>();
                var halfSize = boxCollider.size * 0.5f;
                var offset = boxCollider.offset;
                if(playerImageOffset.x  > -(halfSize.x + offset.x))
                {
                    playerImageOffset.x = -(halfSize.x + offset.x);
                }
                if(playerImageOffset.y < (halfSize.y - offset.y))
                {
                    playerImageOffset.y = (halfSize.y - offset.y);
                }
            }
            // ポジションに差分を足す
            position += (Vector3)playerImageOffset;
            #endregion
            // z座標は動かさない
            position.z = 0;
            return position;
        }

        /// <summary>
        /// プレイヤーの画像のプレファブを配列にセットするメソッド
        /// </summary>
        /// <returns></returns>
        private void SetPlayerImage()
        {
            // オブジェクトの配列の取得
            var playerImageObjects = GameObject.FindGameObjectsWithTag("PlayerImage");
            // 処理順を変えるために1フレーム待つ
            // 配列の要素数を調整
            playerImages = new PlayerImage[playerImageObjects.Length];
            for (int i = 0; i < playerImageObjects.Length; i++)
            {
                // コンポーネントを取得
                var playerImage = playerImageObjects[i].GetComponent<PlayerImage>();
                // リストに追加
                playerImages[i] = (playerImage);
                // 待機状態に変更
                playerImage.IdleStart();
            }
        }

        /// <summary>
        /// 参加処理
        /// </summary>
        /// <param name="controllerNo"></param>
        public void PlayerImageEntry(CONTROLLER_NO controllerNo)
        {
            // 画面外で待機しているプレイヤーの参照を取得
            var playerImage = GetPlayerImage();
            // どのキャラを選択しているか
            var selectCharaNumber = selectCharacterManager.SelectCharacters[controllerNo].SelectCharacterNumber;
            // アニメーターコントローラーを取得
            var animatorController = Resources.Load<RuntimeAnimatorController>
                (string.Format("PlayerImageAnimator/PlayerImage{0}",
                SceneController.Instance._characterMessages[selectCharaNumber].charaType.ToString()
                .Replace("Player", "")));
            // アニメーターコントローラーをセット
            playerImage._animator.runtimeAnimatorController = animatorController;
            foreach(var playerNo in GameManager.Instance.playerAndControllerDictionary)
            {
                if(playerNo.Value == controllerNo)
                {
                    // アウトラインの色を変更
                    playerImage._spriteRenderer.material = 
                        GameManager.Instance.playerOutlines[(int)playerNo.Key];
                    break;
                }
            }
            // 走るステートに変更
            playerImage.RunStart();
            // 参加中のディクショナリーに追加
            entryPlayerImages.Add(controllerNo, playerImage);
        }

        /// <summary>
        /// 待機状態のプレイヤーのイメージのコンポーネントを取得
        /// </summary>
        /// <returns></returns>
        private PlayerImage GetPlayerImage()
        {
            foreach(var playerImage in playerImages)
            {
                // 待機中でないのなら
                if(playerImage.IsIdle == false)
                {
                    // 取得しない
                    continue;
                }

                return playerImage;
            }

            Debug.Log("PlayerImage" + null);
            return null;
        }

        /// <summary>
        /// 参加状態のプレイヤーのキャンセル
        /// </summary>
        /// <param name="ID">キャンセルするプレイヤーのコントローラー番号</param>
        public void PlayerImageCansel(CONTROLLER_NO ID)
        {
            entryPlayerImages[ID].IdleStart();
            entryPlayerImages.Remove(ID);
        }

        /// <summary>
        /// 画面内のプレイヤーの全画像をブーストに変更
        /// </summary>
        public void AllPlayerImageBoost()
        {
            // ディクショナリーから値だけを取り出す
            foreach(var playerImage in entryPlayerImages.Values)
            {
                // 画面内にいるなら
                if(playerImage.IsIdle == false && playerImage.IsGoal == false)
                {
                    // ステートをブーストに変更
                    playerImage.BoostStart();
                }
            }
                   
        }

        /// <summary>
        /// 画面内にプレイヤーがいるかチェック
        /// </summary>
        /// <returns>画面内にプレイヤーがいるかいないか</returns>
        public bool FinishCheck()
        {
            foreach(var playerImage in entryPlayerImages.Values)
            {
                // ゴールしていないプレイヤーがいるのなら
                if(playerImage.IsGoal == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}