using UnityEngine;
using System.Collections;

public class Knockback : MonoBehaviour
{
    [Header("Knockback Settings")]
    public float knockbackDistance = 2f;
    public float duration = 0.2f;

    private Rigidbody2D rb;
    private Coroutine knockbackRoutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D がアタッチされていません！");
    }

    public void ApplyKnockback(Transform attacker)
    {
        if (rb == null || attacker == null) return;

        if (knockbackRoutine != null)
            StopCoroutine(knockbackRoutine);

        knockbackRoutine = StartCoroutine(KnockbackCoroutine(attacker));
    }

    private IEnumerator KnockbackCoroutine(Transform attacker)
    {
        float timer = 0f;

        while (timer < duration)
        {
            Vector2 dir = rb.position - (Vector2)attacker.position;
            dir.y = 0f;

            if (Mathf.Abs(dir.x) < 0.01f)
                dir.x = transform.localScale.x >= 0 ? 1f : -1f;

            dir.Normalize();

            Vector2 moveStep = dir * (knockbackDistance / duration) * Time.deltaTime;
            rb.MovePosition(rb.position + moveStep);

            timer += Time.deltaTime;
            yield return null;
        }

        knockbackRoutine = null;
    }
}
