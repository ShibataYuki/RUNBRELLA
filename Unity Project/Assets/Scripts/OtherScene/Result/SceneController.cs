using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;
using UnityEngine.Playables;

namespace Result
{
    public class SceneController : MonoBehaviour
    {
        PlayableDirector director;
        GameObject TimelineControllerObj;
        // Start is called before the first frame update
        void Start()
        {
            TimelineControllerObj = GameObject.Find("TimelineController");
            director = TimelineControllerObj.GetComponent<PlayableDirector>();
        }

        // Update is called once per frame
        void Update()
        {
           

            // 終了処理
            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                UnityEngine.Application.Quit();
#endif
            }

            GoToNextScean();
        }


        void GoToNextScean()
        {
            bool isTimelinePlaying = director.state == PlayState.Playing;
            if (isTimelinePlaying) { return; }
            if (Input.GetKeyDown(KeyCode.Return) || GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any))
            {
                // キャラ選択に戻る
                SceneManager.LoadScene("SelectMenu");
            }
        }

    }
}