using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResultScene
{
    /// <summary>
    /// リザルト演出でタイムラインから呼び出す関数を持つ
    /// </summary>
    public class EffectByAnimEvent : MonoBehaviour
    {        
        // キャラのタイプによって演出を変更する
        public void PlayImpressions(string childName)
        {
            var Impression = transform.Find("Impressions/" + childName);
            var effect = Impression.GetComponent<ParticleSystem>();
            effect.Play();
        }
    }
}

