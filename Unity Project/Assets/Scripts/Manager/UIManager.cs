using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region シングルトン
    // シングルトン
    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // 複数個作成しないようにする
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    #endregion

    [SerializeField]
    List<GameObject> countdowns = new List<GameObject>();
    [SerializeField]
    AudioClip start_and_endSE = null;
    public ResultUI resultUI = null;
    [SerializeField]
    private GameObject GoalCoinUI = null;


    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        for(int i=0;i<countdowns.Count;i++)
        {
            countdowns[i].SetActive(false);
        }
        // GoalCoinUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ゲームスタート時のカウントダウンをする関数
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartCountdown()
    {
        for(int i=0; i<countdowns.Count;i++)
        {
            if(i==3)
            {
                AudioManager.Instance.PlaySE(start_and_endSE, 1f);
            }
            countdowns[i].SetActive(true);
            yield return new WaitForSeconds(1);
            countdowns[i].SetActive(false);
        }
    }

}
