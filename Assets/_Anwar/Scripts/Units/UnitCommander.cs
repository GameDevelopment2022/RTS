using System;
using AnwarMajid;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommander : MonoBehaviour
{
    [SerializeField] private UnitSelection selectionHandler;

    [SerializeField] private LayerMask layerMask;
    private Camera m_Camera;

    void Start()
    {
        m_Camera = Camera.main;
    }


    private void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame)
            return;

        Ray ray = m_Camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            return;

        if (hit.collider.TryGetComponent(out Targetable targetable))
        {
            if (targetable.isOwned)
            {
                TryMove(hit.point);
                return;
            }

            TryTarget(targetable);
            return;
        }
            
        
        
        
        TryMove(hit.point);


    }

    private void TryTarget(Targetable target)
    {
        foreach (var unit in selectionHandler.SelectedUnits)
        {
            unit.Targeter.CmdSetTarget(target.gameObject);
        }
    }
    private void TryMove(Vector3 point)
    {
        foreach (var unit in selectionHandler.SelectedUnits)
        {
            unit.UnitMovement.CmdMove(point);
        }
    }
}
