using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance { get; private set; }

    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text inventoryText;
    [SerializeField] private Image moneyIcon;

    public Transform flyingCoinTarget;

    private Player _player;
    private int _updateInterval = 5;
    private Vector3 _defaultCoinIconSize;

    private void Start()
    {
        instance = this;
        _player = Player.instance;
        _defaultCoinIconSize = new Vector3(moneyIcon.transform.localScale.x, moneyIcon.transform.localScale.y, moneyIcon.transform.localScale.z);
    }

    private void Update()
    {
        if (Time.frameCount % _updateInterval == 0)
        {
            inventoryText.text = $"{_player.GetInventoryItemCount()}/{Inventory.MAX_ITEM_CAPACITY}";
            moneyText.text = $"{_player.Money}";
        }
    }

    public IEnumerator VibrateMoneyIcon()
    {
        for (int i = 0; i < 5; i++)
        {
            moneyIcon.transform.localScale *= 1.1f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        for (int i = 0; i < 5; i++)
        {
            moneyIcon.transform.localScale /= 1.1f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        moneyIcon.transform.localScale = new Vector3(_defaultCoinIconSize.x, _defaultCoinIconSize.y, _defaultCoinIconSize.z);

        yield return null;
    }
}
