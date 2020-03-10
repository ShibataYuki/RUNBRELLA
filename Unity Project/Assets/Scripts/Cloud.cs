using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    enum Mode
    {
        IDlE,
        INIT,
        MOVE,
        FOLLOW,
        DEATH,
        RELEASE,
    }
    // 雲の移動速度
    [SerializeField]
    List<Player> players = new List<Player>();
    float moveSpeed = 10f;
    Mode mode = Mode.IDlE;
    Color baseColor;
    public ParticleSystem rainDrop;
    SpriteRenderer spriteRenderer; 
    IEnumerator delayChangestate = null;
    IEnumerator death = null;
    float rainRateMax;
    float rainRateMin;
    ParticleSystem.EmissionModule emission;

    // Start is called before the first frame update
    void Start()
    {
        rainDrop = transform.Find("RainDrop").GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
        rainRateMax = rainDrop.emission.rateOverTime.constantMax;
        rainRateMin = rainDrop.emission.rateOverTime.constantMin;
        emission = rainDrop.emission;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void LateUpdate()
    {
        Do();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            players.Add(collision.GetComponent<Player>());
        }
        foreach (Player player in players)
        {
            player.IsRain = true;
        }
    }
   
    /// <summary>
    /// モードによって処理を変える処理
    /// </summary>
    void Do()
    {
        switch(mode)
        {
            case Mode.IDlE:
                {
                    if (InputManager.Instance.CallRainKeyIn())
                    {
                        ChangeMode(Mode.INIT);
                    }
                    break;
                }

            case Mode.INIT:
                {
                    SetCloud();                    
                    break;
                }

            case Mode.MOVE:
                {
                    MoveCloud();
                    break;
                }
            case Mode.FOLLOW:
                {
                    FollowCamera();
                    if(delayChangestate == null)
                    {
                        delayChangestate = DelayChangeMode(3f, Mode.DEATH);
                        StartCoroutine(delayChangestate);
                    }
                    
                    break;
                }
            case Mode.DEATH:
                {
                    FollowCamera();
                    if (death == null)
                    {
                        death = Death();
                        StartCoroutine(Death());
                    }                    
                    break;
                }
            case Mode.RELEASE:
                {
                    Release();
                    break;
                }


        }
    }
    



    /// <summary>
    /// 雨雲を画面の左端に呼び出します
    /// </summary>
    void SetCloud()
    {
        spriteRenderer.color = baseColor;
        rainDrop.Play();
        // スプライトのサイズの半分（offSet用）
        float spriteHurfWhith = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float spriteHurfHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        // オフセット
        Vector3 offSet = new Vector3(spriteHurfWhith, spriteHurfHeight, 0);
        rainDrop.Play();
        // 位置の代入
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0,1,1)) - offSet;
        ChangeMode(Mode.MOVE);
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(rainRateMin, rainRateMax);
    }


    void ChangeMode(Mode mode)
    {
        this.mode = mode;
    }

    /// <summary>
    /// 雲が移動する処理です
    /// </summary>
    void MoveCloud()
    {
        
        // 右方向への移動
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // カメラの中心のｘ座標
        float CameraCenterPosX = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.8f, 1f)).x;
        // 雲の中心のｘ座標
        float cloudPosX = transform.position.x;

        // カメラの中央に当たちしたら追従モードに
        if(cloudPosX >= CameraCenterPosX)
        {
            ChangeMode(Mode.FOLLOW);
        }
    }

    /// <summary>
    /// カメラの座標を追従します
    /// </summary>
    void FollowCamera()
    {
        // スプライトのサイズの半分（offSet用）        
        float spriteHurfHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        // オフセット
        Vector3 offSet = new Vector3(0, spriteHurfHeight, 0);

        // 位置の代入
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 1f)) - offSet;
    }


    IEnumerator Death()
    {
        float workRainRateMax = rainRateMax;
        float workRainRateMin = rainRateMin;

        IEnumerator changeColor = null;

        while(true)
        {
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(workRainRateMin -= 1, workRainRateMax -= 1); 
            if(workRainRateMax <= 10)
            {
               if(changeColor == null)
               {
                    changeColor = ChangeColor();
                    StartCoroutine(changeColor);
               }
            }
            if(workRainRateMax <= 0 )
            {
                break;
            }
            yield return new WaitForSeconds(0.4f);

        }


        ChangeMode(Mode.RELEASE);
        death = null;
        yield break;
        
    }

    IEnumerator ChangeColor()
    {
        while(true)
        {

            spriteRenderer.color += new Color(0.5f / 255f, 0.5f / 255f, 0.5f / 255f, 0);
            Debug.Log(spriteRenderer.color);
            if(spriteRenderer.color.r >= 1f)
            {
                spriteRenderer.color -= new Color(0f, 0f, 0f, 0.7f/255f);                                
            }

            if(spriteRenderer.color.a <= 0)
            {
                break;
            }

            yield return null;
        }

        // スプライトのサイズの半分（offSet用）
        float spriteHurfWhith = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float spriteHurfHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        // オフセット
        Vector3 offSet = new Vector3(spriteHurfWhith, spriteHurfHeight, 0);

        // 位置の代入
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 1)) - offSet;
        ChangeMode(Mode.IDlE);
        yield break;
    }


    IEnumerator DelayChangeMode(float delay, Mode mode)
    {
        yield return new WaitForSeconds(delay);
        ChangeMode(mode);
        delayChangestate = null;
        yield break;
    }

    void Release()
    {
        
        foreach (Player player in players)
        {
            player.IsRain = false;
        }

        players.RemoveRange(0,players.Count-1);
        
        
        
    }

}
