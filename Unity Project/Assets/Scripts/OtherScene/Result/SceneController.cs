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
            SelectMenu,
            Title,
        }
        // 現在選んでいるシーン
        private SelectScene selectScene = SelectScene.SelectMenu;

        PlayableDirector director;
        GameObject TimelineControllerObj;        

        // 選んでいるシーンのボタンのマネージャー
        ButtonManager buttonManager;
        // オーディオソース
        AudioSource audioSource;
        // 選択音
        [SerializeField]
        private AudioClip selectClip;
        // 決定音
        [SerializeField]
        private AudioClip submitClip;
        // 決定中かどうか
        private bool isSubmit = false;
        // 決定音を流す時間
        [SerializeField]
        private float waitTimeForEnter = 1.0f;
        // Start is called before the first frame update
        void Start()
        {
            TimelineControllerObj = GameObject.Find("TimelineController");
            // コンポーネントの取得
            director = TimelineControllerObj.GetComponent<PlayableDirector>();
            audioSource = GetComponent<AudioSource>();
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
                // 決定中なら
                if (isSubmit == true)
                {
                    // キー入力をチェックしない
                    return;
                }

                // 左右入力をチェックするコンポーネント
                KeyCheckHorizontal();

                // ボタンの拡大縮小
                buttonManager.SetAnimator(selectScene);
                if (Input.GetKeyDown(KeyCode.Return) || GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any))
                {
                    //インデックスに応じた終了処理を行う
                    StartCoroutine(Enter());
                }
            }
        }

        /// <summary>
        /// インデックスに応じた終了処理を行う
        /// </summary>
        private IEnumerator Enter()
        {
            // 音量を変更
            audioSource.volume = 0.25f;
            // 音源をセット
            audioSource.clip = submitClip;
            // 選択音の再生
            audioSource.Play();
            // 決定中のフラグをONにする
            isSubmit = true;
            // 音がなる時間を用意する
            yield return new WaitForSeconds(waitTimeForEnter);
            // 前回の順位をリセット
            GameManager.Instance.playerResultInfos.Clear();
            // 選択したシーンに遷移
            SceneManager.LoadScene(selectScene.ToString());
        }

        /// <summary>
        /// 音が終了したかチェック
        /// </summary>
        /// <returns></returns>
        IEnumerator AudioFinishCheck()
        {
            // 音が再生中なら
            while (audioSource.isPlaying)
            {
                // 1フレーム待機する
                yield return null;
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
                selectScene = (SelectScene)Mathf.Clamp((int)selectScene, (int)SelectScene.SelectMenu, (int)SelectScene.Title);
                // 音量を変更
                audioSource.volume = 0.25f;
                // 音源をセット
                audioSource.clip = selectClip;
                // 選択音の再生
                audioSource.Play();
            }

        }
    }
}