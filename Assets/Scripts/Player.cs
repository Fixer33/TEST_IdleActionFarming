using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterControl), typeof(Inventory))]
public class Player : MonoBehaviour
{
    public static Player instance { get; private set; }

    [SerializeField] private Transform flyingBlockTarget;
    public Transform FlyingBlockTarget { get { return flyingBlockTarget; } }

    public int Money { get; private set; } = 0;

    private Inventory _inventory;
    private CharacterControl _characterController;
    private Vegetation _lastVegetation;
    private Animator _animator;
    private bool _inwardSlashPlayed = false;
    private int _updateInterval = 3;

    private const string MONEY_KEY_NAME = "money";

    private void Awake()
    {
        instance = this;
        _inventory = GetComponent<Inventory>();
        _characterController = GetComponent<CharacterControl>();
        _animator = _characterController.GetAnimator();
        if (PlayerPrefs.HasKey(MONEY_KEY_NAME))
        {
            Money = PlayerPrefs.GetInt(MONEY_KEY_NAME);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(MONEY_KEY_NAME, Money);
    }

    private void Update()
    {
        if (Time.frameCount % _updateInterval == 0)
        {
            HandleVegetationCut();
        }
    }

    #region Vegetation actions
    private void HandleVegetationCut()
    {
        if (_lastVegetation == null)
            return;

        AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName("Idle") || currentState.IsName("Run"))
        {
            if (_inwardSlashPlayed)
            {
                _inwardSlashPlayed = false;
                _lastVegetation.DropBlock();
                _lastVegetation.DestroyWithEffect();
            }
        }
        else if (currentState.IsName("InwardSlash"))
        {
            _inwardSlashPlayed = true;
        }
    }

    public void AddVegetationToInventory(VegetationData vData)
    {
        _inventory.AddItem(vData, 1);
    }

    public void VegetationCut(Vegetation vegetation)
    {
        _characterController.AnimateVegetationCut();
        _lastVegetation = vegetation;
    } 
    #endregion

    public bool IsIdling()
    {
        AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName("Idle");
    }

    public bool FreeSpaceAvaible()
    {
        return _inventory.FreeSpaceAvaible();
    }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public void SellInventory(Transform sellPosition)
    {
        _inventory.Sell(flyingBlockTarget, sellPosition);
    }

    public int GetInventoryItemCount()
    {
        return _inventory.ItemCount;
    }

}
