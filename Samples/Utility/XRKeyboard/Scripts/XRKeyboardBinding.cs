using Naukri.InspectorMaid;
using Naukri.Moltk.MVU;
using UnityEngine;

public abstract class XRKeyboardBinding : ConsumerBehaviour
{
    public bool useGlobalKeyboard = true;

    [SerializeField, DisableIf(nameof(useGlobalKeyboard))]
    private XRKeyboardController _keyboard;

    public XRKeyboardController Keyboard
    {
        get
        {
            if (_keyboard == null)
            {
                if (useGlobalKeyboard)
                {
                    _keyboard = FindAnyObjectByType<XRKeyboardController>(FindObjectsInactive.Include);
                }
                if (_keyboard == null)
                {
                    throw new System.Exception("Keyboard not found");
                }
            }
            return _keyboard;
        }
    }
}
