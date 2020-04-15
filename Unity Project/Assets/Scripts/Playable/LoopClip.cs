using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class LoopClip : PlayableAsset
{
    // シーン上のオブジェクトを参照するにはこの書き方をするらしい
    public ExposedReference<GameObject> seanObj;
    // Factory method that generates a playable based on this asset
    // クリップの作成
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        // ビヘイビアの作成
        LoopBehaviour behaviour = new LoopBehaviour();
        // ビヘイビアの変数に参照を渡す        
        var timelineController = seanObj.Resolve(graph.GetResolver());
        behaviour.director = timelineController.GetComponent<PlayableDirector>();
        behaviour.timelineController = timelineController.GetComponent<ResultScene.TimelineController>();   
        // リターンするためのプレイアブルの作成
        ScriptPlayable<LoopBehaviour> playable = ScriptPlayable<LoopBehaviour>.Create(graph, behaviour);

        return playable;
    }
}
