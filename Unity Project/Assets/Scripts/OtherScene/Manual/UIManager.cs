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
        private List<RuleBook> ruleBooks = new List<RuleBook>();
        // ルールブックをスクロールさせるスクロールバー
        private ScrollRect ruleBookScrollRect;
        // スクロール領域のサイズ
        private float contentSize;
        // 左端のポイント
        private float leftBorderPoint;
        // スクロールの表示領域の大きさ
        private Vector2 viewportSize;
        // スクロールするスピード
        private float scrollSpeed = 1.0f;
        // スクロール中かどうか
        private bool isScroll = false;
        // 何ページ目かチェックするインデックス
        private int index = 0;
        // Lアイコンのレクトトランスフォーム
        private RectTransform leftIconTransform;
        // Rアイコンのレクトトランスフォーム
        private RectTransform rightIconTransform;
        // 矢印の移動量
        float arrowMoveValue = 27f;
        // 何回移動させるか
        int arrowMoveCount = 2;

        // Start is called before the first frame update
        void Start()
        {
            // スクロールビューのオブジェクトの参照を取得
            GameObject scrollRectObject = GetScrollView();
            // スクロールビューのレクトトランスフォームをセット
            SetRuleBookScrollRect(scrollRectObject);
            // スクロールの表示領域を取得
            SetViewportSize(scrollRectObject);
            // スクロール領域のレクトトランスフォームを取得
            RectTransform contentRect = GetContentRect(scrollRectObject);
            contentRect.anchoredPosition = Vector2.zero;
            // スクロール領域のサイズ変更
            InitScrollSize(contentRect);
            // 左端にスクロールさせる
            ruleBookScrollRect.horizontalNormalizedPosition = 0.0f;
            // スクロールの左端のポイントを求める
            //leftBorderPoint = contentRect.anchoredPosition.x - (contentRect.rect.width * contentRect.pivot.x);
            leftBorderPoint = -contentSize * 0.5f;
            // ルールブックのリストにセット
            SetRuleBookList(contentRect);
            ruleBooks[index].Entry();
            // Lアイコン・Rアイコンのレクトトランスフォームを取得
            SetIconTransform();
            // パラメータをテキストから読み込んで、メンバー変数を変更
            ReadText();
        }

        /// <summary>
        /// シートから読み込んで作成したテキストからパラメータをセット
        /// </summary>
        private void ReadText()
        {
            try
            {
                // ディクショナリーを取得
                SheetToDictionary.Instance.TextToDictionary("CharaSelect",
                    out var selectCharacterDictionary);
                try
                {
                    // ディクショナリーからパラメータを読み込みセット
                    scrollSpeed = 0.5f / selectCharacterDictionary
                        ["キャラクター選択で左右スクロールするのにかかる秒数"];
                    arrowMoveValue = selectCharacterDictionary["L/Rが移動するピクセル数"];
                    arrowMoveCount = (int)selectCharacterDictionary["キャラクターを一回スクロールする間にL/Rが移動する回数"];
                }
                catch
                {
                    Debug.Assert(false, nameof(UIManager) + "でエラーが発生しました");
                }
            }
            catch
            {
                Debug.Assert(false, nameof(SheetToDictionary.TextToDictionary) + "から" +
                    "Charaselectのディクショナリーを取得できませんでした。");
            }
        }

        /// <summary>
        /// Lアイコン・Rアイコンのレクトトランスフォームをセット
        /// </summary>
        private void SetIconTransform()
        {
            // Lアイコンのオブジェクトを取得
            var leftIconObject = transform.Find("Left");
            // アイコンのオブジェクトからコンポーネントを取得
            leftIconTransform = leftIconObject.GetComponent<RectTransform>();
            // Rアイコンのオブジェクトを取得
            var rightIconObject = transform.Find("Right");
            // アイコンのオブジェクトからコンポーネントを取得
            rightIconTransform = rightIconObject.GetComponent<RectTransform>();
        }

        /// <summary>
        /// ルールブックのリストにルールブックをセット
        /// </summary>
        /// <param name="contentRect">親オブジェクトのRectTransform</param>
        private void SetRuleBookList(RectTransform contentRect)
        {
            for(int i = 1; i <= manualPages.Length; i++)
            {
                var pageObject = contentRect.transform.Find(string.Format("Manual_Page{0}", i)).gameObject;
                var ruleBook = pageObject.GetComponent<RuleBook>();
                ruleBooks.Add(ruleBook);
            }
        }

        /// <summary>
        /// スクロールビューのオブジェクトの参照を取得
        /// </summary>
        /// <returns></returns>
        private GameObject GetScrollView()
        {
            // スクロールビューのオブジェクトの参照を取得
            return transform.Find("Scroll View").gameObject;
        }

        /// <summary>
        /// スクロールビューのレクトトランスフォームをセットするメソッド
        /// </summary>
        /// <param name="scrollRectObject"></param>
        private void SetRuleBookScrollRect(GameObject scrollRectObject)
        {
            // スクロールビューのコンポーネントを取得
            ruleBookScrollRect = scrollRectObject.GetComponent<ScrollRect>();
        }

        /// <summary>
        /// スクロールの表示領域を取得
        /// </summary>
        /// <param name="scrollRectObject"></param>
        private void SetViewportSize(GameObject scrollRectObject)
        {
            // スクロールビューのレクトトランスフォームを取得
            var scrollViewRectTransform = scrollRectObject.GetComponent<RectTransform>();
            // スクロールの表示領域の大きさを取得
            viewportSize = scrollViewRectTransform.rect.size;
        }

        /// <summary>
        /// スクロール領域のレクトトランスフォームを取得
        /// </summary>
        /// <param name="scrollRectObject"></param>
        /// <returns></returns>
        private static RectTransform GetContentRect(GameObject scrollRectObject)
        {
            // スクロールの表示領域のトランスフォームの参照を取得
            var contentTransform = scrollRectObject.transform.Find("Viewport/Content");
            // スクロールの表示領域のコンポーネントの取得
            var contentRect = contentTransform.GetComponent<RectTransform>();
            return contentRect;
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
            contentSize = 0.0f;
            foreach (var manualPage in manualPages)
            {
                // ルールブックすべてのページのサイズを加算
                contentSize += manualPage.rect.width;
                // ルールブックの高さがスクロール領域の高さより高い場合
                if (rect.height < manualPage.rect.height)
                {
                    // ルールブックの高さをセット
                    rect.height = manualPage.rect.height;
                }
            }
            // ルールブックの合計サイズをセット
            rect.width = contentSize;
            // スクロール領域のサイズを変更
            contentRect.sizeDelta = new Vector2(rect.width, rect.height);
        }

        /// <summary>
        /// 右へのスクロールが出来るかチェック
        /// </summary>
        /// <returns>スクロールの成功/失敗</returns>
        public bool ScrollLeftCheck()
        {
            var value = ruleBookScrollRect.horizontalNormalizedPosition;
            if (value <= 0.0f)
            {
                return false;
            }
            // スクロール中でなければ
            if (isScroll == false)
            {
                // スクロール処理
                StartCoroutine(ScrollLeft());
                // Lの矢印を動かす
                StartCoroutine(MoveLeftArrow());
            }
            return true;
        }

        /// <summary>
        /// 右へのスクロールが出来るかチェック
        /// </summary>
        /// <returns>スクロールの成功/失敗</returns>
        public bool ScrollRightCheck()
        {
            var value = ruleBookScrollRect.horizontalNormalizedPosition;
            if (value >= 1.0f)
            {
                return false;
            }
            // スクロール中でなければ
            if (isScroll == false)
            {
                // スクロール処理
                StartCoroutine(ScrollRight());
                // Rの矢印を動かす
                StartCoroutine(MoveRightArrow());
            }
            return true;
        }

        /// <summary>
        /// 左にスクロール
        /// </summary>
        /// <returns></returns>
        private IEnumerator ScrollLeft()
        {
            ruleBooks[index].Exit();
            isScroll = true;
            var diffSize = contentSize - viewportSize.x;
            var left = diffSize * ruleBookScrollRect.horizontalNormalizedPosition + leftBorderPoint;
            // 計算誤差をチェック
            if (left > (int) left && (left -(int) left) < 0.05f)
            {
                left = (int)left;
            }
            else if(left <(int) (left + 1) && ((int) left + 1 - left)< 0.05f)
            {
                left = (int)left + 1;
            }
            var right = left + viewportSize.x;
            float targetPosLeft = (left - viewportSize.x);
            // 左端が画面内に収まっているなら
            if (left <= -(manualPages[index].rect.width * manualPages[index].pivot.x) + manualPages[index].anchoredPosition.x)
            {
                index--;
                // 要素数をオーバーしないように範囲内に収める
                index = Mathf.Clamp(index, 0, ruleBooks.Count - 1);
            }
            Debug.Log(index);
            var manualPage = manualPages[index];
            // 画面サイズ以下なら
            if (manualPages[index].rect.width <= viewportSize.x)
            {
                var targetCenter = manualPage.anchoredPosition.x + (manualPage.rect.width * (0.5f - manualPage.pivot.x));
                targetPosLeft = (targetCenter - (viewportSize.x * 0.5f));
            }
            else
            {
                var pageRight = manualPage.anchoredPosition.x + (manualPage.rect.width * (1 - manualPage.pivot.x));
                if(pageRight > left && pageRight < right)
                {
                    var targetPosRight = pageRight;
                    targetPosLeft = targetPosRight - viewportSize.x;
                }
                else
                {
                    // いくつに区切るか
                    int pageCount = (int)(manualPages[index].rect.width / viewportSize.x);
                    if (pageCount < (manualPages[index].rect.width / viewportSize.x))
                    {
                        pageCount++;
                    }
                    // 一ブロック当たりの横幅
                    var onePageWidth = manualPages[index].rect.width / pageCount;
                    // そのページに入ってなければ
                    if (left <= manualPages[index].anchoredPosition.x + (manualPages[index].rect.width * (-manualPages[index].pivot.x)))
                    {
                        // 左端が前のページの左端になるように
                        targetPosLeft = manualPages[index].rect.width * (-manualPages[index].pivot.x) + manualPages[index].anchoredPosition.x;
                    }
                    else
                    {
                        for (int i = 0; i < pageCount; i++)
                        {
                            if (manualPages[index].anchoredPosition.x - (manualPages[index].rect.width * manualPages[index].pivot.x) +
                                (i * onePageWidth) <= left && left <= ((i + 1) * onePageWidth) +
                                manualPages[index].anchoredPosition.x - (manualPages[index].rect.width * manualPages[index].pivot.x))
                            {
                                targetPosLeft = manualPages[index].anchoredPosition.x -
                                    (manualPages[index].rect.width * manualPages[index].pivot.x) + (i * onePageWidth);
                                break;
                            } // if
                        } // for
                    } // else
                } // else (pageRight > left && pageRight < right)
            } // else (manualPage.rect.width <= viewPortSize.x)
            var min = ((targetPosLeft - leftBorderPoint) / diffSize);
            min = Mathf.Clamp01(min);
            var value = ruleBookScrollRect.horizontalNormalizedPosition;
            var diff = value - min;
            while (true)
            {
                value -= diff * Time.deltaTime * scrollSpeed;
                if (value <= min)
                {
                    ruleBookScrollRect.horizontalNormalizedPosition = min;
                    ruleBooks[index].Entry();
                    isScroll = false;
                    yield break;
                }
                else
                {
                    ruleBookScrollRect.horizontalNormalizedPosition = value;
                    yield return null;
                }
            }

        }

        /// <summary>
        /// 右にスクロール
        /// </summary>
        /// <returns></returns>
        private IEnumerator ScrollRight()
        {
            ruleBooks[index].Exit();
            isScroll = true;
            var diffSize = contentSize - viewportSize.x;
            var left = diffSize * ruleBookScrollRect.horizontalNormalizedPosition + leftBorderPoint;
            // 計算誤差をチェック
            if (left > (int)left && (left - (int)left) < 0.05f)
            {
                left = (int)left;
            }
            else if (left < (int)(left + 1) && ((int)left + 1 - left) < 0.05f)
            {
                left = (int)left + 1;
            }

            var right = left + viewportSize.x;
            float targetPosLeft = right;
            // 画像の右端が画面内に収まっているなら
            if (right >= (manualPages[index].rect.width * (1 - manualPages[index].pivot.x)) + manualPages[index].anchoredPosition.x)
            {
                index++;
                // 要素数をオーバーしないように範囲内に収める
                index = Mathf.Clamp(index, 0, ruleBooks.Count - 1);
            }
            Debug.Log(index);
            var manualPage = manualPages[index];
            // 画面サイズ以下なら
            if (manualPage.rect.width <= viewportSize.x)
            {
                var targetCenter = manualPage.anchoredPosition.x + (manualPage.rect.width * (0.5f - manualPage.pivot.x));
                targetPosLeft = (targetCenter - (viewportSize.x * 0.5f));
            } // if
            else 
            {
                var pageLeft = manualPage.anchoredPosition.x - (manualPage.rect.width * manualPage.pivot.x);
                if(pageLeft > left && pageLeft < right)
                {
                    targetPosLeft = pageLeft;
                } // if
                else
                {
                    // いくつに区切るか
                    int pageCount = (int)(manualPages[index].rect.width / viewportSize.x);
                    if (pageCount < (manualPages[index].rect.width / viewportSize.x))
                    {
                        pageCount++;
                    }
                    // 一ブロック当たりの横幅
                    var onePageWidth = manualPages[index].rect.width / pageCount;
                    // そのページに入ってなければ
                    if (right >= manualPages[index].anchoredPosition.x + (manualPages[index].rect.width * (1 - manualPages[index].pivot.x)))
                    {
                        // 右端が前のページの右端になるように
                        var targetPosRight = manualPages[index].rect.width * (1 - manualPages[index].pivot.x) + manualPages[index].anchoredPosition.x;
                        // 右端の座標から左端の座標を割り出し
                        targetPosLeft = targetPosRight - viewportSize.x;
                    } // if
                    else
                    {
                        var center = left + (viewportSize.x * 0.5f);
                        for (int i = 0; i < pageCount; i++)
                        {
                            if (manualPage.anchoredPosition.x - (manualPage.rect.width * manualPage.pivot.x) + (i * onePageWidth) <= center && 
                                center <= ((i + 1) * onePageWidth) + manualPage.anchoredPosition.x - (manualPage.rect.width * manualPage.pivot.x))
                            {
                                if(i <= 0)
                                {
                                    targetPosLeft = manualPage.anchoredPosition.x - (manualPage.rect.width * manualPage.pivot.x);
                                }
                                else if(i >= (pageCount - 1))
                                {
                                    var targetPosRight = manualPage.anchoredPosition.x + (manualPage.rect.width * (1 - manualPage.pivot.x));
                                    targetPosLeft = targetPosRight - viewportSize.x;
                                } // else if
                                else
                                {
                                    targetPosLeft = manualPage.anchoredPosition.x - 
                                        (manualPages[index].rect.width * manualPages[index].pivot.x) + ((i + 1) * onePageWidth);
                                } // else
                                break;
                            } // if (manualPage.anchoredPosition.x - (manualPage.rect.width * manualPage.pivot.x) + (i * onePageWidth) <= center && center <= ((i + 1) * onePageWidth) + manualPage.anchoredPosition.x - (manualPage.rect.width * manualPage.pivot.x))
                        } // for
                    }
                } // else (pageLeft > left && pageLeft < right)
            } // else (manualPage.rect.width <= viewPortSize.x)

            var max = ((targetPosLeft - leftBorderPoint) / diffSize);
            max = Mathf.Clamp01(max);
            var value = ruleBookScrollRect.horizontalNormalizedPosition;
            var diff = max - value;
            while (true)
            {
                value += diff * Time.deltaTime * scrollSpeed;
                if (value >= max)
                {
                    ruleBookScrollRect.horizontalNormalizedPosition = max;
                    ruleBooks[index].Entry();
                    isScroll = false;
                    yield break;
                }
                else
                {
                    ruleBookScrollRect.horizontalNormalizedPosition = value;
                    yield return null;
                }
            } // while

        } // IEnumerator

        private void Update()
        {
            if(isScroll == false)
            {
                ruleBooks[index].Do();
            }
        }

        /// <summary>
        /// 右の矢印を移動させる処理
        /// </summary>
        /// <returns></returns>
        IEnumerator MoveRightArrow()
        {
            // 移動前のポジション
            var defaultPosition = rightIconTransform.anchoredPosition;
            // 移動した量
            var move = 0.0f;

            while (true)
            {
                // 移動量を計算
                move += scrollSpeed * 2 * arrowMoveValue * Time.deltaTime * arrowMoveCount;
                // 移動量の中に収める
                move = Mathf.Clamp(move, 0.0f, arrowMoveValue);
                // ポジションをセット
                var position = new Vector2(defaultPosition.x + (move - arrowMoveValue), defaultPosition.y);
                rightIconTransform.anchoredPosition = position;
                // 移動したなら
                if (move >= arrowMoveValue)
                {
                    // 移動量をリセット
                    move = 0.0f;
                }

                // 移動が終了したなら
                if (isScroll == false)
                {
                    // 元のポジションに戻す
                    rightIconTransform.anchoredPosition = defaultPosition;
                    yield break;
                }
                // 次のフレームまで待つ
                yield return null;
            }
        }

        /// <summary>
        /// 左の矢印を移動させる処理
        /// </summary>
        /// <returns></returns>
        IEnumerator MoveLeftArrow()
        {
            // 移動前のポジション
            var defaultPosition = leftIconTransform.anchoredPosition;
            // 移動した量
            var move = 0.0f;

            while (true)
            {
                // 移動量を計算
                move += scrollSpeed * 2 * arrowMoveValue * Time.deltaTime * arrowMoveCount;
                // 移動量の中に収める
                move = Mathf.Clamp(move, 0.0f, arrowMoveValue);
                // ポジションをセット
                var position = new Vector2(defaultPosition.x - (move - arrowMoveValue), defaultPosition.y);
                leftIconTransform.anchoredPosition = position;
                // 移動したなら
                if (move >= arrowMoveValue)
                {
                    // 移動量をリセット
                    move = 0.0f;
                }

                // 移動が終了したなら
                if (isScroll == false)
                {
                    // 元のポジションに戻す
                    leftIconTransform.anchoredPosition = defaultPosition;
                    yield break;
                }
                // 次のフレームまで待つ
                yield return null;
            } // while
        }

    }
}