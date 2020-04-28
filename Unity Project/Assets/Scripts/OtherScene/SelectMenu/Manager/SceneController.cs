using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelectMenu
{
    public class SceneController : MonoBehaviour
    {
        #region シングルトンインスタンス

        // インスタンスなアクセスポイント
        private static SceneController instance = null;
        public static SceneController Instance { get { return instance; } }

        public void Awake()
        {
            // インスタンスが出来てなければ
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                // このコンポーネントがついたオブジェクトを消去する
                Destroy(this.gameObject);
            }
        }
        #endregion

		// 	現在のステートを表す変数
        private SelectMenuState state = null;
        public SelectMenuState _state { get { return state; } }
# if UNITY_EDITOR
        [SerializeField]
        private string stateName;
#endif
        #region ステート変数
        private SelectCharacterState selectCharacterState = null;
        private AgreeCheckState agreeCheckState = null;
        private SelectMenuEndState selectMenuEndState = null;
        // get
        public SelectCharacterState _selectCharacterState { get { return selectCharacterState; } }
        public AgreeCheckState _agreeCheckState { get { return agreeCheckState; } }
        public SelectMenuEndState _selectMenuEndState { get { return selectMenuEndState; } }
        #endregion
        #region キーボード入力用のフラグ
        private bool isKeyBoard;
        public bool IsKeyBoard { get { return isKeyBoard; } set { isKeyBoard = value; } }
        #endregion
        // キャラクター情報
        [System.Serializable]
        public struct CharacterMessageData
        {
            public GameManager.CHARTYPE charaType; // キャラクターの種類
            public GameManager.CHARATTACKTYPE charaAttackType; // キャラクターの攻撃方法
        }

        // 各キャラクターのキャラクター情報
        [SerializeField]
        private CharacterMessageData[] characterMessages = new CharacterMessageData[4];
        public CharacterMessageData[] _characterMessages { get { return characterMessages; } }

        // 決定時の音のクリップ
        [SerializeField]
        private AudioClip enterClip = null;
        // 選択時の音のクリップ
        [SerializeField]
        private AudioClip choiceClip = null;

        // 参加しているかどうか
        Dictionary<int, bool> isAccess = new Dictionary<int, bool>();
        // 決定したかどうかのフラグ
        Dictionary<int, bool> isSubmits = new Dictionary<int, bool>();

        // プレイヤー人数の上限
        public readonly int MaxPlayerNumber = 4;

        // プレイ人数
        private int playerNumber = 0;
        public int PlayerNumber { get { return playerNumber; } set { playerNumber = value; } }
        // 新たな参加者がいないかチェックするコンポーネントの参照
        private PlayerEntry playerEntry;
        // 何本先取か決めるコンポーネントの参照
        private SelectPlayCount selectPlayCount;
        // キャラ選択画面のマネージャー
        private SelectCharacterManager selectCharacterManager;
        // get set
        public Dictionary<int, bool> IsSubmits { get { return isSubmits; } set { isSubmits = value; } }
        public Dictionary<int, bool> IsAccess { get { return isAccess; } set { isAccess = value; } }
        public SelectCharacterManager _selectCharacterManager { get { return selectCharacterManager; } }
        public SelectPlayCount _selectPlayCount { get { return selectPlayCount; } }
        // プレイヤーの画像のマネージャー
        private PlayerImageManager imageManager;

        private void Start()
        {
            // GameManagerの初期化
            InitGameManager();
            // コンポーネントの取得
            selectPlayCount = GetComponent<SelectPlayCount>();
            playerEntry = GetComponent<PlayerEntry>();
            selectCharacterManager = GetComponent<SelectCharacterManager>();
            imageManager = GetComponent<PlayerImageManager>();
            // ステートのセット
            selectCharacterState = new SelectCharacterState(selectCharacterManager);
            agreeCheckState = new AgreeCheckState(GetComponent<AgreeCheck>());
            selectMenuEndState = new SelectMenuEndState(imageManager, GetComponent<InputManager>());
            // ステートの変更
            ChangeState(selectCharacterState);

            for (int ID = 1; ID <= MaxPlayerNumber; ID++)
            {
                // 参加していない状態に変更する
                isAccess.Add(ID, false);
            }
        }

        /// <summary>
        /// GameManagerの初期化
        /// </summary>
        private void InitGameManager()
        {
            // レース数に関連するものをリセットする
            GameManager.Instance.nowRaceNumber = 0;
            // プレイヤーの人数をリセットする
            GameManager.Instance.playerNumber = 0;
            // キャラクター情報を削除する
            GameManager.Instance.charType.Clear();
            GameManager.Instance.charAttackType.Clear();
            GameManager.Instance.playerIDs.Clear();
        }

        private void Update()
        {
            #region キーボード入力用のフラグ
            isKeyBoard = false;
            #endregion
            if (state != null)
            {
                state.Do();
            }
            // 新たな参加者がいないかチェックする
            playerEntry.EntryCheck();
#if UNITY_EDITOR
            stateName = state.ToString();
                #endif
        }

        /// <summary>
        /// ステートを変更する
        /// </summary>
        /// <param name="newState">変更後のステート</param>
        public void ChangeState(SelectMenuState newState)
        {
            if(state != null && newState != null)
            {
                // ステート終了時の処理を行う
                state.Exit();
                // ステート開始時の処理を行う
                newState.Entry();

            }
            // 新しいステートに変更
            state = newState;
        }

        /// <summary>
        /// キャラクター選択画面に戻る処理
        /// </summary>
        /// <param name="ID"></param>
        public void Cancel(int ID)
        {
            // キャラクター選択が完了していたら
            if(isSubmits[ID] == true)
            {
                // キー説明用UIを表示して、色を付けなおす
                selectCharacterManager.SelectCharacters[ID].Cansel();
                // キャラ選択画面に戻る
                isSubmits[ID] = false;
                // プレイヤーの画像を画面外に移動させる
                imageManager.PlayerImageCansel(ID);
            }
            // ステートの変更
            ChangeState(selectCharacterState);
        }

        /// <summary>
        /// ゲーム開始処理
        /// </summary>
        public void GameStart()
        {
            foreach (var ID in GameManager.Instance.playerIDs)
            {
                // キャラクターのタイプをGameManagerにセット
                GameManager.Instance.charType.Add
                    (characterMessages
                [selectCharacterManager.SelectCharacters[ID].SelectCharacterNumber].charaType);
                // キャラクターの攻撃方法をGameManagerにセット
                GameManager.Instance.charAttackType.Add
                    (characterMessages
                [selectCharacterManager.SelectCharacters[ID].SelectCharacterNumber].charaAttackType);
            }
            // プレイヤーの人数をセット
            GameManager.Instance.playerNumber = playerNumber;
            // 何本先取かをGameManagerにセット
            GameManager.Instance.RaceNumber = selectPlayCount.RaceNumber;
            // ゲーム開始時の初期化処理
            for (int i = 0; i < playerNumber; i++)
            {
                var index = Random.Range(0, GameManager.Instance.playerRanks.Count + 1);
                GameManager.Instance.playerRanks.Insert(index, (i + 1));
                GameManager.Instance.playerWins.Add(0);
            }
            // Stageに遷移

            foreach(var rank in GameManager.Instance.playerRanks)
            {
                Debug.Log(rank);
            }
            SceneManager.LoadScene("Stage");
        } // GameStart

        /// <summary>
        /// 選択時のSE再生
        /// </summary>
        public void PlayChoiseSE()
        {
            AudioManager.Instance.PlaySE(choiceClip, 0.25f);
        }

        /// <summary>
        /// 決定時のSE再生
        /// </summary>
        public void PlayEnterSE()
        {
            AudioManager.Instance.PlaySE(enterClip, 0.25f);
        }

    } // Class
} // namespace
