using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingContraction : MonoBehaviour
{

    private RingContractionManager ringContractionManager;

    private void Start()
    {
        ringContractionManager = gameObject.GetComponentInParent<RingContractionManager>();
    }

    public void Transparency()
    {
        gameObject.SetActive(false);
        // 使用中の縮小用リングから削除
        ringContractionManager.activatedRingList.Remove(gameObject);
        // 使用済み縮小用リングに追加
        ringContractionManager.inactivateRingList.Push(gameObject);
    }

}
