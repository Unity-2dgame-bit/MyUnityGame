using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Transform player;

    private Rigidbody2D rb;
    private SpriteRenderer childSprite;
    private EnemyAttackController attackController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackController = GetComponent<EnemyAttackController>();

        childSprite = GetComponentInChildren<SpriteRenderer>();
        if (childSprite == null)
            Debug.LogError("子に SpriteRenderer が見つかりません！");

        if (player == null)
        {
            Debug.LogError("Player が設定されていません！");
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        // 🔴 攻撃中は移動しない
        if (attackController != null && attackController.IsAttacking())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            player.position,
            speed * Time.fixedDeltaTime
        );

        newPos.y = rb.position.y;
        rb.MovePosition(newPos);

        UpdateDirection();
    }

    void UpdateDirection()
    {
        if (player.position.x > transform.position.x)
            childSprite.flipX = true;
        else
            childSprite.flipX = false;
    }
}
