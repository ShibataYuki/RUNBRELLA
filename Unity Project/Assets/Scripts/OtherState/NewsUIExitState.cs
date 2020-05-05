using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsUIExitState : MonoBehaviour, INewsUIState
{
    public void Entry(int ID)
    {
        // Entry処理
        NewsUIManager.Instance.newsUIs[ID].GetComponent<NewsUIExit>().StartExit();
    }

    public void Do(int ID)
    {
        // Do処理
        NewsUIManager.Instance.newsUIs[ID].GetComponent<NewsUIExit>().OnExit();
    }

    public void Exit(int ID)
    {
        // Exit処理
        NewsUIManager.Instance.newsUIs[ID].GetComponent<NewsUIExit>().EndExit();
    }

}