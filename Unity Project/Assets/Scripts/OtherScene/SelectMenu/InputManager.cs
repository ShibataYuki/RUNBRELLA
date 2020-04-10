using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

namespace SelectMenu
{
    public class InputManager : MonoBehaviour
    {
        // 長押しをチェックするフラグ
        #region Horizontal
        private Dictionary<int, bool> keyFlagsHorizontal_rightStick = new Dictionary<int, bool>();
        private Dictionary<int, bool> keyFlagsHorizontal_leftStick = new Dictionary<int, bool>();
        private Dictionary<int, bool> keyFlagsHorizontal_dpad = new Dictionary<int, bool>();
        #endregion
        #region Vertical
        private Dictionary<int, bool> keyFlagsVertical_rightStick = new Dictionary<int, bool>();
        private Dictionary<int, bool> keyFlagsVertical_leftStick = new Dictionary<int, bool>();
        private Dictionary<int, bool> keyFlagsVertical_dpad = new Dictionary<int, bool>();
        #endregion
        #region Trigger
        private Dictionary<int, bool> keyFlagsRightTrigger = new Dictionary<int, bool>();
        private Dictionary<int, bool> keyFlagsLeftTrigger = new Dictionary<int, bool>();
        #endregion

        // キーフラグのget set
        #region Horizontal
        public Dictionary<int, bool> KeyFlagHorizontal_rightStick { get { return keyFlagsHorizontal_rightStick; } set { keyFlagsHorizontal_rightStick = value; } }
        public Dictionary<int, bool> KeyFlagHorizontal_leftStick { get { return keyFlagsHorizontal_leftStick; } set { keyFlagsHorizontal_leftStick = value; } }
        public Dictionary<int, bool> KeyFlagHorizontal_dpad { get { return keyFlagsHorizontal_dpad; } set { keyFlagsHorizontal_dpad = value; } }
        #endregion
        #region Vertical
        public Dictionary<int, bool> KeyFlagsVertical_rightStick { get { return keyFlagsVertical_rightStick; } set { keyFlagsVertical_rightStick = value; } }
        public Dictionary<int, bool> KeyFlagsVertical_leftStick { get { return keyFlagsVertical_leftStick; } set { keyFlagsVertical_leftStick = value; } }
        public Dictionary<int, bool> KeyFlagsVertical_dpad { get { return keyFlagsVertical_dpad; } set { keyFlagsVertical_dpad = value; } }
        #endregion
        #region Trigger
        public Dictionary<int, bool> KeyFlagsRightTrigger { get { return keyFlagsRightTrigger; } set { keyFlagsRightTrigger = value; } }
        public Dictionary<int, bool> KeyFlagsLeftTrigger { get { return keyFlagsLeftTrigger; } set { keyFlagsLeftTrigger = value; } }
        #endregion

        private void Start()
        {
            // キーフラグのディクショナリーを初期化
            InitKeyFlagDictionary();
        }

        /// <summary>
        /// キーフラグのディクショナリーを初期化
        /// </summary>
        private void InitKeyFlagDictionary()
        {
            for (int ID = 1; ID <= SceneController.Instance.MaxPlayerNumber; ID++)
            {
                #region Horizontal
                keyFlagsHorizontal_rightStick.Add(ID, false);
                keyFlagsHorizontal_leftStick.Add(ID, false);
                keyFlagsHorizontal_dpad.Add(ID, false);
                #endregion
                #region Vertical
                keyFlagsVertical_rightStick.Add(ID, false);
                keyFlagsVertical_leftStick.Add(ID, false);
                keyFlagsVertical_dpad.Add(ID, false);
                #endregion
                #region Trigger
                keyFlagsLeftTrigger.Add(ID, false);
                keyFlagsRightTrigger.Add(ID, false);
                #endregion
            }
        }

        private void LateUpdate()
        {
            // キーフラグをセット
            KeyFlagSet();
        }

        /// <summary>
        /// キーフラグをセットする
        /// </summary>
        private void KeyFlagSet()
        {
            // キーフラグをチェック
            for (int ID = 1; ID <= SceneController.Instance.MaxPlayerNumber; ID++)
            {
                #region Horizontal
                if (HorizontalKeyInRightStick(ID) != 0)
                {
                    // キーフラグをONにする
                    keyFlagsHorizontal_rightStick[ID] = true;
                }

                if (HorizontalKeyInLeftStick(ID) != 0)
                {
                    // キーフラグをONにする
                    keyFlagsHorizontal_leftStick[ID] = true;
                }
                if (HorizontalKeyInDpad(ID) != 0)
                {
                    // キーフラグをONにする
                    keyFlagsHorizontal_dpad[ID] = true;
                }
                #endregion
                #region Vertical
                if (VerticalKeyInRightStick(ID) != 0)
                {
                    // キーフラグをONにする
                    keyFlagsVertical_rightStick[ID] = true;
                }

                if (VerticalKeyInLeftStick(ID) != 0)
                {
                    // キーフラグをONにする
                    keyFlagsVertical_leftStick[ID] = true;
                }

                if (VerticalKeyInDpad(ID) != 0)
                {
                    // キーフラグをONにする
                    keyFlagsVertical_dpad[ID] = true;
                }
                #endregion
                #region Trigger
                if (RightTriggerKeyIn(ID) == true)
                {
                    // キーフラグをONにする
                    keyFlagsRightTrigger[ID] = true;
                }

                if (LeftTriggerKeyIn(ID) == true)
                {
                    // キーフラグをONにする
                    keyFlagsLeftTrigger[ID] = true;
                }
                #endregion
            }
        }

