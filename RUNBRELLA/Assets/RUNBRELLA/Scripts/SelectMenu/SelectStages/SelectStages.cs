using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectStages : MonoBehaviour
{
    // 生成するボタン
    [SerializeField]
    private GameObject selectButtonPrefab = default;
    // 生成したボタンのリスト
    [SerializeField]
    private List<GameObject> selectButtons = new List<GameObject>();
    // マップの制作者ごとのリストを格納するリスト
    public List<List<GameObject>> stages = new List<List<GameObject>>();
    [SerializeField]
    private Color[] createrColor = new Color[3];
    [SerializeField]
    private GameObject scrollViewObj = default;
    // Start is called before the first frame update
    void Start()
    {
        stages.Add(GameManager.Instance.canChooseStage_Ishibashi);
        stages.Add(GameManager.Instance.canChooseStage_Mishima);
        stages.Add(GameManager.Instance.canChooseStage_Osio);
        Create();
        scrollViewObj.SetActive(false);
    }

    private void Update()
    {
    }

    private void Create()
    {
        // 作成する数を決定
        int maxCount = GameManager.Instance.canChooseStage_Ishibashi.Count + 
            GameManager.Instance.canChooseStage_Mishima.Count + GameManager.Instance.canChooseStage_Osio.Count;
        int buttonMaxWidth = 3;
        int buttonMaxHeight = 10;
        // 作成するボタンのオフセットを設定
        // 横のオフセット
        var buttonParentSize = gameObject.transform.Find("Scroll View").GetComponent<RectTransform>().sizeDelta;
        var contentSize = gameObject.transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>().sizeDelta;
        var buttonSize = selectButtonPrefab.GetComponent<RectTransform>().sizeDelta;
        var offsetX = (buttonParentSize.x - (buttonSize.x * buttonMaxWidth)) / (buttonMaxWidth + 1);
        var buttonPosX = (-buttonParentSize.x / 2f) + (buttonSize.x / 2f) + offsetX;
        // 縦のオフセット
        var offsetY = 10f;
        var buttonPosY = (contentSize.y / 2f) - (buttonSize.y / 2f) - offsetY;


        for(int x=0;x<buttonMaxWidth;x++)
        {
            for(int y=0;y<buttonMaxHeight;y++)
            {
                // ボタンを作成
                var buttonObj = Instantiate(selectButtonPrefab);
                // 親オブジェクトを設定
                buttonObj.transform.SetParent(gameObject.transform.Find("Scroll View/Viewport/Content").transform);
                // 位置を変更
                var buttonRect = buttonObj.GetComponent<RectTransform>();
                buttonRect.localPosition = new Vector3(buttonPosX, buttonPosY, 0);
                // 名前を変更
                buttonObj.name = stages[x][y].name;
                // ボタンのtextを変更
                buttonObj.transform.Find("Text").GetComponent<Text>().text = stages[x][y].name;
                // リストに追加
                selectButtons.Add(buttonObj);
                // 色を変更
                var buttonColor = buttonObj.GetComponent<Image>().color;
                buttonColor = createrColor[x];
                buttonObj.GetComponent<Image>().color = buttonColor;
                // オフセットをずらす
                buttonPosY -= buttonSize.y + offsetY;
            }
            // オフセットをずらす
            buttonPosY = (contentSize.y / 2f) - (buttonSize.y / 2f) - offsetY;
            buttonPosX += buttonSize.x + offsetX;
        }

    }


    public void Hide()
    {
        scrollViewObj.SetActive(false);
    }

    public void Show()
    {
        scrollViewObj.SetActive(true);
    }

}
