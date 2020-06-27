using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popper : MonoBehaviour
{
    ParticleSystem effect = null;

    // アクティブになった際
    private void OnEnable()
    {
        effect = GetComponent<ParticleSystem>();
        // エフェクトの再生
        effect.Play();
    }
}
