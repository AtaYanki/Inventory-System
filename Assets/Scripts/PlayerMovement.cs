using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    interact
}

public class PlayerMovement : MonoBehaviour
{
    public float itemDropped;
    public InventoryObject inventory;
    public PlayerState currentState;
    public float speed;
    Animator animator;
    Vector3 movement;
    Rigidbody2D rb;

    void Start()
    {
        inventory.Load();
        currentState = PlayerState.walk;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement = Vector3.zero;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        MoveCharacter();
        if(itemDropped > 0)
        {
            itemDropped -= Time.deltaTime;
        }
    }

    void MoveCharacter()
    {
        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(itemDropped <= 0)
        {
            var item = collider.GetComponent<GroundItem>();
            if(item)
            {
                inventory.AddItem(new Item(item.item), 1);
                Destroy(collider.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Save();
        inventory.Container.Items = new InventorySlot[20];
    }
}
