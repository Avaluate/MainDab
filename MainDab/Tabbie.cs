using System.Windows;
using System.Windows.Controls;
namespace MainDabRedo
{
    static class Tabbie
    {
        // Find the template function as needed for the close icon for the Sentinel tabs
        public static T GetTemplateItem<T>(this Control elem, string name)
        {
            return elem.Template.FindName(name, (FrameworkElement)elem) is T name1 ? name1 : default(T);
        }
    }
}
