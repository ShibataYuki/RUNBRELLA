using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GamepadInput;

public class InputManager : MonoBehaviour
{

    #region シングルトン
    // シングルトン
    private static InputManager instance;
    public static InputManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // 複数個作成しないようにする
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    #endregion

    // 各プレイヤーの連射防止フラグ
    List<bool> shotFlag = new List<bool>();
    List<bool> stickFlag = new List<bool>();

    // デバック用キー入力
    [SerializeField]
    private List<KeyCode> jumpKeyCodes = new List<KeyCode>();
    [SerializeField]
    private List<KeyCode> actionKeyCodes = new List<KeyCode>();
    [SerializeField]
    private List<KeyCode> attackKeyCodes = new List<KeyCode>();
    [SerializeField]
    private List<KeyCode> boostKeyCodes = new List<KeyCode>();

    // 前フレームのトリガーの入力値
    private Dictionary<CONTROLLER_NO, bool> after1FrameTri = new Dictionary<CONTROLLER_NO, bool>();

    private void Start()
    {
        for (int i = 0; i < GameManager.Instance.playerNumber; i++)
        {
            shotFlag.Add(false);
            stickFlag.Add(false);
        }
        foreach(var controllerNo in GameManager.Instance.playerAndControllerDictionary.Values)
        {
            after1FrameTri[controllerNo] = false;
        }
    }

    /// <summary>
    ///  今のフレームでトリガーを押したかチェック
    /// </summary>
    private void LateUpdate()
    {
        foreach (var controllerNo in GameManager.Instance.playerAndControllerDictionary.Values)
        {
            after1FrameTri[controllerNo] = ShotAndBoostKeyIn(controllerNo);
        }

    }

