﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SheetToDictionary : MonoBehaviour
{
    // グーグルスプレッドシートのID
    readonly string seetID = "1k3Pt9YUP6qyZidWYeLmRce71C-gtL9AaFeE7jnrnyfM";    
    
    // テキストの名前からシートの名前を引き出せるディクショナリ
    Dictionary<string, string> textNameToSheetNameDic = new Dictionary<string, string>();
    // シートネームとテキストネームを対応させる構造体(インスペクターで設定)
    [System.Serializable]
    struct Sheet_Text
    {
        public string sheetName;
        public string textName;

        public Sheet_Text(string sheetName,string textName)
        {
            this.sheetName = sheetName;
            this.textName = textName;
        }
    }

    // テキスト名からデータを取り出すことのできるDictionary
    public static Dictionary<string, Dictionary<string, float>> TextNameToData { get; } = new Dictionary<string, Dictionary<string, float>>();   

    // 構造体のリスト
    [SerializeField]
    List<Sheet_Text> text_SheetList = new List<Sheet_Text>();

    // 読み込みエラーを通知するポップアップ
    ErrorPop errorPop = default;
    // 読み込み完了フラグ
    public bool IsCompletedSheetToText{ get; set; } = false;

    #region シングルトンインスタンス
    // シングルトン
    private static SheetToDictionary instance;
    public static SheetToDictionary Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // 複数個作成しないようにする
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    private void Start()
    {
        errorPop = GameObject.Find("SceneController").GetComponent<ErrorPop>();
    }
    /// <summary>
    /// シートを読み込んでディクショナリを更新する処理
    /// </summary>
    /// <param name="sheetName">グーグルスプレッドシートのシート名</param>
    /// <param name="textName">テキストファイルの名前(拡張子なし)</param>
    /// <returns></returns>
    public IEnumerator ReadSheetToDictionary()
    {
        foreach (Sheet_Text element in text_SheetList)
        {
            var sheetName = element.sheetName;
            var textName = element.textName;
            textNameToSheetNameDic.Add(textName, sheetName);
#if UNITY_EDITOR
            // 「streamingAssets」フォルダへのパス　+ 引数のテキストの名前を組み込んだパス
            var pass = Application.streamingAssetsPath + "/Texts/" + textName + ".txt";
            // UnityWebRequestを生成
            UnityWebRequest request = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/" + seetID + "/gviz/tq?tqx=out:csv&sheet=" + sheetName);
            // Webサイトとの通信を開始
            yield return request.SendWebRequest();            
            // サイトと通信失敗した場合
            if (request.isHttpError || request.isNetworkError)
            {
                // 特に何もしない
                Debug.Log("シートとの通信に失敗したためテキストは更新されませんでした");
            }
            else
            {               
                string data = request.downloadHandler.text;
                // スプレッドシートから読み込んだデータをテキストに書き込み           
                WriteDataToText(data, pass);                
            }
#endif
            Dictionary<string, float> tempDictionary;
            // テキストのデータをDictionaryに変換
            TextToDictionary(element.textName, out tempDictionary);
            // Dictionaryを登録
            TextNameToData.Add(element.textName, tempDictionary);            
        }
        // 読み込み完了フラグON
        IsCompletedSheetToText = true;
        yield break;
    }

        
    /// <summary>
    /// テキストファイルのデータからディクショナリーインスタンスを生成し、
    /// 参照を返す処理
    /// </summary>
    /// <param name="textName">テキストファイルの名前(拡張子なし)</param>
    /// <param name="dictionary">参照のないディクショナリ</param>
    /// <returns></returns>
    public void TextToDictionary(string textName, out Dictionary<string,float> dictionary)
    {
        // 「streamingAssets」フォルダへのパス　+ 引数のテキストの名前を組み込んだパス
        var pass = Application.streamingAssetsPath + "/Texts/" + textName + ".txt";
        // ファイルのすべての文字列を1つの文字列として読み込む
        string text = File.ReadAllText(pass);               
        // 文字列を読み込むクラス
        StringReader reader = new StringReader(text);
        // 戻り値用ディクショナリ
        dictionary = new Dictionary<string, float>();
       
        while (reader.Peek() != -1)                     // 戻り値が-1(もう読み取るものがない)までループ
        {
            var a = reader.Peek();
            // １行単位の読み込み
            string line = reader.ReadLine();            
                                                        
            // 文字列を「,」毎に要素として配列に格納
            string[] elements = line.Split(',');        // 例）"a","b","c" → elements[0] "a"
                                                        //                    elements[1] "b" ...
           
            // ダブルクォーテーションを取り除く
            RemoveDoubleQuotation(elements);            // 例)  elements[0] "a"        elements[0] a
                                                        //      elements[1] "b" ... → elements[1] b

            // 完成したstring型の配列をディクショナリーに変換する処理
            SetDataDictionary(elements,dictionary,textName);

        }
        
    }

   
    /// <summary>
    /// 受け取った文字列を解析し、対になる値をディクショナリにセットする処理
    /// </summary>
    /// <param name="elements">文字列の配列</param>
    enum INDEX
    {
        KEY,
        VALUE,
    }
    void SetDataDictionary(string[] elements, Dictionary<string, float> dictionary, string textName)
    {
        
        // ディクショナリの「key」と「value」にするものを保持しておく配列
        string[] keyValue = new string[2];
        // 文字を読み取るたびに増やしていく
        //int workIndex = 0;
        INDEX workIndex = INDEX.KEY;
        for (int i = 0; i < elements.Length; i++)
        {
            // 添え字をKEY(0)→VALUE(1)→KEY(0)→...に循環させる
            workIndex = (INDEX)Mathf.Repeat((int)workIndex, 2);
            // 空のセル( "" )
            string emptyCell = "";
            // 空のセルなら添え字を増加させない
            if (elements[i] == emptyCell)
            {
                continue;
            }

            //　保持領域に数値を代入
            keyValue[(int)workIndex] = elements[i];

            // valueのほうに代入が完了した(key/valueがどっちもそろった)ならディクショナリに移す
            if (workIndex == INDEX.VALUE)
            {
                // ディクショナリのkeyになる変数
                string key = keyValue[(int)INDEX.KEY];
                // keyが数字でないかチェック
                StringCheck(key,textName);
                // ディクショナリのvalueになる変数をfloat型に変換
                float value = ChangeStringToFloat(keyValue[(int)INDEX.VALUE],textName);           
                // ディクショナリにセット
                dictionary.Add(key, value);
            }
            // インデックスを進める
            workIndex++;
        }

        // keyのほうが埋まっているのにディクショナリへの変換が残っている
        // = key と　value が対になっていない場合(要素の数が奇数の場合)に警告を出す
        if(workIndex == INDEX.VALUE)
        {
            // テキストのもとになったシートの名前
            var sheetName = textNameToSheetNameDic[textName];
            // エラー用ポップアップウィンドウをアクティブ化
            errorPop.SetActive();
            // テキスト変更
            errorPop.ChangeText("シートの入力が間違っています\n" +
                                "シート名：" + sheetName + "\n" +
                                "エラー内容：入力されていないセルがあります\n" +
                                "数値を直してアプリを再起動してください\n" +
                                "数値の入っているセルを選択しているとその数値は反映されないので" +
                                "何も記入していないセルをクリックしてください");            
        }
       
    }



    /// <summary>
    /// 文字列からダブルクォーテーションを除去する処理
    /// </summary>
    /// <param name="texts"></param>
    void RemoveDoubleQuotation(string[] texts)    
    {
        
        for (int i = 0; i < texts.Length; i++)
        {
            // ダブルクォーテーションを取り除く
            texts[i] = texts[i].TrimStart('"').TrimEnd('"'); ;
        }
        
    }

    /// <summary>
    /// 文字列をフロート型に変更する処理
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    float ChangeStringToFloat(string value, string textName)
    {
        // 返却する変数
        float result = 0;
        // 変換がうまくいけば「true」
        bool changeComplete = float.TryParse(value, out result); 
        if(changeComplete == true)
        {
            return result;
        }
        // テキストのもとになったシートの名前
        var sheetName = textNameToSheetNameDic[textName];
        // 変換が失敗したら警告を出す       
        // エラー用ポップアップウィンドウをアクティブ化
        errorPop.SetActive();
        errorPop.ChangeText("シートの入力が間違っています\n" +
                            "シート名：" + sheetName + "\n" +
                            "エラー内容：数値が入るセルに項目名が入っています\n" +
                            "間違っている数値(" + value + ")\n" +
                            "数値を直してアプリを再起動してください\n" +
                            "数値の入っているセルを選択しているとその数値は反映されないので" +
                            "何も記入していないセルをクリックしてください");
        
        return -123456;
    }

    /// <summary>
    /// 文字列が数字のみで構成されていないかチェックする処理
    /// </summary>
    /// <param name="keyText"></param>
    void StringCheck(string keyText,string textName)
    {

        float result = 0;
        // フロート型への変換ができないことを確かめる
        bool changeComplete = float.TryParse(keyText, out result);
        // もし変換ができてしまったら警告を出す
        bool notString = changeComplete == true;
        if (!notString) { return; }

        // テキストのもとになったシートの名前
        var sheetName = textNameToSheetNameDic[textName];
        // エラー用ポップアップウィンドウをアクティブ化
        errorPop.SetActive();
        errorPop.ChangeText("シートの入力が間違っています\n" +
                            "シート名：" + sheetName + "\n" +
                            "エラー内容：項目名が入るセルに数値が入っています\n" +
                            "間違っている項目名:(" + keyText + ")\n" +
                            "数値を直してアプリを再起動してください\n" +
                            "数値の入っているセルを選択しているとその数値は反映されないので" +
                            "何も記入していないセルをクリックしてください");
    }

    /// <summary>
    /// 文字列をテキストファイルに上書きする処理
    /// </summary>
    /// <param name="data">書き込む文字列</param>
    /// <param name="pass">テキストファイルへのパス</param>
    static void WriteDataToText(string data,string pass)
    {        
        File.WriteAllText(pass, data);
    }

}
