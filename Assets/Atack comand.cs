using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackArea;
    public int damage = 20;
    private AttackArea attackAreaScript;

    void Start()
    {
        if (attackArea == null)
        {
            Debug.LogError("[PlayerAttack] attackArea が Inspector に設定されていません！");
            return;
        }

        attackAreaScript = attackArea.GetComponent<AttackArea>();
        attackArea.SetActive(false);

        Debug.Log("[PlayerAttack] Start：attackArea = OK");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            
            StartAttack();
            
        }
    }

    void StartAttack()
    {
        attackArea.SetActive(true);

        // ★ ダメージ処理を少し遅らせる（これが重要）
        Invoke(nameof(ApplyDamage), 0.02f);

        Invoke(nameof(EndAttack), 1f);

        Debug.Log("[PlayerAttack] 攻撃！");
    }

    void ApplyDamage()
    {
        Debug.Log("[PlayerAttack] enemies数 = " + attackAreaScript.enemies.Count);

        foreach (var enemy in attackAreaScript.enemies)
        {
            enemy.TakeDamage(damage);
        }
    }


    void EndAttack()
    {
        attackArea.SetActive(false);
    }
}
