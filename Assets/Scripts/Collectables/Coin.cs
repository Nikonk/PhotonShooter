using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Inventory>(out Inventory inventory))
        {
            inventory.AddCoin();

            Destroy(gameObject);
        }
    }
}
