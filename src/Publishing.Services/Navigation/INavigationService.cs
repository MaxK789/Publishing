#if WINDOWS
namespace Publishing.Services
{
    using System;
    using System.Windows.Forms;

    public interface INavigationService
    {
        void Navigate<T>(Form current) where T : Form;
    }
}
#endif
