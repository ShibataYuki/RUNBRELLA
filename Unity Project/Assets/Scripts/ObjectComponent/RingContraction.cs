using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingContraction : MonoBehaviour
{

    // 縮小用リング
    [SerializeField]
    private GameObject contractionRing = default;

    public void Transparency()
    {
        contractionRing.SetActive(false);
    }

}
