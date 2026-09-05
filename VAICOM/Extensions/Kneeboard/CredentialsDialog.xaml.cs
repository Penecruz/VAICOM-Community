using System.Windows;

namespace VAICOM.Extensions.Kneeboard
{
    public partial class CredentialsDialog : Window
    {
        public CredentialsDialog()
        {
            InitializeComponent();
        }

        // Manually load the XAML for cases where the generated InitializeComponent
        // is not available (build tooling issues). This mirrors the generated
        // implementation by loading the compiled XAML resource at runtime.
        private void InitializeComponent()
        {
            var resourceLocater = new System.Uri("/VAICOM;component/Extensions/Kneeboard/CredentialsDialog.xaml", System.UriKind.Relative);
            System.Windows.Application.LoadComponent(this, resourceLocater);
        }

        public string ClientId
        {
            get
            {
                var tb = this.FindName("ClientIdText") as System.Windows.Controls.TextBox;
                return tb?.Text.Trim() ?? string.Empty;
            }
        }

        public string ClientSecret
        {
            get
            {
                var pb = this.FindName("ClientSecretBox") as System.Windows.Controls.PasswordBox;
                return pb?.Password ?? string.Empty;
            }
        }

        public string ClientType
        {
            get
            {
                var cb = this.FindName("ClientTypeBox") as System.Windows.Controls.ComboBox;
                return (cb?.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content as string ?? "Device Flow";
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
