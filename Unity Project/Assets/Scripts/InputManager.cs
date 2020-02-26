using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    /// <summary>
    /// ジャンプのキー入力を受け取る関数
    /// </summary>
    /// <param name="playerNum"></param>
    /// <returns></returns>
    public bool JumpKeyIn(int playerNum)
    {
        if (Input.GetButtonDown("player" + playerNum + "_jump"))
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
    public bool ActionKeyIn(int playerNum)
    {
        if (Input.GetButtonDown("player" + playerNum + "_action"))
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
    public bool ShotKeyIn(int playerNum)
    {
        float tri = Input.GetAxis("player" + playerNum + "_shot");

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
    public bool StartGlidingKeyIn(int playerNum)
    {
        if(Input.GetButtonDown("player" + playerNum + "_jump"))
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
    public bool EndGlidingKeyIn(int playerNum)
    {
        if (Input.GetButtonUp("player" + playerNum + "_jump"))
        {
            return true;
        }
        return false;
    }



    private void Update()
    {
        if(JumpKeyIn(1))
        {
            Debug.Log("プレイヤー１のジャンプ");
        }
        if(ActionKeyIn(1))
        {
            Debug.Log("プレイヤー１のアクション");
        }
        if(ShotKeyIn(1))
        {
            Debug.Log("プレイヤー１のショット");
        }
    }

}
