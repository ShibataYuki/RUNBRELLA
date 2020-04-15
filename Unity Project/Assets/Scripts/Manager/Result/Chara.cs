using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResultScene
{
    public class Chara : MonoBehaviour
    {
        Animator animator;
        GameManager.CHARTYPE charaType;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            charaType = GameManager.Instance.firstCharType;
            // 優勝したプレイヤーのタイプによってアニメーションを差し替え
            animator.SetInteger("charaType", (int)charaType);
        }

        // Update is called once per frame
        void Update()
        {
           
        }
    }
}

