using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResultScene
{
    public class Text : MonoBehaviour
    {
        [SerializeField]
        Image textImage;

        [System.Serializable]
        struct TextStruct
        {
            public List<Sprite> textList;
        }

        [SerializeField]
        List<TextStruct> textStructList = new List<TextStruct>();
       
        GameManager.CHARTYPE charaType;


        // Start is called before the first frame update
        void Start()
        {
            charaType = GameManager.Instance.firstCharType;
        }
   
        public void SetText()
        {
            var TextStruct = SelectTextList();
            var textSprite = SelectText(TextStruct);
            textImage.sprite = textSprite;
        }

        private TextStruct SelectTextList()
        {
            var selectedTextStruct = textStructList[(int)charaType];
            return selectedTextStruct;
        }

        private Sprite SelectText(TextStruct selectedTextStruct)
        {
            var TexList = selectedTextStruct.textList;
            int randomNo = Random.Range(0, 3);
            var selectedText = TexList[randomNo];
            return selectedText;
        }
    }
}

