using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsUIShowState : MonoBehaviour,INewsUIState
{
    public void Entry(int ID)
    {
        // Entry処理
        NewsUIManager.Instance.newsUIs[ID].GetComponent<NewsUIShow>().StartShow();
    }

    public void Do(int ID)
    {
        // Do処理
        NewsUIManager.Instance.newsUIs[ID].GetComponent<NewsUIShow>().OnShow();
    }

    public void Exit(int ID)
    {
        // Exit処理
        NewsUIManager.Instance.newsUIs[ID].GetComponent<NewsUIShow>().EndShow();
    }
}
