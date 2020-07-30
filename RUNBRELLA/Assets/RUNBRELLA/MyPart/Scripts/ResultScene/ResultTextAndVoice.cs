using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResultScene
{
    public class ResultTextAndVoice : MonoBehaviour
    {
        // 勝利リザルトのテキストイメージ       
        Image textImage;
        // ボイスソース
        [SerializeField]
        AudioSource voiceSource = null;
        
        // 各キャラのテキストとボイスのリストを持った構造体
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
            SetTextAndVoice();
        }
   
        /// <summary>
        /// リザルトで使用するメッセージとボイスを決める
        /// </summary>
        public void SetTextAndVoice()
        {
            // 1位のキャラタイプによって構造体の選択
            var selectedStruct = SelectStruct();
            // そのキャラのメッセージテキストのリストからランダムにどの台詞を使うか決定
            var randomNo = ChooseRundomIntUntilListCount(selectedStruct);
            var selectedText = SelectText(selectedStruct,randomNo);
            textImage.sprite = selectedText;
            // そのキャラのボイスのリストからランダムにどの台詞を使うか決定
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

        /// <summary>
        /// リストのサイズ以下でランダムな整数値を返す
        /// </summary>
        /// <param name="selectedTextStruct"></param>
        /// <returns></returns>
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

