using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class SelectMenuEndState : SelectMenuState
    {
        // プレイヤーの画像を管理するマネージャー
        PlayerImageManager imageManager;
        // プレイヤーの入力をチェックするコンポーネント
        InputManager inputManager;

        /// <summary>
        /// フレーム更新前に行う処理
        /// </summary>
        private void Start()
        {
            imageManager = GetComponent<PlayerImageManager>();
            inputManager = GetComponent<InputManager>();
        }

        /// <summary>
        /// ステートの開始処理
        /// </summary>
        public override void Entry()
        {
            // 画面内の全てのプレイヤーの画像のステートをブーストに変更
            imageManager.AllPlayerImageBoost();
        }

        /// <summary>
        /// フレーム更新処理
        /// </summary>
        public override void Do()
        {
            // 画面内にプレイヤーがいなければ
            if(imageManager.FinishCheck() == true)
            {
                // ゲームを開始する
                SceneController.Instance.GameStart();
            }
            // キーパッドのAキーが入力されたら
            if(inputManager.SubmitKeyDown(GamepadInput.GamePad.Index.Any))
            {
                // ゲームを開始する
                SceneController.Instance.GameStart();
            }
            // キーボードのエンターが入力されたら
            if(Input.GetKeyDown(KeyCode.Return))
            {
                // ゲームを開始する
                SceneController.Instance.GameStart();
            }
        }

        /// <summary>
        /// ステート終了時の処理
        /// </summary>
        public override void Exit()
        {

        }
    }
}