using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip oneshotGun;
    public AudioClip machineGun;
    public AudioClip reloadSound;
    public AudioClip noAmmoSound;

    public AudioClip playerDeathSound;
    public AudioClip playerAttackSound;

    public AudioClip playerRollingSound;
    
    public AudioClip bombSound;
    public AudioClip enemyDeathSound;
    public AudioClip enemyAttackSound;

    public AudioClip coinDropSound;
    public AudioClip coinPickupSound;
    public AudioClip heal;

    public AudioClip buttonClickSound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null) 
        {
            Debug.LogError("Multiple SoundManager!");
        }

        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void SetBGMVolume(float value)
    {
        Camera.main.GetComponent<AudioSource>().volume = value;
    }

    public void SetEffectVolume(float value)
    {
        audioSource.volume = value;
    }

    public void PlayNoAmmo()
    {
        audioSource.PlayOneShot(noAmmoSound);
    }
    
    public void PlayRollingSound()
    {
        audioSource.PlayOneShot(playerRollingSound);
    }

    public void PlayCoinDrop()
    {
        audioSource.PlayOneShot(coinDropSound);
    }

    public void PlayHeal()
    {
        audioSource.PlayOneShot(heal);
    }

    public void PlayOneShot()
    {
        audioSource.PlayOneShot(oneshotGun);
    }

    public void PlayMachineShot()
    {
        audioSource.PlayOneShot(machineGun);
    }

    public void PlayPlayerDeathSound()
    {
        audioSource.PlayOneShot(playerDeathSound);
    }

    public void PlayReloadSound()
    {
        audioSource.PlayOneShot(reloadSound);
    }

    public void PlayPlayerAttackSound()
    {
        audioSource.PlayOneShot(playerAttackSound);
    }

    public void PlayEnemyDeathSound()
    {
        audioSource.PlayOneShot(enemyDeathSound);
    }

    public void PlayEnemyAttackSound()
    {
        audioSource.PlayOneShot(enemyAttackSound);
    }

    public void PlayCoinPickupSound()
    {
        audioSource.PlayOneShot(coinPickupSound);
    }

    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }

    public void PlayBombSound()
    {
        audioSource.PlayOneShot(bombSound);
    }
}
