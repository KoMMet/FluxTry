using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public List<ReadingBookData> ReadingBookList { get; set; }
        public List<EndingBookData> EndReadingBookList { get; set; }

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
            bookDataStore.ReadingBookEventHandler += BookDataStoreReadingBookEventHandler;
            bookDataStore.EndBookEventHandler += BookDataStore_EndBookEventHandler;
        }

        private void BookDataStore_EndBookEventHandler(List<EndingBookData> books)
        {
            EndReadingBookList = books;
            OnPropertyChanged(nameof(EndReadingBookList));
        }

        private void BookDataStoreReadingBookEventHandler(List<ReadingBookData> books)
        {
            ReadingBookList = books;
            OnPropertyChanged(nameof(ReadingBookList));
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