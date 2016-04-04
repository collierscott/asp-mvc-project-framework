namespace Project.Domain.Models.Entities
{

    /// <summary>
    /// All objects must have an Id. So, they should inherit this class.
    /// Objects are considered equal if they have the same id
    /// </summary>
    public abstract class Entity<TId>
    {

        public TId Id { get; set; }

        public override bool Equals(object obj)
        {

            if (obj == null) return false;

            if (obj.GetType() != GetType()) return false;

            var sameKey = Id.Equals(((Entity<TId>)obj).Id);

            if (sameKey && Id.Equals(default(TId)))
            {
                return ReferenceEquals(this, obj);
            }

            return sameKey;

        }

        public override int GetHashCode()
        {
            if (Id.Equals(default(TId)))
            {
                // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
                return base.GetHashCode();
            }

            return GetType().GetHashCode() ^ Id.GetHashCode();
        }

    }

}