using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TrafficLight : MonoBehaviour
{
    GameObject controller = null;
    BuindObject buindObject = null;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("TimelineController");
        buindObject = controller.GetComponent<BuindObject>();
        // タイムライントラックに自身をセット
        buindObject.BindObj(this.gameObject, "TrafficLight");
    }

}
