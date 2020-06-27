using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingContractionManager : MonoBehaviour
{

    // 生成する縮小用リング
    [SerializeField]
    private GameObject ringContractionPrefab = default;
    // アクティブな縮小用リング用リスト
    public List<GameObject> activatedRingList = new List<GameObject>();
    // 非アクティブな縮小用リング用リスト
    public Stack<GameObject> inactivateRingList = new Stack<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 縮小用リングをアクティブにするメソッド
    /// </summary>
    /// <param name="ringContraction">縮小用リング</param>
    private void Activate(GameObject ringContraction)
    {
        // 表示する
        ringContraction.SetActive(true);
        var ringContractionAnimator = ringContraction.GetComponent<Animator>();
        ringContractionAnimator.SetTrigger("StartTrigger");
    }


    /// <summary>
    /// どの縮小用リングを使うのか決めるメソッド
    /// </summary>
    public void UseRingContraction()
    {
        // 未使用の縮小用リングがあるなら使う
        if(inactivateRingList.Count>0)
        {
            var useRing = inactivateRingList.Pop();
            // 使用中の縮小用リングに登録
            activatedRingList.Add(useRing);
            // アクティブ化
            Activate(useRing);
        }
        // 未使用の縮小用リングがないなら作成
        else
        {
            var useRing = Instantiate(ringContractionPrefab);
            useRing.transform.SetParent(gameObject.transform);
            useRing.transform.localPosition = Vector3.zero;
            useRing.transform.localScale = new Vector3(1, 1, 1);
            // 使用中の縮小用リングに登録
            activatedRingList.Add(useRing);
            // アクティブ化
            Activate(useRing);
        }
    }


}
