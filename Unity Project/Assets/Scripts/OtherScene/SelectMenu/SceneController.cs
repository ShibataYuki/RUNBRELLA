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

        // 処理内容を表すステート
        public enum State
        {
            SelectCharacter,
            SelectPlayCount,
            SelectSubmitCheck,
        }
		// 	現在のステートを表す変数
        private State state = State.SelectCharacter;
        public State _state { get { return state; } set { state = value; } }

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

        private void Start()
        {
            // GameManagerの初期化
            InitGameManager();
            // コンポーネントの取得
            selectPlayCount = GetComponent<SelectPlayCount>();
            playerEntry = GetComponent<PlayerEntry>();
            selectCharacterManager = GetComponent<SelectCharacterManager>();
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
            GameManager.Instance.RaceNumber = 0;
            // プレイヤーの人数をリセットする
            GameManager.Instance.playerNumber = 0;
            // キャラクター情報を削除する
            GameManager.Instance.charType.Clear();
            GameManager.Instance.charAttackType.Clear();
            GameManager.Instance.playerIDs.Clear();
        }

        private void Update()
        {
            // ステートに応じたフレーム更新処理を行う
            switch(state)
            {
                // キャラ選択画面
                case State.SelectCharacter:
                    selectCharacterManager.SelectCharacter();
                    break;
                // 何本先取か決める  
                case State.SelectPlayCount:
                // 決定していいかチェック   
                case State.SelectSubmitCheck:
                    selectPlayCount.SelectPlayCountEntry();
                    break;
            }

            // 新たな参加者がいないかチェックする
            playerEntry.EntryCheck();
        }


        /// <summary>
        /// キャラクター選択画面に戻る処理
        /// </summary>
        /// <param name="ID"></param>
        public void Cancel(int ID)
        {
            // キー説明用UIを表示して、色を付けなおす
            selectCharacterManager.SelectCharacters[ID].Cansel();
            // キャラ選択画面に戻る
            isSubmits[ID] = false;
            // キーフラグの消去
            selectPlayCount.KeyFlag.Remove(ID);
            // ステートの変更
            state = State.SelectCharacter;
            // テキストの非表示
            selectPlayCount.PlayCountHide();
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
            // Stageに遷移
            SceneManager.LoadScene("Stage");
        } // GameStart

    } // Class
} // namespace
