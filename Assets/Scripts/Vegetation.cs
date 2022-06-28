using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Vegetation : MonoBehaviour
{
    [SerializeField] private VegetationData data;

    public int spawnPositionIndex = -1;

    public VegetationData Data { get { return data; } }

    private VegetationSpawner _spawner;
    private Player _player;
    private int _framesIdling = 0;
    private int _updateInterval = 3;

    private const int FRAMES_TO_START_CUTTING_VEGETATION = 7;

    private void Start()
    {
        _spawner = GetComponentInParent<VegetationSpawner>();
        _player = Player.instance;
    }

    public void DropBlock()
    {
        GameObject droppedBlockObj = Instantiate(Data.DroppedBlockPrefab, transform.position, Quaternion.Euler(-90,0,0), transform.parent);
        DroppedBlock dbScript = droppedBlockObj.GetComponent<DroppedBlock>();
        dbScript.vegetation = data;
    }

    public void DestroyWithEffect()
    {
        _spawner.VegetationDestroyed(spawnPositionIndex);
        //effect
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Time.frameCount % _updateInterval == 0)
        {
            if (other.gameObject == _player.gameObject && _player.IsIdling())
            {
                _framesIdling++;
                if (_framesIdling >= FRAMES_TO_START_CUTTING_VEGETATION)
                {
                    _framesIdling = 0;
                    _player.VegetationCut(this);
                }
            }
            else
            {
                _framesIdling = 0;
            }
        }
    }
}
