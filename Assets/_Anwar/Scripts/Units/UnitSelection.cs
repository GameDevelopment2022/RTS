using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelection : MonoBehaviour
{
    [SerializeField] private RectTransform unitSelectionArea = null;


    public List<Unit> SelectedUnits = new List<Unit>();

    [SerializeField] private LayerMask layerMask;

    private Camera _mCamera;
    private Vector2 _startPosition;
    private RTS_Player _player;

    private readonly float _doubleClickThreshold = 0.3f;
    private float _lastClickTime = -1f;
    private UnitType _currentUnitType;

    void Start()
    {
        _mCamera = Camera.main;
    }


    private void Update()
    {
        if (_player == null)
        {
            _player = NetworkClient.connection.identity.GetComponent<RTS_Player>();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }


    private void StartSelectionArea()
    {
        
        float timeSinceLastClick = Time.time - _lastClickTime;
        
        if (timeSinceLastClick <= _doubleClickThreshold)
        {
            SelectAllOnScreen();
        }
        else
        {
            
            if (!Keyboard.current.leftShiftKey.isPressed)
            {
                DeSelect();
            }

            unitSelectionArea.gameObject.SetActive(true);

            _startPosition = Mouse.current.position.ReadValue();

            UpdateSelectionArea();
        }
        
        _lastClickTime = Time.time;
    }

    private void UpdateSelectionArea()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - _startPosition.x;
        float areaHeight = mousePosition.y - _startPosition.y;

        Vector2 size = new Vector2(MathF.Abs(areaWidth), MathF.Abs(areaHeight));

        unitSelectionArea.sizeDelta = size;
        unitSelectionArea.anchoredPosition = _startPosition + new Vector2(areaWidth / 2f, areaHeight / 2f);
    }

    private void ClearSelectionArea()
    {
        unitSelectionArea.gameObject.SetActive(false);

        if (unitSelectionArea.sizeDelta.magnitude == 0f)
        {
            Ray ray = _mCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
                return;

            if (!hit.collider.TryGetComponent(out Unit unit))
                return;


            if (!unit.authority)
                return;

            _currentUnitType = unit.UnitType;

            SelectedUnits.Add(unit);


            Select();

            return;
        }

        Vector2 min = unitSelectionArea.anchoredPosition - unitSelectionArea.sizeDelta / 2;
        Vector2 max = unitSelectionArea.anchoredPosition + unitSelectionArea.sizeDelta / 2;


        foreach (var unit in _player.MyUnits)
        {
            if (SelectedUnits.Contains(unit))
                continue;

            Vector2 screenPos = _mCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x
                                    && screenPos.y > min.y && screenPos.y < max.y)
            {
                SelectedUnits.Add(unit);
            }
        }

        Select();
    }

    private void SelectAllOnScreen()
    {
        foreach (var unit in _player.MyUnits)
        {
            if (SelectedUnits.Contains(unit))
                continue;

            if (unit.UnitType != _currentUnitType)
                continue;

            Vector2 screenPos = _mCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x >= 0 && screenPos.x <= Screen.width &&
                screenPos.y >= 0 && screenPos.y <= Screen.height)
            {
                SelectedUnits.Add(unit);
            }
        }

        Select();
    }



    private void Select()
    {
        foreach (Unit unit in SelectedUnits)
        {
            unit.Select();
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
}