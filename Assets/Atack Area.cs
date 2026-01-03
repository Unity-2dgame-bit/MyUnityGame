using UnityEngine;
using System.Collections.Generic;

public class AttackArea : MonoBehaviour
{
    public List<EnemyHP> enemies = new List<EnemyHP>();

    // “G‚ª“ü‚Á‚½i¦SetActive’¼Œã‚ÍŒÄ‚Î‚ê‚È‚¢‚±‚Æ‚ª‚ ‚éj
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHP hp = collision.GetComponent<EnemyHP>();
            if (hp != null && !enemies.Contains(hp))
            {
                enemies.Add(hp);
                Debug.Log("[AttackArea] “G‚ª”ÍˆÍ‚É“ü‚Á‚½ ¨ " + collision.name);
            }
        }
    }

    // SetActive(true) ’¼Œã‚ÌŒŸ’m˜R‚ê‚ğE‚¤i’´d—vj
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHP hp = collision.GetComponent<EnemyHP>();
            if (hp != null && !enemies.Contains(hp))
            {
                enemies.Add(hp);
                Debug.Log("[AttackArea] “G‚ª”ÍˆÍ‚É—¯‚Ü‚Á‚Ä‚¢‚é ¨ " + collision.name);
            }
        }
    }

    // “G‚ª”ÍˆÍ‚©‚ço‚½
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHP hp = collision.GetComponent<EnemyHP>();
            if (hp != null && enemies.Contains(hp))
            {
                enemies.Remove(hp);
                Debug.Log("[AttackArea] “G‚ª”ÍˆÍ‚©‚ço‚½ ¨ " + collision.name);
            }
        }
    }
}
