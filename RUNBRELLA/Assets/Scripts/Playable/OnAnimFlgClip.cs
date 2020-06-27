using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class OnAnimFlgClip : PlayableAsset
{    
    // Factory method that generates a playable based on this asset
    // クリップの作成
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        // ビヘイビアの作成
        OnAnimFlgBehavior behaviour = new OnAnimFlgBehavior();                
        // リターンするためのプレイアブルの作成
        ScriptPlayable<OnAnimFlgBehavior> playable = ScriptPlayable<OnAnimFlgBehavior>.Create(graph, behaviour);
        return playable;
    }
}
