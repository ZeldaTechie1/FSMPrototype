using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class CameraRotate : Node3D
{
	[Export] float mouse_sensitivity = .005f;
    [Export(PropertyHint.Range, "-90,0,0.1")] float min_vertical_angle = -Mathf.Pi / 2;
    [Export(PropertyHint.Range, "0,90,0.1")] float max_vertical_angle = Mathf.Pi / 4;
    [Export] SpringArm3D normalViewSpring;
    [Export] SpringArm3D lowFOVViewSpring;
    [Export] Camera3D normalCamera;
    [Export] Camera3D lowFOVCamera;

    float normalFOV;
    float lowFOV;
    float springCurrentLength;
    float playerVisualHeight;
    float newCameraDistance;
    float previousCameraDistance;

    public override void _Ready()
    {
        base._Ready();

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta)
    {
        normalFOV = normalCamera.Fov;
        lowFOV = lowFOVCamera.Fov;
        springCurrentLength = normalViewSpring.GetHitLength();
        playerVisualHeight = 2.0f * Mathf.Tan(((normalFOV * Mathf.Pi)/180) / 2.0f) * springCurrentLength;
        newCameraDistance = playerVisualHeight/(2.0f * Mathf.Tan(((lowFOV * MathF.PI)/180) / 2));
        lowFOVViewSpring.SpringLength = newCameraDistance;

        if(newCameraDistance != previousCameraDistance)
        {
            GD.Print($"normalFOV = {normalFOV}\n" +
                 $"spring current Length = {springCurrentLength}\n" +
                 $"player height = {playerVisualHeight}\n" +
                 $"lowFOV = {lowFOV}\n" +
                 $"new distance = {newCameraDistance}");
        }

        previousCameraDistance = newCameraDistance;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

		if (@event is InputEventMouseMotion motion)
		{
            //rotate x component on up axis, rotate y component on model left axis
			Rotation -= ((Vector3.Up * (motion.Relative.X * mouse_sensitivity)) + (Vector3.ModelLeft * (motion.Relative.Y * mouse_sensitivity)));
            //clamps x rotation to -90° and 45°, also wraps y rotation so that we always have values from 0°s - 360°
            Vector3 clampedRotation = new Vector3(Mathf.Clamp(Rotation.X,Mathf.DegToRad(min_vertical_angle),Mathf.DegToRad(max_vertical_angle)),
                                                  Mathf.Wrap(Rotation.Y,0.0f,Mathf.Tau),
                                                  Rotation.Z);
            Rotation = clampedRotation;
		}
    }
}
