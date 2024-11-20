using FlatMoney.Views.Details;
using FlatMoney.Views.General;

namespace FlatMoney
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddFlatPage), typeof(AddFlatPage));
        }
    }
}
