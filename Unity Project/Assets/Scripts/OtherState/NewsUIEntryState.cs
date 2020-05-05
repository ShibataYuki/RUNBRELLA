using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsUIEntryState : MonoBehaviour,INewsUIState
{

    public void Entry(int ID)
    {
        // Entry処理
        NewsUIManager.Instance.newsUIs[ID].GetComponent<NewsUIEntry>().StartEntry();

    }

    public void Do(int ID)
    {
        // Entryの移動処理
        NewsUIManager.Instance.newsUIs[ID].GetComponent<NewsUIEntry>().OnEntry();
    }

    public void Exit(int ID)
    {
        NewsUIManager.Instance.newsUIs[ID].GetComponent<NewsUIEntry>().EndEntry();
    }

}
