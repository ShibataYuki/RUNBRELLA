using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityData
{
    // 各プレイヤーのプレイヤーコンポーネント
    public Dictionary<CONTROLLER_NO, Player> players;
    // 各プレイヤーのランコンポーネント
    public Dictionary<CONTROLLER_NO, PlayerRun> playerRuns;
    // 各プレイヤーのジャンプコンポーネント
    public Dictionary<CONTROLLER_NO, PlayerJump> playerJumps;
    // 各プレイヤーの滑空コンポーネント
    public Dictionary<CONTROLLER_NO, PlayerGlide> playerGlides;
    // 各プレイヤーの手すりチェックコンポーネント
    public Dictionary<CONTROLLER_NO, PlayerSlide> playerSlides;
    // 各プレイヤーのアタック処理コンポーネント
    public Dictionary<CONTROLLER_NO, PlayerAttack> playerAttacks;
    // 各プレイヤーが空中状態の場合にスピードをチェックするコンポーネント
    public Dictionary<CONTROLLER_NO, PlayerAerial> playerAerial;
    // 各プレイヤーのブースト処理のコンポーネント
    public Dictionary<CONTROLLER_NO, PlayerBoost> playerBoosts;
    // 各プレイヤーのダウン処理のコンポーネント
    public Dictionary<CONTROLLER_NO, PlayerDown> playerDowns;
    public Dictionary<CONTROLLER_NO, HitChecker> playerHitCheckers;
    // チャージ関連のコンポーネント
    public Dictionary<CONTROLLER_NO, PlayerCharge> playerCharges;
    // 各プレイヤーの手すり後のジャンプ受付時間用コンポーネント
    public Dictionary<CONTROLLER_NO, PlayerAfterSlide> playerAfterSlides;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="playerCount"></param>
    public PlayerEntityData(int playerCount)
    {
        // クラスの実体作成
        players = new Dictionary<CONTROLLER_NO, Player>();
        playerRuns = new Dictionary<CONTROLLER_NO, PlayerRun>();
        playerJumps = new Dictionary<CONTROLLER_NO, PlayerJump>();
        playerGlides = new Dictionary<CONTROLLER_NO, PlayerGlide>();
        playerSlides = new Dictionary<CONTROLLER_NO, PlayerSlide>();
        playerAttacks = new Dictionary<CONTROLLER_NO, PlayerAttack>();
        playerAerial = new Dictionary<CONTROLLER_NO, PlayerAerial>();
        playerBoosts = new Dictionary<CONTROLLER_NO, PlayerBoost>();
        playerDowns = new Dictionary<CONTROLLER_NO, PlayerDown>();
        playerHitCheckers = new Dictionary<CONTROLLER_NO, HitChecker>();
        playerCharges = new Dictionary<CONTROLLER_NO, PlayerCharge>();
        playerAfterSlides = new Dictionary<CONTROLLER_NO, PlayerAfterSlide>();
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
            var controllerNo = playerObjectKey.Key;
            players.Add(controllerNo, player);
            playerRuns.Add(controllerNo, playerRun);
            playerJumps.Add(controllerNo, playerJump);
            playerGlides.Add(controllerNo, playerGlide);
            playerSlides.Add(controllerNo, playerSlide);
            playerAerial.Add(controllerNo, playerSpeedCheck);
            playerDowns.Add(controllerNo, playerDown);
            playerHitCheckers.Add(controllerNo, playerHitChecker);
            playerAttacks.Add(controllerNo, playerAttack);
            playerCharges.Add(controllerNo, playerCharge);
            playerAfterSlides.Add(controllerNo, playerAfterSlide);

            // 攻撃手段によって追加するコンポーネントを変更する
            if (playerObjectKey.Value.GetComponent<Player>().charAttackType
                ==GameManager.CHARATTACKTYPE.GUN)
            {
                var playerBoost = playerObjectKey.Value.GetComponent<PlayerBoost>();
                playerBoosts.Add(controllerNo, playerBoost);
            }
            if(playerObjectKey.Value.GetComponent<Player>().charAttackType 
                == GameManager.CHARATTACKTYPE.SWORD)
            {
                var playerBoost = playerObjectKey.Value.GetComponent<PlayerBoost>();
                playerBoosts.Add(controllerNo, playerBoost);
            }

        }

    }

    
    
}
