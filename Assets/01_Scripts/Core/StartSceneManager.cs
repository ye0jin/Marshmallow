using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private Camera main;
    [SerializeField] private float speed;
    
    private void Awake()
    {
        howToPlayPanel.SetActive(false);
    }

    public void Update()
    {
        float rotationY = Mathf.PingPong(Time.time * speed, 122f - 55f) + 55f;
        main.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rotationY, transform.rotation.eulerAngles.z);
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    public void ClickGameStart()
    {
        SceneManager.LoadScene("Main");
    }

    public void ClickHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void ClickXPanelOut()
    {
        howToPlayPanel.SetActive(false);
    }
}
