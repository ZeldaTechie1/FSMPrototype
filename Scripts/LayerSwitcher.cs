using Godot;
using Godot.Collections;

public partial class LayerSwitcher : Node3D
{
    [Export] Array<MeshInstance3D> objectsToSwitch;

    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event is InputEventMouseButton buttonClick)
        {
            if(buttonClick.IsActionPressed("SwitchLayer"))
            {
                foreach (MeshInstance3D node in objectsToSwitch)
                {
                    bool isLastLayer = node.GetLayerMaskValue(20);
                    node.SetLayerMaskValue(1, isLastLayer);
                    node.SetLayerMaskValue(20, !isLastLayer);
                }
            }
        }
    }
}
