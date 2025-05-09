using Mirror;
using Tanks.Complete;
using UnityEngine;

public class CameraAssigner : NetworkBehaviour
{
    private CameraControl camControl;

    public override void OnStartAuthority()
    {
        camControl = Camera.main.GetComponentInParent<CameraControl>();

        if (camControl != null)
        {
            camControl.m_Targets.Add(this.transform);
        }
    }
}
