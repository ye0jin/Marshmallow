using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetBullet : MonoBehaviour
{
    private float speed = 15f;

    private GameObject target;
    private float saveTime = 7f;
    private float currentTime = 0f;

    private bool isDestroy = false;
    public bool IsDestroy => isDestroy;
        
    private PlayerHealth playerHealth;

    [SerializeField] private ParticleSystem boomParticle;

    private void Awake()
    {
        target = GameObject.Find("Player");
        playerHealth= target.GetComponent<PlayerHealth>();
        currentTime = 0f;

        isDestroy = false;

        Vector3 currentScale = transform.localScale;
        transform.localScale = currentScale / 4.0f;

        float randSpeed = Random.Range(7, 16);
        speed = randSpeed;
    }

    private void Update()
    {
        Quaternion rot = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed * 4 * Time.deltaTime);

        transform.position += transform.forward * speed * Time.deltaTime;

        currentTime += Time.deltaTime;
        if (currentTime >= saveTime)
        {
            Explode();
        }
    }

    public void Explode()
    {
        SoundManager.Instance.PlayBombSound();
        ParticleSystem particle = Instantiate(boomParticle, transform.position, Quaternion.identity);
        particle.Play();
        Destroy(particle.gameObject, particle.main.duration);

        isDestroy = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            Debug.Log("¸ñÇ¥ µµÂø");
            playerHealth.OnDamage(30);

            Explode();
        }
    }
}
