namespace Endless.Messaging.Rabbit
{
    /// <summary>
    /// Extension method for generating Hashcodes
    /// </summary>
    public static class HashCodeExtension
    {
        // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        // https://www.bigprimes.net/archive/prime/
        public static int GetHashCode(int seed, int salt, object[] objects)
        {
            unchecked
            {
                int hash = (int)seed;
                foreach (var o in objects)
                {
                    hash = hash * salt;
                    if (o != null)
                    {
                        hash = hash ^ o.GetHashCode();
                    }
                }

                return hash;
            }
        }
    }
}