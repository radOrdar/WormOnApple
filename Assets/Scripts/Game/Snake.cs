using System.Collections.Generic;
using StaticData;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public Joystick joystick;
    public Transform raycastPoint;
    public Transform head;
    public SphereTrigger sphereTrigger;
    public SnakeBodyPart bodyPartPf;

    private List<SnakeBodyPart> bodyParts = new();
    private LayerMask _appleLayerMask;
    private Vector3 direction;
    private Camera mainCamera;

    private ProgressionUnit _progressionUnit;
    private bool _active = false;
    private int _pickupLayer, _poisonLayer;

    public void Init(ref ProgressionUnit progressionUnit)
    {
        _progressionUnit = progressionUnit;
        sphereTrigger.onTrigger += OnTrigger;
        _appleLayerMask = LayerMask.GetMask("Apple");
        _pickupLayer = LayerMask.NameToLayer("Pickup");
        _poisonLayer = LayerMask.NameToLayer("Poison");
        mainCamera = Camera.main;
        direction = mainCamera.transform.TransformDirection(Vector3.up);

        _active = true;
    }
    
    void Update()
    {
        if(_active == false)
            return;
        
        for (int i = bodyParts.Count - 1; i > -1; i--)
        {
            if (i == 0)
            {
                bodyParts[i].transform.position = head.position;
                bodyParts[i].transform.rotation = head.rotation;
            } else
            {
                bodyParts[i].transform.position = bodyParts[i - 1].transform.position;
                bodyParts[i].transform.rotation = bodyParts[i - 1].transform.rotation;
            }
        }
        Vector2 joystickDirection = joystick.Direction;
        if (joystickDirection.sqrMagnitude > 0)
        {
            direction = mainCamera.transform.TransformDirection(joystickDirection);
        } else
        {
            direction = head.forward;
        }
        Debug.DrawLine(head.position, head.position + direction * 10, Color.red);

        head.rotation = Quaternion.LookRotation(direction, transform.up);
        Vector3 rotationAxis = Vector3.Cross(transform.up, direction);
        transform.Rotate(rotationAxis, _progressionUnit.speed * Time.deltaTime, Space.World);

        Physics.Raycast(raycastPoint.position, -transform.up, out RaycastHit hit, 100, _appleLayerMask);
        head.position = hit.point;
    }

    private void OnTrigger(Collider coll)
    {
        if (coll.gameObject.layer == _pickupLayer)
        {
            SnakeBodyPart snakeBodyPart = Instantiate(bodyPartPf);
            snakeBodyPart.name = $"SnakePart{bodyParts.Count}";
            bodyParts.Add(snakeBodyPart);
        } else if (coll.gameObject.layer == _poisonLayer)
        {
            _active = false;
        }
        Destroy(coll.gameObject);
    }
}
