/// <summary>
/// Menu Code Behind
/// Author: Justin Bonello
/// </summary>

using System;
using System.Windows;
using System.Windows.Controls;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for TextEditorMenu.xaml
    /// </summary>
    public partial class TextEditorMenu : UserControl
    {
        public TextEditorMenu()
        {
            InitializeComponent();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Properties.Resources.description, Properties.Resources.about);
        }
    }
}