using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for TextEditorToolBar.xaml
    /// </summary>
    public partial class TextEditorToolBar : UserControl
    {
        public TextEditorToolBar()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (double i = 8; i < 48; i += 2)
            {
                fontSize.Items.Add(i);
            }
        }
        public void SynchronizeWith(TextSelection selection)
        {
            isSynchronizing = true;
            Synchronized<double>(selection, TextBlock.FontSizeProperty,
                                              SetFontSize);
            Synchronized<FontWeight>(selection, TextBlock.FontWeightProperty, SetFontWeight);
            isSynchronizing = false;

        }

        private void Synchronized<T>(TextSelection selection, DependencyProperty property, Action<T> methodToCall)
        {
            object value = selection.GetPropertyValue(property);
            if (value != DependencyProperty.UnsetValue)
            {
                methodToCall((T)value);
            }
        }
        private void SetFontSize(double size)
        {
            fontSize.SelectedValue = size;
        }

        private void SetFontWeight(FontWeight weight)
        {
            boldButton.IsChecked = weight == FontWeights.Bold;
        }

        public bool isSynchronizing { get; private set; }

    }    
}
