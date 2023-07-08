using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject panel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }

    private void Awake()
    {
        panel.SetActive(false);
    }

}
