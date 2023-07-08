using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private bool isGameOver = false;
    public bool IsGameOver => isGameOver;

    private int currentEnemyKill = 0;
    public int CurrentEnemyKill => currentEnemyKill;
    private int currentCoin = 0;
    public int CurrentCoin => currentCoin;

    private WeaponManager weaponManager;
    private PlayerOneShotWeapon oneShot;
    private PlayerMachineGunWeapon machine;
    private PlayerHealth playerHealth;
    [SerializeField] private BossHealth bossHealth;

    [SerializeField] private TextMeshProUGUI doubleText;

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI enemyText;

    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI bulletText;
    [SerializeField] private TextMeshProUGUI gamePlayingText;

    [SerializeField] private GameObject bossHP;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private GameObject gameSuccessPanel;
    [SerializeField] private TextMeshProUGUI gameSuccessText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [SerializeField] private StageSO stage;

    [SerializeField] private GameObject settingPanel;

    private GameObject player;

    private float time = 0f;
    private bool isEscape = false;

    // 두배짜리
    private float timeDoubleDam = 0f;
    private bool buyDoubleDamage;
    public bool BuyDoubleDamage => buyDoubleDamage;

    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider effectSlider;

    public void OnBgmSliderValueChanged()
    {
        float bgmVolume = bgmSlider.value;

        SoundManager.Instance.SetBGMVolume(bgmVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
    }

    public void OnEffectSliderValueChanged()
    {
        float effectVolume = effectSlider.value;

        SoundManager.Instance.SetEffectVolume(effectVolume);
        PlayerPrefs.SetFloat("EffectVolume", effectVolume);
    }

    private void Awake()
    {
        PlayerPrefs.GetFloat("BGMVolume", 0.1f);
        PlayerPrefs.GetFloat("EffectVolume", 0.4f);

        if (Instance != null)
        {
            Debug.LogError("Multiple UIManager");
        }
        Instance = this;

        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        doubleText.alpha = 0;

        bossHP.SetActive(false);
        settingPanel.SetActive(false);
        
        weaponManager = GameObject.Find("Player/Visual/Bone_Body/Bone_Shoulder_R/Bone_Arm_R/RightHand/Weapon").GetComponent<WeaponManager>();
        oneShot = weaponManager.transform.Find("OneShotGun").GetComponent<PlayerOneShotWeapon>();
        machine = weaponManager.transform.Find("MachineGun").GetComponent<PlayerMachineGunWeapon>();
    }

    public void escape()
    {
        player.SetActive(true);
        Time.timeScale = 1;
        isEscape = false;
        settingPanel.SetActive(false);
    }

    private void Update()
    {
        if (isEscape)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                escape();
            }
            return;
        }// escape 버튼 눌렀을 때

        if (isGameOver) return; // 게임 끝났을 때
        if(bossHealth.IsDead) // 보스 죽었을 때
        {
            isGameOver = true;
            time = GameManager.Instance.CurrentEndTime;
            
            gameSuccessText.text = $"{Mathf.FloorToInt(time / 60) % 60:00}:{time % 60:00}";
            float t = PlayerPrefs.GetFloat("BestScore");
            if (t == time)
            {
                bestScoreText.text = "최고기록 달성 ! ! !";
            }
            else
            {
                bestScoreText.text = $"최고기록: {Mathf.FloorToInt(t / 60) % 60:00}:{t % 60:00}";
            }

            gameSuccessPanel.transform.DOLocalMoveY(0, 2.5f);
        }

        time = GameManager.Instance.CurrentPlayingTime;

        if(Input.GetKeyDown(KeyCode.Escape) && !isEscape)
        {
            SoundManager.Instance.PlayButtonClickSound();
            player.SetActive(false);
            isEscape = true;
            Time.timeScale = 0;
            settingPanel.SetActive(true);
            return;
        }

        if(buyDoubleDamage)
        {
            doubleText.alpha = 1;
            timeDoubleDam += Time.deltaTime;
            if(timeDoubleDam>=10)
            {
                doubleText.alpha = 0;
                buyDoubleDamage = false;
                timeDoubleDam = 0;
            }
        }

        if (weaponManager.CurrentWeaponState == WeaponState.Oneshot)
        {
            bulletText.text = $"{oneShot.CurrentAmmo}/{oneShot.MaxAmmo}";
        }
        else if (weaponManager.CurrentWeaponState == WeaponState.Machine)
        {
            bulletText.text = $"{machine.CurrentAmmo}/{machine.MaxAmmo}";
        }

        coinText.text = $"× {currentCoin}";
        if (!GameManager.Instance.Boss)
        {
            enemyText.text = $"목표까지 {stage.Stage[GameManager.Instance.CurrentStage].EnemyCount - currentEnemyKill}마리";
        }   
        else
        {
            bossHP.SetActive(true);
            enemyText.text = $"목표 달성!";
        }
        lifeText.text = $"{playerHealth.CurrentHealth}/{playerHealth.MaxHealth}";
        gamePlayingText.text = $"{Mathf.FloorToInt(time / 60) % 60:00}:{time % 60:00}";
    }

    public void EnemyKill()
    {
        ++currentEnemyKill;
    }

    public void SelectCoin()
    {
        currentCoin += 100;
    }

    public void UsingCoin(int useCoin, bool buyDamage)
    {
        // 일정량 코인 삭제
        currentCoin -= useCoin;
        buyDoubleDamage = buyDamage;
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.transform.DOLocalMoveY(0, 2.5f);
    }

    public IEnumerator BlinkLifeText(Color c)
    {
        lifeText.color = c;
        yield return new WaitForSeconds(0.2f);
        lifeText.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        lifeText.color = c;
        yield return new WaitForSeconds(0.2f);
        lifeText.color = Color.white;
    }

    public void ClickRestartBTN()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ClickTitleBTN()
    {
        SceneManager.LoadScene("Title");
    }

    
}
