public class Comparator
{
    public override bool Equals(object objA, object objB)
    {
        if (objA.GetHashCode() == objB.GetHashCode())
            return true;

        if (ReferenceEquals(objA, objB))
            return true;

        if (objA.GetType() != objB.GetType())
            return false;

        if (objA == null || objB == null)
            return false;

      
    }
}
