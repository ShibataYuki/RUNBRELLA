using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TrafficLight : MonoBehaviour
{
    GameObject controller = null;
    BindObject buindObject = null;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("TimelineController");
        buindObject = controller.GetComponent<BindObject>();
        // タイムライントラックに自身をセット
        buindObject.BindObj(this.gameObject, "TrafficLight");
    }

}
