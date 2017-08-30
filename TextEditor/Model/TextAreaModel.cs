/// <summary>
/// Main Model
/// Author: Justin Bonello
/// </summary>



using System.ComponentModel;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit;

namespace TextEditor.Model
{
    class TextAreaModel : INotifyPropertyChanged
    {
        private string header_;
        public string header
        {
            get { return header_; }
            set
            {
                if (header_ != value)
                {
                    header_ = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("header"));
                }
            }
        }

        private ICSharpCode.AvalonEdit.TextEditor content_;
        public ICSharpCode.AvalonEdit.TextEditor content
        {
            get { return content_; }
            set
            {
                if (content_ != value)
                {
                    content_ = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("content"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
