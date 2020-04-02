using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        for (int i = 0; i < GameManager.Instance.playerNumber; i++)
        {
            shotFlag.Add(false);
            stickFlag.Add(false);
        }
    }

    /// <summary>
    /// ジャンプのキー入力を受け取る関数
    /// </summary>
    /// <param name="playerNum"></param>
    /// <returns></returns>
    public bool JumpKeyIn(int ID)
    {
        if (Input.GetButtonDown("player" + ID + "_jump")||Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// アクションボタンのキー入力を受け取る関数
    /// </summary>
    /// <param name="playerNum"></param>
    /// <returns></returns>
    public bool ActionKeyIn(int ID)
    {
        if (Input.GetButtonDown("player" + ID + "_action") || Input.GetKeyDown(KeyCode.F))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// ショットのキー入力を受け取る関数
    /// </summary>
    /// <param name="playerNum"></param>
    /// <returns></returns>
    public bool AttackKeyIn(int ID)
    {
        float tri = Input.GetAxis("player" + ID + "_attack");
        if(Input.GetKeyDown(KeyCode.Return))
        {
            tri = 1;
        }

        if(tri>0)
        {
            // トリガーを押しっぱなしで連射できないようにフラグがfalseの時のみにreturn true
            if(shotFlag[ID-1]==false)
            {
                shotFlag[ID-1] = true;
                 if(stickFlag[ID - 1] == false)
                {
                    return true;
                }
            }
        }
        else
        {
            shotFlag[ID-1] = false;
        }
        return false;
    }

    /// <summary>
    /// ブーストのキーを押したフレームかチェック
    /// </summary>
    /// <param name="ID">プレイヤーのID</param>
    /// <returns></returns>
    public bool BoostKeyIn(int ID)
    {
        return Input.GetButtonDown("player" + ID + "_boost");
    }

    /// <summary>
    /// ブーストのキーが押されているかチェック
    /// </summary>
    /// <param name="ID">プレイヤーのID</param>
    /// <returns></returns>
    public bool BoostKeyHold(int ID)
    {
        return Input.GetButton("player" + ID + "_boost");
    }

    /// <summary>
    /// ブーストのキーを離したかチェック
    /// </summary>
    /// <param name="ID">プレイヤーのID</param>
    /// <returns></returns>
    public bool BoostKeyOut(int ID)
    {
        return Input.GetButtonUp("player" + ID + "_boost");
    }

    /// <summary>
    /// ブーストのキー入力を受け取る関数
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    //public bool BoostKeyIn(int ID)
    //{
    //    if (Input.GetAxis("player" + ID + "_boost") > 0.5f)
    //    {
    //        stickFlag[ID - 1] = true;
    //        Debug.Log("bost :"+ Input.GetAxis("player" + ID + "_boost").ToString());
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
    public bool StartGlidingKeyIn(int ID)
    {
        if(Input.GetButtonDown("player" + ID + "_jump") || Input.GetKeyDown(KeyCode.Space))
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
    public bool EndGlidingKeyIn(int ID)
    {
        if (Input.GetButtonUp("player" + ID + "_jump") || Input.GetKeyUp(KeyCode.Space))
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
        for(int i=0;i<4; i++)
        {
            if(Input.GetButtonDown("player"+ID+"_Rise"+"_button"+i))
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// デバック用の雨を呼ぶ関数
    /// </summary>
    /// <returns></returns>
    public bool CallRainKeyIn()
    {
        if(Input.GetButtonDown("CallRain"))
        {
            return true;
        }
        return false;
    }

}
