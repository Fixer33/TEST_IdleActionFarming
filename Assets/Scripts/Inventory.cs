
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public const int MAX_ITEM_CAPACITY = 40;

    public int ItemCount { 
        get
        {
            int total = 0;
            foreach (var item in items)
            {
                total += item.Value;
            }
            return total;
        }
    }

    [SerializeField] private GameObject flyingCoinPrefab;
    [SerializeField] private GameObject blockFiller;

    private Dictionary<VegetationData, int> items = new Dictionary<VegetationData, int>();
    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    public void Sell(Transform flyingBlockStartPoint, Transform sellingPosition)
    {
        StartCoroutine(AnimateBlockAndCoinSelling(flyingBlockStartPoint, sellingPosition));
    }

    private IEnumerator AnimateBlockAndCoinSelling(Transform flyingBlockStartPoint, Transform sellingPosition)
    {
        List<int> addingAmounts = new List<int>();
        Dictionary<VegetationData, int> itemsCopy = new Dictionary<VegetationData, int>();
        foreach (var item in items)
        {
            itemsCopy.Add(item.Key, item.Value);
        }

        foreach (var item in itemsCopy)
        {
            int count = item.Value;
            for (int i = 0; i < count; i++)
            {
                items[item.Key]--;
                GameObject flyingBlock = Instantiate(item.Key.FlyingBlockPrefab, flyingBlockStartPoint.position, Quaternion.identity, transform.parent);
                FlyingBlock blockScript = flyingBlock.GetComponent<FlyingBlock>();
                blockScript.StartMovement(sellingPosition);
                addingAmounts.Add(item.Key.Price);
                UpdateFiller();
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
        yield return new WaitForSecondsRealtime(0.2f);

        for (int i = 0; i < addingAmounts.Count; i++)
        {
            SendCoin(sellingPosition, addingAmounts[i]);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        ClearInventory();
    }

    private void SendCoin(Transform sellingPosition, int moneyToAdd)
    {
        GameObject coinObj = Instantiate(flyingCoinPrefab, Camera.main.WorldToScreenPoint(sellingPosition.position), Quaternion.identity, UI.instance.transform);
        FlyingBlock coinScript = coinObj.GetComponent<FlyingBlock>();
        
        coinScript.StartMovement(UI.instance.flyingCoinTarget, () => 
        { 
            _player.AddMoney(moneyToAdd); 
            StartCoroutine(UI.instance.VibrateMoneyIcon()); 
        });
    }

    public void ClearInventory()
    {
        items.Clear();
    }

    public void AddItem(VegetationData type, int amount)
    {
        if (items.ContainsKey(type))
        {
            items[type] += amount;
        }
        else
        {
            items.Add(type, amount);
        }
        UpdateFiller();
    }

    private void UpdateFiller()
    {
        int itemCount = ItemCount;
        Vector3 originalScale = blockFiller.transform.localScale;
        Vector3 scale = new Vector3(originalScale.x, 0.02f * itemCount, originalScale.z);

        Vector3 position = new Vector3(0, -0.43f + (0.01f * itemCount), 0);

        blockFiller.transform.localScale = scale;
        blockFiller.transform.localPosition = position;
    }

    public bool FreeSpaceAvaible()
    {
        return ItemCount < MAX_ITEM_CAPACITY;
    }
}
