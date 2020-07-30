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
   
}
