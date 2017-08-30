/// <summary>
/// Main View Model
/// Author: Justin Bonello
/// Credits: AvalonText Editor ( http://avalonedit.net/ )
/// </summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditor.Model;     // 1.   add model
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Data;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Input;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;

namespace TextEditor.ViewModel
{   

    class MainWindowViewModel : DependencyObject, INotifyPropertyChanged
    {
        //relay commands to use for command bindings
        #region Relay Commands 

        public ICommand CommentCmd
        {
            get{ return new RelayCommand(param => Comment(), true); }
        }

        public ICommand UncommentCmd
        {
            get{ return new RelayCommand(param => Uncomment(), true); }
        }

        private ICommand _SaveCmd { get; set; }
        public ICommand SaveCmd
        {
            get { return _SaveCmd; }
            set
            {
                if (PropertyChanged != null)
                {
                    _SaveCmd = value;
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("SaveCmd"));
                }
            }
        }

        public ICommand SaveAsCmd
        {
            get{ return new RelayCommand(param => SaveDocumentAs(), true); }
        }

        public ICommand NewDocCmd
        {
            get{ return new RelayCommand(param => NewDocument(), true); }
        }

        public ICommand OpenDocCmd
        {
            get{ return new RelayCommand(param => OpenDocument(), true); }
        }
        #endregion

        //status to display actions
        private string _status { get; set; }
        public string status
        {
            get { return _status; }
            set
            {
                if (PropertyChanged != null)
                {
                    _status = value;
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("status"));
                }
            }
        }

        //currently selected tab
        public TextAreaModel _selectedTab { get; set; }
        public TextAreaModel selectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (PropertyChanged != null)
                {                  
                    _selectedTab = value;
                    if(showLineNumber != selectedTab.content.ShowLineNumbers)
                        showLineNumber = selectedTab.content.ShowLineNumbers;
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("selectedTab"));
                }
            }
        }

        public bool showLineNumber
        {
            get
            {
                return selectedTab.content.ShowLineNumbers;
            }
            set
            {
                if (PropertyChanged != null)
                {
                    selectedTab.content.ShowLineNumbers = value;
                    status = Properties.Resources.lineNumsToggled;  //update line number toggle
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("showLineNumber"));
                }              
            }
        }
        public string _currentFile { get; set; } //currentfile path
        public ObservableCollection<TextAreaModel> tabs { get; set; } //collection of tabs
        public MainWindowViewModel()
        {
            tabs = new ObservableCollection<TextAreaModel>()
            {
                new TextAreaModel { header = Properties.Resources.newDocument, content = CreateRTB() }
            }; //set tab to new tab
            selectedTab = tabs.FirstOrDefault();
            SaveCmd = new RelayCommand(param => SaveDocument(), false);
        }

        private ICSharpCode.AvalonEdit.TextEditor CreateRTB()
        {
            var content = new ICSharpCode.AvalonEdit.TextEditor();
            content.Name = "display";
            return content;
        }

        public void NewDocument()
        { //create a new tab
            _currentFile = null;
            selectedTab = new TextAreaModel()
            {
                header = Properties.Resources.newDocument,
                content = CreateRTB()
            };
            tabs.Add(selectedTab);
            status = Properties.Resources.documentCreated;
        }

        public void OpenDocument()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                _currentFile = dlg.FileName;
                selectedTab = new TextAreaModel()
                {
                    header = dlg.FileName.Split('\\').Last(), 
                    content = CreateRTB()
                };

                selectedTab.content.Load(_currentFile);
                tabs.Add(selectedTab);
                CanSaveDocument();
                status = Properties.Resources.documentLoaded;
            }
        }

        public void SaveDocument()
        {
            if (string.IsNullOrEmpty(_currentFile))
                SaveDocumentAs();
            else
            {
                selectedTab.content.Save(_currentFile);
                status = Properties.Resources.documentSaved;
            }
        }

        public void SaveDocumentAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();

            if (dlg.ShowDialog() == true)
            {
                _currentFile = dlg.FileName;
                SaveDocument();
                CanSaveDocument();
            }
        }

        public void Comment()
        {
            selectedTab.content.SelectedText = selectedTab.content.SelectedText.Insert(0, "<!--");
            selectedTab.content.SelectedText = selectedTab.content.SelectedText.Insert(selectedTab.content.SelectedText.Length, "-->");
            status = Properties.Resources.commentedLines;
        }

        public void Uncomment()
        {
            selectedTab.content.SelectedText = selectedTab.content.SelectedText.Replace("<!--", "");
            selectedTab.content.SelectedText = selectedTab.content.SelectedText.Replace("-->", "");
            status = Properties.Resources.uncommentedLines;
        }

        public void CanSaveDocument()
        {
            SaveCmd = new RelayCommand(param => SaveDocument(), !string.IsNullOrEmpty(_currentFile));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class RelayCommand : ICommand
    {
        private Action<Object> _action;
        private bool _canExecute;
        public RelayCommand(Action<Object> action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}