using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStages : MonoBehaviour
{

    // 生成するボタン
    [SerializeField]
    private GameObject selectButtonPrefab;
    // 生成するボタンの最大数(横)
    [SerializeField]
    private int buttonMaxWidth = 0;
    // 生成するボタンの最大数(縦)
    [SerializeField]
    private int buttonMaxHeight = 0;
    // 生成したボタンのリスト
    [SerializeField]
    private List<GameObject> selectButtons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Create();
    }


    private void Create()
    {
        // 作成する数を決定
        int maxCount = GameManager.Instance.canChooseStage.Count;
        // 作成するボタンのオフセットを設定
        // 横のオフセット
        var buttonParentSize = gameObject.GetComponent<RectTransform>().sizeDelta;
        var buttonSize = selectButtonPrefab.GetComponent<RectTransform>().sizeDelta;
        var offsetX = (buttonParentSize.x - (buttonSize.x * buttonMaxWidth)) / (buttonMaxWidth + 1);
        var buttonPosX = (-buttonParentSize.x / 2f) + (buttonSize.x / 2f) + offsetX;
        // 縦のオフセット
        var offsetY = (buttonParentSize.y - (buttonSize.y * buttonMaxHeight)) / (buttonMaxHeight + 1);
        var buttonPosY = (buttonParentSize.y / 2f) - (buttonSize.x / 2f) - offsetX;


        for(int y=0;y<buttonMaxHeight;y++)
        {
            for(int x=0;x<buttonMaxWidth;x++)
            {
                // ボタンを作成
                var buttonObj = Instantiate(selectButtonPrefab);
                // 親オブジェクトを設定
                buttonObj.transform.SetParent(gameObject.transform);
                // 位置を変更
                var buttonRect = buttonObj.GetComponent<RectTransform>();
                buttonRect.localPosition = new Vector3(buttonPosX, buttonPosY, 0);
                // 名前を変更
                buttonObj.name = GameManager.Instance.canChooseStage[((y * buttonMaxWidth) + x)].name;
                // ボタンのtextを変更
                buttonObj.transform.Find("Text").GetComponent<Text>().text = GameManager.Instance.canChooseStage[((y * buttonMaxWidth) + x)].name;
                // リストに追加
                selectButtons.Add(buttonObj);
                // 非表示にする
                buttonObj.SetActive(false);
                // オフセットをずらす
                buttonPosX += buttonSize.x + offsetX;
            }
            // オフセットをずらす
            buttonPosX  = (-buttonParentSize.x / 2f) + (buttonSize.x / 2f) + offsetX;
            buttonPosY -= buttonSize.y + offsetY;
        }

    }


    public void Show()
    {
        foreach(var buttonObj in selectButtons)
        {
            buttonObj.SetActive(true);
        }
    }

    public void Hide()
    {
        foreach (var buttonObj in selectButtons)
        {
            buttonObj.SetActive(false);
        }
    }
}
