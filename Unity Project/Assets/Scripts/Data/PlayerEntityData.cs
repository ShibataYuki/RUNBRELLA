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
    // 各プレイヤーのアタック処理コンポーネント
    public Dictionary<int, PlayerAttack> playerAttacks;
    // 各プレイヤーが空中状態の場合にスピードをチェックするコンポーネント
    public Dictionary<int, PlayerAerial> playerAerial;
    // 各プレイヤーのブースト処理のコンポーネント
    public Dictionary<int, PlayerBoost> playerBoosts;
    // 各プレイヤーのダウン処理のコンポーネント
    public Dictionary<int, PlayerDown> playerDowns;
    public Dictionary<int, HitChecker> playerHitCheckers;
    // チャージ関連のコンポーネント
    public Dictionary<int, PlayerCharge> playerCharges;
    // 各プレイヤーの手すり後のジャンプ受付時間用コンポーネント
    public Dictionary<int, PlayerAfterSlide> playerAfterSlides;

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
        playerAttacks = new Dictionary<int, PlayerAttack>();
        playerAerial = new Dictionary<int, PlayerAerial>();
        playerBoosts = new Dictionary<int, PlayerBoost>();
        playerDowns = new Dictionary<int, PlayerDown>();
        playerHitCheckers = new Dictionary<int, HitChecker>();
        playerCharges = new Dictionary<int, PlayerCharge>();
        playerAfterSlides = new Dictionary<int, PlayerAfterSlide>();
        // 各プレイヤーのコンポーネントの実体格納
        foreach (var playerObjectKey in SceneController.Instance.playerObjects)
        {
            var player = playerObjectKey.Value.GetComponent<Player>();
            var playerRun = playerObjectKey.Value.GetComponent<PlayerRun>();
            var playerJump = playerObjectKey.Value.GetComponent<PlayerJump>();
            var playerGlide = playerObjectKey.Value.GetComponent<PlayerGlide>();
            var playerSlide = playerObjectKey.Value.GetComponent<PlayerSlide>();
            var playerSpeedCheck = playerObjectKey.Value.GetComponent<PlayerAerial>();
            var playerDown = playerObjectKey.Value.GetComponent<PlayerDown>();
            var playerHitChecker = playerObjectKey.Value.GetComponent<HitChecker>();
            var playerAttack = playerObjectKey.Value.GetComponent<PlayerAttack>();
            var playerCharge = playerObjectKey.Value.GetComponent<PlayerCharge>();
            var playerAfterSlide = playerObjectKey.Value.GetComponent<PlayerAfterSlide>();
            var ID = playerObjectKey.Key;
            players.Add(ID, player);
            playerRuns.Add(ID, playerRun);
            playerJumps.Add(ID, playerJump);
            playerGlides.Add(ID, playerGlide);
            playerSlides.Add(ID, playerSlide);
            playerAerial.Add(ID, playerSpeedCheck);
            playerDowns.Add(ID, playerDown);
            playerHitCheckers.Add(ID, playerHitChecker);
            playerAttacks.Add(ID, playerAttack);
            playerCharges.Add(ID, playerCharge);
            playerAfterSlides.Add(ID, playerAfterSlide);

            // 攻撃手段によって追加するコンポーネントを変更する
            if (playerObjectKey.Value.GetComponent<Player>().charAttackType
                ==GameManager.CHARATTACKTYPE.GUN)
            {
                var playerBoost = playerObjectKey.Value.GetComponent<PlayerBoost>();
                playerBoosts.Add(ID, playerBoost);
            }
            if(playerObjectKey.Value.GetComponent<Player>().charAttackType 
                == GameManager.CHARATTACKTYPE.SWORD)
            {
                var playerBoost = playerObjectKey.Value.GetComponent<PlayerBoost>();
                playerBoosts.Add(ID, playerBoost);
            }

        }

    }

    
    
}
