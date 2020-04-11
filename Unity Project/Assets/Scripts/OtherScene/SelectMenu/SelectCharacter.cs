using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamepadInput;

namespace SelectMenu
{
    public class SelectCharacter : MonoBehaviour
    {
        // 移動中かどうかのフラグ
        bool isMove = false;

        // get set
        public bool IsMove { get { return isMove; } set { isMove = value; } }
        public int SelectCharacterNumber
        { get { return selectCharacterData.CharacterNumber; } }

        // 選択中のキャラの情報
        private CharacterData selectCharacterData;
        // 選択中のキャラの左右のキャラの情報
        private CharacterData leftCharacterData;
        private CharacterData rightCharacterData;

        // スクロールバー
        ScrollRect scrollRect = null;
        // 左右にスクロールするスピード
        float scrollSpeed = 1.0f;
        // 矢印の移動量
        float arrowMoveValue = 27f;
        // 何回移動させるか
        int arrowMoveCount = 2;

        // キー説明用UI
        private GameObject left;
        private GameObject right;
        // 選択中のキャラのイメージ
        Image selectCharacterImage = null;
        Image selectCharacterMotionImage = null;
        Image characterBackImage = null;
        Image selectCharacterNameFrameImage = null;
        Image selectFlavorTextFrameImage = null;

        // Playerいくつかを表示するUI
        Charaselect charaselect = null;

        // 変更後の色の倍率
        private float colorValue = 0.5f;

        /// <summary>
        /// フレーム更新処理を行うにあたっての初期化
        /// </summary>
        private void Start()
        {
            var viewportShutter = transform.Find("Scroll View Shutter/Viewport").gameObject;
            // スクロールバーの取得
            var scrollView = viewportShutter.transform.Find("Content/Scroll View Character").gameObject;
            scrollRect = scrollView.GetComponent<ScrollRect>();
            scrollRect.horizontalNormalizedPosition = 0.5f;
            var content = scrollView.transform.Find("Viewport/Content");
            // 選択中のプレイヤーを取得
            var selectPlayer = content.Find("SelectPlayer").gameObject;
            // 左側のプレイヤーを取得
            var leftPlayer = content.Find("LeftPlayer").gameObject;
            // 右側のプレイヤーを取得
            var rightPlayer = content.Find("RightPlayer").gameObject;
            var charaselectObject = scrollView.transform.Find("Charaselect").gameObject;
            charaselect = charaselectObject.GetComponent<Charaselect>();
            // コンポーネントの取得
            selectCharacterData = selectPlayer.GetComponent<CharacterData>();
            leftCharacterData = leftPlayer.GetComponent<CharacterData>();
            rightCharacterData = rightPlayer.GetComponent<CharacterData>();
            // キー説明用UIをセット
            left = transform.Find("Left").gameObject;
            right = transform.Find("Right").gameObject;
            // 選択中のキャラのイメージを取得
            selectCharacterImage = selectPlayer.transform.Find("CharacterImage").gameObject.GetComponent<Image>();
            selectCharacterMotionImage = selectPlayer.transform.Find("CharacterMotionImage").gameObject.GetComponent<Image>();
            characterBackImage = transform.Find("CharacterBack").gameObject.GetComponent<Image>();
            selectCharacterNameFrameImage = selectPlayer.transform.Find("CharacterName").gameObject.GetComponent<Image>();
            selectFlavorTextFrameImage = selectPlayer.transform.Find("FlavorText").gameObject.GetComponent<Image>();
            // セット
            leftCharacterData.DownCheck();
            rightCharacterData.UpCheck();
        } // Start

