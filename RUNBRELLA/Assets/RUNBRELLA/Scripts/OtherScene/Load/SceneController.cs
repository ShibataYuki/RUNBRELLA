using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Load
{
    public class SceneController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(SheetToDictionary.Instance.IsCompletedSheetToText)
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
}

