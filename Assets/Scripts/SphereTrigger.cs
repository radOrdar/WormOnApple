using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class SphereTrigger : MonoBehaviour
{
    public Action<Collider> onTrigger;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        onTrigger?.Invoke(other);
    }
}