        /// <summary>
        /// キー入力をチェックするメソッド
        /// キャラクターの変更をチェックするメソッド
        /// </summary>
        /// <param name="ID">ジョイスティックのID</param>
        public void MoveCheck(int ID)
        {
            // 左右移動
            // 変更アニメーション中でなければ
            if (isMove == false)
            {
                // 左に倒したなら
                if (GamePad.GetButton(GamePad.Button.LeftShoulder, (GamePad.Index)ID))
                {
                    StartCoroutine(MoveLeft());
                    StartCoroutine(MoveLeftArrow());
                }
                // 右に倒したなら
                else if (GamePad.GetButton(GamePad.Button.RightShoulder, (GamePad.Index)ID))
                {
                    StartCoroutine(MoveRight());
                    StartCoroutine(MoveRightArrow());
                }
            } // if(Mathf.Abs(horizontal) > 0.7f)

        } // MoveCheck

        /// <summary>
        /// 右への移動
        /// </summary>
        /// <returns></returns>
        IEnumerator MoveRight()
        {
            // 移動中のフラグを立てる
            isMove = true;
            // スクロール量の割合
            var value = scrollRect.horizontalNormalizedPosition;
            // Cosカーブ用の角度
            var angle = 180.0f;
            while (true)
            {
                // Cosカーブを基にスピードを計算
                var speed = Mathf.Cos(Mathf.Deg2Rad * angle) + 1.0f;
                // 値の変更
                value += (speed * scrollSpeed * Time.deltaTime);
                // 0～1の間に収めるように変更
                Mathf.Clamp(value, 0.0f, 1.0f);
                // スクロールビューに値をセット
                scrollRect.horizontalNormalizedPosition = value;
                // 横スクロールが完了したら
                if (value >= 1.0f)
                {
                    // インデックスの変更
                    selectCharacterData.UpCheck();
                    leftCharacterData.UpCheck();
                    rightCharacterData. UpCheck();
                    // スクロールビューの位置を中央に戻す
                    scrollRect.horizontalNormalizedPosition = 0.5f;
                    // 移動中のフラグをオフにする
                    isMove = false;
                    // コルーチンの終了
                    yield break;
                } // if
                // Cosカーブ用の角度を変える
                angle += Time.deltaTime * (180.0f * scrollSpeed) * 2;
                // 次のフレームまで待つ
                yield return null;
            } // while
        } // IEnumerator

        /// <summary>
        /// 左への移動
        /// </summary>
        /// <returns></returns>
        IEnumerator MoveLeft()
        {
            // 移動中のフラグを立てる
            isMove = true;
            // スクロール量の割合
            var value = scrollRect.horizontalNormalizedPosition;
            // Cosカーブ用の角度
            var angle = 180.0f;
            while (true)
            {
                // Cosカーブを基にスピードを計算
                var speed = Mathf.Cos(Mathf.Deg2Rad * angle) + 1.0f;
                // 値の変更
                value -= (speed * scrollSpeed * Time.deltaTime);
                // 0～1の間に収めるように変更
                Mathf.Clamp(value, 0.0f, 1.0f);
                // スクロールビューに値をセット
                scrollRect.horizontalNormalizedPosition = value;
                // 横スクロールが完了したら
                if (value <= 0.0f)
                {
                    // インデックスの変更
                    selectCharacterData.DownCheck();
                    leftCharacterData.DownCheck();
                    rightCharacterData.DownCheck();
                    // スクロールビューの位置を中央に戻す
                    scrollRect.horizontalNormalizedPosition = 0.5f;
                    // 移動中のフラグをオフにする
                    isMove = false;
                    // コルーチンの終了
                    yield break;
                } // if
                // Cosカーブ用の角度を変える
                angle += Time.deltaTime * (180.0f * scrollSpeed) * 2;
                // 次のフレームまで待つ
                yield return null;
            } // while
        } // IEnumerator

