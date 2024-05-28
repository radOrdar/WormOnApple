using Game;
using StaticData;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private Pickup pickupPf;
    [SerializeField] private Poison poisonPf;

    private ProgressionUnit _progressionUnit;
    
    public void Init(ref ProgressionUnit progressionUnit)
    {
        _progressionUnit = progressionUnit;
        LayerMask appleLayer = LayerMask.GetMask("Apple");
        for (int i = 0; i < _progressionUnit.pickupNum; i++)
        {
            Vector3 insideUnitSphere = Random.insideUnitSphere * 100;
            Physics.Raycast(insideUnitSphere, -insideUnitSphere, out RaycastHit hit, 100, appleLayer);
            Instantiate(pickupPf, hit.point, Quaternion.LookRotation(insideUnitSphere), transform);
        }

        for (int i = 0; i < _progressionUnit.posionNum; i++)
        {
            Vector3 insideUnitSphere = Random.insideUnitSphere * 100;
            Physics.Raycast(insideUnitSphere, -insideUnitSphere, out RaycastHit hit, 100, appleLayer);
            Instantiate(poisonPf, hit.point, Quaternion.LookRotation(insideUnitSphere), transform);
        }
    }
}