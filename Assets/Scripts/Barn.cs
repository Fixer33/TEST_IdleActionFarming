using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barn : MonoBehaviour
{
    [SerializeField] private Transform BlockSellingPosition;

    private Player _player;

    private void Start()
    {
        _player = Player.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_player != null)
        {
            _player.SellInventory(BlockSellingPosition);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