    /// <summary>
    /// ジャンプのキー入力を受け取る関数
    /// </summary>
    /// <param name="controllerNo"></param>
    /// <returns></returns>
    public bool JumpKeyIn(CONTROLLER_NO controllerNo)
    {
        var key = GameManager.Instance.ContorllerNoToPlayerNo(controllerNo);
        if (GamePad.GetButtonDown(GamePad.Button.A, (GamePad.Index)controllerNo) ||
            Input.GetKeyDown(jumpKeyCodes[(int)key]))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// アクションボタンのキー入力を受け取る関数
    /// </summary>
    /// <param name="controllerNo"></param>
    /// <returns></returns>
    public bool ActionKeyIn(CONTROLLER_NO controllerNo)
    {
        // keyからvalueを取得
        var key = GameManager.Instance.ContorllerNoToPlayerNo(controllerNo);
        if (GamePad.GetButtonDown(GamePad.Button.B,(GamePad.Index)controllerNo) || 
            Input.GetKeyDown(actionKeyCodes[(int)key]))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// トリガーを押したことを検知するメソッド
    /// </summary>
    /// <param name="controllerNo"></param>
    /// <returns></returns>
    public bool ShotAndBoostKeyIn(CONTROLLER_NO controllerNo)
    {
        // keyからvalueを取得
        var key = GameManager.Instance.ContorllerNoToPlayerNo(controllerNo);
        var keyState = GamePad.GetState((GamePad.Index)controllerNo, false);
        var tri = keyState.RightTrigger;
        if (Input.GetKey(attackKeyCodes[(int)key]))
        {
            tri = 1;
        }
        // トリガーを押していたらTrueを返す
        if (tri>=0.8f)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// トリガーを離したことを検知するメソッド
    /// </summary>
    /// <param name="controllerNo"></param>
    /// <returns></returns>
    public bool ShotAndBoostKeyOut(CONTROLLER_NO controllerNo)
    {
        // keyからvalueを取得
        var key = GameManager.Instance.ContorllerNoToPlayerNo(controllerNo);
        var keyState = GamePad.GetState((GamePad.Index)controllerNo, false);
        float tri = keyState.RightTrigger;
        if (Input.GetKeyUp(attackKeyCodes[(int)key]))
        {
            tri = 0;
        }
        // トリガーを離しているかつ前フレームにトリガーを押していたら
        if (tri < 0.8f&&after1FrameTri[controllerNo])
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// ショットのキー入力を受け取る関数
    /// </summary>
    /// <param name="controllerNo"></param>
    /// <returns></returns>
    //public bool AttackKeyIn(CONTROLLER_NO controllerNo)
    //{
    //    // keyからvalueを取得
    //    var key = GameManager.Instance.ContorllerNoToPlayerNo(controllerNo);
    //    var keyState = GamePad.GetState((GamePad.Index)controllerNo, false);
    //    float tri = keyState.RightTrigger;
    //    if (Input.GetKeyDown(attackKeyCodes[(int)key]))
    //    {
    //        tri = 1;
    //    }

    //    if (tri > 0)
    //    {
    //        // トリガーを押しっぱなしで連射できないようにフラグがfalseの時のみにreturn true
    //        if (shotFlag[(int)key] == false)
    //        {
    //            shotFlag[(int)key] = true;
    //            if (stickFlag[(int)key] == false)
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        shotFlag[(int)key] = false;
    //    }
    //    return false;
    //}

    ///// <summary>
    ///// ブーストのキーを押したフレームかチェック
    ///// </summary>
    ///// <param name="controllerNo">プレイヤーのID</param>
    ///// <returns></returns>
    //public bool BoostKeyIn(CONTROLLER_NO controllerNo)
    //{
    //    // keyからvalueを取得
    //    var key = GameManager.Instance.ContorllerNoToPlayerNo(controllerNo);
    //    if (GamePad.GetButtonDown(GamePad.Button.RightShoulder, (GamePad.Index)controllerNo) ||
    //       Input.GetKeyDown(boostKeyCodes[(int)key]))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    ///// <summary>
    ///// ブーストのキーが押されているかチェック
    ///// </summary>
    ///// <param name="controllerNo">プレイヤーのID</param>
    ///// <returns></returns>
    //public bool BoostKeyHold(CONTROLLER_NO controllerNo)
    //{
    //    // keyからvalueを取得
    //    var key = GameManager.Instance.ContorllerNoToPlayerNo(controllerNo);
    //    if (GamePad.GetButton(GamePad.Button.RightShoulder, (GamePad.Index)controllerNo)||
    //        Input.GetKey(boostKeyCodes[(int)key]))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    ///// <summary>
    ///// ブーストのキーを離したかチェック
    ///// </summary>
    ///// <param name="controllerNo">プレイヤーのID</param>
    ///// <returns></returns>
    //public bool BoostKeyOut(CONTROLLER_NO controllerNo)
    //{
    //    // keyからvalueを取得
    //    var key = GameManager.Instance.ContorllerNoToPlayerNo(controllerNo);
    //    if (GamePad.GetButtonUp(GamePad.Button.RightShoulder, (GamePad.Index)controllerNo)||
    //        Input.GetKeyUp(boostKeyCodes[(int)key]))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    /// <summary>
    /// ブーストのキー入力を受け取る関数
    /// </summary>
    /// <param name="controllerNo"></param>
    /// <returns></returns>
    //public bool BoostKeyIn(int ID)
    //{
    //    if (Input.GetAxis("player" + ID + "_boost") > 0.5f)
    //    {
    //        stickFlag[ID - 1] = true;
    //        Debug.Log("bost :" + Input.GetAxis("player" + ID + "_boost").ToString());
    //        if (shotFlag[ID - 1] == true)
    //        {
    //            shotFlag[ID - 1] = true;
    //            return true;
    //        }
    //    }
    //    else
    //    {
    //        stickFlag[ID - 1] = false;
    //    }

    //    return false;
    //}

    /// <summary>
    /// 滑空開始のキー入力を受け取る関数
    /// </summary>
    /// <param name="playerNum"></param>
    /// <returns></returns>
    public bool StartGlidingKeyIn(CONTROLLER_NO controllerNo)
    {
        // keyからvalueを取得
        var key = GameManager.Instance.ContorllerNoToPlayerNo(controllerNo);
        if (GamePad.GetButtonDown(GamePad.Button.A,(GamePad.Index)controllerNo) || 
            Input.GetKeyDown(jumpKeyCodes[(int)key]))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 滑空終了のキー入力を受け取る関数
    /// </summary>
    /// <param name="playerNum"></param>
    /// <returns></returns>
    public bool EndGlidingKeyIn(CONTROLLER_NO controllerNo)
    {
        // keyからvalueを取得
        var key = GameManager.Instance.ContorllerNoToPlayerNo(controllerNo);
        if (GamePad.GetButtonUp(GamePad.Button.A,(GamePad.Index)controllerNo) || 
            Input.GetKeyUp(jumpKeyCodes[(int)key]))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// プレイヤーがダウン中のキー入力を受け取る処理
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public bool RiseKeyIn(int ID)
    {
        //for (int i = 0; i < 4; i++)
        //{
        //    if (Input.GetButtonDown("player" + ID + "_Rise" + "_button" + i))
        //    {
        //        return true;
        //    }
        //}
        return false;
    }


    /// <summary>
    /// デバック用の雨を呼ぶ関数
    /// </summary>
    /// <returns></returns>
    public bool CallRainKeyIn()
    {
        if (GamePad.GetButtonDown(GamePad.Button.X,GamePad.Index.Any))
        {
            return true;
        }
        return false;
    }

}
