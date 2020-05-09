using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Result
{
    public class ButtonManager : MonoBehaviour
    {
        // 現在選んでいるシーンのボタン
        Dictionary<SceneController.SelectScene, GameObject> selectSceneButtons = new Dictionary<SceneController.SelectScene, GameObject>();
        // 拡大縮小するコンポーネント
        ScalingAnimation scalingAnimation;
        // ボタンの親オブジェクト
        GameObject buttons;
        // ボタンの出現時の拡大する速度
        private float scaleUpSpeedButtons = 2.0f;
        // スケーリング中かどうかのフラグ
        private bool isScalingButton = false;
        // ボタンのスケーリング
        // 選ばれていない場合のスケールの倍率
        Vector3 defaultScale = Vector3.one * 0.5f;
        // 選ばれているボタンのスケールの最大
        float maxScale = 1;
        // 選ばれているボタンのスケールの最小
        float minScale = 0.25f;
        // スケールを変更するスピード
        float scaleUpSpeed = 0.75f;
        public bool IsScalingButton { get { return isScalingButton; } }

        // Start is called before the first frame update
        void Start()
        {
            // 各種パラメータをメンバー変数にセット
            SetParameter();
            // 拡大縮小のパラメータをセット
            InitScalingAnimation();
            // ボタンのディクショナリーにセット
            SetButtonDictionary();
        }

        /// <summary>
        /// 拡大縮小するコンポーネントに各種パラメータをセット
        /// </summary>
        private void InitScalingAnimation()
        {
            // 拡大縮小するスクリプトをコンポーネントしているオブジェクト
            var scalingAnimationObject = GameObject.Find("ScalingAnimation");
            // コンポーネントの取得
            scalingAnimation = scalingAnimationObject.GetComponent<ScalingAnimation>();
            // 拡大率をセット
            scalingAnimation.MaxScale = maxScale;
            scalingAnimation.MinScale = minScale;
			// 拡大縮小を行うスピードをセット
            scalingAnimation.ScaleUpSpeed = scaleUpSpeed;
        }

        /// <summary>
        /// ボタンのディクショナリーにセット
        /// </summary>
        private void SetButtonDictionary()
        {
            // ボタンの親オブジェクト
            buttons = GameObject.Find("Canvas/Buttons");
            buttons.transform.localScale = Vector3.zero;
            for (var scene = SceneController.SelectScene.Title; scene <= SceneController.SelectScene.SelectMenu; scene++)
            {
                // 拡大縮小するボタンのオブジェクト
                var button = buttons.transform.Find(scene.ToString()).gameObject;
                // ディクショナリーに追加
                selectSceneButtons.Add(scene, button);
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
                    maxScale = resultDictionary["ボタンの大きさが最大時のスケールの倍率"];
                    minScale = resultDictionary["ボタンの大きさが最小時のスケールの倍率"];
                    defaultScale = Vector3.one *
                        resultDictionary["選ばれていないボタンのスケールの倍率"];
                    scaleUpSpeed = 1f / resultDictionary["選ばれているボタンの拡大処理を行う秒数"];
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
        /// ボタンの拡大縮小を行う
        /// </summary>
        /// <param name="selectScene">現在選んでいる次に読み込むシーン</param>
        public void ScalingUpdate(SceneController.SelectScene selectScene)
        {
            foreach(var selectSceneButton in selectSceneButtons)
            {
                // 選ばれているシーンのボタンならScalingAnimationにセット
                if (selectSceneButton.Key == selectScene)
                {
                    scalingAnimation.SetScalingObject(selectSceneButton.Value);
                }
                // 選ばれてないならスケールを0にする
                else
                {
                    selectSceneButton.Value.transform.localScale = defaultScale;
                }
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
