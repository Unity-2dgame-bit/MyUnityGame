using UnityEngine;
using System.Collections;

public class WarpAttack : MonoBehaviour
{
    public GameObject warpEffect;
    public float appearTime = 0.3f;

    private void Awake()
    {
        warpEffect.SetActive(false);
    }

    public IEnumerator Attack()
    {
        Debug.Log("[WarpAttack] ワープ表示");

        warpEffect.SetActive(true);
        yield return new WaitForSeconds(appearTime);
        warpEffect.SetActive(false);

        Debug.Log("[WarpAttack] ワープ終了");
    }
}
