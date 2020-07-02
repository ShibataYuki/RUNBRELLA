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
        ScrollRect characterScrollRect = null;
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

        private InputManager inputManager;

        /// <summary>
        /// フレーム更新処理を行うにあたっての初期化
        /// </summary>
        private void Start()
        {
            // テキストからパラメータをセット
            ReadText();

            // コンポーネントを取得
            inputManager = SceneController.Instance.gameObject.GetComponent<InputManager>();
            // キャラクターのスクロール領域のオブジェクトを取得
            GameObject characterScrollViewObject = GetCharacterScrollViewObject();
            // スクロール領域のコンポーネントを取得して、初期化する
            InitScrollRect(characterScrollViewObject);
            // スクロール上の真ん中、右、左に表示されるキャラクターのオブジェクト
            GameObject selectPlayer, leftPlayer, rightPlayer;
            // スクロール上の真ん中、右、左に表示されるキャラクターのオブジェクトを取得
            GetCharacterObjects(characterScrollViewObject, out selectPlayer, out leftPlayer, out rightPlayer);
            // 各キャラクターのコンポーネントを取得
            GetCharacterData(selectPlayer, leftPlayer, rightPlayer);
            // キャラクター選択が完了した時に変更する項目をメンバー変数にセット
            SetForSubmit(characterScrollViewObject, selectPlayer);
            // 左右のキャラクターを初期キャラクターに変更
            InitCharacter();
            // シートの読み込みが終わり次第もう一回パラメータをセットしなおす
            StartCoroutine(RoadSheetCheck());
        } // Start

        /// <summary>
        /// シートの読み込みをチェックして、完了したらパラメータを変更する
        /// </summary>
        /// <returns></returns>
        IEnumerator RoadSheetCheck()
        {
            // シートからの読み込みが完了しているのなら
            if (SheetToDictionary.Instance.IsCompletedSheetToText == true)
            {
                // コルーチンを終了
                yield break;
            }
            while (true)
            {
                // スプレッドシートの読み込みが完了したのなら
                if (SheetToDictionary.Instance.IsCompletedSheetToText == true)
                {
                    // パラメータをテキストから読み込んで、メンバー変数を変更
                    ReadText();
                    yield break;
                }
                // 1フレーム待機する
                yield return null;
            }
        }


        /// <summary>
        /// 左右のキャラクターを初期のキャラクターに変更する
        /// </summary>
        private void InitCharacter()
        {
            // 値のセット
            leftCharacterData.DownCheck();
            rightCharacterData.UpCheck();
        }

        /// <summary>
        /// キャラクター選択が完了した時に変更する項目をメンバー変数にセット
        /// </summary>
        /// <param name="characterScrollViewObject"></param>
        /// <param name="selectPlayer"></param>
        private void SetForSubmit(GameObject characterScrollViewObject, GameObject selectPlayer)
        {
            // プレイヤーのNoの表示/非表示を切り替えるコンポーネントの取得
            SetPlayerNo(characterScrollViewObject);
            // キー説明用UIをセット
            SetLeftAndRightArrow();
            // キャラクター選択が完了すると色を変更するイメージをメンバー変数にセット
            SetSelectCharacterImages(selectPlayer);
        }

        /// <summary>
        /// キャラクター選択が完了すると色を変更するイメージをメンバー変数にセット
        /// </summary>
        /// <param name="selectPlayer"></param>
        private void SetSelectCharacterImages(GameObject selectPlayer)
        {
            // キャラクターの後ろのイメージを取得
            characterBackImage = transform.Find("CharacterBack").gameObject.GetComponent<Image>();
            // 選択中のキャラのイメージを取得
            selectCharacterImage = selectPlayer.transform.Find("CharacterImage").gameObject.GetComponent<Image>();
            selectCharacterMotionImage = selectPlayer.transform.Find("CharacterMotionImage").gameObject.GetComponent<Image>();
            selectCharacterNameFrameImage = selectPlayer.transform.Find("CharacterName").gameObject.GetComponent<Image>();
            selectFlavorTextFrameImage = selectPlayer.transform.Find("FlavorText").gameObject.GetComponent<Image>();
        }

        /// <summary>
        /// 右矢印と左矢印のオブジェクトをセット
        /// </summary>
        private void SetLeftAndRightArrow()
        {
            // キー説明用UIをセット
            left = transform.Find("Left").gameObject;
            right = transform.Find("Right").gameObject;
        }

        /// <summary>
        /// プレイヤーのNoの表示/非表示を切り替えるコンポーネントの取得
        /// </summary>
        /// <param name="characterScrollViewObject"></param>
        private void SetPlayerNo(GameObject characterScrollViewObject)
        {
            // プレイヤーのナンバーを表示するオブジェクトを取得
            var charaselectObject = characterScrollViewObject.transform.Find("Charaselect").gameObject;
            // プレイヤーのナンバーの表示/非表示を切り替えるコンポーネントの取得
            charaselect = charaselectObject.GetComponent<Charaselect>();
        }

        /// <summary>
        /// 各キャラクターのコンポーネントを取得
        /// </summary>
        /// <param name="selectPlayer"></param>
        /// <param name="leftPlayer"></param>
        /// <param name="rightPlayer"></param>
        private void GetCharacterData(GameObject selectPlayer, GameObject leftPlayer, GameObject rightPlayer)
        {
            // コンポーネントの取得
            selectCharacterData = selectPlayer.GetComponent<CharacterData>();
            leftCharacterData = leftPlayer.GetComponent<CharacterData>();
            rightCharacterData = rightPlayer.GetComponent<CharacterData>();
        }

        /// <summary>
        /// スクロール上の真ん中、右、左に表示されるキャラクターのオブジェクトを取得
        /// </summary>
        /// <param name="characterScrollViewObject"></param>
        /// <param name="selectPlayer"></param>
        /// <param name="leftPlayer"></param>
        /// <param name="rightPlayer"></param>
        private static void GetCharacterObjects(GameObject characterScrollViewObject, out GameObject selectPlayer, out GameObject leftPlayer, out GameObject rightPlayer)
        {
            // キャラクターの親オブジェクトを取得
            var content = characterScrollViewObject.transform.Find("Viewport/Content");
            // 選択中のプレイヤーを取得
            selectPlayer = content.Find("SelectPlayer").gameObject;
            // 左側のプレイヤーを取得
            leftPlayer = content.Find("LeftPlayer").gameObject;
            // 右側のプレイヤーを取得
            rightPlayer = content.Find("RightPlayer").gameObject;
        }

        /// <summary>
        /// スクロール領域のコンポーネントを取得して、初期化する
        /// </summary>
        /// <param name="characterScrollViewObject">scrollRectにセットするGameObject</param>
        private void InitScrollRect(GameObject characterScrollViewObject)
        {
            // スクロール領域のコンポーネントを取得
            characterScrollRect = characterScrollViewObject.GetComponent<ScrollRect>();
            // スクロールの位置を真ん中にする
            characterScrollRect.horizontalNormalizedPosition = 0.5f;
        }

        /// <summary>
        /// キャラクターのスクロール領域のオブジェクトの参照を取得
        /// </summary>
        /// <returns></returns>
        private GameObject GetCharacterScrollViewObject()
        {
            // シャッターっ部分の表示領域のオブジェクト
            var viewportShutter = transform.Find("Scroll View Shutter/Viewport").gameObject;
            // スクロールバーの取得
            var scrollView = viewportShutter.transform.Find("Content/Scroll View Character").gameObject;
            return scrollView;
        }

        /// <summary>
        /// シートから読み込んで作成したテキストからパラメータをセット
        /// </summary>
        private void ReadText()
        {
            try
            {
                // ディクショナリーを取得
                SheetToDictionary.Instance.TextToDictionary(SceneController.Instance.textName, 
                    out var selectCharacterDictionary);
                try
                {
                    // ディクショナリーからパラメータを読み込みセット
                    scrollSpeed = 0.5f / selectCharacterDictionary
                        ["キャラクター選択で左右スクロールするのにかかる秒数"];
                    arrowMoveValue = selectCharacterDictionary["L/Rが移動するピクセル数"];
                    arrowMoveCount = (int)selectCharacterDictionary["キャラクターを一回スクロールする間にL/Rが移動する回数"];
                    colorValue = selectCharacterDictionary["キャラを決定した後の色の強さ"];
                }
                catch
                {
                    Debug.Assert(false, nameof(SelectCharacter) + "でエラーが発生しました");
                }
            }
            catch
            {
                Debug.Assert(false, nameof(SheetToDictionary.TextToDictionary) + "から" +
                    "Charaselectのディクショナリーを取得できませんでした。");
            }
        }

        /// <summary>
        /// キー入力をチェックするメソッド
        /// キャラクターの変更をチェックするメソッド
        /// </summary>
        /// <param name="controllerNo">ジョイスティックのNo</param>
        public void MoveCheck(CONTROLLER_NO controllerNo)
        {
            // 左右移動
            // 変更アニメーション中でなければ
            if (isMove == false)
            {
                // 左に倒したなら
                if (GamePad.GetButton(GamePad.Button.LeftShoulder, (GamePad.Index)controllerNo))
                {
                    StartCoroutine(MoveLeft());
                    StartCoroutine(MoveLeftArrow());
                    // SE再生
                    SceneController.Instance.PlayChoiseSE();
                }
                // 右に倒したなら
                else if (GamePad.GetButton(GamePad.Button.RightShoulder, (GamePad.Index)controllerNo))
                {
                    StartCoroutine(MoveRight());
                    StartCoroutine(MoveRightArrow());
                    SceneController.Instance.PlayChoiseSE();
                    // SE再生
                    SceneController.Instance.PlayChoiseSE();
                }
                #region キーボード入力
                else
                {
                    if (Input.GetKey(inputManager.LeftKeyCodes[(int)controllerNo - 1]))
                    {
                        StartCoroutine(MoveLeft());
                        StartCoroutine(MoveLeftArrow());
                        SceneController.Instance.IsKeyBoard = true;
                        // SE再生
                        SceneController.Instance.PlayChoiseSE();
                    }
                    else if (Input.GetKey(inputManager.RightKeyCodes[(int)controllerNo - 1]))
                    {
                        StartCoroutine(MoveRight());
                        StartCoroutine(MoveRightArrow());
                        SceneController.Instance.IsKeyBoard = true;
                        // SE再生
                        SceneController.Instance.PlayChoiseSE();
                    }
                }
                #endregion
            } // if
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
            var value = characterScrollRect.horizontalNormalizedPosition;
            // Cosカーブ用の角度
            var angle = 180.0f;
            while (true)
            {
                // Cosカーブを基にスピードを計算
                var speed = Mathf.Cos(Mathf.Deg2Rad * angle) + 1.0f;
                // 値の変更
                value += (speed * scrollSpeed * Time.deltaTime);
                // 0～1の間に収めるように変更
                value = Mathf.Clamp01(value);
                // スクロールビューに値をセット
                characterScrollRect.horizontalNormalizedPosition = value;
                // 横スクロールが完了したら
                if (value >= 1.0f)
                {
                    // インデックスを一つ上げる
                    IndexUp();
                    // スクロールビューの位置を中央に戻す
                    characterScrollRect.horizontalNormalizedPosition = 0.5f;
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
        /// 選んでいるキャラと左右のキャラのインデックスを一つ上げる
        /// </summary>
        public void IndexUp()
        {
            // インデックスの変更
            selectCharacterData.UpCheck();
            leftCharacterData.UpCheck();
            rightCharacterData.UpCheck();
        }

        /// <summary>
        /// 左への移動
        /// </summary>
        /// <returns></returns>
        IEnumerator MoveLeft()
        {
            // 移動中のフラグを立てる
            isMove = true;
            // スクロール量の割合
            var value = characterScrollRect.horizontalNormalizedPosition;
            // Cosカーブ用の角度
            var angle = 180.0f;
            while (true)
            {
                // Cosカーブを基にスピードを計算
                var speed = Mathf.Cos(Mathf.Deg2Rad * angle) + 1.0f;
                // 値の変更
                value -= (speed * scrollSpeed * Time.deltaTime);
                // 0～1の間に収めるように変更
                value = Mathf.Clamp01(value);
                // スクロールビューに値をセット
                characterScrollRect.horizontalNormalizedPosition = value;
                // 横スクロールが完了したら
                if (value <= 0.0f)
                {
                    // インデックスを一つ手前にする
                    IndexDown();
                    // スクロールビューの位置を中央に戻す
                    characterScrollRect.horizontalNormalizedPosition = 0.5f;
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
        /// 選んでいるキャラと左右のキャラのインデックスを一つ下げる
        /// </summary>
        private void IndexDown()
        {
            // インデックスの変更
            selectCharacterData.DownCheck();
            leftCharacterData.DownCheck();
            rightCharacterData.DownCheck();
        }

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
            } // while
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
