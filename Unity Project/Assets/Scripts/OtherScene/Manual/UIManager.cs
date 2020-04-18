using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manual
{
    public class UIManager : MonoBehaviour
    {
        // ルールブックのプレファブ配列
        [SerializeField]
        private RectTransform[] manualPages = new RectTransform[3];
        // ルールブックをスクロールさせるスクロールバー
        private ScrollRect ruleBookScrollRect;

        // Start is called before the first frame update
        void Start()
        {
            // スクロールビューのオブジェクトの参照を取得
            var scrollRectObject = transform.Find("Scroll View").gameObject;
            // スクロールビューのコンポーネントを取得
            ruleBookScrollRect = scrollRectObject.GetComponent<ScrollRect>();
            // スクロールの表示領域のトランスフォームの参照を取得
            var contentTransform = scrollRectObject.transform.Find("Viewport/Content");

            // スクロールの表示領域のコンポーネントの取得
            var contentRect = contentTransform.GetComponent<RectTransform>();

            // スクロール領域のサイズ変更
            InitScrollSize(contentRect);
            // スクロール領域にルールブックをセット
            SetRuleBook(contentRect);
            // 左端にスクロールさせる
            ruleBookScrollRect.horizontalNormalizedPosition = 0.0f;
        }

        /// <summary>
        /// スクロール領域にルールブックをセットするメソッド
        /// </summary>
        /// <param name="contentRect"></param>
        private void SetRuleBook(RectTransform contentRect)
        {
            // 作業用のアンカーのポジション
            var workAnchoredPositionX = contentRect.anchoredPosition.x
                - (contentRect.pivot.x * contentRect.rect.width);
            foreach (var manualPage in manualPages)
            {
                // ルールブックの生成
                var ruleBookPageObject = Instantiate(manualPage);
                // スクロール領域の子オブジェクトにする
                ruleBookPageObject.transform.SetParent(contentRect);
                // コンポーネントの取得
                var ruleBookRectTransform = ruleBookPageObject.GetComponent<RectTransform>();
                // 新しいアンカーのポジション
                var anchoredPosition = ruleBookRectTransform.anchoredPosition;
                var rect = ruleBookRectTransform.rect;
                // 右端からアンカーのポジションまでの長さ分
                //アンカーのポジションを左に移動
                workAnchoredPositionX +=
                    (rect.width * ruleBookRectTransform.pivot.x);
                anchoredPosition.x = workAnchoredPositionX;
                // ポジションをセット
                ruleBookRectTransform.anchoredPosition = anchoredPosition;
                // ルールブックの左端を計算
                workAnchoredPositionX +=
                    (rect.width * (1 - ruleBookRectTransform.pivot.x));
            }
        }

        /// <summary>
        /// スクロール領域のサイズを初期化する
        /// 横幅：ルールブックの横幅の合計
        /// 縦幅：ルールブックのうち、一番縦幅が長いものの幅に合わせる
        /// </summary>
        /// <param name="contentObject">スクロール領域を表すオブジェクト</param>
        private void InitScrollSize(RectTransform contentRect)
        {

            // サイズを取得するために使用
            var rect = contentRect.rect;

            // ルールブックの合計サイズ
            var size = 0.0f;
            foreach (var manualPage in manualPages)
            {
                // ルールブックすべてのページのサイズを加算
                size += manualPage.rect.width;
                // ルールブックの高さがスクロール領域の高さより高い場合
                if (rect.height < manualPage.rect.height)
                {
                    // ルールブックの高さをセット
                    rect.height = manualPage.rect.height;
                }
            }
            // ルールブックの合計サイズをセット
            rect.width = size;
            // スクロール領域のサイズを変更
            contentRect.sizeDelta = new Vector2(rect.width, rect.height);
        }

        /// <summary>
        /// 右へのスクロール
        /// </summary>
        public void ScrollLeft()
        {

        }

        /// <summary>
        /// 右へのスクロールが出来るかチェック
        /// </summary>
        /// <returns>スクロールの成功/失敗</returns>
        public bool ScrollRight()
        {
            var value = ruleBookScrollRect.horizontalNormalizedPosition;
            if(value >= 1.0f)
            {
                return false;
            }

            return true;
        }
    }
}
