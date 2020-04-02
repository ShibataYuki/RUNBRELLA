using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class TimeLine : MonoBehaviour
{
    PlayableDirector director;
    Animator playerAnimater;
    [SerializeField]
    GameObject playerObj;
    Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = playerObj.GetComponent<Animator>();
        director = GetComponent<PlayableDirector>();
        

        foreach (var track in director.playableAsset.outputs)
        {
            if(track.streamName == "A")
            {
                director.SetGenericBinding(track.sourceObject, playerAnimator);
            }
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
