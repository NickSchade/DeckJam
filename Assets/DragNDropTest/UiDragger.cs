using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class UiDragger : MonoBehaviour
{
    public const string _dragTag = "UiDraggable";

    bool _isDragging = false;

    Vector2 _originalPosition;
    Transform _objectToDrag;
    RawImage _imageToDrag;

    List<RaycastResult> _hits = new List<RaycastResult>();

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _objectToDrag = GetTransformUnderMouse();
            if (_objectToDrag != null)
            {
                _isDragging = true;
                _objectToDrag.SetAsLastSibling();
                _originalPosition = _objectToDrag.position;
                _imageToDrag = _objectToDrag.GetComponent<RawImage>();
                _imageToDrag.raycastTarget = false;
            }
        }

        if (_isDragging)
            _objectToDrag.position = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            if (_objectToDrag != null)
            {
                Transform objectToReplace = GetTransformUnderMouse();
                if (objectToReplace != null)
                {
                    _objectToDrag.position = objectToReplace.position;
                    objectToReplace.position = _originalPosition;
                }
                else
                {
                    _objectToDrag.position = _originalPosition;
                }

                _imageToDrag.raycastTarget = true;
                _objectToDrag = null;
            }
            _isDragging = false;
        }
    }


    GameObject GetObjectUnderMouse()
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;
        EventSystem.current.RaycastAll(ped, _hits);
        if (_hits.Count == 0) return null;
        return _hits.First().gameObject;
    }

    Transform GetTransformUnderMouse()
    {
        GameObject clicked = GetObjectUnderMouse();
        if (clicked != null && clicked.tag == _dragTag)
            return clicked.transform;
        else
            return null;
    }
}
