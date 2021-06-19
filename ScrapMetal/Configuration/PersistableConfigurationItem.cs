using System;

namespace ScrapMetal.Configuration
{
    public class PersistableConfigurationItem<T>
    {
        private PersistentConfiguration _configuration;
        private readonly string _bindingName;
        private T _value;

        /// <summary>
        /// If true this configuration item will be persisted whenever it is changed.
        /// <p>
        /// Equivalent to calling <see cref="Persist()"> after any changes
        /// </p>
        /// </summary>
        public bool PersistOnChanges { get; set; }

        /// <summary>
        /// The value being managed by this <see cref="PersistableConfigurationItem{T}"/>
        /// </summary>
        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (PersistOnChanges)
                {
                    Persist();
                }
            }
        }

        internal PersistableConfigurationItem(PersistentConfiguration configuration, string bindingName)
        {
            _configuration = configuration;
            _bindingName = bindingName;
        }

        /// <summary>
        /// Persists this configuration item to its backing store, optionally saving the store as well
        /// </summary>
        /// <param name="persist"></param>
        public void Persist(bool persist = false)
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("Persistable item was initialized without a persistable configuration store.");
            }
            typeof(PersistentConfiguration).GetProperty(_bindingName).SetValue(_configuration, Value);

            if (persist)
            {
                _configuration.Write();
            }
        }

        /// <summary>
        /// Persists this configuration item to a provided backing store, optionally saving the store as well.
        /// If specified, the provided store will replace the item's current backing store
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="persist"></param>
        /// <param name="setPermanentStore"></param>
        public void Persist(PersistentConfiguration configuration, bool persist = false, bool setPermanentStore = false)
        {
            if (configuration == null)
            {
                throw new InvalidOperationException("Provided persistable configuration store cannot be null.");
            }
            typeof(PersistentConfiguration).GetProperty(_bindingName).SetValue(configuration, Value);

            if (persist)
            {
                configuration.Write();
            }

            if (setPermanentStore)
            {
                _configuration = configuration;
            }
        }
    }
}