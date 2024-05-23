using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    public Joystick joystick;
    public float speed;
    public Transform raycastPoint;
    public Transform head;
    public SphereTrigger sphereTrigger;
    public SnakeBodyPart bodyPartPf;

    private List<SnakeBodyPart> bodyParts = new();
    private LayerMask layerMask;
    private Vector3 direction;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        sphereTrigger.onTrigger += OnTrigger;
        layerMask = LayerMask.GetMask("Apple");
        mainCamera = Camera.main;
        direction = mainCamera.transform.TransformDirection(Vector3.up);
    }

    // Update is called once per frame

    void Update()
    {
        for (int i = bodyParts.Count - 1; i > -1; i--)
        {
            if (i == 0)
                bodyParts[i].transform.position = head.position;
            else
            {
                bodyParts[i].transform.position = bodyParts[i - 1].transform.position;
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
        transform.Rotate(rotationAxis, speed * Time.deltaTime, Space.World);

        Physics.Raycast(raycastPoint.position, -transform.up, out RaycastHit hit, 100, layerMask);
        head.position = hit.point;
    }

    private void OnTrigger(Collider obj)
    {
        Destroy(obj.gameObject);
        bodyParts.Add(Instantiate(bodyPartPf));
        print($"Triggered {Time.time}");
    }
}
