
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;
using VRC.SDK3.Components;

public class glowstickclean : UdonSharpBehaviour
{
    public bool dropped = true;

    private int gcTimeoutSeconds = 60;
    private void Start()
    {
        dropped = true;
    }

    public override void OnPickup()
    {
        dropped = false;
    }

    public override void OnDrop()
    {
        _Drop();
    }

    public void _Drop()
    {
        dropped = true;
        SendCustomEventDelayedSeconds(nameof(_CleanStick), gcTimeoutSeconds);
    }

    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        var pickup = (VRCPickup)GetComponentInParent(typeof(VRCPickup));
        if (pickup.IsHeld)
        {
            return;
        }

        _Drop();
    }

    public void _CleanStick()
    {
        if (!dropped)
        {
            return;
        }

        var parent = (VRCObjectPool)GetComponentInParent(typeof(VRCObjectPool));
        Networking.SetOwner(Networking.LocalPlayer, parent.gameObject);
        parent.Return(this.gameObject);

    }

}
