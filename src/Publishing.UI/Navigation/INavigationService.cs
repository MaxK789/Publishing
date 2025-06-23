using System.Windows.Forms;

namespace Publishing.Services
{
    public interface INavigationService
    {
        void Navigate<T>(Form current) where T : Form;
    }
}
