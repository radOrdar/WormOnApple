using System.Collections.Generic;
using Game;
using StaticData;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private Pickup pickupPf;
    [SerializeField] private Poison poisonPf;

    private ProgressionUnit _progressionUnit;
    private List<Vector3> points = new();
    public void Init(ref ProgressionUnit progressionUnit)
    {
       
        _progressionUnit = progressionUnit;
        LayerMask appleLayer = LayerMask.GetMask("Apple");
        for (int i = 0; i < _progressionUnit.pickupNum; i++)
        {
            Vector3 onUnitSphere = GenerateNewPoint();
            
            Physics.Raycast(onUnitSphere, -onUnitSphere, out RaycastHit hit, 100, appleLayer);
            Instantiate(pickupPf, hit.point, Quaternion.LookRotation(onUnitSphere), transform);
        }

        for (int i = 0; i < _progressionUnit.posionNum; i++)
        {
            Vector3 onUnitSphere = GenerateNewPoint();
            Physics.Raycast(onUnitSphere, -onUnitSphere, out RaycastHit hit, 100, appleLayer);
            Instantiate(poisonPf, hit.point, Quaternion.LookRotation(onUnitSphere), transform);
        }

        points = null;
    }
    private Vector3 GenerateNewPoint()
    {
        Vector3 onUnitSphere;
        while (true)
        {
            onUnitSphere = Random.onUnitSphere * 50;
            if (CheckForUnique(onUnitSphere)) break;
        }
        points.Add(onUnitSphere);
        return onUnitSphere;
    }
   
    private bool CheckForUnique(Vector3 onUnitSphere)
    {
        foreach (var p in points)
        {
            if ((p - onUnitSphere).sqrMagnitude < 10f) return false;
        }

        return true;
    }
}