using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlusText : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }


    /// <summary>
    /// textを表示するコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator Show()
    {
        // 表示
        gameObject.SetActive(true);
        // 待機 
        yield return new WaitForSeconds(0.5f);
        // 非表示
        gameObject.SetActive(false);
    }
}
