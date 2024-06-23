using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputProvider
{
    private static PlayerActions playerActions;

    private static bool isSetup = false;

    private static PlayerActions Actions => isSetup ? playerActions : SetupPlayerActions();
    private static PlayerActions.DrivingActions DrivingActions => Actions.Driving;

    private static PlayerActions SetupPlayerActions()
    {
        playerActions = new PlayerActions();
        playerActions.Enable();

        playerActions.Driving.Enable();

        isSetup = true;

        return playerActions;
    }

    public static float AccelerateInput => DrivingActions.Accelerate.ReadValue<float>();
    public static float SteeringInput => DrivingActions.Steering.ReadValue<float>();
}
