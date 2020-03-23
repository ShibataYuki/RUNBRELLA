using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectStage
{
    public class SelectCharacterComponent : MonoBehaviour
    {
        // プレイ人数
        int playerNumber;

        // 前のフレームにキー入力を行っているかどうかのフラグ
        Dictionary <int, bool> keyFlag = new Dictionary<int, bool>();
        // 決定したかどうかのフラグ
        Dictionary<int, bool> isSubmits = new Dictionary<int, bool>();
        // 選んだキャラクター
        Dictionary<int, int> isSelect = new Dictionary<int, int>();

        [SerializeField]
        private GameObject[] charImages = null;

        /// <summary>
        /// フレーム更新処理を行うにあたっての初期化
        /// </summary>
        private void Init()
        {
            // プレイ人数の設定
            playerNumber = GameManager.Instance.playerNumber;

            // ディクショナリーの初期化
            for (int i = 1; i <= playerNumber; i++)
            {
                keyFlag.Add(i, false);
                isSubmits.Add(i, false);
                isSelect.Add(i, 0);
            }

        }

        /// <summary>
        /// キャラクター選択画面
        /// </summary>
        /// <returns></returns>
        public IEnumerator SelectCharacter()
        {
            // フレーム更新処理を行うにあたっての初期化
            Init();

            // 時間経過を待つ
            yield return new WaitForSeconds(1.0f);

            while (true)
            {
                // キー入力をチェックする
                KeyIn();

                // 全員の入力が終わっていれば
                if (SubmitCheck() == true)
                {
                    // ステージに遷る
                    SceneController.Instance.LoadScene("Stage");
                    yield break;
                }

                yield return null;
            } // while
        } // IEnumerator

        /// <summary>
        /// キー入力をチェックするメソッド
        /// </summary>
        private void KeyIn()
        {
            // 全員のキー入力を見る
            for (int i = 1; i <= playerNumber; i++)
            {
                // Aボタンを押したなら
                if (Input.GetButtonDown("Player" + i.ToString() + "Submit"))
                {
                    // キャラクター選択を完了
                    isSubmits[i] = true;
                }
                // Bボタンを押したなら
                else if (Input.GetButtonDown("Player" + i.ToString() + "Cancel"))
                {
                    // キャラクターを選んでいたなら
                    if (isSubmits[i] == true)
                    {
                        // キャラクターを選びなおす
                        isSubmits[i] = false;
                    }
                    else
                    {
                        // プレイ人数選択画面に戻る
                        BackSelectPlayerNumber();
                        return;
                    }
                }
                // 左右移動
                var horizontal = Input.GetAxis("Player" + i.ToString() + "Horizontal");
                if (Mathf.Abs(horizontal) > 0.7f)
                {
                    if (keyFlag[i] == false)
                    {
                        keyFlag[i] = true;

                        if (horizontal > 0.0f)
                        {
                            isSelect[i]++;
                        }

                        else if (horizontal < 0.0f)
                        {
                            isSelect[i]--;
                        }

                        // 範囲内に収まるように
                        isSelect[i] = Mathf.Clamp(isSelect[i], 0, charImages.Length - 1);

                        Debug.Log(isSelect[i]);
                    }
                }
                else
                {
                    keyFlag[i] = false;
                }
            }
        }

        /// <summary>
        /// プレイ人数選択画面に戻る
        /// </summary>
        private void BackSelectPlayerNumber()
        {
            // ディクショナリーのリセット
            for (int i = 1; i <= playerNumber; i++)
            {
                keyFlag.Remove(i);
                isSubmits.Remove(i);
                isSelect.Remove(i);
            }

            // 人数選択画面に戻る
            StartCoroutine(SceneController.Instance.SelectPlayerNumberComponent.SelectPlayerNumber());
            // キャラ選択画面を停止させる
            StopCoroutine(SelectCharacter());
        }

        /// <summary>
        /// 全員の入力が終わったかチェックするメソッド
        /// </summary>
        /// <returns></returns>
        private bool SubmitCheck()
        {
            // 全員のキャラ選択が終わったかチェック
            for (int i = 1; i <= playerNumber; i++)
            {
                // 一人でも入力中の人がいれば
                if (isSubmits[i] == false)
                {
                    // 入力を待つ
                    return false;
                }
            }

            // 入力の完了
            return true;
        }
    }
}
