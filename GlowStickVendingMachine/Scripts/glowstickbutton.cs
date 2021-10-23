using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

public class glowstickbutton : UdonSharpBehaviour
{
    public VRCObjectPool pool;

    public Transform spawnPos;

    public override void Interact()
    {
        Networking.SetOwner(Networking.LocalPlayer, pool.gameObject);

        var stick = pool.TryToSpawn();

        if (stick == null)
        {
            return;
        }

        Networking.SetOwner(Networking.LocalPlayer, stick);
        stick.gameObject.transform.SetPositionAndRotation(spawnPos.position, spawnPos.rotation);
    }
}
