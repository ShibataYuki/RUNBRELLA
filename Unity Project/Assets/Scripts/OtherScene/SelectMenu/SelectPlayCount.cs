using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SelectMenu
{
    public class SelectPlayCount : MonoBehaviour
    {
        // 何本先取か
        private int raceNumber = 3;
        public int RaceNumber { get { return raceNumber; } }
        public Dictionary<int, bool> KeyFlag
        { get { return inputManager.KeyFlags; } set { inputManager.KeyFlags = value; } }
        // 入力をチェックするコンポーネント
        private InputManager inputManager;
		// 何本先取かを表示するオブジェクト
        private GameObject playCountObject;
		// 何本先取かを表示するテキスト
        private Text playCountText;

        // 同意しているか
        public enum IsAgree
        {
            Agree,    // 同意する
            Disagree, // 同意しない
        }
        // 同意しているか
        private IsAgree isAgree = IsAgree.Disagree;
        public IsAgree _isAgree { get { return isAgree; } }
        // 同意しているかどうかを表すオブジェクト
        private GameObject isAgreeObject;

        // Start is called before the first frame update
        void Start()
        {
            inputManager = GetComponent<InputManager>();
			// 参照の取得
            playCountObject = GameObject.Find("Canvas/PlayCount");
			// コンポーネントの取得
            playCountText = playCountObject.transform.Find("Text").gameObject.GetComponent<Text>();
            // 参照の取得
            isAgreeObject = playCountObject.transform.Find("AgreeCheck").gameObject;
			// テキストを非表示にする
            PlayCountHide();
        }

        private void Update()
        {
            // テキストの更新
            playCountText.text = string.Format("{0}本先取", raceNumber);
        }

        /// <summary>
        /// テキストの非表示
        /// </summary>
        public void PlayCountHide()
        {
            playCountObject.SetActive(false);
            IsAgreeHide();
        }

        /// <summary>
        /// テキストの表示
        /// </summary>
        public void PlayCountOpen()
        {
            playCountObject.SetActive(true);
        }

        /// <summary>
        /// 同意しているかどうかを表すオブジェクトを非表示にする
        /// </summary>
        public void IsAgreeHide()
        {
            isAgreeObject.SetActive(false);
        }

        /// <summary>
        ///同意しているかどうかを表すオブジェクトを表示する
        /// </summary>
        public void IsAgreeOpen()
        {
            isAgreeObject.SetActive(true);
            // 同意していない状態に変更する
            isAgree = IsAgree.Disagree;
        }

        /// <summary>
        /// 何本先取か入力する人をチェックするメソッド
        /// </summary>
        public void SelectPlayCountEntry()
        {
            for (int ID = 1; ID <= SceneController.Instance.MaxPlayerNumber; ID++)
            {
                // 参加者なら
                if (SceneController.Instance.IsAccess[ID] == true)
                {
                    // 何本先取か選択する
                    if(SceneController.Instance._state == SceneController.State.SelectPlayCount)
                    {
                        // 何本先取か選択する
                        SelectCount(ID);
                        // ステートが変化したなら
                        if(SceneController.Instance._state != SceneController.State.SelectPlayCount)
                        {
                            return;
                        }
                    }
                    // 了承確認
                    else if(SceneController.Instance._state == SceneController.State.SelectSubmitCheck)
                    {
                        // 了承しているか
                        SubmitCheck(ID);
                        // ステートが変化したなら
                        if(SceneController.Instance._state != SceneController.State.SelectSubmitCheck)
                        {
                            return;
                        }
                    }

                    // Bボタンを押したなら
                    if (Input.GetButtonDown(string.Format("Player{0}Cancel", ID)))
                    {
                        // キャラクター選択に戻る
                        SceneController.Instance.Cancel(ID);
                        return;
                    } // if
                } // if
            } // for
        } // SelectPlayCountEntry


        /// <summary>
        /// 何本先取か決める個別の入力
        /// </summary>
        /// <returns></returns>
        private void SelectCount(int ID)
        {
            if (Input.GetButtonDown(string.Format("Player{0}Submit", ID)))
            {
                // ステートを変更
                SceneController.Instance._state = SceneController.State.SelectSubmitCheck;

                IsAgreeOpen();
                return;
            } // if
            // 左右に押し倒されたかチェック
            raceNumber = inputManager.HorizontalKeyIn(ID, raceNumber);
            // １～３の間に収める
            raceNumber = Mathf.Clamp(raceNumber, 1, 3);
        } // SelectCount

        /// <summary>
        /// ルールについて同意するかチェック
        /// </summary>
        /// <param name="ID">ジョイスティックの番号</param>
        public void SubmitCheck(int ID)
        {
            // 左右にスティックを押し倒したかチェック
            isAgree = (IsAgree)inputManager.HorizontalKeyIn(ID, (int)isAgree);
            // Agree と Disagree の間に収める
            isAgree = (IsAgree)Mathf.Clamp((int)isAgree, (int)IsAgree.Agree, (int)IsAgree.Disagree);
            // 決定ボタンを押したなら
            if(inputManager.SubmitKeyIn(ID))
            {
                // 同意しているなら
                if(isAgree == (int)IsAgree.Agree)
                {
                    // ゲームマネージャーに何本先取か伝える
                    GameManager.Instance.RaceNumber = raceNumber;
                    // ステージに遷移
                    SceneController.Instance.GameStart();
                    return;
                } // if
                // 同意していないなら
                else
                {
                    // 何本先取か選択する
                    SceneController.Instance._state = SceneController.State.SelectPlayCount;
                    // 非表示にする
                    IsAgreeHide();
                    return;
                } // else
            } // if
        } // SubmitCheck 
    } // class
} // namesace
