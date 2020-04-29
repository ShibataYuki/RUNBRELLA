using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirstAnim : MonoBehaviour
{
    Animator animator;
    private Player player;
    private int chartype;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        chartype = (int)player.charType;
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
        var playerID = player.controllerNo;
        var topPlayerID = GameManager.Instance.playerRanks[0];
        bool isTop = playerID == topPlayerID;
        if (isTop) return true;
        return false;
    }

}
