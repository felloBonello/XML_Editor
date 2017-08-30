using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;


namespace TextEditor
{
    class DocumentManager
    {
        private string _currentFile;
        private RichTextBox _textBox;

        public DocumentManager(RichTextBox textBox)
        {
            _textBox = textBox;
        }
        public bool OpenDocument()
        {
            OpenFileDialog dig = new OpenFileDialog();
            if (dig.ShowDialog() == true)
            {
                _currentFile = dig.FileName;

                using (Stream stream = dig.OpenFile())
                {
                    TextRange range = new TextRange(
                        _textBox.Document.ContentStart,
                        _textBox.Document.ContentEnd
                        );

                    range.Load(stream, DataFormats.Rtf);
                }
                return true;
            }
            return false;
        }

        public bool SaveDocument()
        {
            if (string.IsNullOrEmpty(_currentFile))
            {
                return SaveDocumentAs();
            }
            else
            {
                using (Stream stream = new FileStream(_currentFile, FileMode.Create))
                {
                    TextRange range = new TextRange(
                        _textBox.Document.ContentStart,
                        _textBox.Document.ContentEnd
                        );

                    range.Save(stream, DataFormats.Rtf);
                }
                return true;
            }
        }
        public bool SaveDocumentAs()
        {
            SaveFileDialog dig = new SaveFileDialog();
            dig.Filter = "Rich Text DocumentManager|*.rtf|Text  DocumentManager|*.txt";

            if (dig.ShowDialog() == true)
            {
                _currentFile = dig.FileName;
                return SaveDocument();
            }
            return false;
        }
       
        public void ApplyToSelection(DependencyProperty property, object value)
        {
	        if(value != null)
	            _textBox.Selection.ApplyPropertyValue(property, value);
        }

        public void NewDocument()
        {
            _currentFile = null;
            _textBox.Document = new FlowDocument();
        }
    }
}

