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
    // 読み込むテキストの配列
    [SerializeField]
    private TextAsset[] textAssets = new TextAsset[1];

    // テキスト情報を分解した構造体
    public struct TextData
    {
        // テキスト名
        public string fileName;
        // 変数のID
        public List<int> IDs;
        // 変数名
        public Dictionary<int, string> paramNames;
        // 変数の値
        public Dictionary<int,List<string>> values;
    }

    // 一行のどのブロックか
    private enum DataPosition
    {
        ID    = 0,
        NAME  = 1,
        VALUE = 2,
    }

    // 読み込んだテキスト内の情報のリスト
    private List<TextData> textDatas;

    /// <summary>
    ///  指定されたファイルの指定した変数名のパラメータを取得
    /// </summary>
    /// <param name="fileName">パラメータを探すファイルのファイル名</param>
    /// <param name="paramName">パラメータの変数名</param>
    /// <returns></returns>
    public float GetValue_float(string fileName, string paramName, string path = "Text/")
    {
        // リストから必要なデータを一ファイル分取り出す
        var textData = SarchFile(fileName, path);

        try
        {
            // リストから必要な1行を探す
            foreach (var ID in textData.IDs)
            {
                if (paramName != textData.paramNames[ID])
                {
                    continue;
                }

                // データの中から数字を取り出す
                for (int i = 0; i < textData.values[ID].Count; i++)
                {
                    var workString = textData.values[ID][i];
                    // nullだったらcontinue
                    if (workString == "" || workString == null)
                    {
                        continue;
                    }
                    try
                    {
                        // nullでなければfloat型に変換
                        return float.Parse(textData.values[ID][i]);
                    }
                    catch
                    {
                        Debug.Assert(false, string.Format("{0}の{1}はfloat型に変換できませんでした。", fileName, paramName));
                        return -1;
                    }
                }
            }
        }
        catch
        {
            Debug.Assert(false, fileName + paramName + "Nothing");
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
    public float GetValue_float(string fileName, int ID, string path = "Text/")
    {
        // リストから必要なデータを一ファイル分取り出す
        var textData = SarchFile(fileName, path);
        try
        {
            for (int i = 0; i < textData.values[ID].Count; i++)
            {
                var workString = textData.values[ID][i];
                // nullだったらcontinue
                if (workString == "" || workString == null)
                {
                    continue;
                }
                try
                {
                    // nullでなければfloat型に変換
                    return float.Parse(textData.values[ID][i]);
                }
                catch
                {
                    Debug.Assert(false, string.Format("{0}の{1}番目はfloat型に変換できませんでした。", fileName, ID));
                    return -1;
                }
            }
        }
        catch
        {
            Debug.Assert(false, fileName + ID + "Nothing");
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
    public int GetValue_int(string fileName, string paramName, string path = "Text/")
    {
        // リストから必要なデータを一ファイル分取り出す
        var textData = SarchFile(fileName, path);
        try
        {
            foreach (var ID in textData.IDs)
            {
                if (paramName != textData.paramNames[ID])
                {
                    continue;
                }

                for (int i = 0; i < textData.values[ID].Count; i++)
                {
                    var workString = textData.values[ID][i];
                    // nullだったらcontinue
                    if (workString == "" || workString == null)
                    {
                        continue;
                    }
                    try
                    {
                        // nullでなければint型に変換
                        return int.Parse(textData.values[ID][i]);
                    }
                    catch {
                        Debug.Assert(false, string.Format("{0}の{1}はint型に変換できませんでした。", fileName, paramName));
                        return -1;
                    }
                }
            }
        }
        catch
        {
        }
        Debug.Assert(false, fileName + paramName + "Nothing");
        return -1;
    }

    /// <summary>
    ///  指定されたファイルの指定した変数名のパラメータを取得
    /// </summary>
    /// <param name="fileName">パラメータを探すファイルのファイル名</param>
    /// <param name="paramName">パラメータの変数名</param>
    /// <returns></returns>

    public string[] GetString(string fileName, string paramName, string path = "Text/")
    {
        // リストから必要なデータを一ファイル分取り出す
        var textData = SarchFile(fileName, path);
        try
        {
            foreach (var ID in textData.IDs)
            {
                if (paramName != textData.paramNames[ID])
                {
                    continue;
                }
                // 配列の作成
                var workStrings = new string[textData.values[ID].Count];
                // リストから文字列をコピー
                for(int i = 0; i < workStrings.Length; i++)
                {
                    workStrings[i] = textData.values[ID][i];
                }

                return workStrings;
            }
        }
        catch
        {
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
    public string[] GetString(string fileName, int ID, string path = "Text/")
    {
        // テキストを探す
        var textData = SarchFile(fileName, path);
        try
        {
            // 配列の作成
            var workStrings = new string[textData.values[ID].Count];
            // リストから文字列をコピー
            for (int i = 0; i < workStrings.Length; i++)
            {
                workStrings[i] = textData.values[ID][i];
            }

            return workStrings;
        }
        catch
        {
            Debug.Log(string.Format("{0}の{1}番目のパラメータが{2}",fileName, ID, "ありません。"));
            return null;
        }
    }


    /// <summary>
    /// 指定されたファイルのID番目のパラメータを取得
    /// </summary>
    /// <param name="fileName">パラメータを探すファイルのファイル名</param>
    /// <param name="ID">パラメータのID</param>
    /// <returns></returns>
    public int GetValue_int(string fileName, int ID, string path = "Text/")
    {
        // リストから必要なデータを一ファイル分取り出す
        var textData = SarchFile(fileName, path);
        try
        {
            for (int i = 0; i < textData.values[ID].Count; i++)
            {
                var workString = textData.values[ID][i];
                // nullだったらcontinue
                if (workString == "" || workString == null)
                {
                    Debug.Log(string.Format("{0}の{1}番目の行ににNULL文字があります。", fileName, ID));
                    continue;
                }
                try
                {
                    // nullでなければint型に変換
                    return int.Parse(textData.values[ID][i]);
                }
                catch
                {
                    Debug.Assert(false, string.Format("{0}の{1}番目のパラメータをint型に変換できませんでした。", fileName, ID));
                    return -1;
                }
            }
        }
        catch
        {
        }
        Debug.Assert(false, fileName + ID + "Nothing");
        return -1;
    }


    /// <summary>
    /// 指定したファイルのデータ取得
    /// </summary>
    /// <param name="fileName">探すファイルのファイル名</param>
    /// <returns>指定されたファイルのデータ</returns>
    public TextData SarchFile(string fileName, string path = "Text/")
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
        var textAsset = Resources.Load<TextAsset>(path + fileName);

        Debug.Assert((textAsset != null), fileName + null);

        try
        {
            // データを読み込む
            var newText = SetTextData(textAsset);
            // リストに追加
            textDatas.Add(newText);
            // 読み込んだデータを return する
            return newText;
        }
        catch
        {
            Debug.Assert(false, string.Format("{0}の読み込みに失敗しました。", fileName));
            return new TextData();
        }
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
        textData.values = new Dictionary<int, List<string>>();

        // ファイル名をセット
        textData.fileName = textAsset.name;
        // Text部分
        var text = textAsset.text;

        var textLines = text.Split(char.Parse("\n")); // 1行単位で取り出す

        for (int line = 1; line < textLines.Length; line++)
        {
            var textWords = textLines[line].Split(char.Parse("\t"), char.Parse(" "), char.Parse(",")); // TAB等 で区切る

            if (textWords.Length <= (int) DataPosition.VALUE)
            {
                Debug.Log(string.Format("{0}の{1}行目は{2}ブロックしかありません。", textAsset.name ,line, textWords.Length));
            }
            // どこのデータにセットするか
            var dataPosition = DataPosition.ID;
            int ID = 0;
            var values = new List<string>();
            foreach(var textWord in textWords)
            {
                // NULL文字だったら
                if(textWord == "" || textWord == null)
                {
                    Debug.Log(string.Format("{0}の{1}行目にNULL文字があります。", textAsset.name, line));
                    continue;
                }
                else
                {
                    switch (dataPosition) {
                        case DataPosition.ID:
                            try{
                                ID = int.Parse(Regex.Replace(textWord, @"[^0-9]", "")); // 数字以外の文字の消去
                                textData.IDs.Add(ID); // リストに追加
                            }
                            catch
                            {
                                Debug.Assert(false, string.Format("{0}の{1}行目のIDが正しくありません", textAsset.name, line));
                                textData.IDs.Add(-1);
                            }
                            break;
                        case DataPosition.NAME:
                            var name = textWord; // 変数名の取得
                            textData.paramNames.Add(ID, name); // ディクショナリーに追加

                            break;
                        case DataPosition.VALUE:
                            values.Add(textWord);// リストに追加
                            break;
                    }
                }
                if(dataPosition < DataPosition.VALUE)
                {
                    dataPosition++; // 次のループでは別のデータにセットする
                }
            }
            if(values.Count <= 0)
            {
                Debug.Log(string.Format("{0}の{1}行目にはデータがありません。", textAsset.name, line));
            }

            textData.values.Add(ID, values); // ディクショナリーに追加
        }

        return textData;
    }

    /// <summary>
    /// ファイル内から指定した変数名を持つパラメータのIDを取得
    /// </summary>
    /// <param name="fileName">探すファイルのファイル名</param>
    /// <param name="paramName">IDを調べる変数名</param>
    /// <returns></returns>
    public int GetID(string fileName, string paramName, string path = "Text/")
    {
        // リストから必要なデータを一ファイル分取り出す
        var data = SarchFile(fileName, path);
        foreach(var ID in data.IDs)
        {
            if (data.paramNames[ID] == paramName)
            {
                return ID;
            }
        }

        Debug.Assert(false, fileName + paramName +  "がありません。");
        return -1;
    }
}
