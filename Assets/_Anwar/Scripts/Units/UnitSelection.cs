
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelection : MonoBehaviour
{
    private Camera m_Camera;

    public List<Unit> SelectedUnits { get; } = new List<Unit>();

    [SerializeField] private LayerMask layerMask;

    void Start()
    {
        m_Camera = Camera.main;
    }



    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            DeSelect();
            // Start Selection area for multiple selection
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }

    }

    private void DeSelect()
    {
        foreach (var unit in SelectedUnits)
        {
            unit.DeSelect();
        }

        SelectedUnits.Clear();
    }

    private void ClearSelectionArea()
    {
        Ray ray = m_Camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            return;

        if (!hit.collider.TryGetComponent(out Unit unit))
            return;


        if (!unit.authority)
            return;

        SelectedUnits.Add(unit);


        Select();
    }


    private void Select()
    {
        foreach (Unit unit in SelectedUnits)
        {
            unit.Select();
        }
    }
}