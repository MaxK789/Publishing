using System;
using System.Windows.Forms;
using Publishing.Services;

namespace Publishing
{
    public abstract class BaseForm : Form
    {
        protected IUserSession _session = null!;
        protected INavigationService _navigation = null!;

        [Obsolete("Designer only", error: false)]
        protected BaseForm() { }

        protected BaseForm(IUserSession session, INavigationService navigation)
        {
            _session = session;
            _navigation = navigation;
        }

        protected void Logout()
        {
            _session.UserId = string.Empty;
            _session.UserName = string.Empty;
            _session.UserType = string.Empty;
            _session.Token = string.Empty;
            _navigation.Navigate<loginForm>(this);
        }
    }
}
