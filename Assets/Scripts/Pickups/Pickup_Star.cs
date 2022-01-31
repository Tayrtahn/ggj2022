using UnityEngine;

public class Pickup_Star : Pickup
{
    protected override void Collect(Collider other)
    {
        // TODO: 

        //Destroy(other.gameObject);
        Destroy(gameObject);
    }

    protected override void Goodbye()
    {
        
    }

    protected override PickupEffect GetEffect(GameObject target)
    {
        throw new System.NotImplementedException();
    }
}
