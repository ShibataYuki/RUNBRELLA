using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveCheck : MonoBehaviour
{
    Enemy enemy;
    Rigidbody2D enemyRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        enemy = GetComponent<Enemy>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        // パラメータを取得
        ReadTextParameter();
    }

    // 1秒あたりの加速量
    float addVelocityXPersecond;
    // 最高速度
    float maxVelocityX;
    // 手すりモードを終わるとき、どの程度y軸方向の慣性を残すか(%)
    float slideInertiaYPercent;
    // ジャンプ力
    float jumpPower;
    // 空中状態の重力の大きさ
    float aerialGravityScale;

    /// <summary>
    /// テキストからパラメータを取得
    /// </summary>
    private void ReadTextParameter()
    {
        // 追加部分
        try
        {
            // 読み込むテキストの名前
            var textName = "";
            switch (enemy.charType)
            {
                case GameManager.CHARTYPE.PlayerA:
                    textName = "Chara_A";
                    break;
                case GameManager.CHARTYPE.PlayerB:
                    textName = "Chara_B";
                    break;
            }
            // テキストの中のデータをセットするディクショナリー        
            SheetToDictionary.Instance.TextToDictionary(textName, out var textDataDic);
            try
            {
                // ファイル読み込み
                addVelocityXPersecond = textDataDic["1秒間に増加する速度"];
                maxVelocityX = textDataDic["最高速度の秒速"];
                slideInertiaYPercent = textDataDic["手すりを離れた後にY軸方向への慣性を何パーセント残すか(%)"];
                jumpPower = textDataDic["ジャンプ力(重力を考えなかった場合に1秒間に上に移動する移動量)"];
                aerialGravityScale = textDataDic["空中状態の場合における重力加速度の倍率"];
            }
            catch
            {
                Debug.Assert(false, textName + "でエラーが発生しました");
            }
        }
        catch
        {
            Debug.Assert(false, nameof(EnemyMoveCheck) + "でエラーが発生しました");
        }
    }

    #region 移動の予測
    /// <summary>
    /// ジャンプ時の移動量を算出
    /// </summary>
    /// <returns></returns>
    public Vector2 JumpVelocity()
    {
        // 自身の移動量
        var velocity = enemyRigidbody.velocity;
        // 手すり中なら
        if (enemy.IsSlide == true)
        {
            // 手すりを終了する時に速度に残す慣性を計算
            velocity.y *= (slideInertiaYPercent / 100.0f);
        }
        // 速度のY方向にジャンプ力を加える
        velocity.y += jumpPower;
        return velocity;
    }

    /// <summary>
    /// 空中状態での加速処理の再現
    /// </summary>
    /// <param name="velocity"></param>
    /// <returns></returns>
    public void AerialAcceleration(ref Vector2 velocity)
    {
        // 加速する
        velocity = Acceleration(velocity);
        // 重力を掛ける
        velocity.y += Physics2D.gravity.y * aerialGravityScale * Time.fixedDeltaTime;
    }

    /// <summary>
    /// 地上での加速処理の再現
    /// </summary>
    /// <param name="velocity"></param>
    /// <returns></returns>
    public Vector2 Acceleration(Vector2 velocity)
    {
        // 加速
        velocity.x += addVelocityXPersecond * Time.fixedDeltaTime;
        // 速度制限
        velocity.x = Mathf.Clamp(velocity.x, 0.0f, maxVelocityX);
        return velocity;
    }
    #endregion
}
