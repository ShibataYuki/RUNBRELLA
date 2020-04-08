using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SelectMenu
{
    public class CharacterData : MonoBehaviour
    {
        // 現在選んでいるキャラクターの番号
        private int characterNumber = 0;
        public int CharacterNumber { get { return characterNumber; } }
        // プレイヤーの選択キャラのアニメーター
        Animator animator = null;
        // 選択キャラクターの名前用テキスト
        Text nameText = null;
        // 選択キャラクターのフレーバーテキスト用テキスト
        Text flavorText = null;

        // アニメーターのID
        readonly int selectID = Animator.StringToHash("SelectPlayerType");

        private void Awake()
        {
            // コンポーネントの取得
            animator = GetComponent<Animator>();
            var nameObject = transform.Find("CharacterNameFrame/CharaNameText").gameObject;
            nameText = nameObject.GetComponent<Text>();
            var flavorTextObject = transform.Find("FlavorTextFrame/FlavorText").gameObject;
            flavorText = flavorTextObject.GetComponent<Text>();
        }

        private void Start()
        {
            // アニメーターの初期化
            SetAnimator();
            // テキストの初期化
            SetText();
        }

        /// <summary>
        /// 右に移動できるかチェックして、
        /// 駄目なら左端にループさせるメソッド
        /// </summary>
        /// <returns></returns>
        public void UpCheck()
        {
            // 範囲外に出るようなら
            if (characterNumber >= SceneController.Instance._characterMessages.Length - 1)
            {
                characterNumber = 0;
            }
            else
            {
                // 1つ右に移動
                characterNumber++;
            }

            // アニメーションの切り替え
            SetAnimator();
            // テキストの更新
            SetText();
        } // UpCheck

        /// <summary>
        /// 左に移動できるかチェックして
        /// 駄目なら右端にループさせるメソッド
        /// </summary>
        public void  DownCheck()
        {
            // 範囲外に出るようなら
            if (characterNumber <= 0)
            {
                characterNumber = (SceneController.Instance._characterMessages.Length - 1);
            }
            else
            {
                // １つ左に移動
                characterNumber--;
            }

            // アニメーションの切り替えアニメーションの切り替え
            SetAnimator();
            // テキストの更新
            SetText();
        } // DownCheck

        /// <summary>
        /// テキストに名前とフレーバーテキストをセット
        /// </summary>
        void SetText()
        {
            // テキストにセット
            nameText.text = SceneController.Instance._characterMessages[characterNumber].name;
            flavorText.text = SceneController.Instance._characterMessages[characterNumber].flavorText;
        }

        /// <summary>
        /// アニメーターにパラメータをセット
        /// </summary>
        void SetAnimator()
        {
            // アニメーターにパラメータをセット
            animator.SetInteger(selectID, characterNumber);
        }
    }
}
