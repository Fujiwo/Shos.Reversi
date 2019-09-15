using System.Windows;
using System.Windows.Data;

namespace Shos.Reversi.Wpf.Helpers
{
    public static class XamlExtension
    {
        public static void SetBinding(this FrameworkElement @this, DependencyProperty property, string path, BindingMode mode)
        {
            var binding = new Binding(path);
            binding.Mode = mode;
            @this.SetBinding(property, binding);
        }
    }
}
