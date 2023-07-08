using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState
{
    Oneshot=1,
    Machine=2,
}
public class WeaponManager : MonoBehaviour
{
    private bool errorNoshot = true;
    private GameObject currentWeapon;
    public GameObject CurrentWeapon => currentWeapon;

    public WeaponState CurrentWeaponState;

    [SerializeField] private GameObject OneShotGun;
    [SerializeField] private GameObject MachineGun;

    private void Awake()
    {
        currentWeapon = OneShotGun; // 초기 설정

        CurrentWeaponState = WeaponState.Oneshot;
        currentWeapon.SetActive(true);
    }

    private void Update()
    {
        if(CameraManager.Instance.BossRot && errorNoshot)
        {
            errorNoshot = false;

            // 초기화
            OneShotGun.GetComponent<PlayerOneShotWeapon>()._isShoot = false;
            MachineGun.GetComponent<PlayerMachineGunWeapon>()._isShoot = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 1 누르면 단발총
        {
            currentWeapon = OneShotGun;
            CurrentWeaponState = WeaponState.Oneshot;

            currentWeapon.GetComponent<PlayerOneShotWeapon>()._isShoot = false;
            ChangeWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // 2 누르면 머신건
        {
            currentWeapon = MachineGun;
            CurrentWeaponState = WeaponState.Machine;

            currentWeapon.GetComponent<PlayerMachineGunWeapon>()._isShoot = false;
            ChangeWeapon();
        }
    }

    private void ChangeWeapon()
    {
        OneShotGun.SetActive(currentWeapon == OneShotGun);
        MachineGun.SetActive(currentWeapon == MachineGun);
    }
}
