using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlideState : IState
{

    public void Entry(int ID)
    {
        var hit = SceneManager.Instance.playerEntityData.playerSliderChecks[ID].hit;
        SceneManager.Instance.playerEntityData.players[ID].GetComponent<Rigidbody2D>().position
            = hit.point;
        SceneManager.Instance.playerEntityData.players[ID].GetComponent<Rigidbody2D>().gravityScale = 0;

    }

   

    public void Do(int ID)
    {
        var hit = SceneManager.Instance.playerEntityData.playerSliderChecks[ID].hit;
        SceneManager.Instance.playerEntityData.players[ID].GetComponent<Rigidbody2D>().velocity
            = hit.collider.gameObject.transform.right * 5f;
    }

    public void Do_Fix(int ID)
    {
        
    }
    
    public void Exit(int ID)
    {
        SceneManager.Instance.playerEntityData.players[ID].GetComponent<Rigidbody2D>().gravityScale = 1;
    }
}
