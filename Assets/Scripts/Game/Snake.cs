using System.Collections.Generic;
using Services;
using Services.Ads;
using Services.Audio;
using Services.Event;
using StaticData;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public Joystick joystick;
    public Transform raycastPoint;
    public Transform head;
    public SphereTrigger sphereTrigger;
    public SnakeBodyPart bodyPartPf;
    [SerializeField] private ParticleSystem destroyFx;

    private List<SnakeBodyPart> bodyParts = new();
    private LayerMask _appleLayerMask;
    private Vector3 direction;
    private Camera mainCamera;

    private IAudioService _audioService;
    private IEventService _eventService;

    private ProgressionUnit _progressionUnit;
    private bool _active = false;
    private int _pickupLayer, _poisonLayer;

    public void Init(ref ProgressionUnit progressionUnit)
    {
        _audioService = ServiceLocator.Instance.Get<IAudioService>();
        _eventService = ServiceLocator.Instance.Get<IEventService>();
        _progressionUnit = progressionUnit;
        sphereTrigger.onTrigger += OnTrigger;
        _appleLayerMask = LayerMask.GetMask("Apple");
        _pickupLayer = LayerMask.NameToLayer("Pickup");
        _poisonLayer = LayerMask.NameToLayer("Poison");
        mainCamera = Camera.main;
        direction = mainCamera.transform.TransformDirection(Vector3.up);

        _active = true;
    }

    public void Stop()
    {
        _active = false;
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
            _eventService.OnPickupPicked();
            SnakeBodyPart snakeBodyPart = Instantiate(bodyPartPf);
            snakeBodyPart.name = $"SnakePart{bodyParts.Count}";
            bodyParts.Add(snakeBodyPart);
            _audioService.PlayPickup();
            Instantiate(destroyFx, coll.transform.position, Quaternion.identity);
        } else if (coll.gameObject.layer == _poisonLayer)
        {
            _eventService.OnPoisonPicked();
            _active = false;
        }
        Destroy(coll.gameObject);
    }
}
