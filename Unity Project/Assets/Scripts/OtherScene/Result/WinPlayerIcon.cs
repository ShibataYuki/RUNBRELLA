using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPlayerIcon : MonoBehaviour
{
    Image image;
    [SerializeField]
    List<Sprite> iconSpriteList = new List<Sprite>();
    PLAYER_NO firstPlayer;
    // Start is called before the first frame update
    void Start()
    {
        firstPlayer = GameManager.Instance.playerRanks[0];
        image = GetComponent<Image>();
        // 1位のプレイヤーのプレイヤーナンバーに合わせたスプライトに変更
        image.sprite = iconSpriteList[(int)firstPlayer];
    }
 
}
