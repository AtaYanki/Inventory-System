using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class Objects : MonoBehaviour, IPointerDownHandler
{
    bool isOpen;

    void Start()
    {
        Grid grid = transform.parent.parent.GetComponent<Grid>();
        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(cellPosition);
    }

    void Update()
    {
        Grid grid = transform.parent.parent.GetComponent<Grid>();
        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(cellPosition);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("test");
            isOpen = true;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player" && !isOpen)
        {
            Debug.Log("detected");
        }
    }
}
