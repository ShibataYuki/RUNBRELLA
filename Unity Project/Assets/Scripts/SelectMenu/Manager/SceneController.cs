using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;

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
        #endregion
        // 了承するステートかどうか
        public bool IsAgreeCheck { get { return (state == agreeCheckState); } }
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
            public AudioClip audioClip; // キャラクターの選択時ボイス
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
        Dictionary<CONTROLLER_NO, bool> isAccess = new Dictionary<CONTROLLER_NO, bool>();
        // 決定したかどうかのフラグ
        Dictionary<CONTROLLER_NO, bool> isSubmits = new Dictionary<CONTROLLER_NO, bool>();

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
        public Dictionary<CONTROLLER_NO, bool> IsSubmits { get { return isSubmits; } set { isSubmits = value; } }
        public Dictionary<CONTROLLER_NO, bool> IsAccess { get { return isAccess; } set { isAccess = value; } }
        public SelectCharacterManager _selectCharacterManager { get { return selectCharacterManager; } }
        public SelectPlayCount _selectPlayCount { get { return selectPlayCount; } }
        // プレイヤーの画像のマネージャー
        private PlayerImageManager imageManager;
        public readonly string textName = "CharaSelect";
        // 入力をチェックするマネージャー
        InputManager inputManager;
        // 何秒長押ししたらタイトルに戻るか
        [SerializeField]
        private float holdTimeForReturnTitle = 1.5f;

        private void Start()
        {
            // GameManagerの初期化
            InitGameManager();
            // コンポーネントの取得
            selectPlayCount = GetComponent<SelectPlayCount>();
            playerEntry = GetComponent<PlayerEntry>();
            selectCharacterManager = GetComponent<SelectCharacterManager>();
            imageManager = GetComponent<PlayerImageManager>();
            inputManager = GetComponent<InputManager>();
            // ステートのセット
            selectCharacterState = GetComponent<SelectCharacterState>();
            agreeCheckState = GetComponent<AgreeCheckState>();
            selectMenuEndState = GetComponent<SelectMenuEndState>();
            // ステートの変更
            ChangeState(selectCharacterState);

            for (var controllerNo = CONTROLLER_NO.CONTROLLER1; controllerNo <= CONTROLLER_NO.CONTROLLER4; controllerNo++)
            {
                // 参加していない状態に変更する
                isAccess.Add(controllerNo, false);
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
            GameManager.Instance.playerAndControllerDictionary.Clear();
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
            // タイトルに戻るかチェック
            ReturnCheck();

            // 新たな参加者がいないかチェックする
            playerEntry.EntryCheck();
#if UNITY_EDITOR
            stateName = state.ToString();
#endif
        }

        /// <summary>
        /// タイトルに戻るかチェック
        /// </summary>
        private void ReturnCheck()
        {
            // Bボタンを長押ししているかチェック
            for (var controllerNo = CONTROLLER_NO.CONTROLLER1; controllerNo <= CONTROLLER_NO.CONTROLLER4; controllerNo++)
            {
                // 長押ししている時間をチェック
                if (inputManager.keyHoldTimeDictionary[controllerNo] > holdTimeForReturnTitle)
                {
                    // タイトルに遷移  
                    SceneManager.LoadScene("Title");
                }
            } // Bボタンを長押ししているかチェック
        }
        #region ステートの変更
        /// <summary>
        /// ステートを変更する
        /// </summary>
        /// <param name="newState">変更後のステート</param>
        private void ChangeState(SelectMenuState newState)
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
        /// キャラクター選択画面に戻る
        /// </summary>
        public void ReturnToCharaSelect()
        {
            ChangeState(selectCharacterState);
        }

        /// <summary>
        /// 了承したかチェックするステートに変更
        /// </summary>
        public void AgreeCheckStart()
        {
            ChangeState(agreeCheckState);
        }

        /// <summary>
        /// 入力後の演出のステートに変更
        /// </summary>
        public void EndStateStart()
        {
            ChangeState(selectMenuEndState);
        }
        #endregion
        /// <summary>
        /// キャラクター選択画面に戻る処理
        /// </summary>
        /// <param name="controllerNo"></param>
        public void Cancel(CONTROLLER_NO controllerNo)
        {
            // キャラクター選択が完了していたら
            if(isSubmits[controllerNo] == true)
            {
                // キー説明用UIを表示して、色を付けなおす
                selectCharacterManager.SelectCharacters[controllerNo].Cansel();
                // キャラ選択画面に戻る
                isSubmits[controllerNo] = false;
                // プレイヤーの画像を画面外に移動させる
                imageManager.PlayerImageCansel(controllerNo);
            }
            // ステートの変更
            ChangeState(selectCharacterState);
        }

        /// <summary>
        /// ゲーム開始処理
        /// </summary>
        public void GameStart()
        {
            for (var playerNo = PLAYER_NO.PLAYER1; playerNo < (PLAYER_NO)playerNumber; playerNo++)
            {
                // プレイヤーのコントローラー番号
                var controllerNo = GameManager.Instance.PlayerNoToControllerNo(playerNo);
                // キャラクター選択
                var selectCharacter = selectCharacterManager.SelectCharacters[controllerNo];
                // プレイヤーが選んだキャラクター
                var characterMessage = characterMessages[selectCharacter.SelectCharacterNumber];
                // キャラクターのタイプをGameManagerにセット
                GameManager.Instance.charType.Add(characterMessage.charaType);
                // キャラクターの攻撃方法をGameManagerにセット
                GameManager.Instance.charAttackType.Add(characterMessage.charaAttackType);
                // ゲーム開始時の初期化処理
                var index = Random.Range(0, GameManager.Instance.playerResultInfos.Count + 1);
                GameManager.PlayerResultInfo playerInfo;
                playerInfo.playerNo = playerNo;
                playerInfo.charType = characterMessage.charaType;
                GameManager.Instance.playerResultInfos.Insert(index, playerInfo);
                GameManager.Instance.playerWins.Add(playerNo, 0);

            }
            // プレイヤーの人数をセット
            GameManager.Instance.playerNumber = playerNumber;
            // 何本先取かをGameManagerにセット
            GameManager.Instance.RaceNumber = selectPlayCount.RaceNumber;
            // ゲーム開始時の初期化処理
            //foreach (var playerNo in GameManager.Instance.playerAndControllerDictionary.Keys)
            //{
            //    var index = Random.Range(0, GameManager.Instance.playerResultInfos.Count + 1);
            //    var charType=GameManager.Instance.
            //    GameManager.Instance.playerResultInfos.Insert(index, playerNo,);
            //    GameManager.Instance.playerWins.Add(playerNo, 0);
            //}
            // Stageに遷移

            SceneManager.LoadScene("Stage");
        } // GameStart

        /// <summary>
        /// 選択時のSE再生
        /// </summary>
        public void PlayChoiseSE()
        {
            AudioManager.Instance.PlaySE(choiceClip, 0.1f);
        }

        /// <summary>
        /// 決定時のSE再生
        /// </summary>
        public void PlayEnterSE()
        {
            AudioManager.Instance.PlaySE(enterClip, 0.1f);
        }

    } // Class
} // namespace