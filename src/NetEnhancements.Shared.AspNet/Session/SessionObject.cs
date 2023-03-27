using Microsoft.AspNetCore.Http;

namespace NetEnhancements.AspNet.Session
{
    /// <summary>
    /// Stores an object of type <typeparamref name="TValue"/> in the session by key, serializing it to JSON.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class SessionObject<TValue>
        where TValue : class, new()
    {
        private readonly ISession _session;
        private readonly string _key;

        protected SessionObject(ISession session, string key)
        {
            _session = session;
            _key = key;
        }

        /// <summary>
        /// Load the object from the session.
        /// </summary>
        public TValue Load() => _session.ReadObject<TValue>(_key) ?? new TValue();

        /// <summary>
        /// Save the object to the session.
        /// </summary>
        public void Save(TValue value) => _session.WriteObject(_key, value);

        /// <summary>
        /// Remove the object from the session.
        /// </summary>
        public void Clear() => _session.Remove(_key);
    }
}
