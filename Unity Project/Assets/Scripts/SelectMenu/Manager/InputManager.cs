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
        private Dictionary<GamePad.Index, bool> keyFlagsHorizontal_rightStick = new Dictionary<GamePad.Index, bool>();
        private Dictionary<GamePad.Index, bool> keyFlagsHorizontal_leftStick = new Dictionary<GamePad.Index, bool>();
        private Dictionary<GamePad.Index, bool> keyFlagsHorizontal_dpad = new Dictionary<GamePad.Index, bool>();
        #endregion
        #region Vertical
        private Dictionary<GamePad.Index, bool> keyFlagsVertical_rightStick = new Dictionary<GamePad.Index, bool>();
        private Dictionary<GamePad.Index, bool> keyFlagsVertical_leftStick = new Dictionary<GamePad.Index, bool>();
        private Dictionary<GamePad.Index, bool> keyFlagsVertical_dpad = new Dictionary<GamePad.Index, bool>();
        #endregion
        #region Trigger
        private Dictionary<GamePad.Index, bool> keyFlagsRightTrigger = new Dictionary<GamePad.Index, bool>();
        private Dictionary<GamePad.Index, bool> keyFlagsLeftTrigger = new Dictionary<GamePad.Index, bool>();
        #endregion

        // キーフラグのget set
        #region Horizontal
        public Dictionary<GamePad.Index, bool> KeyFlagHorizontal_rightStick { get { return keyFlagsHorizontal_rightStick; } set { keyFlagsHorizontal_rightStick = value; } }
        public Dictionary<GamePad.Index, bool> KeyFlagHorizontal_leftStick { get { return keyFlagsHorizontal_leftStick; } set { keyFlagsHorizontal_leftStick = value; } }
        public Dictionary<GamePad.Index, bool> KeyFlagHorizontal_dpad { get { return keyFlagsHorizontal_dpad; } set { keyFlagsHorizontal_dpad = value; } }
        #endregion
        #region Vertical
        public Dictionary<GamePad.Index, bool> KeyFlagsVertical_rightStick { get { return keyFlagsVertical_rightStick; } set { keyFlagsVertical_rightStick = value; } }
        public Dictionary<GamePad.Index, bool> KeyFlagsVertical_leftStick { get { return keyFlagsVertical_leftStick; } set { keyFlagsVertical_leftStick = value; } }
        public Dictionary<GamePad.Index, bool> KeyFlagsVertical_dpad { get { return keyFlagsVertical_dpad; } set { keyFlagsVertical_dpad = value; } }
        #endregion
        #region Trigger
        public Dictionary<GamePad.Index, bool> KeyFlagsRightTrigger { get { return keyFlagsRightTrigger; } set { keyFlagsRightTrigger = value; } }
        public Dictionary<GamePad.Index, bool> KeyFlagsLeftTrigger { get { return keyFlagsLeftTrigger; } set { keyFlagsLeftTrigger = value; } }
        #endregion

        #region キーボード入力用のキーコードの配列
        // 変数
        [SerializeField]
        private KeyCode[] leftKeyCodes = new KeyCode[5];
        [SerializeField]
        private KeyCode[] rightKeyCodes = new KeyCode[5];
        [SerializeField]
        private KeyCode[] entryKeyCodes = new KeyCode[5];
        [SerializeField]
        private KeyCode[] cancelKeyCodes = new KeyCode[5];
        [SerializeField]
        private KeyCode[] menuKeyCodes = new KeyCode[5];
        // get
        public KeyCode[] LeftKeyCodes { get { return leftKeyCodes; } }
        public KeyCode[] RightKeyCodes { get { return rightKeyCodes; } }
        public KeyCode[] EntryKeyCodes { get { return entryKeyCodes; } }
        public KeyCode[] CancelKeyCodes { get { return cancelKeyCodes; } }
        public KeyCode[] MenuKeyCodes { get { return menuKeyCodes; } }
        #endregion

        // Bボタンを長押ししている時間
        public Dictionary<CONTROLLER_NO, float> keyHoldTimeDictionary { get; private set; } = new Dictionary<CONTROLLER_NO, float>();
        // Bボタンを押しているかどうか
        public Dictionary<CONTROLLER_NO, bool> IsChanselDictionary { get; set; } = new Dictionary<CONTROLLER_NO, bool>();

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
            for (var gamePadIndex = GamePad.Index.Any; gamePadIndex <= GamePad.Index.Four; gamePadIndex++)
            {
                #region Horizontal
                keyFlagsHorizontal_rightStick.Add(gamePadIndex, false);
                keyFlagsHorizontal_leftStick.Add(gamePadIndex, false);
                keyFlagsHorizontal_dpad.Add(gamePadIndex, false);
                #endregion
                #region Vertical
                keyFlagsVertical_rightStick.Add(gamePadIndex, false);
                keyFlagsVertical_leftStick.Add(gamePadIndex, false);
                keyFlagsVertical_dpad.Add(gamePadIndex, false);
                #endregion
                #region Trigger
                keyFlagsLeftTrigger.Add(gamePadIndex, false);
                keyFlagsRightTrigger.Add(gamePadIndex, false);
                #endregion
            }
            // ディクショナリーを初期化
            for(var controllerNo = CONTROLLER_NO.CONTROLLER1; controllerNo <= CONTROLLER_NO.CONTROLLER4; controllerNo++)
            {
                // ディクショナリーに追加
                keyHoldTimeDictionary.Add(controllerNo, 0.0f);
                IsChanselDictionary.Add(controllerNo, false);
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
            for (var gamePadNo = GamePad.Index.Any; gamePadNo <= GamePad.Index.Four; gamePadNo++)
            {
                #region Horizontal
                if (HorizontalKeyInRightStick(gamePadNo) != 0)
                {
                    // キーフラグをONにする
                    keyFlagsHorizontal_rightStick[gamePadNo] = true;
                }

                if (HorizontalKeyInLeftStick(gamePadNo) != 0)
                {
                    // キーフラグをONにする
                    keyFlagsHorizontal_leftStick[gamePadNo] = true;
                }
                if (HorizontalKeyInDpad(gamePadNo) != 0)
                {
                    // キーフラグをONにする
                    keyFlagsHorizontal_dpad[gamePadNo] = true;
                }
                #endregion
                #region Vertical
                if (VerticalKeyInRightStick(gamePadNo) != 0)
                {
                    // キーフラグをONにする
                    keyFlagsVertical_rightStick[gamePadNo] = true;
                }

                if (VerticalKeyInLeftStick(gamePadNo) != 0)
                {
                    // キーフラグをONにする
                    keyFlagsVertical_leftStick[gamePadNo] = true;
                }

                if (VerticalKeyInDpad(gamePadNo) != 0)
                {
                    // キーフラグをONにする
                    keyFlagsVertical_dpad[gamePadNo] = true;
                }
                #endregion
                #region Trigger
                if (RightTriggerKeyIn(gamePadNo) == true)
                {
                    // キーフラグをONにする
                    keyFlagsRightTrigger[gamePadNo] = true;
                }

                if (LeftTriggerKeyIn(gamePadNo) == true)
                {
                    // キーフラグをONにする
                    keyFlagsLeftTrigger[gamePadNo] = true;
                }
                #endregion
            } // キーフラグをチェック
            // Bボタンの長押しをチェック
            for (var controllerNo = CONTROLLER_NO.CONTROLLER1; controllerNo <= CONTROLLER_NO.CONTROLLER4; controllerNo++)
            {
                // Bボタンを離したなら
                if(GamePad.GetButtonUp(GamePad.Button.B, (GamePad.Index) controllerNo))
                {
                    // フラグをOFFに変更
                    IsChanselDictionary[controllerNo] = false;
                }
                // Bボタンを長押ししている時間を計測
                keyHoldTimeDictionary[controllerNo] = IsChanselDictionary[controllerNo] ?
                    keyHoldTimeDictionary[controllerNo] + Time.deltaTime : 0.0f;
            } // 
        } // KeyFlagSet

        #region Buttonの入力
        /// <summary>
        /// Aボタンを押したかどうか
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>
        public bool SubmitKeyDown(GamePad.Index controllerNo)
        {
            return GamePad.GetButtonDown(GamePad.Button.A, controllerNo);
        }

        /// <summary>
        /// Bボタンを押したか
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>
        public bool CancelKeyDown(GamePad.Index controllerNo)
        {
            return GamePad.GetButtonDown(GamePad.Button.B, controllerNo);
        }

        /// <summary>
        /// Xボタンを押したか
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>
        public bool XKeyDown(GamePad.Index controllerNo)
        {
            return GamePad.GetButtonDown(GamePad.Button.X, controllerNo);
        }

        /// <summary>
        /// Yボタンを押したか
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>
        public bool YKeyDown(GamePad.Index controllerNo)
        {
            return GamePad.GetButtonDown(GamePad.Button.Y, controllerNo);
        }

        /// <summary>
        /// 右のボタンを押したかチェック
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>
        public bool RightShoulderKeyDown(GamePad.Index controllerNo)
        {
            return GamePad.GetButtonDown(GamePad.Button.RightShoulder, controllerNo);
        }

        /// <summary>
        /// 左のボタンを押したかチェック
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>
        public bool LeftShoulderKeyDown(GamePad.Index controllerNo)
        {
            return GamePad.GetButtonDown(GamePad.Button.LeftShoulder, controllerNo);
        }

        /// <summary>
        /// 右のスティックを押したかチェック
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>
        public bool RightStickKeyDown(GamePad.Index controllerNo)
        {
            return GamePad.GetButtonDown(GamePad.Button.RightStick, controllerNo);
        }

        /// <summary>
        /// 左のスティックを押したかチェック
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>
        public bool LeftStickKeyDown(GamePad.Index controllerNo)
        {
            return GamePad.GetButtonDown(GamePad.Button.LeftStick, controllerNo);
        }

        /// <summary>
        /// バックが押されたかチェック
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>
        public bool BackKeyDown(GamePad.Index controllerNo)
        {
            return GamePad.GetButtonDown(GamePad.Button.Back, controllerNo);
        }

        /// <summary>
        /// スタートが押されたかチェック
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>

        public bool StartKeyDown(GamePad.Index controllerNo)
        {
            return GamePad.GetButtonDown(GamePad.Button.Start, controllerNo);
        }
        #endregion

        #region Triggerの入力
        /// <summary>
        /// 右のトリガーが押されたかチェック
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>
        public bool RightTriggerKeyIn(GamePad.Index controllerNo)
        {
            var triggerValue = GamePad.GetTrigger(GamePad.Trigger.RightTrigger, controllerNo);
            if(Mathf.Abs(triggerValue) > 0.7f)
            {
                if(keyFlagsRightTrigger[controllerNo] == false)
                {
                    return true;
                }
            }
            else
            {
                keyFlagsRightTrigger[controllerNo] = false;
            }

            return false;
        }

        /// <summary>
        /// 左のトリガーが押されたかチェック
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>
        public bool LeftTriggerKeyIn(GamePad.Index controllerNo)
        {
            var triggerValue = GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, controllerNo);
            if (Mathf.Abs(triggerValue) > 0.7f)
            {
                if (keyFlagsLeftTrigger[controllerNo] == false)
                {
                    return true;
                }
            }
            else
            {
                keyFlagsLeftTrigger[controllerNo] = false;
            }

            return false;
        }
        #endregion

        #region 縦方向のキー入力
        /// <summary>
        /// 上下にキーが倒されたかチェック
        /// </summary>
        /// <param name="gamePadNo"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int VerticalKeyInRightStick(GamePad.Index gamePadNo, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.RightStick, (GamePad.Index)gamePadNo);
            var vertical = vec.y;
            if (Mathf.Abs(vertical) > 0.7f)
            {
                if (keyFlagsVertical_rightStick[gamePadNo] == false)
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
                keyFlagsVertical_rightStick[gamePadNo] = false;
            } // else
            // 値を返す
            return param;
        }

        /// <summary>
        /// 上下にキーが倒されたかチェック
        /// </summary>
        /// <param name="gamePadNo"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int VerticalKeyInLeftStick(GamePad.Index gamePadNo, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.LeftStick, gamePadNo);
            var vertical = vec.y;
            if (Mathf.Abs(vertical) > 0.7f)
            {
                if (keyFlagsVertical_leftStick[gamePadNo] == false)
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
                keyFlagsVertical_leftStick[gamePadNo] = false;
            } // else
            // 値を返す
            return param;
        }

        /// <summary>
        /// 上下にキーが倒されたかチェック
        /// </summary>
        /// <param name="gamePadNo"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int VerticalKeyInDpad(GamePad.Index gamePadNo, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.Dpad, gamePadNo);
            var vertical = vec.y;
            if (Mathf.Abs(vertical) > 0.7f)
            {
                if (keyFlagsVertical_dpad[gamePadNo] == false)
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
                keyFlagsVertical_dpad[gamePadNo] = false;
            } // else
            // 値を返す
            return param;
        }
        #endregion

        #region 横方向のキー入力
        /// <summary>
        /// 左右にスティックが倒されたかチェックするメソッド
        /// </summary>
        /// <param name="gamePadNo">ジョイスティックのNo</param>
        /// <param name="param">倒された方向に影響される整数</param>
        /// <returns></returns>
        public int HorizontalKeyInRightStick(GamePad.Index gamePadNo, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.RightStick, gamePadNo);
            var horizontal = vec.x;
            if (Mathf.Abs(horizontal) > 0.7f)
            {
                if (keyFlagsHorizontal_rightStick[gamePadNo] == false)
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
                keyFlagsHorizontal_rightStick[gamePadNo] = false;
            } // else
            // 値を返す
            return param;
        } // HorizontalKeyIn

        /// <summary>
        /// 左右にスティックが倒されたかチェックするメソッド
        /// </summary>
        /// <param name="gamePadNo">ジョイスティックのNo</param>
        /// <param name="param">倒された方向に影響される整数</param>
        /// <returns></returns>
        public int HorizontalKeyInLeftStick(GamePad.Index gamePadNo, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.LeftStick, gamePadNo);
            var horizontal = vec.x;
            if (Mathf.Abs(horizontal) > 0.7f)
            {
                if (keyFlagsHorizontal_leftStick[gamePadNo] == false)
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
                keyFlagsHorizontal_leftStick[gamePadNo] = false;
            } // else
            // 値を返す
            return param;
        } // HorizontalKeyIn

        /// <summary>
        /// 左右にスティックが倒されたかチェックするメソッド
        /// </summary>
        /// <param name="gamePadNo">ジョイスティックのNo</param>
        /// <param name="param">倒された方向に影響される整数</param>
        /// <returns></returns>
        public int HorizontalKeyInDpad(GamePad.Index gamePadNo, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.Dpad, gamePadNo);
            var horizontal = vec.x;
            if (Mathf.Abs(horizontal) > 0.7f)
            {
                if (keyFlagsHorizontal_dpad[gamePadNo] == false)
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
                keyFlagsHorizontal_dpad[gamePadNo] = false;
            } // else
            // 値を返す
            return param;
        } // HorizontalKeyIn
        #endregion

        /// <summary>
        /// いずれかのキーが押されたかチェック
        /// </summary>
        /// <param name="controllerNo"></param>
        /// <returns></returns>
        public bool AnyKeyIn(GamePad.Index controllerNo)
        {
            #region ボタンの入力
            if (SubmitKeyDown(controllerNo) == true)
            {
                return true;
            }

            if(CancelKeyDown(controllerNo) == true)
            {
                return true;
            }

            if(XKeyDown(controllerNo) == true)
            {
                return true;
            }

            if(YKeyDown(controllerNo) == true)
            {
                return true;
            }

            if(RightShoulderKeyDown(controllerNo) == true)
            {
                return true;
            }

            if(LeftShoulderKeyDown(controllerNo) == true)
            {
                return true;
            }

            if(RightStickKeyDown(controllerNo) == true)
            {
                return true;
            }

            if(LeftStickKeyDown(controllerNo) == true)
            {
                return true;
            }

            if (BackKeyDown(controllerNo) == true)
            {
                return true;
            }

            if(StartKeyDown(controllerNo) == true)
            {
                return true;
            }
            #endregion

            #region トリガーの入力
            if(LeftTriggerKeyIn(controllerNo) == true)
            {
                return true;
            }
            if(RightTriggerKeyIn(controllerNo) == true)
            {
                return true;
            }
            #endregion

            #region 縦方向のキー入力
            if (VerticalKeyInRightStick(controllerNo) != 0)
            {
                return true;
            }

            if (VerticalKeyInLeftStick(controllerNo) != 0)
            {
                return true;
            }

            if (VerticalKeyInDpad(controllerNo) != 0)
            {
                return true;
            }
            #endregion

            #region 横方向のキー入力
            if (HorizontalKeyInLeftStick(controllerNo) != 0)
            {
                return true;
            }

            if (HorizontalKeyInRightStick(controllerNo) != 0)
            {
                return true;
            }

            if (HorizontalKeyInDpad(controllerNo) != 0)
            {
                return true;
            }
            #endregion

            return false;
        }
    } // class
} // namespace
