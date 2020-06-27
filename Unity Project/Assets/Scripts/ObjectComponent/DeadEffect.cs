using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEffect : MonoBehaviour
{

    // 死亡時に
    [SerializeField]
    List<ParticleSystem> effects = new List<ParticleSystem>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// 死亡時のエフェクトを再生する関数
    /// </summary>
    /// <param name="pos"></param>
    public void StartDeadEffect(Vector3 pos)
    {
        // 死亡時の座標まで移動
        transform.position = pos;
        // エフェクトを再生
        for (int i = 0; i < effects.Count;i++)
        {
            // エフェクトを再生
            effects[i].Play();
        }
    }
}
