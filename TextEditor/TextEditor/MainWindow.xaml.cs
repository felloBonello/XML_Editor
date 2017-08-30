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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   public partial class MainWindow: Window
    {
	    private DocumentManager _documentManager;
	    public MainWindow()
	    {
		    InitializeComponent();
		    _documentManager = new DocumentManager(body);
		    if(_documentManager.OpenDocument())
		    	status.Text = "Document loaded."; 
	    }

        private void TextEditorToolBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox source = e.OriginalSource as ComboBox;
            if (source == null) return;
            switch (source.Name)
            {
                case "fonts":
                    _documentManager.ApplyToSelection(TextBlock.FontFamilyProperty, source.SelectedItem);
                    break;
                case "fontSize":
                    _documentManager.ApplyToSelection(TextBlock.FontSizeProperty, source.SelectedItem);
                    break;
            }
            body.Focus();
        }

       private void body_SelectionChanged(object sender, RoutedEventArgs e)
       {
           toolbar.SynchronizeWith(body.Selection);
       }

        private void NewDocument(object sender, ExecutedRoutedEventArgs e)
        {
            _documentManager.NewDocument();
            status.Text = "New Document";
        }

        private void OpenDocument(object sender, ExecutedRoutedEventArgs e)
        {
            _documentManager.OpenDocument();
            status.Text = "Open Document";
        }

        private void SaveDocument(object sender, ExecutedRoutedEventArgs e)
        {
            _documentManager.SaveDocument();
            status.Text = "Document Saved";
        }

        private void SaveDocumentAs(object sender, ExecutedRoutedEventArgs e)
        {
            _documentManager.SaveDocumentAs();
            status.Text = "Document Saved";

        }

        private void ApplicationClose(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void ApplicationPrint(object sender, ExecutedRoutedEventArgs e)
        {
            PrintDialog dlg = new PrintDialog();
            if(dlg.ShowDialog() == true)
            {
               dlg.PrintDocument((((IDocumentPaginatorSource)body.Document).DocumentPaginator), "Text Editor Printing");
            }
        }
    }
}
