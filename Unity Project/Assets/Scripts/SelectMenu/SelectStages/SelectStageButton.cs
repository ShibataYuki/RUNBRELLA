using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStageButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {
        foreach(var stage in GameManager.Instance.canChooseStage)
        {
            if(gameObject.name==stage.name)
            {
                GameManager.Instance.choosedStage = stage;
                break;
            }
        }
    }

}
