using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState
{

    PlayerFacade playerFacade ;
   
    public void Entry()
    {
        playerFacade._PlayerRun.Run();
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
