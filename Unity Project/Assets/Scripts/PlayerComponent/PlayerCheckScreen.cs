using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckScreen : MonoBehaviour
{

    // プレイヤーのRenderer
    Renderer playerRenderer;
    // プレイヤーが画面内にいるかどうか
    bool isScreen = false;


    // Start is called before the first frame update
    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if(!isScreen)
        {
            // プレイヤーの順位順のリストに格納
            SceneController.Instance.goalRunkOrder.Insert(SceneController.Instance.goalRunkOrder.Count - SceneController.Instance.deadPlayerCount, gameObject);
            gameObject.SetActive(false);
        }
        isScreen = false;
    }

    private void OnWillRenderObject()
    {
        if(Camera.current.name=="Main Camera")
        {
            isScreen = true;
        }
    }
}
