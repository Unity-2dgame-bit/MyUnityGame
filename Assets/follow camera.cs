using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Camera cam;

    [Header("X Dead Zone (0~1)")]
    public float leftMargin = 0.4f;
    public float rightMargin = 0.6f;

    [Header("Y Screen Margin (0~1)")]
    public float topMargin = 0.9f;
    public float bottomMargin = 0.1f;

    [Header("Smooth")]
    public float smoothSpeed = 8f;

    float fixedY;
    bool isJumping;

    void Start()
    {
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        Vector3 camPos = transform.position;

        /* =====================
         * X方向（FPS依存しない）
         * ===================== */
        float viewportX = cam.WorldToViewportPoint(target.position).x;
        float targetX = camPos.x;

        if (viewportX < leftMargin || viewportX > rightMargin)
        {
            targetX = Mathf.Lerp(
                camPos.x,
                target.position.x,
                smoothSpeed * Time.deltaTime
            );
        }

        /* =====================
         * Y方向（ジャンプ制御）
         * ===================== */
        float viewportY = cam.WorldToViewportPoint(target.position).y;
        float targetY = fixedY;

        // ジャンプ中 & 画面外に出そう
        if (isJumping && (viewportY > topMargin || viewportY < bottomMargin))
        {
            targetY = Mathf.Lerp(
                camPos.y,
                target.position.y,
                smoothSpeed * Time.deltaTime
            );
            fixedY = targetY;
        }

        // 着地後はプレイヤーに合わせて固定
        if (!isJumping)
        {
            targetY = Mathf.Lerp(
                camPos.y,
                target.position.y,
                smoothSpeed * Time.deltaTime
            );
            fixedY = targetY;
        }

        transform.position = new Vector3(
            targetX,
            targetY,
            camPos.z
        );
    }

    // Player から呼ぶ
    public void SetJumping(bool jumping)
    {
        isJumping = jumping;
    }
}
