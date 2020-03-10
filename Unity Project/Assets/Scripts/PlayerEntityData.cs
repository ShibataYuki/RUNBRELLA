using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityData
{
    // 各プレイヤーのプレイヤーコンポーネント
    public Dictionary<int, Player> players;
    // 各プレイヤーのランコンポーネント
    public Dictionary<int, PlayerRun> playerRuns;
    // 各プレイヤーのジャンプコンポーネント
    public Dictionary<int, PlayerJump> playerJumps;
    // 各プレイヤーの滑空コンポーネント
    public Dictionary<int, PlayerGlide> playerGlides;
    // 各プレイヤーの手すりチェックコンポーネント
    public Dictionary<int, PlayerSlide> playerSlides;
    // 各プレイヤーのショットチェックコンポーネント
    public Dictionary<int, PlayerShot> playerShots;
    // 各プレイヤーが空中状態の場合にスピードをチェックするコンポーネント
    public Dictionary<int, PlayerAerial> playerSpeedChecks;
    // 各プレイヤーのブースト処理のコンポーネント
    public Dictionary<int, PlayerBoost> playerBoosts;
    // 各プレイヤーのダウン処理のコンポーネント
    public Dictionary<int, PlayerDown> playerDowns;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="playerCount"></param>
    public PlayerEntityData(int playerCount)
    {
        // クラスの実体作成
        players = new Dictionary<int, Player>();
        playerRuns = new Dictionary<int, PlayerRun>();
        playerJumps = new Dictionary<int, PlayerJump>();
        playerGlides = new Dictionary<int, PlayerGlide>();
        playerSlides = new Dictionary<int, PlayerSlide>();
        playerShots = new Dictionary<int, PlayerShot>();
        playerSpeedChecks = new Dictionary<int, PlayerAerial>();
        playerBoosts = new Dictionary<int, PlayerBoost>();
        playerDowns = new Dictionary<int, PlayerDown>();

        // 各プレイヤーのコンポーネントの実体格納
        for (int ID = 1; ID <= playerCount; ID++)
        {
            var player = SceneController.Instance.playerObjects[ID].GetComponent<Player>();
            var playerRun = SceneController.Instance.playerObjects[ID].GetComponent<PlayerRun>();
            var playerJump = SceneController.Instance.playerObjects[ID].GetComponent<PlayerJump>();
            var playerGlide = SceneController.Instance.playerObjects[ID].GetComponent<PlayerGlide>();
            var playerSlide = SceneController.Instance.playerObjects[ID].GetComponent<PlayerSlide>();
            var playerShot = SceneController.Instance.playerObjects[ID].GetComponent<PlayerShot>();
            var playerSpeedCheck = SceneController.Instance.playerObjects[ID].GetComponent<PlayerAerial>();
            var playerBoost = SceneController.Instance.playerObjects[ID].GetComponent<PlayerBoost>();
            var playerDown = SceneController.Instance.playerObjects[ID].GetComponent<PlayerDown>();

            players.Add(ID, player);
            playerRuns.Add(ID, playerRun);
            playerJumps.Add(ID, playerJump);
            playerGlides.Add(ID, playerGlide);
            playerSlides.Add(ID, playerSlide);
            playerShots.Add(ID, playerShot);
            playerSpeedChecks.Add(ID, playerSpeedCheck);
            playerBoosts.Add(ID, playerBoost);
            playerDowns.Add(ID, playerDown);
        }

    }

    
    
}
