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
    public Dictionary<int, PlayerAerial> playerAerial;
    // 各プレイヤーのブースト処理のコンポーネント
    public Dictionary<int, PlayerBoost> playerBoosts;
    // 各プレイヤーのダウン処理のコンポーネント
    public Dictionary<int, PlayerDown> playerDowns;
    public Dictionary<int, HitChecker> playerHitCheckers;

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
        playerAerial = new Dictionary<int, PlayerAerial>();
        playerBoosts = new Dictionary<int, PlayerBoost>();
        playerDowns = new Dictionary<int, PlayerDown>();
        playerHitCheckers = new Dictionary<int, HitChecker>();

        // 各プレイヤーのコンポーネントの実体格納
        for (int ID = 1; ID <= playerCount; ID++)
        {
            var player = SceneController.Instance.playerObjects[ID].GetComponent<Player>();
            var playerRun = SceneController.Instance.playerObjects[ID].GetComponent<PlayerRun>();
            var playerJump = SceneController.Instance.playerObjects[ID].GetComponent<PlayerJump>();
            var playerGlide = SceneController.Instance.playerObjects[ID].GetComponent<PlayerGlide>();
            var playerSlide = SceneController.Instance.playerObjects[ID].GetComponent<PlayerSlide>();
            var playerSpeedCheck = SceneController.Instance.playerObjects[ID].GetComponent<PlayerAerial>();
            var playerDown = SceneController.Instance.playerObjects[ID].GetComponent<PlayerDown>();
            var playerHitChecker = SceneController.Instance.playerObjects[ID].GetComponent<HitChecker>();

            players.Add(ID, player);
            playerRuns.Add(ID, playerRun);
            playerJumps.Add(ID, playerJump);
            playerGlides.Add(ID, playerGlide);
            playerSlides.Add(ID, playerSlide);
            playerAerial.Add(ID, playerSpeedCheck);
            playerDowns.Add(ID, playerDown);
            playerHitCheckers.Add(ID, playerHitChecker);

            // 攻撃手段によって追加するコンポーネントを変更する
            if(GameManager.Instance.charAttackType[ID-1]==GameManager.CHARATTACKTYPE.GUN)
            {
                var playerShot = SceneController.Instance.playerObjects[ID].GetComponent<PlayerShot>();
                var playerBoost = SceneController.Instance.playerObjects[ID].GetComponent<PlayerBoost>();
                playerShots.Add(ID, playerShot);
                playerBoosts.Add(ID, playerBoost);
            }
            if(GameManager.Instance.charAttackType[ID-1] == GameManager.CHARATTACKTYPE.SORD)
            {

            }

        }

    }

    
    
}
