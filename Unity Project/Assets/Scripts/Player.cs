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
    
    [SerializeField]
    private float baseSpeed = 6;
    public float BaseSpeed { get { return baseSpeed; } set { baseSpeed = value; } }

    // プレイヤーの速度保存領域
    public float VelocityXStorage { get;  set; } = 0;
    // リジッドボディ
    public Rigidbody2D rigidBody;

#if UNITY_EDITOR
    // ステートの名前をデバッグ表示する変数
    [SerializeField]
    private string stateName;
#endif
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();        
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
        if(state != PlayerStateManager.Instance.playerRunState && rigidBody.velocity.x > 0)
        {
            VelocityXStorage = rigidBody.velocity.x;
        }
        
    }

    /// <summary>
    /// 最高速度保存処理
    /// </summary>
    public void SaveVelocity()
    {
        if(rigidBody.velocity.x > BaseSpeed / 2)
        {
            VelocityXStorage = rigidBody.velocity.x;
        }
    }

    /// <summary>
    /// 最高速度リセット
    /// </summary>
    public void ResetVelocityXStorage()
    {
        VelocityXStorage = 0;
    }
}
