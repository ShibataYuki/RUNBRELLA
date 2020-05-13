using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Text.RegularExpressions;

public class MovieCotroller : MonoBehaviour
{
    // 必要なコンポーネント
    private Animator animator;
    private VideoPlayer videoPlayer;
    // アニメーターのID
    private readonly int movieID = Animator.StringToHash("IsPlay");
    // 映像を再生する/再生している/終了処理中かどうか
    public bool IsMoviePlay { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        animator = GetComponent<Animator>();
        videoPlayer = GetComponent<VideoPlayer>();
    }

    /// <summary>
    /// ウィンドウが目的地に移動して、目標のサイズに変更されたかチェック
    /// </summary>
    /// <returns></returns>
    IEnumerator ArriveWindowCheck()
    {
        while(true)
        {
            // 再生しているアニメーションをチェック
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if(clipInfo[0].clip.name != Regex.Replace(name, "MoviePlaying", ""))
            {
                // 動画の再生中でなければ
                if (videoPlayer.isPlaying == false)
                {
                    videoPlayer.Play();
                }
                yield break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 映像を再生
    /// </summary>
    public void MoviePlay()
    {
        // 動画の再生中でなければ
        if(IsMoviePlay == false)
        {
            animator.SetBool(movieID, true);
            StartCoroutine(ArriveWindowCheck());
            IsMoviePlay = true;
        }
    }

    /// <summary>
    /// 映像を停止
    /// </summary>
    public void MovieStop()
    {
        // 動画の再生中なら
        if (videoPlayer.isPlaying == true)
        {
            videoPlayer.Stop();
            animator.SetBool(movieID, false);
            StartCoroutine(WindowReturnCheck());
        }
    }

    /// <summary>
    /// ウィンドウが元の座標に戻って、元のサイズに変更されたかチェック
    /// </summary>
    /// <returns></returns>
    private IEnumerator WindowReturnCheck()
    {
        while(true)
        {
            // 再生しているアニメーションをチェック
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            var name = clipInfo[0].clip.name;
            if (name != "MovieIdle")
            {
                IsMoviePlay = false;
                yield break;
            }
            Debug.Log(clipInfo[0].clip.name);
            yield return null;
        }
    }
}
