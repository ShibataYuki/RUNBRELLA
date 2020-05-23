using ResultScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityData
{
    // 各プレイヤーのプレイヤーコンポーネント
    public Dictionary<PLAYER_NO, Character> players;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="playerCount"></param>
    public PlayerEntityData(int playerCount)
    {
        // クラスの実体作成
        players = new Dictionary<PLAYER_NO, Character>();
        // 各プレイヤーのコンポーネントの実体格納
        foreach (var playerObjectKey in SceneController.Instance.playerObjects.Values)
        {
            var character = playerObjectKey.GetComponent<Character>();
            var playerNo = character.playerNO;
            players.Add(playerNo, character);
        }

    }

    
    
}
