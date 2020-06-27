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
        [SerializeField]
        AudioSource voiceSource = null;
        
        // 各キャラのテキストリストを持った構造体
        [System.Serializable]
        struct CharaType
        {
            public List<Sprite> textList;
            public List<AudioClip> voiceList;

            public CharaType(List<Sprite> textList, List<AudioClip> voiceList)
            {
                this.textList = textList;
                this.voiceList = voiceList;
            }

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
            var selectedStruct = SelectStruct();
            var randomNo = ChooseRundomIntUntilListCount(selectedStruct);
            var selectedText = SelectText(selectedStruct,randomNo);
            textImage.sprite = selectedText;
            var selectedVoice = SelectVoice(selectedStruct, randomNo);
            voiceSource.clip = selectedVoice;
        }

        /// <summary>
        /// 1位のプレイヤーのキャラタイプによって使う構造体を選択する
        /// </summary>
        /// <returns></returns>
        private CharaType SelectStruct()
        {
            var selectedStruct = charaTypeList[(int)firstCharaType];
            return selectedStruct;
        }

        private int ChooseRundomIntUntilListCount(CharaType selectedTextStruct)
        {
            var TextList = selectedTextStruct.textList;
            int randomNo = Random.Range(0, TextList.Count);
            return randomNo;
        }

        /// <summary>
        /// 引数で渡された構造体のスプライトリストの中からどのスプライトを使用するか選択する
        /// </summary>
        /// <param name="selectedTextStruct"></param>
        /// <returns></returns>
        private Sprite SelectText(CharaType selectedTextStruct,int randomNo)
        {
            var TextList = selectedTextStruct.textList;
            var selectedTextSprite = TextList[randomNo];
            return selectedTextSprite;
        }

        /// <summary>
        /// 引数で渡された構造体のスプライトリストの中からどのスプライトを使用するか選択する
        /// </summary>
        /// <param name="selectedTextStruct"></param>
        /// <returns></returns>
        private AudioClip SelectVoice(CharaType selectedTextStruct, int randomNo)
        {
            var VoiceList = selectedTextStruct.voiceList;
            var selectedVoiceSprite = VoiceList[randomNo];
            return selectedVoiceSprite;
        }
    }
}

