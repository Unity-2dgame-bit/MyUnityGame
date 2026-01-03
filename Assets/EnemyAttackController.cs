using UnityEngine;
using System.Collections;

public class EnemyAttackController : MonoBehaviour
{
    private enum HanakoAttackType
    {
        Hand,
        Water,
        Warp
    }

    [Header("Target")]
    public Transform player;

    [Header("Attacks")]
    public HandAttack handAttack;
    public WaterAttack waterAttack;
    public WarpAttack warpAttack;

    [Header("Attack Settings")]
    public float attackStartDistance = 3f;
    public float checkInterval = 0.5f;
    public float attackCooldown = 10f;

    [Header("Phase")]
    public int phase = 1;

    bool isAttacking;

    bool handReady = true;
    bool waterReady = true;
    bool warpReady = true;

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (DialogueManager.isDialoguePlaying || isAttacking || player == null)
            {
                yield return null;
                continue;
            }

            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= attackStartDistance)
            {
                HanakoAttackType attack = SelectAttack(distance);

                if (IsAttackAvailable(attack))
                {
                    yield return StartCoroutine(ExecuteAttack(attack));
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private HanakoAttackType SelectAttack(float distance)
    {
        if (phase == 1)
            return HanakoAttackType.Hand;

        if (phase == 2)
            return distance < 2f ? HanakoAttackType.Hand : HanakoAttackType.Water;

        if (distance < 1.5f)
            return HanakoAttackType.Hand;
        else if (distance < 3.5f)
            return HanakoAttackType.Water;
        else
            return HanakoAttackType.Warp;
    }

    private IEnumerator ExecuteAttack(HanakoAttackType type)
    {
        Debug.Log($"[Controller] 攻撃開始: {type}");

        isAttacking = true;
        SetReady(type, false);

        switch (type)
        {
            case HanakoAttackType.Hand:
                yield return StartCoroutine(handAttack.Attack());
                break;
            case HanakoAttackType.Water:
                yield return StartCoroutine(waterAttack.Attack());
                break;
            case HanakoAttackType.Warp:
                yield return StartCoroutine(warpAttack.Attack());
                break;
        }

        Debug.Log($"[Controller] 攻撃終了: {type}");

        yield return new WaitForSeconds(0.3f);
        isAttacking = false;

        StartCoroutine(AttackCooldown(type));
    }

    private IEnumerator AttackCooldown(HanakoAttackType type)
    {
        yield return new WaitForSeconds(attackCooldown);
        SetReady(type, true);
    }

    private void SetReady(HanakoAttackType type, bool ready)
    {
        if (type == HanakoAttackType.Hand) handReady = ready;
        if (type == HanakoAttackType.Water) waterReady = ready;
        if (type == HanakoAttackType.Warp) warpReady = ready;
    }

    private bool IsAttackAvailable(HanakoAttackType type)
    {
        if (type == HanakoAttackType.Hand) return handReady;
        if (type == HanakoAttackType.Water) return waterReady;
        if (type == HanakoAttackType.Warp) return warpReady;
        return false;
    }

    // 🔽 ★ Enemy から参照するための最重要メソッド
    public bool IsAttacking()
    {
        return isAttacking;
    }

    public void SetPhase(int newPhase)
    {
        phase = newPhase;
        Debug.Log($"[EnemyAttackController] Phase changed to {phase}");
    }
}
