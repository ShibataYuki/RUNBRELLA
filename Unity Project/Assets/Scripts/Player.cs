using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // プレイヤーID
    public int ID { get; set; } = 0;
    // 地面にいるか
    bool isGround = false;
    // プレイヤーステート
    public IState state = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {           
        // stateのDo関数を呼ぶ
        state.Do(ID);
    }


    private void FixedUpdate()
    {
        // stateのDo_Fix関数を呼ぶ
        state.Do_Fix(ID);
    }
}
