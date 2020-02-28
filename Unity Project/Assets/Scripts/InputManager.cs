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

    /// <summary>
    /// ジャンプのキー入力を受け取る関数
    /// </summary>
    /// <param name="playerNum"></param>
    /// <returns></returns>
    public bool JumpKeyIn(int ID)
    {
        if (Input.GetButtonDown("player" + ID + "_jump"))
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
        if (Input.GetButtonDown("player" + ID + "_action"))
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
    public bool ShotKeyIn(int ID)
    {
        float tri = Input.GetAxis("player" + ID + "_shot");

        if(tri>0)
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
    public bool StartGlidingKeyIn(int ID)
    {
        if(Input.GetButtonDown("player" + ID + "_jump"))
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
        if (Input.GetButtonUp("player" + ID + "_jump"))
        {
            return true;
        }
        return false;
    }


}
