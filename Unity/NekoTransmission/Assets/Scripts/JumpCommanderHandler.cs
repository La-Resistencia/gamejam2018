using UnityEngine;


public class JumpCommanderHandler: MonoBehaviour
{
    public Rigidbody2D CatRigidbody2D;

    public void Start()
    {
        
    }

    private void Update()
    {
        CatRigidbody2D.AddForce(5f*Vector2.right, ForceMode2D.Force);
    }

    public void Jump()
    {
        CatRigidbody2D.AddForce(10*Vector2.up, ForceMode2D.Impulse);
    }
}