        /// <summary>
        /// 右の矢印を移動させる処理
        /// </summary>
        /// <returns></returns>
        IEnumerator MoveRightArrow()
        {
            // レクトトランスフォームの取得
            var rightRectTransform = right.GetComponent<RectTransform>();
            // 移動前のポジション
            var defaultPosition = rightRectTransform.anchoredPosition;
            // 移動した量
            var move = 0.0f;

            while(true)
            {
                // 移動量を計算
                move += scrollSpeed * 2 * arrowMoveValue * Time.deltaTime * arrowMoveCount;
                // 移動量の中に収める
                move = Mathf.Clamp(move, 0.0f, arrowMoveValue);
                // ポジションをセット
                var position = new Vector2(defaultPosition.x + (move - arrowMoveValue), defaultPosition.y);
                rightRectTransform.anchoredPosition = position;
                // 移動したなら
                if(move >= arrowMoveValue)
                {
                    // 移動量をリセット
                    move = 0.0f;
                }

                // 移動が終了したなら
                if(isMove == false)
                {
                    // 元のポジションに戻す
                    rightRectTransform.anchoredPosition = defaultPosition;
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
            // レクトトランスフォームの取得
            var leftRectTransform = left.GetComponent<RectTransform>();
            // 移動前のポジション
            var defaultPosition = leftRectTransform.anchoredPosition;
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
                leftRectTransform.anchoredPosition = position;
                // 移動したなら
                if (move >= arrowMoveValue)
                {
                    // 移動量をリセット
                    move = 0.0f;
                }

                // 移動が終了したなら
                if (isMove == false)
                {
                    // 元のポジションに戻す
                    leftRectTransform.anchoredPosition = defaultPosition;
                    yield break;
                }
                // 次のフレームまで待つ
                yield return null;
            }
        }

        /// <summary>
        /// キー説明用UIを表示する
        /// </summary>
        private void KeyUIOpen()
        {
            if(left.activeSelf == true)
            {
                return;
            }
            left.SetActive(true);
            right.SetActive(true);
            charaselect.gameObject.SetActive(true);
        }

        /// <summary>
        /// キー説明用UIを非表示にする
        /// </summary>
        private void KeyUIClose()
        {
            if (left.activeSelf == false)
            {
                return;
            }
            left.SetActive(false);
            right.SetActive(false);
            charaselect.Close();
        }

        /// <summary>
        /// キャラ選択で決定したときに行うメソッド
        /// </summary>
        public void Submit()
        {
            KeyUIClose();
            SetSelectCharacterColor(colorValue);
        }

        /// <summary>
        /// キャラ選択で選択しなおす場合に行うメソッド
        /// </summary>
        public void Cansel()
        {
            KeyUIOpen();
            SetSelectCharacterColor(1.0f / colorValue);
        }

        /// <summary>
        /// キャラ選択の色と掛け算を行う
        /// </summary>
        /// <param name="setColor">掛け合わせる色</param>
        void SetSelectCharacterColor(Color setColor)
        {
            selectCharacterImage.color *= setColor;
            selectCharacterMotionImage.color *= setColor;
            characterBackImage.color *= setColor;
            selectCharacterNameFrameImage.color *= setColor;
            selectFlavorTextFrameImage.color *= setColor;
        }

        /// <summary>
        /// キャラ選択の色と掛け算を行う
        /// </summary>
        /// <param name="colorValue">掛ける倍率/param>
        void SetSelectCharacterColor(float colorValue)
        {
            selectCharacterImage.color *= colorValue;
            selectCharacterMotionImage.color *= colorValue;
            characterBackImage.color *= colorValue;
            selectCharacterNameFrameImage.color *= colorValue;
            selectFlavorTextFrameImage.color *= colorValue;
        }

        /// <summary>
        /// キャラ選択の色と掛け算を行う
        /// </summary>
        /// <param name="setR">赤に掛ける倍率</param>
        /// <param name="setG">緑に掛ける倍率</param>
        /// <param name="setB">青に掛ける倍率</param>
        void SetSelectCharacterColor(float setR, float setG, float setB)
        {
            selectCharacterImage.color *= new Color(setR, setG, setB);
            selectCharacterMotionImage.color *= new Color(setR, setG, setB);
            characterBackImage.color *= new Color(setR, setG, setB);
            selectCharacterNameFrameImage.color *= new Color(setR, setG, setB);
            selectFlavorTextFrameImage.color *= new Color(setR, setG, setB);
        }
    } // class
} // namespace
