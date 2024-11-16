using FlatMoney.Views.Details;

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
