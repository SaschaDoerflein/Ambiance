using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ambiance.Views
{
    /// <summary>
    /// Interaction logic for ScenarioView.xaml
    /// </summary>
    public partial class ScenarioView : UserControl
    {
        public ScenarioView()
        {
            InitializeComponent();
        }

        private void txtSelectedName_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindingOperations.GetBindingExpression(txtSelectedName, TextBox.TextProperty).UpdateSource();
        }
    }
}
