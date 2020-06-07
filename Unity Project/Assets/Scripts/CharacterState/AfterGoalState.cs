using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterGoalState : CharacterState
{

    protected PlayerMove playerMove;
    protected PlayerAerial playerAerial;
    protected Rigidbody2D rigidbody2;

    // Start is called before the first frame update
    protected virtual void Start() 
    {
        playerMove = GetComponent<PlayerMove>();
        playerAerial = GetComponent<PlayerAerial>();
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    public override void Entry()
    {
        // 角度を初期化
        gameObject.transform.localRotation = Quaternion.identity;
        // 重力を変更
        rigidbody2.gravityScale = playerAerial.aerialGravityScale;
        
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
