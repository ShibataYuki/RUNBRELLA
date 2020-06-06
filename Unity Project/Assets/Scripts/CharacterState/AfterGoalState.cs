using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterGoalState : CharacterState
{

    protected PlayerMove playerMove;

    // Start is called before the first frame update
    protected virtual void Start() 
    {
        playerMove = GetComponent<PlayerMove>();
    }

    public override void Entry()
    {
        // 角度を初期化
        gameObject.transform.localRotation = Quaternion.identity;
        
    }

    public override void Do()
    {
    }

    public override void Do_Fix()
    {
        // 減速処理
        playerMove.MinusAcceleration();
        playerMove.SpeedChange();
    }

    public override void Exit()
    {
    }

}
