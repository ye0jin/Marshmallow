using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private float rotationSpeed = 20f;

    private void Start()
    {
        SoundManager.Instance.PlayCoinDrop();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlayCoinPickupSound();
            UIManager.Instance.SelectCoin();
            //Debug.Log("Coin ∏‘¿Ω");
            Destroy(gameObject);
        }
    }
}
