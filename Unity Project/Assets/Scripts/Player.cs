using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // プレイヤーID
    int id = 0;
    // プレイヤーステート
    IState state = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        // IDを指定して現在のステートを問い合わせ
        PlayerStateManager.Instance.CheckState(id);

        // stateのDo関数を呼ぶ
        state.Do();
    }


    private void FixedUpdate()
    {
        // stateのDo_Fix関数を呼ぶ
        state.Do_Fix();
    }
}
