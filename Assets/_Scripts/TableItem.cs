using UnityEngine;

public class TableItem : CameraObject
{
    public override void OnClick()
    {
        Debug.Log($"Clicked on: {itemName} (GameObject: {gameObject.name})");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Table"))
        {
            Debug.Log($"{itemName} has landed on the table.");
        }
    }
}