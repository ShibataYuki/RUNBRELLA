using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace ResultScene
{
    public class ResultOnAnimFlgClip : PlayableAsset
    {
        // Factory method that generates a playable based on this asset
        // ゲームオブジェクトを参照するにはこの書き方をする    
        public ExposedReference<GameObject> topChara;
        // クリップの作成
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            // ビヘイビアの作成
            ResultOnAnimFlgBehavior behaviour = new ResultOnAnimFlgBehavior();
            behaviour.topChara = topChara.Resolve(graph.GetResolver());
            // リターンするためのプレイアブルの作成
            ScriptPlayable<ResultOnAnimFlgBehavior> playable = ScriptPlayable<ResultOnAnimFlgBehavior>.Create(graph, behaviour);
            return playable;
        }
    }
}

