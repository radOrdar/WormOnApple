using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private Pickup pickupPf;
    [SerializeField] private int numOfPickups;
    private LayerMask appleLayer;

    // Start is called before the first frame update
    void Start()
    {
        appleLayer = LayerMask.GetMask("Apple");
        for (int i = 0; i < numOfPickups; i++)
        {
            Vector3 insideUnitSphere = Random.insideUnitSphere * 100;
            Physics.Raycast(insideUnitSphere, -insideUnitSphere, out RaycastHit hit, 100, appleLayer);
            Instantiate(pickupPf, hit.point, Quaternion.LookRotation(insideUnitSphere));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print($"Collission {Time.time}");

    }

    // Update is called once per frame
    void Update()
    { }
}