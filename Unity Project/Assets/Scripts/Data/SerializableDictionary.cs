using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// シリアライズするためのディクショナリーもどきを基に
/// ディクショナリーを作成するクラス
/// </summary>
/// <typeparam name="Tkey"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="KeyAndValue"></typeparam>
[System.Serializable]
public class DictionaryList<Tkey, TValue, KeyAndValue> where KeyAndValue : KeyAndValue<Tkey, TValue>
{
    // シリアライズ出来るキーと値のリスト
    [SerializeField]
    protected List<KeyAndValue> keyAndValueList;
    // リスト基準で初期化するディクショナリー
    protected Dictionary<Tkey, TValue> dictionary;

    /// <summary>
    /// リストのデータを基に初期化したディクショナリーを取得
    /// </summary>
    /// <returns></returns>
    public Dictionary<Tkey, TValue> GetDictionary()
    {
        // ディクショナリーがnullなら
        if(dictionary == null)
        {
            // ディクショナリーの参照を取得
            dictionary = ConvertListToDictionary(keyAndValueList);
        }
        return dictionary;
    }

    /// <summary>
    /// リスト内のキーと値をディクショナリーにセット
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    static Dictionary<Tkey, TValue> ConvertListToDictionary(List<KeyAndValue> list)
    {
        // キーと値を入れるディクショナリーを初期化
        Dictionary<Tkey, TValue> keyValuePairs = new Dictionary<Tkey, TValue>();
        // リストのパラメータをディクショナリーに追加
        foreach(KeyAndValue<Tkey, TValue> pair in list)
        {
            keyValuePairs.Add(pair.key, pair.value);
        }

        return keyValuePairs;
    }
}

/// <summary>
/// シリアル化できるキーと値のペア
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
[System.Serializable]
public class KeyAndValue<TKey, TValue>
{
    // キー
    public TKey key;
    // 値
    public TValue value;
}
