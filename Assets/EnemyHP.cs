using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 100;
    private int currentHP;

    [Header("UI")]
    public Slider hpSlider;               // HPバー（Fill）
    public RectTransform hpBackground;    // 背景（縮める対象）

    private float initialBgWidth;

    // ★ 追加
    EnemyAttackController attackController;
    int currentPhase = -1;

    void Start()
    {
        currentHP = maxHP;

        // EnemyAttackController取得
        attackController = GetComponent<EnemyAttackController>();

        // Slider 初期化
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
        else
        {
            Debug.LogWarning("[EnemyHP] hpSlider が設定されていません");
        }

        // 背景初期サイズ保存
        if (hpBackground != null)
        {
            initialBgWidth = hpBackground.sizeDelta.x;
        }
        else
        {
            Debug.LogWarning("[EnemyHP] hpBackground が設定されていません");
        }

        // 初期Phase設定
        UpdatePhase();
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP < 0) currentHP = 0;

        // HPバー更新
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }

        // 背景をHP割合で縮める
        if (hpBackground != null)
        {
            float ratio = (float)currentHP / maxHP;
            hpBackground.sizeDelta = new Vector2(
                initialBgWidth * ratio,
                hpBackground.sizeDelta.y
            );
        }

        // ★ Phase更新
        UpdatePhase();

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    // ★ HP割合でPhase切替
    void UpdatePhase()
    {
        if (attackController == null) return;

        float hpRate = (float)currentHP / maxHP;

        int newPhase =
            hpRate <= 0.3f ? 3 :
            hpRate <= 0.6f ? 2 : 1;

        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;
            attackController.SetPhase(currentPhase);

            Debug.Log($"[EnemyHP] Phase Changed → {currentPhase}");
        }
    }
}
