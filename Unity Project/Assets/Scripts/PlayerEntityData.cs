using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityData
{
    // 各プレイヤーのランコンポーネント
    public Dictionary<int, PlayerRun> playerRuns;
    // 各プレイヤーのジャンプコンポーネント
    public Dictionary<int, PlayerJump> playerJumps;
    // 各プレイヤーのプレイヤーコンポーネント
    public Dictionary<int, Player> players;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="playerCount"></param>
    public PlayerEntityData(int playerCount)
    {
        // クラスの実体作成
        playerRuns = new Dictionary<int, PlayerRun>();
        playerJumps = new Dictionary<int, PlayerJump>();
        players = new Dictionary<int, Player>();

        // 各プレイヤーのコンポーネントの実体格納
        for (int ID = 1; ID <= playerCount; ID++)
        {
            var playerRun = SceneManager.Instance.playerObjects[ID].GetComponent<PlayerRun>();
            var playerJump = SceneManager.Instance.playerObjects[ID].GetComponent<PlayerJump>();
            var player = SceneManager.Instance.playerObjects[ID].GetComponent<Player>();

            playerRuns.Add(ID, playerRun);
            playerJumps.Add(ID, playerJump);
            players.Add(ID, player);

        }

    }

    
    
}
