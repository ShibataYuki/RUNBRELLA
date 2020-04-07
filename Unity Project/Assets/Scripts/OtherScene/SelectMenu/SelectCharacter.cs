using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        //
        private CharacterData selectCharacterData;
        private CharacterData leftCharacterData;
        private CharacterData rightCharacterData;

        // スクロールバー
        ScrollRect scrollRect = null;
        // 左右にスクロールするスピード
        float scrollSpeed = 1.0f;

        /// <summary>
        /// フレーム更新処理を行うにあたっての初期化
        /// </summary>
        private void Start()
        {
            // スクロールバーの取得
            var scrollView = transform.Find("Scroll View Character").gameObject;
            scrollRect = scrollView.GetComponent<ScrollRect>();
            scrollRect.horizontalNormalizedPosition = 0.5f;
            var content = transform.Find("Scroll View Character/Viewport/Content");
            // 選択中のプレイヤーを取得
            var selectPlayer = content.Find("SelectPlayer").gameObject;
            // 左側のプレイヤーを取得
            var leftPlayer = content.Find("LeftPlayer").gameObject;
            // 右側のプレイヤーを取得
            var rightPlayer = content.Find("RightPlayer").gameObject;
            // コンポーネントの取得
            selectCharacterData = selectPlayer.GetComponent<CharacterData>();
            leftCharacterData = leftPlayer.GetComponent<CharacterData>();
            rightCharacterData = rightPlayer.GetComponent<CharacterData>();

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
            var horizontal = Input.GetAxis(string.Format("Player{0}Horizontal", ID));
            // スティックを倒していたら
            if (Mathf.Abs(horizontal) > 0.7f)
            {
                // 変更アニメーション中でなければ
                if (isMove == false)
                {
                    // 右に倒したなら
                    if (horizontal > 0.0f)
                    {
                        StartCoroutine(MoveRight());
                    }
                    // 左に倒したなら
                    else if (horizontal < 0.0f)
                    {
                        StartCoroutine(MoveLeft());
                    }

                }
            } // if(Mathf.Abs(horizontal) > 0.7f)

        } // MoveCheck

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
            while (true)
            {
                // 値の変更
                value += (scrollSpeed * Time.deltaTime);
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
                // 次のフレームまで待つ
                yield return null;
            } // while
        } // IEnumerator

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
            while (true)
            {
                // 値の変更
                value -= (scrollSpeed * Time.deltaTime);
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
                // 次のフレームまで待つ
                yield return null;
            } // while
        } // IEnumerator

    } // class
} // namespace
