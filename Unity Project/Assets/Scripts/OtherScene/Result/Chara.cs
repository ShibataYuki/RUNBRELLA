using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ResultScene
{
    public class Chara : MonoBehaviour
    {
        Animator animator;
        GameManager.CHARTYPE charaType;
        Material material;
        PLAYER_NO firstPlayer;
        SpriteRenderer spriteRenderer;
        // Start is called before the first frame update
        void Start()
        {
            firstPlayer = GameManager.Instance.playerResultInfos[0].playerNo;
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            charaType = GameManager.Instance.firstCharType;
            // 優勝したプレイヤーのタイプによってアニメーションを差し替え
            animator.SetInteger("charaType", (int)charaType);
            // 優勝したプレイヤーのキャラクターナンバーによってアウトライン用マテリアルを差し替え
            material = GameManager.Instance.playerOutlines[(int)firstPlayer];
            spriteRenderer.material = material;                
        }
        
    }
}

