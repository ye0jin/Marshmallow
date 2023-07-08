using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    [SerializeField] private GameObject bossCam;
    [SerializeField] private GameObject boss;

    private GameObject player;

    private bool bossEnd = false;
    private bool bossRot = false;
    public bool BossRot => bossRot;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple CameraManager");
        }
        Instance = this;
        player = GameObject.Find("Player");
        boss = GameObject.Find("Boss");
    }
    private void Update()
    {
        if(!bossEnd && GameManager.Instance.Boss)
        {
            bossEnd = true;
            player.SetActive(false);
            StartCoroutine(WaitForSecond(4f));
        }
        transform.position = _target.position + _offset;
    }

    private IEnumerator WaitForSecond(float time)
    {
        yield return new WaitForSeconds(time);
        
        bossCam.SetActive(false);
        bossRot = true;
        player.SetActive(true);
    }
}
