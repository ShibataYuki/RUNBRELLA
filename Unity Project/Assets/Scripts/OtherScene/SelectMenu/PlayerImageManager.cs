using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class PlayerImageManager : MonoBehaviour
    {
        List<PlayerImage> playerImages = new List<PlayerImage>();
        Dictionary<int, PlayerImage> entryPlayerImages = new Dictionary<int, PlayerImage>();
        private SelectCharacterManager selectCharacterManager;

        #region ステート変数
        private PlayerImageIdleState idleState = new PlayerImageIdleState();
        private PlayerImageRunState runState = new PlayerImageRunState();
        private PlayerImageGoalState goalState = new PlayerImageGoalState();
        // get
        public PlayerImageGoalState _goalState { get { return goalState; } }
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            // コンポーネントの取得
            selectCharacterManager = GetComponent<SelectCharacterManager>();
            // プレイヤーの画像のプレファブを配列にセット
            StartCoroutine(SetPlayerImage());
        }

        /// <summary>
        /// プレイヤーの画像のプレファブを配列にセットするメソッド
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetPlayerImage()
        {
            // 処理順を変えるために1フレーム待つ
            yield return null;
            // オブジェクトの配列の取得
            var playerImageObjects = GameObject.FindGameObjectsWithTag("PlayerImage");
            foreach (var playerImageObject in playerImageObjects)
            {
                // コンポーネントを取得
                var playerImage = playerImageObject.GetComponent<PlayerImage>();
                // ステートの変更
                playerImage.ChangeState(idleState);
                // リストに追加
                playerImages.Add(playerImage);
            }
        }

        /// <summary>
        /// 参加処理
        /// </summary>
        /// <param name="ID"></param>
        public void PlayerImageEntry(int ID)
        {
            var playerImage = GetPlayerImage();
            var selectCharaNumber = selectCharacterManager.SelectCharacters[ID].SelectCharacterNumber;
            var animatorController = Resources.Load<RuntimeAnimatorController>(string.Format("PlayerAnimator/{0}", SceneController.Instance._characterMessages[selectCharaNumber].charaType.ToString()));
            playerImage._animator.runtimeAnimatorController = animatorController;
            playerImage.ChangeState(runState);
            entryPlayerImages.Add(ID, playerImage);
        }

        /// <summary>
        /// 待機状態のプレイヤーのイメージのコンポーネントを取得
        /// </summary>
        /// <returns></returns>
        private PlayerImage GetPlayerImage()
        {
            foreach(var playerImage in playerImages)
            {
                if(playerImage.State != idleState)
                {
                    continue;
                }

                return playerImage;
            }

            Debug.Log("PlayerImage" + null);
            return null;
        }
    }
}