using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TextEditor.Action;
using TextEditor.model;
using TextEditor.Store;
using System.Runtime.CompilerServices;
using TextEditor.Properties;

namespace TextEditor.ViewModel
{
    public sealed class BeginPageVm : INotifyPropertyChanged
    {
        public string ButtonText1 { get; set; } = "Begin";
        public string ButtonText2 { get; set; } = "End";
        public string BookTitle { get; set; }
        public ObservableCollection<ReadingBookData> ReadingBookList { get; set; }=new ObservableCollection<ReadingBookData>();
        public ObservableCollection<EndingBookData> EndReadingBookList { get; set; }=new ObservableCollection<EndingBookData>();

        private readonly Dispatcher.Dispatcher dispatcher;
        private BookDataStore bookDataStore;
        private ActionCreator actionCreator;

        public BeginPageVm()
        {
            dispatcher = new Dispatcher.Dispatcher();
            actionCreator = ActionCreator.Get(dispatcher);
            bookDataStore = BookDataStore.Get(dispatcher);
            AddCommand.action = () => actionCreator.Begin(BookTitle);
            EndCommand.action = () => actionCreator.End(BookTitle);
            bookDataStore.ReadingBookEventHandler += BookDataStore_ReadingBookEventHandler;
            bookDataStore.EndBookEventHandler += BookDataStore_EndBookEventHandler;
        }

        private void BookDataStore_ReadingBookEventHandler(ReadingBookData books)
        {
            ReadingBookList.Add(books);
            OnPropertyChanged(nameof(ReadingBookList));
        }

        private void BookDataStore_EndBookEventHandler(EndingBookData books)
        {
            EndReadingBookList.Add(books);
            OnPropertyChanged(nameof(EndReadingBookList));
        }

        public AddButtonCommand AddCommand = new AddButtonCommand();
        public AddButtonCommand EndCommand = new AddButtonCommand();

        public class AddButtonCommand : ICommand
        {
            public System.Action action;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                action();
            }

            public event EventHandler CanExecuteChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}