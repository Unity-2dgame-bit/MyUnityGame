using UnityEngine;

public class DamageToPlayer : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHP hp = other.GetComponent<PlayerHP>();
            if (hp != null)
            {
                // 攻撃者位置（ノックバック用）
                Vector2 attackerPos = transform.position;

                hp.TakeDamage(damage, attackerPos);
            }
        }
    }
}
