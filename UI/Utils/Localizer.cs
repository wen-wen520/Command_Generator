using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Markup;

namespace Command_Generator.UI.Utils
{
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    public class ResourceString : MarkupExtension
    {
        public string Name { get; set; }

        protected override object ProvideValue()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidOperationException("The resource name must be set.");
            }

            var resourceLoader = ResourceLoader.GetForCurrentView();
            return resourceLoader.GetString(Name);
        }
    }
}