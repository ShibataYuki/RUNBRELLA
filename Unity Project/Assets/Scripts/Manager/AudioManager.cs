using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> audioSources = new List<AudioSource>();
    private int defaultSourceCount = 8;


    #region シングルトンインスタンス

    /// <summary>
    /// このクラスのインスタンスを取得するプロパティーです。
    /// </summary>
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static AudioManager instance = null;

    /// <summary>
    /// Start()の実行より先行して処理したい内容を記述します。
    /// </summary>
    void Awake()
    {
        // 初回作成時
        if (instance == null)
        {
            instance = this;
        }
        // 2個目以降の作成時
        else
        {
            Destroy(gameObject);
        }
        // AudioSorceを作る数が決まっているならそれ分作り、
        // 決まっていないなら規定個作る
        if(audioSources.Count!=0)
        {
            defaultSourceCount = audioSources.Count;
        }
        else
        {
            for(int i=0;i<defaultSourceCount;i++)
            {
                audioSources.Add(null);
            }
        }
        // audioSourcesにAudioMnanagerのAudioSourceを格納
        for (int i = 0; i < defaultSourceCount; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].playOnAwake = false;
            audioSources[i].loop = false;
        }

    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }

    /// <summary>
    /// BGMを再生する関数です
    /// </summary>
    /// <param name="audioClip">鳴らしたいAudioClip</param>
    /// <param name="isLoop">ループ再生フラグ</param>
    public void PlayBGM(AudioClip audioClip, bool isLoop, float volume)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            // !NULLチェック
            if (audioSources[i].clip != null)
            {
                continue;
            }

            audioSources[i].loop = isLoop;
            audioSources[i].clip = audioClip;
            audioSources[i].volume = volume;
            audioSources[i].Play();

            return;
        }
        Debug.Log("音楽を再生できませんでした");
        return;
    }


    /// <summary>
    /// SEを再生する関数です
    /// </summary>
    /// <param name="audioClip">鳴らしたいAudioClip</param>
    public void PlaySE(AudioClip audioClip, float volume)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            // 再生中かチェック
            if (audioSources[i].isPlaying == true)
            {
                continue;
            }
            Debug.Log(i + "番目のAudioSorceを使用します。");
            audioSources[i].volume = volume;
            audioSources[i].clip = audioClip;
            audioSources[i].Play();
            return;
        }
        // 新しいAudioSorceを作成しそのAudioSourcesで鳴らす
        AudioSource.PlayClipAtPoint(audioClip,Camera.main.transform.position,volume);
        Debug.Log("AudioSourceが足りなかったので新しくAudioSourceを作成しましました");
        return;
    }


    /// <summary>
    /// 音を停止する関数です
    /// </summary>
    /// <param name="audioClip">停止する音</param>
    public void StopAudio(AudioClip audioClip)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            // 違う曲なら
            if (audioSources[i].clip != audioClip)
            {
                continue;
            }
            audioSources[i].Stop();
            audioSources[i].clip = null;
            return;
        }
        Debug.Log("音楽の停止に失敗しました");
        return;
    }


    /// <summary>
    /// 音を停止する関数です
    /// </summary>
    /// <param name="audioClip">停止する音</param>
    public void StopAllAudio()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            // clipがはいってないならreturn
            if (audioSources[i].clip == null)
            {
                continue;
            }
            audioSources[i].Stop();
            audioSources[i].clip = null;
        }
        return;
    }



    /// <summary>
    /// 音を中断させる関数です
    /// </summary>
    /// <param name="audioClip">中断させたいAudioClip</param>
    public void PauseAudio(AudioClip audioClip)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            // 曲が違うならreturn
            if (audioSources[i].clip != audioClip)
            {
                continue;
            }
            // 再生中でないならreturn
            if (!(audioSources[i].isPlaying))
            {
                continue;
            }
            audioSources[i].Pause();
            return;
        }
        Debug.Log("音楽の停止に失敗しました");
        return;
    }


    /// <summary>
    /// すべて音を中断させる関数です
    /// </summary>
    public void PauseAllAudio()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            // clipがnullならreturn
            if (audioSources[i].clip == null)
            {
                continue;
            }
            // 再生中でないならreturn
            if (!(audioSources[i].isPlaying))
            {
                continue;
            }
            audioSources[i].Pause();
        }
        return;
    }



    /// <summary>
    /// 中断した音楽を再度再生する関数です
    /// </summary>
    /// <param name="audioClip">再度再生させたいAudioClip</param>
    public void ReStartAudio(AudioClip audioClip)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            // clipが違うのならreturn
            if (audioSources[i].clip != audioClip)
            {
                continue;
            }
            // 再生中ならreturn
            if (audioSources[i].isPlaying)
            {
                continue;
            }
            audioSources[i].UnPause();
            return;
        }
        Debug.Log("音楽の再度再生に失敗しました");
        return;
    }


    /// <summary>
    /// すべての中断した音楽を再度再生する関数です
    /// </summary>
    public void ReStartAllAudio()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            // clipがnullならreturn
            if (audioSources[i].clip == null)
            {
                continue;
            }
            // 再生中ならreturn
            if (audioSources[i].isPlaying)
            {
                continue;
            }
            audioSources[i].UnPause();
            return;
        }
        Debug.Log("音楽の再度再生に失敗しました");
        return;
    }


    /// <summary>
    /// 引数に指定したclipが再生中か調べる関数
    /// </summary>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    public bool CheckAudio(AudioClip audioClip)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            // clipがnullならreturn
            if (audioSources[i].clip == null)
            {
                continue;
            }
            // clipが違うならreturn
            if (audioSources[i].clip != audioClip)
            {
                continue;
            }
            return true;
        }
        return false;
    }
}
