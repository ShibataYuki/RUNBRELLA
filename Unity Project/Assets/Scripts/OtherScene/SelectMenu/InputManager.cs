using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class InputManager : MonoBehaviour
    {
        // 長押しをチェックするフラグ
        private Dictionary<int, bool> keyFlags = new Dictionary<int, bool>();
        public Dictionary<int, bool> KeyFlags { get { return keyFlags; } set { keyFlags = value; } }

        /// <summary>
        /// Aボタンを押したかどうか
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool SubmitKeyIn(int ID)
        {
            return Input.GetButtonDown(string.Format("Player{0}Submit", ID));
        }

        /// <summary>
        /// 左右にスティックが倒されたかチェックするメソッド
        /// </summary>
        /// <param name="ID">ジョイスティックのID</param>
        /// <param name="param">倒された方向に影響される整数</param>
        /// <returns></returns>
        public int HorizontalKeyIn(int ID, int param = 0)
        {
            var horizontal = Input.GetAxis(string.Format("Player{0}Horizontal", ID));
            if (Mathf.Abs(horizontal) > 0.7f)
            {
                if (keyFlags[ID] == false)
                {
                    // 右に倒されたなら
                    if (horizontal < 0.0f)
                    {
                        param++;
                    }
                    // 左に倒されたなら
                    else if (horizontal > 0.0f)
                    {
                        param--;
                    }
                    // キーフラグをONにする
                    keyFlags[ID] = true;
                } // if(keyFlag)
            } // if(Mathf.Abs...)
            else
            {
                keyFlags[ID] = false;
            } // else
            // 値を返す
            return param;
        } // HorizontalKeyIn
    } // class
} // namespace
