using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VegetationSpawner : MonoBehaviour
{
    public int MaximumVegetation = 30;
    public float VegetationSpacing = 0.1f;

    [SerializeField] private VegetationData vegetationData;

    private List<Vector3> _spawnPositions;
    private Dictionary<Vector3, DateTime> _destroyedVegetationTime = new Dictionary<Vector3, DateTime>();
    private int _updateInterval = 11;

    void Start()
    {
        CalculateSpawnPositions();
        FillField();
    }

    void Update()
    {
        if (Time.frameCount % _updateInterval == 0)
        {
            RegenerateField();

        }
    }

    public void VegetationDestroyed(int index)
    {
        _destroyedVegetationTime.Add(_spawnPositions[index], DateTime.UtcNow);
    }

    private void RegenerateField()
    {
        List<Vector3> toRegenerate = new List<Vector3>();
        foreach (var item in _destroyedVegetationTime)
        {
            TimeSpan delta = DateTime.UtcNow - item.Value;
            if (delta.TotalSeconds > vegetationData.GrowthTime)
            {
                toRegenerate.Add(item.Key);
            }
        }

        foreach (var item in toRegenerate)
        {
            SpawnVegetation(item);
            _destroyedVegetationTime.Remove(item);
        }
    }


    private void FillField()
    {
        for (int i = 0; i < MaximumVegetation; i++)
        {
            if (i >= _spawnPositions.Count)
                break;
            SpawnVegetation(_spawnPositions[i]);
        }
    }
    
    private void SpawnVegetation(Vector3 position)
    {
        GameObject newVegetation = Instantiate(vegetationData.Prefab, position, Quaternion.identity, transform);
        Vegetation script = newVegetation.GetComponent<Vegetation>();
        script.spawnPositionIndex = _spawnPositions.IndexOf(position);
    }

    private void CalculateSpawnPositions()
    {
        _spawnPositions = new List<Vector3>();
        BoxCollider collider = GetComponent<BoxCollider>();
        Vector3 offset = collider.center;
        Vector2 size = collider.size;
        int perRow = Mathf.RoundToInt(size.x / (VegetationSpacing + vegetationData.Radius / 2));
        int rowCount = MaximumVegetation / perRow;
        
        for (int r = 0; r < rowCount; r++)
        {
            for (int i = 0; i < perRow; i++)
            {
                float x = i * (VegetationSpacing + vegetationData.Radius / 2) - size.x / 2;
                float z = 0 - (VegetationSpacing + vegetationData.Radius / 2) * r;
                Vector3 localPos = new Vector3(x + 0.9f, 0, z);
                Vector3 globalPos = transform.TransformPoint(localPos);


                _spawnPositions.Add(globalPos);
            }
        }
    }
}
