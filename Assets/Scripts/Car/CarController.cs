using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform carVisuals;
    [SerializeField] private Transform modelContainer;
    [SerializeField] private Rigidbody ballEngine;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Settings")]
    [SerializeField] private CarSettings settings;

    [Header("Visuals")]
    [SerializeField] private float groundAlignSpeed;

    private float accelerateInput;
    private float steeringInput;

    private bool isGrounded;

    public UnityAction<Vector3> OnCarLeftGround;

    public void UpdateAccelerateInput(float accelerateInput)
    {
        this.accelerateInput = accelerateInput;
    }

    public void UpdateSteeringInput(float steeringInput)
    {
        this.steeringInput = steeringInput;
    }

    public void SetSettings(CarSettings carSettings)
    {
        settings = carSettings;

        Instantiate(settings.carModel, modelContainer);
    }

    private void Update()
    {
        CheckGrounded();

        carVisuals.position = ballEngine.transform.position;

        if (steeringInput != 0)
        {
            carVisuals.Rotate(Vector3.up, accelerateInput * steeringInput * settings.turnStrength * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        ballEngine.drag = isGrounded ? settings.groundDrag : settings.airDrag;

        if (ballEngine.velocity.magnitude < settings.maxSpeed && accelerateInput != 0 && isGrounded)
            ballEngine.AddForce(carVisuals.forward * accelerateInput * (accelerateInput > 0 ? settings.fowardAccel : settings.reverseAccel), ForceMode.Acceleration);
    }

    private void CheckGrounded()
    {
        bool wasGrounded = isGrounded;

        isGrounded = Physics.Raycast(carVisuals.position, Vector3.down, out RaycastHit hit, 1f, whatIsGround);

        if (wasGrounded && !isGrounded)
        {
            OnCarLeftGround?.Invoke(ballEngine.position);
        }

        if (isGrounded)
        {
            Quaternion groundAlignRotation = Quaternion.FromToRotation(carVisuals.transform.up, hit.normal) * carVisuals.transform.rotation;
            carVisuals.transform.rotation = Quaternion.Lerp(carVisuals.transform.rotation, groundAlignRotation, Time.deltaTime * groundAlignSpeed);
        }
    }

    public void OverridePosition(Vector3 resetPos, Quaternion resetRot)
    {
        ballEngine.velocity = Vector3.zero;
        ballEngine.position = resetPos;
        carVisuals.rotation = resetRot;
    }
}