        #region Buttonの入力
        /// <summary>
        /// Aボタンを押したかどうか
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool SubmitKeyDown(GamePad.Index ID)
        {
            return GamePad.GetButtonDown(GamePad.Button.A, ID);
        }

        /// <summary>
        /// Bボタンを押したか
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CancelKeyDown(GamePad.Index ID)
        {
            return GamePad.GetButtonDown(GamePad.Button.B, ID);
        }

        /// <summary>
        /// Xボタンを押したか
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool XKeyDown(GamePad.Index ID)
        {
            return GamePad.GetButtonDown(GamePad.Button.X, ID);
        }

        /// <summary>
        /// Yボタンを押したか
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool YKeyDown(GamePad.Index ID)
        {
            return GamePad.GetButtonDown(GamePad.Button.Y, ID);
        }

        /// <summary>
        /// 右のボタンを押したかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool RightShoulderKeyDown(GamePad.Index ID)
        {
            return GamePad.GetButtonDown(GamePad.Button.RightShoulder, ID);
        }

        /// <summary>
        /// 左のボタンを押したかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool LeftShoulderKeyDown(GamePad.Index ID)
        {
            return GamePad.GetButtonDown(GamePad.Button.LeftShoulder, ID);
        }

        /// <summary>
        /// 右のスティックを押したかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool RightStickKeyIn(GamePad.Index ID)
        {
            return GamePad.GetButtonDown(GamePad.Button.RightStick, ID);
        }

        /// <summary>
        /// 左のスティックを押したかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool LeftStickKeyIn(GamePad.Index ID)
        {
            return GamePad.GetButtonDown(GamePad.Button.LeftStick, ID);
        }

        /// <summary>
        /// バックが押されたかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool BackKeyIn(GamePad.Index ID)
        {
            return GamePad.GetButtonDown(GamePad.Button.Back, ID);
        }

        /// <summary>
        /// スタートが押されたかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        public bool StartKeyIn(GamePad.Index ID)
        {
            return GamePad.GetButtonDown(GamePad.Button.Start, ID);
        }
        #endregion

        #region Triggerの入力
        /// <summary>
        /// 右のトリガーが押されたかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool RightTriggerKeyIn(int ID)
        {
            var triggerValue = GamePad.GetTrigger(GamePad.Trigger.RightTrigger, (GamePad.Index)ID);
            if(Mathf.Abs(triggerValue) > 0.7f)
            {
                if(keyFlagsRightTrigger[ID] == false)
                {
                    return true;
                }
            }
            else
            {
                keyFlagsRightTrigger[ID] = false;
            }

            return false;
        }

        /// <summary>
        /// 左のトリガーが押されたかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool LeftTriggerKeyIn(int ID)
        {
            var triggerValue = GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, (GamePad.Index)ID);
            if (Mathf.Abs(triggerValue) > 0.7f)
            {
                if (keyFlagsLeftTrigger[ID] == false)
                {
                    return true;
                }
            }
            else
            {
                keyFlagsLeftTrigger[ID] = false;
            }

            return false;
        }
        #endregion

