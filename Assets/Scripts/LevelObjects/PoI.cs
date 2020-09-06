using UnityEngine;

public class PoI : MonoBehaviour
{
    private DestroyVegetation controller;
    private int idNr = -1;

    public DestroyVegetation Controller { get => controller;}
    public int Number { get => idNr; }

    public void Setup(DestroyVegetation dV, int number)
    {
        controller = dV;
        idNr = number;
    }


    public override bool Equals(object other)
    {
        if(other.GetType().Equals(typeof(GameObject)))
        {
            if (ReferenceEquals(this.gameObject, other))
            {
                return true;
            }
        }
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
