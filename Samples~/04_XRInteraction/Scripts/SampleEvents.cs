using UnityEngine;
using UnityEngine.UI;

public class SampleEvents : MonoBehaviour
{
    public MeshRenderer switchRenderer;

    public Text switchText;

    public Text handwheelText;

    public Text knobStateText;

    public Text knobAngleText;

    public void UpdateSwitch(string state)
    {
        switchRenderer.material.color = state switch
        {
            "blue" => Color.blue,
            "red" => Color.red,
            "green" => Color.green,
            _ => throw new System.NotImplementedException(),
        };
    }

    public void UpdateSwitchText(string state)
    {
        switchText.text = $"State: {state}";
    }

    public void UpdateHandwheelText(float angle)
    {
        handwheelText.text = $"Angle: {angle: 0.0}";
    }

    public void UpdateKnobAngleText(float angle)
    {
        knobAngleText.text = $"Angle: {angle: 0.0}";
    }

    public void UpdateKnobStateText(string state)
    {
        knobStateText.text = $"State: {state}";
    }
}
