using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour
{

    [SerializeField]
    Animator pushButtonAnimator = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// アニメーションを開始する関数
    /// </summary>
    public void StartPushButtonAnimetion()
    {
        if(pushButtonAnimator)
        {
            pushButtonAnimator.SetBool("isDown", true);
        }
    }


    /// <summary>
    /// アニメーションを終了する関数
    /// </summary>
    public void EndPushButtonAnimetion()
    {
        if(pushButtonAnimator)
        {
            pushButtonAnimator.SetBool("isDown", false);
        }
    }

}
