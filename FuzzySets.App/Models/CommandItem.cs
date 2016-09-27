using System.Windows.Input;

namespace FuzzySets.App.Models
{
    public class CommandItem
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
    }
}