        #region 縦方向のキー入力
        /// <summary>
        /// 上下にキーが倒されたかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int VerticalKeyInRightStick(int ID, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.RightStick, (GamePad.Index)ID);
            var vertical = vec.y;
            if (Mathf.Abs(vertical) > 0.7f)
            {
                if (keyFlagsVertical_rightStick[ID] == false)
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
                    keyFlagsVertical_rightStick[ID] = true;
                } // if(keyFlag)
            } // if(Mathf.Abs...)
            else
            {
                keyFlagsVertical_rightStick[ID] = false;
            } // else
            // 値を返す
            return param;
        }

        /// <summary>
        /// 上下にキーが倒されたかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int VerticalKeyInLeftStick(int ID, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.LeftStick, (GamePad.Index)ID);
            var vertical = vec.y;
            if (Mathf.Abs(vertical) > 0.7f)
            {
                if (keyFlagsVertical_leftStick[ID] == false)
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
                } // if(keyFlag)
            } // if(Mathf.Abs...)
            else
            {
                keyFlagsVertical_rightStick[ID] = false;
            } // else
            // 値を返す
            return param;
        }

        /// <summary>
        /// 上下にキーが倒されたかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int VerticalKeyInDpad(int ID, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.Dpad, (GamePad.Index)ID);
            var vertical = vec.y;
            if (Mathf.Abs(vertical) > 0.7f)
            {
                if (keyFlagsVertical_dpad[ID] == false)
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
                } // if(keyFlag)
            } // if(Mathf.Abs...)
            else
            {
                keyFlagsVertical_rightStick[ID] = false;
            } // else
            // 値を返す
            return param;
        }
        #endregion

        #region 横方向のキー入力
        /// <summary>
        /// 左右にスティックが倒されたかチェックするメソッド
        /// </summary>
        /// <param name="ID">ジョイスティックのID</param>
        /// <param name="param">倒された方向に影響される整数</param>
        /// <returns></returns>
        public int HorizontalKeyInRightStick(int ID, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.RightStick, (GamePad.Index)ID);
            var horizontal = vec.x;
            if (Mathf.Abs(horizontal) > 0.7f)
            {
                if (keyFlagsHorizontal_rightStick[ID] == false)
                {
                    // 右に倒されたなら
                    if (horizontal > 0.0f)
                    {
                        param++;
                    }
                    // 左に倒されたなら
                    else if (horizontal < 0.0f)
                    {
                        param--;
                    }
                } // if(keyFlag)
            } // if(Mathf.Abs...)
            else
            {
                keyFlagsHorizontal_rightStick[ID] = false;
            } // else
            // 値を返す
            return param;
        } // HorizontalKeyIn

        /// <summary>
        /// 左右にスティックが倒されたかチェックするメソッド
        /// </summary>
        /// <param name="ID">ジョイスティックのID</param>
        /// <param name="param">倒された方向に影響される整数</param>
        /// <returns></returns>
        public int HorizontalKeyInLeftStick(int ID, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.LeftStick, (GamePad.Index)ID);
            var horizontal = vec.x;
            if (Mathf.Abs(horizontal) > 0.7f)
            {
                if (keyFlagsHorizontal_leftStick[ID] == false)
                {
                    // 右に倒されたなら
                    if (horizontal > 0.0f)
                    {
                        param++;
                    }
                    // 左に倒されたなら
                    else if (horizontal < 0.0f)
                    {
                        param--;
                    }
                } // if(keyFlag)
            } // if(Mathf.Abs...)
            else
            {
                keyFlagsHorizontal_leftStick[ID] = false;
            } // else
            // 値を返す
            return param;
        } // HorizontalKeyIn

        /// <summary>
        /// 左右にスティックが倒されたかチェックするメソッド
        /// </summary>
        /// <param name="ID">ジョイスティックのID</param>
        /// <param name="param">倒された方向に影響される整数</param>
        /// <returns></returns>
        public int HorizontalKeyInDpad(int ID, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.Dpad, (GamePad.Index)ID);
            var horizontal = vec.x;
            if (Mathf.Abs(horizontal) > 0.7f)
            {
                if (keyFlagsHorizontal_dpad[ID] == false)
                {
                    // 右に倒されたなら
                    if (horizontal > 0.0f)
                    {
                        param++;
                    }
                    // 左に倒されたなら
                    else if (horizontal < 0.0f)
                    {
                        param--;
                    }
                } // if(keyFlag)
            } // if(Mathf.Abs...)
            else
            {
                keyFlagsHorizontal_dpad[ID] = false;
            } // else
            // 値を返す
            return param;
        } // HorizontalKeyIn
        #endregion

        /// <summary>
        /// いずれかのキーが押されたかチェック
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool AnyKeyIn(GamePad.Index ID)
        {
            #region ボタンの入力
            if (SubmitKeyDown(ID) == true)
            {
                return true;
            }

            if(CancelKeyDown(ID) == true)
            {
                return true;
            }

            if(XKeyDown(ID) == true)
            {
                return true;
            }

            if(YKeyDown(ID) == true)
            {
                return true;
            }

            if(RightShoulderKeyDown(ID) == true)
            {
                return true;
            }

            if(LeftShoulderKeyDown(ID) == true)
            {
                return true;
            }

            if(RightStickKeyIn(ID) == true)
            {
                return true;
            }

            if(LeftStickKeyIn(ID) == true)
            {
                return true;
            }

            if (BackKeyIn(ID) == true)
            {
                return true;
            }

            if(StartKeyIn(ID) == true)
            {
                return true;
            }
            #endregion

            #region トリガーの入力
            if(LeftTriggerKeyIn((int)ID) == true)
            {
                return true;
            }
            if(RightTriggerKeyIn((int)ID) == true)
            {
                return true;
            }
            #endregion

            #region 縦方向のキー入力
            if (VerticalKeyInRightStick((int)ID) != 0)
            {
                return true;
            }

            if (VerticalKeyInLeftStick((int)ID) != 0)
            {
                return true;
            }

            if (VerticalKeyInDpad((int)ID) != 0)
            {
                return true;
            }
            #endregion

            #region 横方向のキー入力
            if (HorizontalKeyInLeftStick((int)ID) != 0)
            {
                return true;
            }

            if (HorizontalKeyInRightStick((int)ID) != 0)
            {
                return true;
            }

            if (HorizontalKeyInDpad((int)ID) != 0)
            {
                return true;
            }
            #endregion

            return false;
        }
    } // class
} // namespace
