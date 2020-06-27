using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResultScene
{
    public class CharaImage : MonoBehaviour
    {
        [SerializeField]
        Image charaImage = null;
        [SerializeField]
        List<Sprite> charaSpriteList = new List<Sprite>();
       
        GameManager.CHARTYPE charaType;
       
        // Start is called before the first frame update
        void Start()
        {
            
            charaType = GameManager.Instance.firstCharType;
            SetCharaSpriteAtTopPlayer();
        }

        /// <summary>
        /// 一位のキャラタイプのスプライトに差し替える処理
        /// </summary>
        private void SetCharaSpriteAtTopPlayer()
        {
            charaImage.sprite = charaSpriteList[(int)charaType];
        }
    }
}

