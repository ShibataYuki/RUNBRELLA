using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirstAnim : MonoBehaviour
{
    Animator animator;
    private Character character;
    private int chartype;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
        chartype = (int)character.charType;
        SetFirstAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetFirstAnimator()
    {
        animator.SetBool("isTop", IsTop());
        animator.SetInteger("charaType", chartype);
    }

    private bool IsTop()
    {
        var playerID = character.playerNO;
        var topPlayerID = GameManager.Instance.playerResultInfos[0].playerNo;
        bool isTop = playerID == topPlayerID;
        if (isTop) return true;
        return false;
    }

}
