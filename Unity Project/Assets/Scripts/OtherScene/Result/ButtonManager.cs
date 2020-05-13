using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Result
{
    public class ButtonManager : MonoBehaviour
    {
        // 現在選んでいるシーンのボタン
        Dictionary<SceneController.SelectScene, Animator> selectSceneButtonsAnimator = new Dictionary<SceneController.SelectScene, Animator>();

        private readonly int selectID = Animator.StringToHash("IsSelected");
        // ボタンの親オブジェクト
        GameObject buttons;
        // ボタンの出現時の拡大する速度
        private float scaleUpSpeedButtons = 2.0f;
        // スケーリング中かどうかのフラグ
        private bool isScalingButton = false;
        public bool IsScalingButton { get { return isScalingButton; } }

        // Start is called before the first frame update
        void Start()
        {
            // 各種パラメータをメンバー変数にセット
            SetParameter();
            // ボタンのディクショナリーにセット
            SetButtonDictionary();
        }

        /// <summary>
        /// ボタンのディクショナリーにセット
        /// </summary>
        private void SetButtonDictionary()
        {
            // ボタンの親オブジェクト
            buttons = GameObject.Find("Canvas/Buttons");
            buttons.transform.localScale = Vector3.zero;
            for (var scene = SceneController.SelectScene.SelectMenu; scene <= SceneController.SelectScene.Title; scene++)
            {
                // 拡大縮小するボタンのオブジェクト
                var button = buttons.transform.Find(scene.ToString()).gameObject;
                // ボタンのアニメーターを手に入れる
                var buttonAnimator = button.GetComponent<Animator>();
                // ディクショナリーに追加
                selectSceneButtonsAnimator.Add(scene, buttonAnimator);
            }
        }

        /// <summary>
        /// 各種パラメータをテキストから読み込んで、メンバー変数にセット
        /// </summary>
        void SetParameter()
        {
            // 読み込むテキストの名前
            var textName = "FinalResult";
            try
            {
                // テキストの中のデータをセットするディクショナリー
                SheetToDictionary.Instance.TextToDictionary(textName, out var resultDictionary);
                try
                {
                    // ディクショナリ－からパラメータを取得してセット
                    scaleUpSpeedButtons = 1f / resultDictionary["ボタンが拡大しながら現れるのにかかる秒数"];

                } // try
                catch
                {
                    Debug.Assert(false, nameof(ButtonManager) + "でエラーが発生しました");
                }// catch
            } // try
            catch
            {
                Debug.Assert(false, nameof(SheetToDictionary.TextToDictionary) + "から"
                    + textName + "の読み込みに失敗しました。");
            } // catch
        }

        /// <summary>
        /// インデックスに応じてアニメーションで画像を変更する
        /// </summary>
        /// <param name="selectScene">現在選んでいる次に読み込むシーン</param>
        public void SetAnimator(SceneController.SelectScene selectScene)
        {
            foreach(var selectSceneButtonAnimator in selectSceneButtonsAnimator)
            {
                // 選ばれているシーンのボタンかどうかをアニメーターにセット
                selectSceneButtonAnimator.Value.SetBool(selectID, selectSceneButtonAnimator.Key == selectScene);
            }
        }

        /// <summary>
        /// ボタンの親オブジェクトを拡大する
        /// </summary>
        public void ButtonsScaleUp()
        {
            // 現在のスケール
            var scale = buttons.transform.localScale;
            // スケールに足す
            scale.x += scaleUpSpeedButtons * Time.deltaTime;
            // 0から1の間に収める
            scale.x = Mathf.Clamp(scale.x, 0.0f, 1.0f);
            // スケールに足す
            scale.y += scaleUpSpeedButtons * Time.deltaTime;
            // 0から1の間に収める
            scale.y = Mathf.Clamp(scale.y, 0.0f, 1.0f);
            // スケールに足す
            scale.z += scaleUpSpeedButtons * Time.deltaTime;
            // 0から1の間に収める
            scale.z = Mathf.Clamp(scale.z, 0.0f, 1.0f);
            // スケールをセット
            buttons.transform.localScale = scale;

            // ボタンの親オブジェクトのスケールが1倍ならば
            if (buttons.transform.localScale == Vector3.one)
            {
                isScalingButton = true;
                return;
            }
        }
    }
}
