using UnityEngine;
using System.Collections;

public class HandAttack : MonoBehaviour
{
    [Header("Hand")]
    public GameObject handObject;
    public Transform handTransform;
    public Collider2D hitCollider;

    [Header("Target")]
    public Transform player;

    [Header("Move Settings")]
    public float extendSpeed = 8f;
    public float retractSpeed = 10f;
    public float maxDistance = 3f;
    public float stayTime = 0.2f;

    private Vector3 startPos;

    private void Awake()
    {
        handObject.SetActive(false);
        hitCollider.enabled = false;
    }

    public IEnumerator Attack()
    {
        // ===== 攻撃開始時に必ず現在位置を基準にする =====
        handObject.SetActive(true);
        hitCollider.enabled = false;

        startPos = handTransform.position;

        // ===== プレイヤー方向（左右のみ） =====
        float dirX = Mathf.Sign(player.position.x - startPos.x);
        Vector3 dir = new Vector3(dirX, 0f, 0f);

        Vector3 targetPos = startPos + dir * maxDistance;

        // =====================
        // 伸びる
        // =====================
        while (Vector3.Distance(handTransform.position, targetPos) > 0.01f)
        {
            handTransform.position = Vector3.MoveTowards(
                handTransform.position,
                targetPos,
                extendSpeed * Time.deltaTime
            );
            yield return null;
        }

        // ===== 最大到達で当たり判定ON =====
        hitCollider.enabled = true;

        yield return new WaitForSeconds(stayTime);

        // =====================
        // 戻る
        // =====================
        hitCollider.enabled = false;

        while (Vector3.Distance(handTransform.position, startPos) > 0.01f)
        {
            handTransform.position = Vector3.MoveTowards(
                handTransform.position,
                startPos,
                retractSpeed * Time.deltaTime
            );
            yield return null;
        }

        handTransform.position = startPos;
        handObject.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (player == null || handTransform == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(handTransform.position, player.position);
    }
#endif
}
