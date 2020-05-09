using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
    public class ButtonManager : MonoBehaviour
    {
        // ボタンのディクショナリー
        private Dictionary<SceneController.SelectIndex, Transform> selectButtons = new Dictionary<SceneController.SelectIndex, Transform>();
        // 拡大縮小を行うコンポーネント
        private ScalingAnimation scalingAnimation;
        // 選ばれていないボタンのスケール
        private Vector3 defaultScale = Vector3.one * 0.5f;
        // 選ばれているボタンのスケールの最小
        private float minScale = 0.25f;
        // 選ばれているボタンのスケールの最大
        private float maxScale = 1.0f;
        // スケーリングするスピード
        private float scaleUpSpeed = 0.75f;

        // Start is called before the first frame update
        void Start()
        {
            // ボタンのディクショナリーにセット
            SetButtonDictionary();
            // 各種パラメータをセット
            SetScalingAnimationParameter();
            // シートの読み込みが終わり次第もう一回パラメータをセットしなおす
            StartCoroutine(RoadSheetCheck());
        }

        /// <summary>
        /// シートの読み込みをチェックして、完了したらパラメータを変更する
        /// </summary>
        /// <returns></returns>
        IEnumerator RoadSheetCheck()
        {
            // シートからの読み込みが完了しているのなら
            if(SheetToDictionary.Instance.IsCompletedSheetToText == true)
            {
                // コルーチンを終了
                yield break;
            }
            while(true)
            {
                // スプレッドシートの読み込みが完了したのなら
                if(SheetToDictionary.Instance.IsCompletedSheetToText == true)
                {
                    // パラメータをテキストから読み込んで、scalingAnimationにセット
                    SetScalingAnimationParameter();
                    yield break;
                }
                // 1フレーム待機する
                yield return null;
            }
        }

        /// <summary>
        /// 拡大縮小のパラメータをセット
        /// </summary>
        private void SetScalingAnimationParameter()
        {
            // 各種パラメータをメンバー変数にセット
            SetMemberParameter();
            // 拡大縮小のパラメータをセット
            InitScalingAnimation();
        }

        /// <summary>
        /// ボタンのディクショナリーにセット
        /// </summary>
        private void SetButtonDictionary()
        {
            // ボタンの親オブジェクトの参照を取得
            var buttons = GameObject.Find("Canvas/Buttons");
            for (var i = SceneController.SelectIndex.SelectMenu; i <= SceneController.SelectIndex.Exit; i++)
            {
                // 子オブジェクトのボタンの参照を取得
                var button = buttons.transform.Find(i.ToString());
                // ディクショナリーに追加
                selectButtons.Add(i, button);
            }
        }

        /// <summary>
        /// テキストから読み込み各種パラメータをメンバー変数にセット
        /// </summary>
        private void SetMemberParameter()
        {
            // 読み込むテキストの名前
            var textName = "Title";
            try
            {
                // テキストの中のデータをセットするディクショナリー
                SheetToDictionary.Instance.TextToDictionary(textName, out var titleDictionary);
                try
                {
                    // ディクショナリ－からパラメータを取得してセット
                    maxScale = titleDictionary["ボタンの大きさが最大時のスケールの倍率"];
                    minScale = titleDictionary["ボタンの大きさが最小時のスケールの倍率"];
                    defaultScale = Vector3.one *
                        titleDictionary["選ばれていないボタンのスケールの倍率"];
                    scaleUpSpeed = 1f / titleDictionary["スケーリング処理を行う秒数"];

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
        } // SetParameter

        /// <summary>
        /// 拡大縮小のパラメータをセット
        /// </summary>
        private void InitScalingAnimation()
        {
            // 拡大縮小を行うコンポーネントを取得
            var scalingAnimationObject = GameObject.Find("ScalingAnimation");
            scalingAnimation = scalingAnimationObject.GetComponent<ScalingAnimation>();
            // パラメータをセット
            scalingAnimation.MinScale = minScale;
            scalingAnimation.MaxScale = maxScale;
            scalingAnimation.ScaleUpSpeed = scaleUpSpeed;
        }

        /// <summary>
        /// インデックスに応じてスケールを変更する
        /// </summary>
        public void ChangeScale(SceneController.SelectIndex selectIndex)
        {
            for (var i = SceneController.SelectIndex.SelectMenu; i <= SceneController.SelectIndex.Exit; i++)
            {
                // 選ばれているなら
                if (i == selectIndex)
                {
                    // インデックス番目のボタンのゲームオブジェクトを拡大縮小させる
                    scalingAnimation.SetScalingObject(selectButtons[i].gameObject);
                }
                // 選ばれていないなら
                else
                {
                    // 通常時のスケールに変更する
                    selectButtons[i].localScale = defaultScale;
                }
            }
        }

    }
}
