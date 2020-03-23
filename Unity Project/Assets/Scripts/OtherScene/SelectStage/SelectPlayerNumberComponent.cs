using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectStage
{
    public class SelectPlayerNumberComponent : MonoBehaviour
    {
        // 人数表示
        [SerializeField]
        private RectTransform[] numberUI = new RectTransform[4];
        // 選択中のものを示す外枠
        [SerializeField]
        private RectTransform frame = null;

        // プレイ人数
        int playerNumber = 1;
        // スティックを前のフレームから倒してたかどうかのフラグ
        bool stickFlag = false;


        /// <summary>
        /// 人数選択画面
        /// </summary>
        /// <returns></returns>
        public IEnumerator SelectPlayerNumber()
        {
            // 人数表示を表示
            OpenUI();

            while (true)
            {
                // スティック入力をチェック
                StickIn();
                // 枠を移動させる
                MoveFrame();
                // キー入力をチェック
                KeyIn();

                yield return null;
            } // while
        } // SelectPlayerNumber

        /// <summary>
        /// 人数表示を表示するメソッド
        /// </summary>
        private void OpenUI()
        {
            // 人数表示を行う
            for (int i = 0; i < numberUI.Length; i++)
            {
                numberUI[i].gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// キー入力をチェックするメソッド
        /// </summary>
        private void KeyIn()
        {
            // Aボタンが押されたら
            if (Input.GetButtonDown("Submit"))
            {
                // GameManagerに人数を教える
                GameManager.Instance.playerNumber = playerNumber;
                // 人数表示を非表示にする
                HideUI();
                // 人数選択の終了
                StopCoroutine(SelectPlayerNumber());
                // キャラクター選択画面に遷る
                StartCoroutine(SceneController.Instance.SelectCharacterComponent.SelectCharacter());
            }
            // Bボタンが押されたら
            else if (Input.GetButtonDown("Cancel"))
            {
                // タイトル画面に戻る
                SceneController.Instance.LoadScene("Title");
            }
        }

        /// <summary>
        /// スティック入力をチェックするメソッド
        /// </summary>
        void StickIn()
        {
            // スティックの左右入力を検知
            var vertical = Input.GetAxis("Vertical");

            Debug.Log(vertical);
            // ある程度強く倒されてたら
            if (Mathf.Abs(vertical) > 0.7f)
            {
                // 前のフレームでは押されてなければ
                if (stickFlag == false)
                {
                    stickFlag = true;
                    // 下に押し倒されたら
                    if (vertical < 0)
                    {
                        // プレイ人数を増やす
                        playerNumber++;
                    }
                    // 上に押し倒されたら
                    else if (vertical > 0)
                    {
                        // プレイ人数を減らす
                        playerNumber--;
                    }
                    // 人数を超えないように
                    playerNumber = Mathf.Clamp(playerNumber, 1, 4);
                    Debug.Log(playerNumber);
                }
            }
            else
            {
                stickFlag = false;
            }
        }

        /// <summary>
        /// 枠の位置を選んでいる人数を示す表示に合わせる
        /// </summary>
        void MoveFrame()
        {
            frame.position = numberUI[playerNumber - 1].position;
        }

        /// <summary>
        /// 人数表示を非表示にする
        /// </summary>
        void HideUI()
        {
            for (int i = 0; i < numberUI.Length; i++)
            {
                numberUI[i].gameObject.SetActive(false);
            }
        }
    }
}
