using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelectStage
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
        // 必要なコンポーネント
        private SelectPlayerNumberComponent selectPlayerNumberComponent;
        private SelectCharacterComponent selectCharacterComponent;
        // コンポーネントのアクセスポイント
        public SelectPlayerNumberComponent SelectPlayerNumberComponent { get { return selectPlayerNumberComponent; } }
        public SelectCharacterComponent SelectCharacterComponent { get { return selectCharacterComponent; } }

        // Start is called before the first frame update
        void Start()
        {
            // コンポーネントの取得
            selectPlayerNumberComponent = GetComponent<SelectPlayerNumberComponent>();
            selectCharacterComponent = GetComponent<SelectCharacterComponent>();
            // プレイ人数選択処理を開始する
            StartCoroutine(selectPlayerNumberComponent.SelectPlayerNumber());
        }

        /// <summary>
        /// シーンの読み込み処理
        /// </summary>
        /// <param name="sceneName">読み込むシーンの名前</param>
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }


    }
}
