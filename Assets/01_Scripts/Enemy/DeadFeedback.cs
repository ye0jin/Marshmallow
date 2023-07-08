using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeadFeedback : MonoBehaviour
{
    [SerializeField] private GameObject coinPref;

    [SerializeField]
    [Range(0, 1f)]
    private float dropPercent;

    public void Drop()
    {
        float dropValue = Random.value; // 0~1 에서의 값

        if(dropValue < dropPercent)
        {
            GameObject coin = Instantiate(coinPref);
            coin.transform.position = transform.position;

            Vector3 offset = Random.insideUnitCircle * 1.5f;
            coin.transform.DOJump(transform.position + offset, 2f, 1, 0.4f);
        }
    }
}
