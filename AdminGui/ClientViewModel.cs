using Framework;

namespace AdminGui
{
    public class ClientViewModel : ObservableObject
    {
        private Admin admin;

        public ClientViewModel(Admin admin)
        {
            this.admin = admin;
        }
    }
}