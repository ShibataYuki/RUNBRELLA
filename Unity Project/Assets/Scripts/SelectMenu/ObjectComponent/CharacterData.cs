using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class CharacterData : MonoBehaviour
    {
        // 現在選んでいるキャラクターの番号
        private int characterNumber = 0;
        public int CharacterNumber { get { return characterNumber; } }
        // プレイヤーの選択キャラのアニメーター
        Animator animator = null;
        // アニメーターのID
        readonly int selectID = Animator.StringToHash("SelectPlayerType");

        private void Awake()
        {
            // コンポーネントの取得
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            // アニメーターの初期化
            SetAnimator();
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
        } // DownCheck

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
