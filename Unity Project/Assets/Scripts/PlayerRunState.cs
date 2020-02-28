using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState
{

    PlayerRun playerRun;
   
    public void Entry()
    {
        playerRun = SceneManager.Instance.keyValuePairs[1].GetComponent<PlayerRun>();
        playerRun.Run();
    }

    public void Do()
    {
    }

    public void Do_Fix()
    {
    }

    public void Exit()
    {
    }
}
