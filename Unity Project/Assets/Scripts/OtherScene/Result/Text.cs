using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResultScene
{
    public class Text : MonoBehaviour
    {
        // 勝利リザルトのテキストイメージ       
        Image textImage;
        // 各キャラのテキストリストを持った構造体
        [System.Serializable]
        struct CharaType
        {
            public List<Sprite> textList;
        }

        [SerializeField]
        List<CharaType> charaTypeList = new List<CharaType>();
       
        // 優勝したプレイヤーのキャラタイプ
        GameManager.CHARTYPE firstCharaType;


        // Start is called before the first frame update
        void Start()
        {
            // キャラタイプ取得
            firstCharaType = GameManager.Instance.firstCharType;
            textImage = GetComponent<Image>();
            SetText();
        }
   
        public void SetText()
        {
            var TextStruct = SelectTextList();
            var textSprite = SelectText(TextStruct);
            textImage.sprite = textSprite;
        }

        /// <summary>
        /// 1位のプレイヤーのキャラタイプによって使う構造体を選択する
        /// </summary>
        /// <returns></returns>
        private CharaType SelectTextList()
        {
            var selectedTextStruct = charaTypeList[(int)firstCharaType];
            return selectedTextStruct;
        }
        /// <summary>
        /// 引数で渡された構造体のスプライトリストの中からどのスプライトを使用するか選択する
        /// </summary>
        /// <param name="selectedTextStruct"></param>
        /// <returns></returns>
        private Sprite SelectText(CharaType selectedTextStruct)
        {
            var TexList = selectedTextStruct.textList;
            int randomNo = Random.Range(0, TexList.Count);
            var selectedTextSprite = TexList[randomNo];
            return selectedTextSprite;
        }
    }
}

