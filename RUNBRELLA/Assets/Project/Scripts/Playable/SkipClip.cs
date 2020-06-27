using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SkipClip : PlayableAsset
{
    // ゲームオブジェクトを参照するにはこの書き方をする
    public ExposedReference<PlayableDirector> director;    

    // Factory method that generates a playable based on this asset
    // クリップの作成
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        // ビヘイビアの作成
        SkipBehaviour behaviour = new SkipBehaviour();       
        // GameObjectを取り出す
        var directorObj = director.Resolve(graph.GetResolver());        
        // ビヘイビアの変数に参照を渡す  
        behaviour.Director = directorObj.GetComponent<PlayableDirector>();        
        // リターンするためのプレイアブルの作成
        ScriptPlayable<SkipBehaviour> playable = ScriptPlayable<SkipBehaviour>.Create(graph, behaviour);

        return playable;
    }
}
