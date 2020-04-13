using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class PlayerImage : MonoBehaviour
    {
        private PlayerImageState state;
        public PlayerImageState State { get { return state; } }

        // 必要なコンポーネント
        private Animator animator;
        private RectTransform rectTransform;
        // get
        public Animator _animator { get { return animator; } }
        public RectTransform _rectTransform { get { return rectTransform; } }
        // 管理するマネージャー
        private PlayerImageManager playerImageManager;
        public PlayerImageManager _playerImageManager { get { return playerImageManager; } }

        private readonly int boostID = Animator.StringToHash("IsBoost");

        // Start is called before the first frame update
        void Start()
        {
            // コンポーネントの取得
            animator = GetComponent<Animator>();
            rectTransform = GetComponent<RectTransform>();
            // 別オブジェクトのコンポーネントを取得
            playerImageManager = SceneController.Instance.gameObject.GetComponent<PlayerImageManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (state != null)
            {
                // フレーム更新処理
                state.Do(this);
            }
        }

        /// <summary>
        /// ステートの変更
        /// </summary>
        /// <param name="state">変更後のステート</param>
        public void ChangeState(PlayerImageState state)
        {
            // ステートの変更
            this.state = state;
            if (state != null)
            {
                // ステートの開始処理を行う
                state.Entry(this);
                // アニメーターにパラメータをセット
                animator.SetBool(boostID, (state == playerImageManager.BoostState));
            }
        }
    }
}