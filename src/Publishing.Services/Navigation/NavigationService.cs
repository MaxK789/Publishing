#if WINDOWS
namespace Publishing.Services
{
    using System;
    using System.Windows.Forms;
    using Microsoft.Extensions.DependencyInjection;

    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _provider;

        public NavigationService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void Navigate<T>(Form current) where T : Form
        {
            current.Hide();
            var form = _provider.GetRequiredService<T>();
            form.Show();
        }
    }
}
#endif
