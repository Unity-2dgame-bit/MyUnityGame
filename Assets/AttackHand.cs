using UnityEngine;
using System.Collections;

public class AttackHand : MonoBehaviour
{
    /* =====================
     * Damage
     * ===================== */
    [Header("Damage")]
    public int damage = 20;
    public float attackRange = 1.5f;
    public LayerMask playerLayer;

    /* =====================
     * Warp
     * ===================== */
    [Header("Warp")]
    public float warpDistance = 2.0f;     // プレイヤーから出現する距離
    public float warpWaitTime = 0.6f;     // ワープ後の硬直時間

    /* =====================
     * Timing
     * ===================== */
    [Header("Timing")]
    public float followTime = 1.2f;       // ペタペタ追跡時間
    public float stopTime = 0.4f;         // 攻撃前の溜め
    public float attackInterval = 1.5f;   // 次の攻撃まで

    /* =====================
     * Creepy Move
     * ===================== */
    [Header("Creepy Move")]
    public float stepDistance = 0.25f;
    public float stepInterval = 0.12f;
    public float rushDuration = 0.15f;
    public float rushSpeed = 25f;

    /* =====================
     * Offset
     * ===================== */
    [Header("Offset")]
    public Vector2 originOffset;

    /* =====================
     * Internal
     * ===================== */
    private Transform player;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool hasHit = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    /// <summary>
    /// Enemy から呼ぶ唯一の関数
    /// </summary>
    public void StartAttack()
    {
        if (!canAttack || isAttacking || player == null) return;
        StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        canAttack = false;
        isAttacking = true;
        hasHit = false;

        /* =====================
         * ① ワープ出現（攻撃なし）
         * ===================== */
        Vector2 warpPos =
            (Vector2)player.position +
            Random.insideUnitCircle.normalized * warpDistance;

        transform.position = warpPos;

        yield return new WaitForSeconds(warpWaitTime);

        /* =====================
         * ② ペタペタ追跡
         * ===================== */
        float t = 0f;
        while (t < followTime)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)(dir * stepDistance);
            transform.position += (Vector3)Random.insideUnitCircle * 0.03f;

            yield return new WaitForSeconds(stepInterval);
            t += stepInterval;
        }

        /* =====================
         * ③ 溜め
         * ===================== */
        yield return new WaitForSeconds(stopTime);

        /* =====================
         * ④ 襲撃（ここでだけダメージ）
         * ===================== */
        Vector2 startPos = transform.position;
        Vector2 targetPos = player.position;

        float rushT = 0f;
        while (rushT < rushDuration)
        {
            transform.position = Vector2.Lerp(
                startPos,
                targetPos,
                rushT * rushSpeed
            );

            DoDamageCheck();

            rushT += Time.deltaTime;
            yield return null;
        }

        /* =====================
         * ⑤ クールタイム
         * ===================== */
        yield return new WaitForSeconds(attackInterval);

        isAttacking = false;
        canAttack = true;
    }

    private void DoDamageCheck()
    {
        if (hasHit) return;

        Vector2 origin = (Vector2)transform.position + originOffset;
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(origin, attackRange, playerLayer);

        foreach (var c in hits)
        {
            if (!c.CompareTag("Player")) continue;

            PlayerHP hp = c.GetComponent<PlayerHP>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
                hasHit = true; // 1回だけ
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position + (Vector3)originOffset,
            attackRange
        );
    }
}
