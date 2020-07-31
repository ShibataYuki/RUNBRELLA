using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

namespace Title
{
    public class InputManager : MonoBehaviour
    {
        // 長押しをチェックするフラグ
        #region Vertical
        private Dictionary<GamePad.Index, bool> keyFlagsVertical_rightStick = new Dictionary<GamePad.Index, bool>();
        private Dictionary<GamePad.Index, bool> keyFlagsVertical_leftStick = new Dictionary<GamePad.Index, bool>();
        private Dictionary<GamePad.Index, bool> keyFlagsVertical_dpad = new Dictionary<GamePad.Index, bool>();
        #endregion

        // キーフラグのget set
        #region Vertical
        public Dictionary<GamePad.Index, bool> KeyFlagsVertical_rightStick { get { return keyFlagsVertical_rightStick; } set { keyFlagsVertical_rightStick = value; } }
        public Dictionary<GamePad.Index, bool> KeyFlagsVertical_leftStick { get { return keyFlagsVertical_leftStick; } set { keyFlagsVertical_leftStick = value; } }
        public Dictionary<GamePad.Index, bool> KeyFlagsVertical_dpad { get { return keyFlagsVertical_dpad; } set { keyFlagsVertical_dpad = value; } }
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
            for (var gamePadIndex = GamePad.Index.Any; gamePadIndex <= GamePad.Index.Four; gamePadIndex++)
            {
                #region Vertical
                keyFlagsVertical_rightStick.Add(gamePadIndex, false);
                keyFlagsVertical_leftStick.Add(gamePadIndex, false);
                keyFlagsVertical_dpad.Add(gamePadIndex, false);
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
            for (var gamePadNo = GamePad.Index.Any; gamePadNo <= GamePad.Index.Four; gamePadNo++)
            {
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
            }
        }

        #region 縦方向のキー入力
        /// <summary>
        /// 上下にキーが倒されたかチェック
        /// </summary>
        /// <param name="gamePadNo"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int VerticalKeyInRightStick(GamePad.Index gamePadNo, int param = 0)
        {
            var vec = GamePad.GetAxis(GamePad.Axis.RightStick, gamePadNo);
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

        /// <summary>
        /// 上キーが押されたかチェック
        /// </summary>
        /// <param name="gamePadNo"></param>
        /// <returns></returns>
        public bool UpKeyCheck(GamePad.Index gamePadNo)
        {
            // キー入力の結果をセットする作業用変数
            int num = 0;
            // 上キーが押されたら+1、下キーが押されたら-1
            num = VerticalKeyInRightStick(gamePadNo, num);
            num = VerticalKeyInLeftStick(gamePadNo, num);
            num = VerticalKeyInDpad(gamePadNo, num);
            // 0を下回っているかどうかを返す
            return (num < 0);
        }

        /// <summary>
        /// 下キーが押されたかチェック
        /// </summary>
        /// <param name="gamePadNo"></param>
        /// <returns></returns>
        public bool DownKeyCheck(GamePad.Index gamePadNo)
        {
            // キー入力の結果をセットする作業用変数
            int num = 0;
            // 上キーが押されたら+1、下キーが押されたら-1
            num = VerticalKeyInRightStick(gamePadNo, num);
            num = VerticalKeyInLeftStick(gamePadNo, num);
            num = VerticalKeyInDpad(gamePadNo, num);
            // 0を上回っているかどうかを返す
            return (num > 0);
        }

        #endregion
    } // class
} // namespace
