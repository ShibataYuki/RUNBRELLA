using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;
using UnityEngine.Playables;

namespace Result
{
    public class SceneController : MonoBehaviour
    {
        // 選んでいるシーンのenum
        public enum SelectScene
        {
            Title,
            SelectMenu,
        }
        // 現在選んでいるシーン
        private SelectScene selectScene = SelectScene.Title;

        PlayableDirector director;
        GameObject TimelineControllerObj;

        // 選んでいるシーンのボタンのマネージャー
        ButtonManager buttonManager;
        // Start is called before the first frame update
        void Start()
        {
            TimelineControllerObj = GameObject.Find("TimelineController");
            // コンポーネントの取得
            director = TimelineControllerObj.GetComponent<PlayableDirector>();
            // マネージャーのオブジェクト
            var buttonManagerObject = GameObject.Find("ButtonManager");
            // コンポーネントの取得
            buttonManager = buttonManagerObject.GetComponent<ButtonManager>();
        }

        // Update is called once per frame
        void Update()
        {
           

            // 終了処理
            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                UnityEngine.Application.Quit();
#endif
            }

            GoToNextScean();
        }


        void GoToNextScean()
        {
            bool isTimelinePlaying = director.state == PlayState.Playing;
            if (isTimelinePlaying) { return; }

            // ボタンの親オブジェクトの拡大処理
            if(buttonManager.IsScalingButton == false)
            {
                // ボタンの親オブジェクトの拡大処理
                buttonManager.ButtonsScaleUp();
            }
            // ボタンの親オブジェクトの拡大が終わっているなら
            if(buttonManager.IsScalingButton == true)
            {
                // 左右入力をチェックするコンポーネント
                KeyCheckHorizontal();

                // ボタンの拡大縮小
                buttonManager.ScalingUpdate(selectScene);
                if (Input.GetKeyDown(KeyCode.Return) || GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any))
                {
                    // 前回の順位をリセット
                    GameManager.Instance.playerRanks.Clear();
                    // 選択したシーンに遷移
                    SceneManager.LoadScene(selectScene.ToString());
                }
            }
        }

        /// <summary>
        /// 左右入力をチェックするコンポーネント
        /// </summary>
        void KeyCheckHorizontal()
        {
            // 左に移動させる入力を受け付けたかどうかのフラグ
            var isLeft =  (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || 
                GamePad.GetButtonDown(GamePad.Button.LeftShoulder, GamePad.Index.Any));
            // 右に移動させる入力を受け付けたかどうかのフラグ
            var isRight = (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) ||
                GamePad.GetButtonDown(GamePad.Button.RightShoulder, GamePad.Index.Any));

            // 右への移動と左への移動のどちらか片方だけをうけたなら
            if(isLeft != isRight)
            {
                // 左への移動
                if(isLeft == true)
                {
                    selectScene--;
                }
                // 右への移動
                else if(isRight == true)
                {
                    selectScene++;
                }
                // タイトルとリザルトの間に収める
                selectScene = (SelectScene)Mathf.Clamp((int)selectScene, (int)SelectScene.Title, (int)SelectScene.SelectMenu);
            }

        }
    }
}