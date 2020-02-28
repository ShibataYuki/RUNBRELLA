using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState
{

    PlayerRun playerRun;
   
    public void Entry(int ID)
    {
        playerRun = SceneManager.Instance.Players[ID].GetComponent<PlayerRun>();
        playerRun.Run();
    }

    public void Do(int ID)
    {
    }

    public void Do_Fix(int ID)
    {
    }

    public void Exit(int ID)
    {
    }
}
