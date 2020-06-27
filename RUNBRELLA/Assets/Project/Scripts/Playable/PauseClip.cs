using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using GamepadInput;

public class PauseClip : PlayableAsset
{
    // ゲームオブジェクトを参照するにはこの書き方をする    
    public ExposedReference<PlayableDirector> director;

    // Factory method that generates a playable based on this asset
    // クリップの作成
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        // ビヘイビアの作成
        PauseBehavior behaviour = new PauseBehavior();
        behaviour.Director = director.Resolve(graph.GetResolver());
        // リターンするためのプレイアブルの作成
        ScriptPlayable<PauseBehavior> playable = ScriptPlayable<PauseBehavior>.Create(graph, behaviour);

        return playable;
    }
}



