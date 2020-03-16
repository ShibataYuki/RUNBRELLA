using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDown : MonoBehaviour
{
    // 現在の時間
    public float nowTime = 0;
    // ダウン時にボタンを押したときに１フレームごとに減る時間の値
    [SerializeField]
    public float addTime = 0.05f;
    // ダメージを受けたときのSE
    [SerializeField]
    private AudioClip damageSE = null;
    // SEのボリューム
    private float damageSEVolume = 1f;

    // 読み込むファイルのファイル名
    private readonly string fileName = nameof(PlayerDown) + "Data";

    // Start is called before the first frame update
    void Start()
    {
        addTime = TextManager.Instance.GetValue(fileName, nameof(addTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DownStart()
    {
        // SEの再生
        AudioManager.Instance.PlaySE(damageSE, damageSEVolume);
    }

    /// <summary>
    /// 時間測定関数
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool TimeCounter(float time)
    {
        nowTime += Time.deltaTime;
        if (nowTime >= time)
        {
            nowTime = 0;
            return true;
        }
        return false;
    }

}
