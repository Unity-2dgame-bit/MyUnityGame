using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 100;
    public int currentHP;

    [Header("UI")]
    public TextMeshProUGUI hpText;
    public Slider hpSlider;
    public RectTransform hpBackground;

    private float initialBgWidth;
    private Knockback knockback;

    void Start()
    {
        currentHP = maxHP;

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }

        if (hpBackground != null)
            initialBgWidth = hpBackground.sizeDelta.x;

        UpdateHPUI();

        knockback = GetComponent<Knockback>();
    }

    // Transform 引数付き（推奨）
    public void TakeDamage(int amount, Transform attacker)
    {
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;

        UpdateHPUI();

        if (knockback != null && attacker != null)
            knockback.ApplyKnockback(attacker);
    }

    // Vector2 引数付き互換用（ノックバックはなし）
    public void TakeDamage(int amount, Vector2 attackerPosition)
    {
        TakeDamage(amount, (Transform)null);
    }

    // 引数なし互換用（ノックバックなし）
    public void TakeDamage(int amount)
    {
        TakeDamage(amount, (Transform)null);
    }

    private void UpdateHPUI()
    {
        if (hpText != null)
            hpText.text = "HP: " + currentHP;

        if (hpSlider != null)
            hpSlider.value = currentHP;

        if (hpBackground != null)
        {
            float ratio = (float)currentHP / maxHP;
            hpBackground.sizeDelta = new Vector2(initialBgWidth * ratio, hpBackground.sizeDelta.y);
        }
    }
}
