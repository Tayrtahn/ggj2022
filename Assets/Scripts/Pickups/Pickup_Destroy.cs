using UnityEngine;

public class Pickup_Destroy : Pickup
{
    protected override void Collect(Collider other)
    {
        // TODO: Wipe stage? All Balls and pickups?

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
