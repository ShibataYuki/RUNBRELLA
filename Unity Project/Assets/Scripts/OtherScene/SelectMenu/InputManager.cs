using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class InputManager : MonoBehaviour
    {
        // 長押しをチェックするフラグ
        private Dictionary<int, bool> keyFlagsHorizontal = new Dictionary<int, bool>();
        public Dictionary<int, bool> KeyFlagHorizontal { get { return keyFlagsHorizontal; } set { keyFlagsHorizontal = value; } }
        private Dictionary<int, bool> keyFlagsVertical = new Dictionary<int, bool>();
        public Dictionary<int, bool> KeyFlagsVertical { get { return keyFlagsVertical; } set { keyFlagsVertical = value; } }

        // 
        private Dictionary<int, int> isKeyHorizontal = new Dictionary<int, int>();
        public Dictionary<int, int> IsKeyHorizontal { get { return isKeyHorizontal; } }
        private Dictionary<int, int> isKeyVertical = new Dictionary<int, int>();
        public Dictionary<int, int> IsKeyVertical { get { return isKeyVertical; } }

        private void Start()
        {
            for(int ID = 1; ID <= SceneController.Instance.MaxPlayerNumber; ID++)
            {
                keyFlagsHorizontal.Add(ID, false);
                keyFlagsVertical.Add(ID, false);

                isKeyHorizontal.Add(ID, 0);
                isKeyVertical.Add(ID, 0);
            }
        }

        private void Update()
        {
            for(int ID = 1; ID <= SceneController.Instance.MaxPlayerNumber; ID++)
            {

            }
        }

        private void LateUpdate()
        {
            
        }

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
        /// Bボタンを押したか
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CancelKeyIn(int ID)
        {
            return Input.GetButtonDown(string.Format("Player{0}Cansel", ID));
        }

        /// <summary>
        /// 上下にキーが倒されたかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int VerticalKeyIn(int ID, int param = 0)
        {
            var vertical = Input.GetAxis(string.Format("Player{0}Vertical", ID));
            if (Mathf.Abs(vertical) > 0.7f)
            {
                if (keyFlagsVertical[ID] == false)
                {
                    // 右に倒されたなら
                    if (vertical < 0.0f)
                    {
                        param++;
                    }
                    // 左に倒されたなら
                    else if (vertical > 0.0f)
                    {
                        param--;
                    }
                    // キーフラグをONにする
                    keyFlagsVertical[ID] = true;
                } // if(keyFlag)
            } // if(Mathf.Abs...)
            else
            {
                keyFlagsVertical[ID] = false;
            } // else
            // 値を返す
            return param;
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
                if (keyFlagsHorizontal[ID] == false)
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
                    keyFlagsHorizontal[ID] = true;
                } // if(keyFlag)
            } // if(Mathf.Abs...)
            else
            {
                keyFlagsHorizontal[ID] = false;
            } // else
            // 値を返す
            return param;
        } // HorizontalKeyIn

        public bool AnyKeyIn(int ID)
        {
            if(SubmitKeyIn(ID) == true)
            {
                return true;
            }

            if(CancelKeyIn(ID))
            {
                return true;
            }

            if(HorizontalKeyIn(ID) != 0)
            {
                return true;
            }

            if(VerticalKeyIn(ID) != 0)
            {
                return true;
            }

            return false;
        }
    } // class
} // namespace
