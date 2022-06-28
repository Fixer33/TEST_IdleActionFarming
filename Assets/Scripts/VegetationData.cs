using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vegetation Data", menuName = "Vegetation Data", order = 51)]
public class VegetationData : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private float growthTime;
    [SerializeField] private float radius;
    [SerializeField] private int price;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject flyingBlockPrefab;
    [SerializeField] private GameObject droppedBlockPrefab;

    public string Name { get { return name; } }
    public float GrowthTime { get { return growthTime; } }
    public float Radius { get { return radius; } }
    public int Price { get { return price; } }
    public GameObject Prefab { get { return prefab; } }
    public GameObject FlyingBlockPrefab { get { return flyingBlockPrefab; } }
    public GameObject DroppedBlockPrefab { get { return droppedBlockPrefab; } }
}
