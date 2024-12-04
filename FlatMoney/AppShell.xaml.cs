using FlatMoney.Views.Details;

namespace FlatMoney
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddFlatPage), typeof(AddFlatPage));
            Routing.RegisterRoute(nameof(AddServicePage), typeof(AddServicePage));
            Routing.RegisterRoute(nameof(AddExpenseTypePage), typeof(AddExpenseTypePage));
            Routing.RegisterRoute(nameof(AddExpensePage), typeof(AddExpensePage));
            Routing.RegisterRoute(nameof(AddReservationPage), typeof(AddReservationPage));
        }
    }
}
