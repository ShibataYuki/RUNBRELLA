using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckScreen : MonoBehaviour
{

    // プレイヤーのRenderer
    Renderer playerRenderer;



    // Start is called before the first frame update
    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (!playerRenderer.isVisible)
        {
            gameObject.SetActive(false);
        }

    }
}
