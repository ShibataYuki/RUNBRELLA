using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactors : MonoBehaviour
{
    [SerializeField]
    public List<Animator> charactorAnimatorList;    
    private void Awake()
    {       
        CharaInit();
    }

    private void CharaInit()
    {
        var playerResultInfos = GameManager.Instance.playerResultInfos;
        for (int i = 0; i <= playerResultInfos.Count - 1; i++ )
        {
            // プレイヤーのキャラクターナンバーによってアウトライン用マテリアルを差し替え
            var spriteRenderer = charactorAnimatorList[i].GetComponent<SpriteRenderer>();
            var material = GameManager.Instance.playerOutlines[(int)playerResultInfos[i].playerNo];
            spriteRenderer.material = material;
            // プレイヤーのタイプによってアニメーションを差し替え
            charactorAnimatorList[i].SetInteger("charaType", (int)playerResultInfos[i].charType);
        }
    }
}
