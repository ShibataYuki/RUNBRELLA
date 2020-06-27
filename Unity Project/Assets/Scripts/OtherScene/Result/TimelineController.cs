using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using GamepadInput;

namespace ResultScene
{
    public class TimelineController : MonoBehaviour
    {
        [SerializeField]
        public List<PlayableDirector> timelineList;
        #region シングルトン
        // シングルトン
        private static TimelineController instance;
        public static TimelineController Instance
        {
            get { return instance; }
        }

        private void Awake()
        {
            // 複数個作成しないようにする
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        #endregion      
    } 
}



