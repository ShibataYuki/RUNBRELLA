using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResultScene
{
    public class PushAnyUI : MonoBehaviour
    {

        Image image = null;
        // α値を変動させるコルーチン
        IEnumerator alphaPingPong = null;

        /// <summary>
        /// アクティブになった際に行われる処理
        /// </summary>
        private void OnEnable()
        {
            image = GetComponent<Image>();
            alphaPingPong = ChangeAlpha_PingPong();
            StartCoroutine(alphaPingPong);
        }

        /// <summary>
        /// 非アクティブになった際に行われる処理
        /// </summary>
        private void OnDisable()
        {
            StopCoroutine(alphaPingPong);
        }

        /// <summary>
        /// α値を0～1の範囲で往復させ、テキストを明滅させる
        /// </summary>
        /// <returns></returns>
        IEnumerator ChangeAlpha_PingPong()
        {
            float workAlpha = 0;
            while (true)
            {
                Color newColor = image.color;
                workAlpha = Mathf.PingPong(Time.time, 1);
                newColor.a = workAlpha;
                image.color = newColor;
                yield return null;
            }
        }

    }
}

