using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class TextManager : MonoBehaviour
{
    #region シングルトンインスタンス
    private static TextManager instance = null;
    public static TextManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // 初期化
            textDatas = new List<TextData>();

            for (int i = 0; i < textAssets.Length; i++)
            {
                var textAsset = textAssets[i]; // TextAsset単位で取り出す
                var textData = SetTextData(textAsset); // textAssetからデータを取り出す
                textDatas.Add(textData); // データをリストに追加
            }

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    [SerializeField]
    private TextAsset[] textAssets = new TextAsset[1];

    public struct TextData
    {
        // テキスト名
        public string fileName;
        // 変数のID
        public List<int> IDs;
        // 変数名
        public Dictionary<int, string> paramNames;
        // 変数の値
        public Dictionary<int,string[]> values;
    }

    // 一行のどのブロックか
    private enum DataPosition
    {
        ID    = 0,
        NAME  = 1,
        VALUE = 2,
    }

    private List<TextData> textDatas;

    /// <summary>
    ///  指定されたファイルの指定した変数名のパラメータを取得
    /// </summary>
    /// <param name="fileName">パラメータを探すファイルのファイル名</param>
    /// <param name="paramName">パラメータの変数名</param>
    /// <returns></returns>
    public float GetValue_float(string fileName, string paramName)
    {
        var textData = SarchFile(fileName);

        foreach (var ID in textData.IDs)
        {
            if (paramName != textData.paramNames[ID])
            {
                continue;
            }

            for (int i = 0; i < textData.values[ID].Length; i++)
            {
                var workString = textData.values[ID][i];
                // nullだったらcontinue
                if (workString == "" || workString == null)
                {
                    continue;
                }
                // nullでなければfloat型に変換
                return float.Parse(textData.values[ID][i]);
            }
        }

        Debug.Log(fileName + paramName + "Nothing");
        return 0.0f;
    }

    /// <summary>
    /// 指定されたファイルのID番目のパラメータを取得
    /// </summary>
    /// <param name="fileName">パラメータを探すファイルのファイル名</param>
    /// <param name="ID">パラメータのID</param>
    /// <returns></returns>
    public float GetValue_float(string fileName, int ID)
    {
        var textData = SarchFile(fileName);
        for (int i = 0; i < textData.values[ID].Length; i++)
        {
            var workString = textData.values[ID][i];
            // nullだったらcontinue
            if (workString == "" || workString == null)
            {
                continue;
            }
            // nullでなければfloat型に変換
            return float.Parse(textData.values[ID][i]);
        }
        Debug.Log(fileName + ID + "Nothing");
        return 0.0f;
    }

    /// <summary>
    ///  指定されたファイルの指定した変数名のパラメータを取得
    /// </summary>
    /// <param name="fileName">パラメータを探すファイルのファイル名</param>
    /// <param name="paramName">パラメータの変数名</param>
    /// <returns></returns>
    public int GetValue_int(string fileName, string paramName)
    {
        var textData = SarchFile(fileName);

        foreach (var ID in textData.IDs)
        {
            if (paramName != textData.paramNames[ID])
            {
                continue;
            }

            for (int i = 0; i < textData.values[ID].Length; i++)
            {
                var workString = textData.values[ID][i];
                // nullだったらcontinue
                if (workString == "" || workString == null)
                {
                    continue;
                }
                // nullでなければint型に変換
                return int.Parse(textData.values[ID][i]);
            }
        }

        Debug.Log(fileName + paramName +  "Nothing");
        return -1;
    }

    /// <summary>
    ///  指定されたファイルの指定した変数名のパラメータを取得
    /// </summary>
    /// <param name="fileName">パラメータを探すファイルのファイル名</param>
    /// <param name="paramName">パラメータの変数名</param>
    /// <returns></returns>

    public string[] GetString(string fileName, string paramName)
    {
        var textData = SarchFile(fileName);

        foreach (var ID in textData.IDs)
        {
            if (paramName != textData.paramNames[ID])
            {
                continue;
            }

            return textData.values[ID];
        }

        Debug.Log(fileName + paramName + "Nothing");
        return null;
    }

    /// <summary>
    /// 指定されたファイルのID番目のパラメータを取得
    /// </summary>
    /// <param name="fileName">パラメータを探すファイルのファイル名</param>
    /// <param name="ID">パラメータのID</param>
    /// <returns></returns>
    public string[] GetString(string fileName, int ID)
    {
        var textData = SarchFile(fileName);
        return textData.values[ID];
    }


    /// <summary>
    /// 指定されたファイルのID番目のパラメータを取得
    /// </summary>
    /// <param name="fileName">パラメータを探すファイルのファイル名</param>
    /// <param name="ID">パラメータのID</param>
    /// <returns></returns>
    public int GetValue_int(string fileName, int ID)
    {
        var textData = SarchFile(fileName);
        for (int i = 0; i < textData.values[ID].Length; i++)
        {
            var workString = textData.values[ID][i];
            // nullだったらcontinue
            if (workString == "" || workString == null)
            {
                continue;
            }
            // nullでなければint型に変換
            return int.Parse(textData.values[ID][i]);
        }
        Debug.Log(fileName + ID + "Nothing");
        return -1;
    }


    /// <summary>
    /// 指定したファイルのデータ取得
    /// </summary>
    /// <param name="fileName">探すファイルのファイル名</param>
    /// <returns>指定されたファイルのデータ</returns>
    public TextData SarchFile(string fileName)
    {
        // リスト内を探す
        foreach(var textData in textDatas)
        {
            // 探しているファイルなら
            if (textData.fileName == fileName)
            {
                // そのファイルのデータを return する
                return textData;
            }
        }

        // Resources フォルダーから指定されたファイルを読み込む
        var textAsset = Resources.Load<TextAsset>("Text/" + fileName);
        if (textAsset == null)
        {
            Debug.Log(fileName + null);
        }
        // データを読み込む
        var newText = SetTextData(textAsset);
        // リストに追加
        textDatas.Add(newText);
        // 読み込んだデータを return する
        return newText;
    }

    /// <summary>
    /// ファイルを読み込みデータをセットするメソッド
    /// </summary>
    /// <param name="textAsset">読み込むTextAsset</param>
    /// <returns>読み込んだTextAssetのデータの塊</returns>
    public TextData SetTextData(TextAsset textAsset)
    {
        // 戻り値用の変数の初期化
        TextData textData = new TextData();
        textData.IDs = new List<int>();
        textData.paramNames = new Dictionary<int, string>();
        textData.values = new Dictionary<int, string[]>();

        // ファイル名をセット
        textData.fileName = textAsset.name;
        // Text部分
        var text = textAsset.text;

        var textLines = text.Split(char.Parse("\n")); // 1行単位で取り出す

        for (int line = 1; line < textLines.Length; line++)
        {
            var textWords = textLines[line].Split(char.Parse("\t")); // TAB で区切る

            if (textWords.Length <= (int) DataPosition.VALUE)
            {
                Debug.Log(string.Format("{0}の{1}行目は{2}ブロックしかありません。", textAsset.name ,line, textWords.Length));
            }

            var ID = int.Parse(Regex.Replace(textWords[(int)DataPosition.ID], @"[^0-9]", "")); // 数字以外の文字の消去
            textData.IDs.Add(ID); // リストに追加
            var name = textWords[(int)DataPosition.NAME]; // 変数名の取得
            textData.paramNames.Add(ID, name); // ディクショナリーに追加
            var value = new string[textWords.Length - (int)DataPosition.VALUE]; // 配列の作成

            for (int block = 0; block < value.Length; block++)
            {
                value[block] = textWords[block + (int)DataPosition.VALUE]; // テキストからコピー
            }

            textData.values.Add(ID, value); // ディクショナリーに追加
        }

        return textData;
    }

    /// <summary>
    /// ファイル内から指定した変数名を持つパラメータのIDを取得
    /// </summary>
    /// <param name="fileName">探すファイルのファイル名</param>
    /// <param name="paramName">IDを調べる変数名</param>
    /// <returns></returns>
    public int GetID(string fileName, string paramName)
    {
        var data = SarchFile(fileName);
        foreach(var ID in data.IDs)
        {
            if (data.paramNames[ID] == paramName)
            {
                return ID;
            }
        }

        Debug.Log(fileName + paramName +  "がありません。");
        return -1;
    }
}
