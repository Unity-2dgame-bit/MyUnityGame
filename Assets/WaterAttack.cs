using UnityEngine;
using System.Collections;

public class WaterAttack : MonoBehaviour
{
    public GameObject waterEffect;
    public Collider2D hitCollider;
    public float duration = 1.0f;

    private void Awake()
    {
        waterEffect.SetActive(false);
        hitCollider.enabled = false;
    }

    public IEnumerator Attack()
    {
        Debug.Log("[WaterAttack] î≠éÀ");

        waterEffect.SetActive(true);
        hitCollider.enabled = true;

        yield return new WaitForSeconds(duration);

        hitCollider.enabled = false;
        waterEffect.SetActive(false);

        Debug.Log("[WaterAttack] èIóπ");
    }
}
