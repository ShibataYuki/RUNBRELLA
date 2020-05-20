using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayByTimeline : MonoBehaviour
{
    AudioSource BGMSource = null;

    private void OnEnable()
    {
        BGMSource = GameObject.Find("BGMSource").GetComponent<AudioSource>();
        BGMSource.Play();
    }
        
}
