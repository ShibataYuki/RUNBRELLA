using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // プレイヤーID
    public int ID { get; set; } = 0;
    // 地面にいるか    
    [SerializeField]
    bool isGround = false;
    public bool IsGround { set { isGround = value; } get { return isGround; } }
    // たまに当たったか
    [SerializeField]
    bool isHitBullet = false;
    public bool IsHitBullet { set { isHitBullet = value; } get { return isHitBullet; } }
    // プレイヤーステート   
    public IState state = null;
    // プレイヤーがダウンしている時間
    public float downTime = 0;

#if UNITY_EDITOR
    // ステートの名前をデバッグ表示する変数
    [SerializeField]
    private string stateName;
#endif
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {           
        // stateのDo関数を呼ぶ
        state.Do(ID);
#if UNITY_EDITOR
        // 現在のステートをInspecter上に表示
        stateName = state.ToString();
#endif
    }


    private void FixedUpdate()
    {
        // stateのDo_Fix関数を呼ぶ
        state.Do_Fix(ID);
    }
}
