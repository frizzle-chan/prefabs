using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

public class glowstickbutton : UdonSharpBehaviour
{

    private VRCObjectPool pool;
    private VRCPickup[] glowstickPickups;
    private VRCPlayerApi player;
    private void Start()
    {
        player = Networking.LocalPlayer;
        pool = (VRCObjectPool)GetComponentInParent(typeof(VRCObjectPool));
        glowstickPickups = new VRCPickup[pool.Pool.Length];
        for (int i = 0; i < glowstickPickups.Length; i++)
        {
            glowstickPickups[i] = (VRCPickup)pool.Pool[i].GetComponent(typeof(VRCPickup));
        }
    }

    public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "SpawnGlowStick");
    }

    public void SpawnGlowStick()
    {
        if (!player.IsOwner(pool.gameObject))
        {
            return;
        }

        // clean up unused glow sticks
        foreach(VRCPickup pickup in glowstickPickups)
        {
            if (pickup.gameObject.activeSelf && !pickup.IsHeld)
            {
                pool.Return(pickup.gameObject);
            }
        }


        var stick = pool.TryToSpawn();
        if (!Utilities.IsValid(stick))
        {
            return;
        }

        // set glow stick on its side
        var gsRotation = pool.transform.rotation * Quaternion.Euler(0, 0, 90); 

        stick.gameObject.transform.SetPositionAndRotation(pool.transform.position, gsRotation);
    }
}
