using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("EnemyÇ∆è’ìÀÅIHPå∏ÇÁÇ∑ÇÊ");
            GetComponent<PlayerHP>().TakeDamage(10);
        }
    }
}


