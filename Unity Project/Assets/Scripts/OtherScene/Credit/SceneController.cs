using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;

namespace Credit
{
    public class SceneController : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if(GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any))
            {
                // タイトルに遷移する
                SceneManager.LoadScene("Title");
            }
            else if(Input.GetKeyDown(KeyCode.Return))
            {
                // タイトルに遷移する
                SceneManager.LoadScene("Title");
            }
        }
    }
}