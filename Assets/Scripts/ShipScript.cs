using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    public float DryMass;
    public float FuelMass;
    public float ThrustFuelConsumptionRate;
    public float RotationFuelConsumptionRate;
    public float ExhaustVelocity;
    public float ManeuveringSpeed;
    public Light EngineLight;
    private new Rigidbody rigidbody;
    private float HorizontalSteering;
    private float VerticalSteering;
    private bool Thrust;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = DryMass + FuelMass;
    }

    // Update is called once per frame
    private void Update()
    {
        HorizontalSteering = Input.GetAxis("Horizontal");
        VerticalSteering = Input.GetAxis("Vertical");
        Thrust = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        if (Thrust)
        {
            ApplyThrust(Time.fixedDeltaTime);
            EngineLight.intensity = 2;
        }
        else
        {
            EngineLight.intensity = 1;
        }

        if (HorizontalSteering != 0f || VerticalSteering != 0f)
            ApplyRotation(Time.fixedDeltaTime);
    }

    void ApplyThrust(float deltaTime)
    {
        if (FuelMass <= 0)
            return;

        float currentMass = DryMass + FuelMass; // m0
        float fuelToBurn = ThrustFuelConsumptionRate * deltaTime;

        if (fuelToBurn > FuelMass)
            fuelToBurn = FuelMass;

        float finalMass = currentMass - fuelToBurn; // m1

        // ΔV = I * ln( m0 / m1 )
        float dV = ExhaustVelocity * Mathf.Log(currentMass / finalMass);

        Vector3 thrustDirection = transform.up;
        rigidbody.AddForce(dV * rigidbody.mass * thrustDirection, ForceMode.Impulse);

        FuelMass -= fuelToBurn;
        // rigidbody.mass -= fuelToBurn;
    }

    void ApplyRotation(float deltaTime)
    {
        if (FuelMass <= 0)
            return;

         float fuelToBurn = RotationFuelConsumptionRate * deltaTime;

        if (fuelToBurn > FuelMass)
            fuelToBurn = FuelMass;

        float xRotation = ManeuveringSpeed * -VerticalSteering * deltaTime;
        float zRotation = ManeuveringSpeed * HorizontalSteering * deltaTime;

        Quaternion rotation = Quaternion.Euler(xRotation, 0, zRotation);
        rigidbody.MoveRotation(rigidbody.rotation * rotation);

        FuelMass -= fuelToBurn;
        // rigidbody.mass -= fuelToBurn;
    }

}
