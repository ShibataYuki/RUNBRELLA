using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
    public class PlayerImageIdleState : SelectMenu.PlayerImageState
    {
        // 地面のレイヤーマスク
        private LayerMask groundLayer;
        // 待機時のポジション
        private Vector3 initPosition;

        /// <summary>
        /// Startの前に行う初期化処理
        /// </summary>
        private void Awake()
        {
            // レイヤーマスクの取得
            groundLayer = LayerMask.GetMask("Ground");
            // プレイヤーの画像の待機児のポジションを計算
            initPosition = PlayerInitPosition();
        }

        /// <summary>
        /// プレイヤーのイメージの待機時のポジションを計算
        /// </summary>
        /// <returns></returns>
        private Vector3 PlayerInitPosition()
        {
            #region プレイヤーのイメージのポジションの初期位置の右上
            var position = Camera.main.ViewportToWorldPoint(Vector2.zero);
            var center = Camera.main.transform.position;
            // 地面のチェックを行う
            var ground = Physics2D.OverlapArea(position, center, groundLayer);
            if (ground != null)
            {
                // コライダー
                var groundCollider = ground.GetComponent<BoxCollider2D>();
                // 領域
                var groundColliderSize = groundCollider.size;
                // 差分
                var groundColliderOffset = groundCollider.offset;
                // 当たり判定の領域の左上
                var colliderLeftTop = ground.transform.position;
                colliderLeftTop += (Vector3)groundColliderOffset;
                colliderLeftTop.x -= groundColliderSize.x * 0.5f;
                colliderLeftTop.y += groundColliderSize.y * 0.5f;
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
            // コライダー領域
            var boxCollider = GetComponent<BoxCollider2D>();
            var halfSize = boxCollider.size * 0.5f;
            var offset = boxCollider.offset;
            if (playerImageOffset.x > -(halfSize.x + offset.x))
            {
                playerImageOffset.x = -(halfSize.x + offset.x);
            }
            if (playerImageOffset.y < (halfSize.y - offset.y))
            {
                playerImageOffset.y = (halfSize.y - offset.y);
            }
            // ポジションに差分を足す
            position += (Vector3)playerImageOffset;
            #endregion
            // z座標は動かさない
            position.z = 0;
            return position;
        }


        /// <summary>
        /// ステートの開始処理
        /// </summary>
        public override void Entry()
        {
            // 座標セット
            transform.position = initPosition;
        }

        public override void Do()
        {

        }
    }
}