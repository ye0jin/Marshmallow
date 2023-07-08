using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour
{
    private EnemyController controller;

    private bool isAttacking = true;

    [SerializeField] private float atkLength = 2f;

    [SerializeField] private int damage;

    [SerializeField] private LayerMask target;


    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (EnemyState.Attacking == controller.CurrentState && isAttacking)
        {
            isAttacking = false;
            Collider[] cols = Physics.OverlapSphere(transform.position, atkLength, target);
            foreach (Collider c in cols)
            {
                //Debug.Log("Player 잡음!");
                StartCoroutine(StartAttack(c.gameObject));
            }
        }
    }

        private IEnumerator StartAttack(GameObject player)
        {
            //Debug.Log("Attacking: " + target.name);
            Vector3 dirTarget = player.transform.position - transform.position;
            dirTarget.y = 0f;

            Vector3 dir = transform.forward;
            if (Vector3.Dot(dir, dirTarget) >= 0.8f)
            {
                yield return new WaitForSeconds(2f);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, dir, out hit, atkLength))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        Debug.Log("플레이어 잡음");
                        PlayerHealth.Instance.OnDamage(damage);
                    }
                }
            else
                {
                    Debug.Log("플레이어 놓침");
                }
            }
            yield return new WaitForSeconds(1f);

            isAttacking = true;
        }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 forward = transform.forward;

        Vector3 startPoint = transform.position + forward * atkLength;
        Gizmos.DrawLine(transform.position, startPoint);
    }
#endif
}
