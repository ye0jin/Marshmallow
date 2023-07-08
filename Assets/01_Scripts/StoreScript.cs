using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreScript : MonoBehaviour
{
    [SerializeField] private Button hpBtn;
    [SerializeField] private Button damageBtn;

    private int hp = 300;
    private int damageAdd = 500;

    private void Awake()
    {
        hpBtn.interactable = false;
        damageBtn.interactable = false;
    }

    private void Update()
    {
        hpBtn.interactable = UIManager.Instance.CurrentCoin >= 300;
        damageBtn.interactable = UIManager.Instance.CurrentCoin >= 500 && !UIManager.Instance.BuyDoubleDamage;
    }

    public void AddHP()
    {
        UIManager.Instance.UsingCoin(hp, false);
    }

    public void AddDamage()
    {
        UIManager.Instance.UsingCoin(damageAdd, true);
    }
}
