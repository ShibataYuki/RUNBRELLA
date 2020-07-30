using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResultScene
{
    /// <summary>
    /// タイムライン上でアクティブ化することでBGMを鳴らす機能を持つ
    /// </summary>
    public class PlayBGMByTimeline : MonoBehaviour
    {
        AudioSource BGMSource = null;

        /// <summary>
        /// アクティブになった際にBGMを再生する
        /// </summary>
        private void OnEnable()
        {
            BGMSource = transform.parent.GetComponent<AudioSource>();
            BGMSource.Play();
        }
    }
}

