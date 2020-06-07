using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;


namespace Title
{
    public class SceneController : MonoBehaviour
    {
        public enum SelectIndex
        {
            SelectMenu,
            Manual,
            Exit,
            Credit,
            MAX,
        }
        // 現在選んでいる次の項目
        private SelectIndex selectIndex = 0;
        // ボタンを管理するマネージャー
        private ButtonManager buttonManager;
        // 入力をチェックするマネージャー
        private InputManager inputManager;
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
        // Start is called before the first frame update
        void Start()
        {
            // コンポーネントを取得
            buttonManager = GetComponent<ButtonManager>();
            inputManager = GetComponent<InputManager>();
            audioSource = GetComponent<AudioSource>();
        }


        // Update is called once per frame
        void Update()
        {
            // 決定中なら
            if(isSubmit == true)
            {
                // キー入力をチェックしない
                return;
            }

            // 上下のキー入力をチェック
            bool upKeyIn, downKeyIn;
            VerticalKeyCheck(out upKeyIn, out downKeyIn);
            // キー入力に応じてインデックスを変更する
            ChangeIndex(upKeyIn, downKeyIn);
            // インデックスに応じて画像を変更する
            buttonManager.SetAnimation(selectIndex);
            // 終了処理を行うかチェック
            if(EnterCheck())
            {
                // インデックスに応じた終了処理を行う
                StartCoroutine(Enter());
            }
        }

        /// <summary>
        /// 上下のキー入力をチェック
        /// </summary>
        /// <param name="upKeyIn">上のキーが押されたかどうか</param>
        /// <param name="downKeyIn">下のキーが押されたかどうか</param>
        private void VerticalKeyCheck(out bool upKeyIn, out bool downKeyIn)
        {
            // 上ボタンが押されたかどうか
            upKeyIn = inputManager.UpKeyCheck(GamePad.Index.Any);
            // 下ボタンがが押されたかどうか
            downKeyIn = inputManager.DownKeyCheck(GamePad.Index.Any);
            #region キーボード入力
            // キーボード入力で上が押されたかチェック
            upKeyIn |= Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
            // キーボード入力で下が押されたかチェック
            downKeyIn |= Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
            #endregion
        }

        /// <summary>
        /// キー入力に応じてインデックスを変更する
        /// </summary>
        /// <param name="upKeyIn">上のキーが押されたかどうか</param>
        /// <param name="downKeyIn">>下のキーが押されたかどうか</param>
        private void ChangeIndex(bool upKeyIn, bool downKeyIn)
        {
            // 上だけ押されてたら
            if (upKeyIn == true)
            {
                selectIndex--;
            }
            // 下だけ押されてたら
            if (downKeyIn == true)
            {
                selectIndex++;
            }
            // 範囲内に収める
            if(selectIndex < 0)
            {
                selectIndex = SelectIndex.MAX - 1;
            }
            else if(selectIndex >= SelectIndex.MAX)
            {
                selectIndex = 0;
            }
            if(upKeyIn != downKeyIn)
            {
                // 音量を変更
                audioSource.volume = 0.25f;
                // 音源をセット
                audioSource.clip = selectClip;
                // 選択音の再生
                audioSource.Play();
            }
        }

        /// <summary>
        /// 終了処理を行うかチェック
        /// </summary>
        private bool EnterCheck()
        {
            return (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any) || Input.GetKeyDown(KeyCode.Return));
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
            // 音の終了をチェック
            yield return StartCoroutine(AudioFinishCheck());
            // シーンを変更
            switch (selectIndex)
            {
                case SelectIndex.SelectMenu:
                case SelectIndex.Manual:
                case SelectIndex.Credit:
                    SceneManager.LoadScene(selectIndex.ToString());
                    break;
                case SelectIndex.Exit:
                    Exit();
                    break;
            }
        }

        /// <summary>
        /// 音が終了したかチェック
        /// </summary>
        /// <returns></returns>
        IEnumerator AudioFinishCheck()
        {
            // 音が再生中なら
            while(audioSource.isPlaying)
            {
                // 1フレーム待機する
                yield return null;
            }
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        private void Exit()
        {
            // 終了処理
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
        }
    }
}
