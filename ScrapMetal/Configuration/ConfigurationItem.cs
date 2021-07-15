namespace ScrapMetal.Configuration
{
    public class ConfigurationItem<T>
    {
        internal readonly string _bindingName;

        public T Value { get; set; }

        internal ConfigurationItem(string bindingName)
        {
            _bindingName = bindingName;
        }
    }
}