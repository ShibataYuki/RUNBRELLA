using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStageButton : MonoBehaviour
{

    private SelectStages selectStages;

    // Start is called before the first frame update
    void Start()
    {
        selectStages = GameObject.Find("SelectStagesUI").GetComponent<SelectStages>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {
        for(int x=0;x<selectStages.stages.Count;x++)
        {
            for(int y=0;y<selectStages.stages[x].Count;y++)
            {
                if(gameObject.name==selectStages.stages[x][y].name)
                {
                    GameManager.Instance.choosedStage = selectStages.stages[x][y];
                    break;
                }
            }
        }

    }

}
