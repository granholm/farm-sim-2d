using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask interactLayerMask;
    public Rigidbody2D rig;
    public SpriteRenderer sr;

    private Vector2 moveInput;
    private bool interactInput;

    private Vector2 facingDir;

    private void Update()
    {
        if (moveInput.magnitude != 0.0f)
        {
            facingDir = moveInput.normalized;
            sr.flipX = moveInput.x > 0;
        }

        if (interactInput)
        {
            TryInteractTile();
            interactInput = false;
        }
    }


    void FixedUpdate()
    {
        rig.velocity = moveInput.normalized * moveSpeed;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            interactInput = true;
        }
    }

    void TryInteractTile ()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + facingDir, Vector3.up, 0.1f, interactLayerMask);
        if(hit.collider != null)
        {
            FieldTile tile = hit.collider.GetComponent<FieldTile>();
            tile.Interact();        }
    }


}
