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
        // LayerMask appleLayer = LayerMask.GetMask("Apple");
        for (int i = 0; i < _progressionUnit.pickupNum; i++)
        {
            Vector3 onUnitSphere = Random.onUnitSphere * 22.5f;
            
            // Physics.Raycast(insideUnitSphere, -insideUnitSphere, out RaycastHit hit, 100, appleLayer);
            Instantiate(pickupPf, onUnitSphere, Quaternion.LookRotation(onUnitSphere), transform);
        }

        for (int i = 0; i < _progressionUnit.posionNum; i++)
        {
            Vector3 onUnitSphere = Random.onUnitSphere * 22.5f;
            // Physics.Raycast(insideUnitSphere, -insideUnitSphere, out RaycastHit hit, 100, appleLayer);
            Instantiate(poisonPf, onUnitSphere, Quaternion.LookRotation(onUnitSphere), transform);
        }
    }
}