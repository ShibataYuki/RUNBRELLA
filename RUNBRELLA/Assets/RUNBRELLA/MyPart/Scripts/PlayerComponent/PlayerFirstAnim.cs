using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirstAnim : MonoBehaviour
{
    //
    Animator animator;
    private Character character;
    private int chartype;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネント取得
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
        chartype = (int)character.charType;
        // 1のアニメーター
        SetFirstAnimator();
    }
    
    // 1位のキャラクターのみアニメーターのフラグを変更
    void SetFirstAnimator()
    {
        animator.SetBool("isTop", IsTop());
        animator.SetInteger("charaType", chartype);
    }

    // １位のキャラクターか調べる
    private bool IsTop()
    {
        var playerID = character.playerNO;
        var topPlayerID = GameManager.Instance.playerResultInfos[0].playerNo;
        bool isTop = playerID == topPlayerID;
        if (isTop) return true;
        return false;
    }

}
