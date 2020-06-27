using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResultScene
{
    public class PlayerTimeline : MonoBehaviour
    {        
        public void PlayImpressions(string childName)
        {
            var Impression = transform.Find("Impressions/" + childName);
            var effect = Impression.GetComponent<ParticleSystem>();
            effect.Play();
        }
    }
}

