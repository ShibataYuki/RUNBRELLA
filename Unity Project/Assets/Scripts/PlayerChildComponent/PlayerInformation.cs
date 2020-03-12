using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    [SerializeField]
    private TextMesh playerTextMesh = null;
    [SerializeField]
    private PlayerShot playerShot = null;
    // プレイヤーのID
    public int playerID = 0;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        playerTextMesh.text = "Player" + playerID.ToString() + "\n残弾数" + playerShot.nowBulletCount.ToString();
    }
}
