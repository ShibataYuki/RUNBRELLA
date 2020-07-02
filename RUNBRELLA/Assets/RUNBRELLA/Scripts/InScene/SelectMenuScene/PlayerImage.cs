using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public class PlayerImage : MonoBehaviour
    {
        // 現在のステート
        private PlayerImageState state;
        // ステート変数のコンポーネント
        private PlayerImageIdleState idleState;
        private PlayerImageRunState runState;
        private PlayerImageBoostState boostState;
        private PlayerImageGoalState goalState;
        // 待機状態かどうか
        public bool IsIdle { get { return (state == idleState);  } }
        // 画面外にのステートに変更されたかどうか
        public bool IsGoal { get { return (state == goalState);  } }
        // アニメーションの切り替えに必要なコンポーネント
        public Animator _animator { get; private set; }
        // 外部から参照するコンポーネント
        public SpriteRenderer _spriteRenderer { get; private set; }
        // エフェクト
        private ParticleSystem playerParticleSystem;
        public ParticleSystem _particleSystem { get { return playerParticleSystem; } }
        // メインカメラに写っているかどうか
        private bool isScreen = false;
        public bool IsScreen { get { return isScreen; } }

        // アニメーターのパラメータID
        private readonly int boostID = Animator.StringToHash("IsBoost");

        private void Awake()
        {
            // ステート変数のコンポーネントを取得
            idleState = GetComponent<PlayerImageIdleState>();
            runState = GetComponent<PlayerImageRunState>();
            boostState = GetComponent<PlayerImageBoostState>();
            goalState = GetComponent<PlayerImageGoalState>();
            // コンポーネントの取得
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // エフェクトの参照
            var particalObject = transform.Find("Particle System").gameObject;
            playerParticleSystem = particalObject.GetComponent<ParticleSystem>();
            // シートの読み込みが終わり次第もう一回パラメータをセットしなおす
            StartCoroutine(RoadSheetCheck());
        }

        /// <summary>
        /// シートの読み込みをチェックして、完了したらパラメータを変更する
        /// </summary>
        /// <returns></returns>
        IEnumerator RoadSheetCheck()
        {
            // シートからの読み込みが完了しているのなら
            if (SheetToDictionary.Instance.IsCompletedSheetToText == true)
            {
                // コルーチンを終了
                yield break;
            }
            while (true)
            {
                // スプレッドシートの読み込みが完了したのなら
                if (SheetToDictionary.Instance.IsCompletedSheetToText == true)
                {
                    // パラメータをテキストから読み込んで、speedを変更
                    boostState.SetSpeed();
                    runState.SetSpeed();
                    yield break;
                }
                // 1フレーム待機する
                yield return null;
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (state != null)
            {
                // 現在のステートのフレーム更新処理
                state.Do();
            }
            // 次のフレームまでにメインカメラに写っているかチェックするため、フラグをOFFにする
            isScreen = false;
        }

        /// <summary>
        /// カメラに写っているなら行う
        /// </summary>
        private void OnWillRenderObject()
        {
            // メインカメラに写っているなら
            if(Camera.current == Camera.main)
            {
                // フラグをONにする
                isScreen = true;
            }
        }
        #region ステートの変更
        /// <summary>
        /// ステートの変更
        /// </summary>
        /// <param name="state">変更後のステート</param>
        private void ChangeState(PlayerImageState state)
        {
            // ステートの変更
            this.state = state;
            if (state != null)
            {
                // ステートの開始処理を行う
                state.Entry();
                // アニメーターにパラメータをセット
                _animator.SetBool(boostID, (state == boostState));
            } // if
        } // ChangeState

        /// <summary>
        /// 待機状態のステートに変更
        /// </summary>
        public void IdleStart()
        {
            ChangeState(idleState);
        } // IdleState
        
        /// <summary>
        /// 走るステートに変更
        /// </summary>
        public void RunStart()
        {
            ChangeState(runState);
        } // RunState

        /// <summary>
        /// ブーストのステートに変更
        /// </summary>
        public void BoostStart()
        {
            ChangeState(boostState);
        } // BoostState

        /// <summary>
        /// 画面外のステートに変更
        /// </summary>
        public void Goal()
        {
            ChangeState(goalState);
        } // Goal
        #endregion
    }
}