using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DroppedBlock : MonoBehaviour
{
    public VegetationData vegetation;

    private Collider _collider;
    private Player _player;
    private void Start()
    {
        _player = Player.instance;
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        StartCoroutine(ActivateAfterSeconds(1));
    }

    private IEnumerator ActivateAfterSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        _collider.enabled = true;
    }

    public void CreateFlyingBlock()
    {
        GameObject flying = Instantiate(vegetation.FlyingBlockPrefab, transform.position, Quaternion.identity, transform.parent);
        FlyingBlock script = flying.GetComponent<FlyingBlock>();
        Player player = Player.instance;
        script.StartMovement(player.FlyingBlockTarget, () => { player.AddVegetationToInventory(vegetation); });
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player.gameObject && _player.FreeSpaceAvaible())
            CreateFlyingBlock();
    }
}
