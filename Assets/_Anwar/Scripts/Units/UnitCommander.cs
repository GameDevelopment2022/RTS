using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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

        TryMove(hit.point);


    }

    private void TryMove(Vector3 point)
    {
        foreach (var unit in selectionHandler.SelectedUnits)
        {
            unit.GetUnitMovement().CmdMove(point);
        }
    }
}
