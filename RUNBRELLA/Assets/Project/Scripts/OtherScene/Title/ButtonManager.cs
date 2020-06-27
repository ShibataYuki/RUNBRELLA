using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
    public class ButtonManager : MonoBehaviour
    {
        // ボタンのディクショナリー
        private Dictionary<SceneController.SelectIndex, Animator> selectButtonsAnimator = new Dictionary<SceneController.SelectIndex, Animator>();

        private readonly int selectID = Animator.StringToHash("IsSelected");
        // Start is called before the first frame update
        void Start()
        {
            // ボタンのディクショナリーにセット
            SetButtonDictionary();
        }

        /// <summary>
        /// ボタンのディクショナリーにセット
        /// </summary>
        private void SetButtonDictionary()
        {
            // ボタンの親オブジェクトの参照を取得
            var buttons = GameObject.Find("Canvas/Buttons");
            for (var i = (SceneController.SelectIndex)0; i < SceneController.SelectIndex.MAX; i++)
            {
                // 子オブジェクトのボタンの参照を取得
                var button = buttons.transform.Find(i.ToString());
                var buttonAnimator = button.GetComponent<Animator>();
                // ディクショナリーに追加
                selectButtonsAnimator.Add(i, buttonAnimator);
            }
        }

        /// <summary>
        /// インデックスに応じてアニメーションで画像を変更する
        /// </summary>
        public void SetAnimation(SceneController.SelectIndex selectIndex)
        {
            for (var i = (SceneController.SelectIndex) 0; i < SceneController.SelectIndex.MAX; i++)
            {
                // 選ばれているかどうかをアニメーターにセット
                selectButtonsAnimator[i].SetBool(selectID, (i == selectIndex));
            }
        }

    }
}
