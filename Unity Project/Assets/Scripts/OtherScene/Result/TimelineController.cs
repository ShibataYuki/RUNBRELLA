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
    
        /// <summary>
        /// ループクリップ再生中か？
        /// </summary>
        public bool IsLooping { get; set; } = false;

        private void Update()
        {
            // キー入力があればループクリップを抜ける
            if (Input.GetKeyDown(KeyCode.Return) || GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any))
            {
                BreakLoopClip();
            }
            
        }

        /// <summary>
        /// ループクリップを抜ける処理
        /// (実際にはフラグを参照して「LoopBehaviour」が処理を変更する）
        /// </summary>
        public void BreakLoopClip()
        {
            // 現在ループ中でなければリターン
            var notLooping = !IsLooping;
            if (notLooping) { return; }                        
            IsLooping = false;            
        }
    } 
}



