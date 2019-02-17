using System;
using System.Collections.Generic;
using System.Linq;
using TextEditor.Action;
using TextEditor.model;

namespace TextEditor.Store
{
    public class BookDataStore
    {
        public delegate void ReadingBookChangeEventHandler(List<ReadingBookData> books);
        public event ReadingBookChangeEventHandler ReadingBookEventHandler;
        public delegate void EndBookChangeEventHandler(List<EndingBookData> books);
        public event EndBookChangeEventHandler EndBookEventHandler;

        private static BookDataStore instance;
        private List<ReadingBookData> Readingbooks;
        private List<EndingBookData> Endbooks;
        private readonly Dispatcher.Dispatcher dispacher;

        private BookDataStore(Dispatcher.Dispatcher dispacher)
        {
            this.dispacher = dispacher;
            Readingbooks = new List<ReadingBookData>();
            Endbooks = new List<EndingBookData>();
            this.dispacher.Action += OnAction;

        }

        public static BookDataStore Get(Dispatcher.Dispatcher dispatcher)
        {
            if(instance == null) instance = new BookDataStore(dispatcher);
            return instance;
        }

        public void OnAction(ReadingBookAction action)
        {
            switch(action.GetActionType())
            {
                case ActionType.Begin:
                    {
                        var book = action.GetBookDatas();
                        var item = new ReadingBookData() { Title = book.Item1, StartTime = book.Item2 };
                        Readingbooks.Add(item);
                        ReadingBookEventHandler?.Invoke(Readingbooks);
                        break;
                    }
                case ActionType.End:
                    {
                        var book = action.GetBookDatas();
                        ReadingBookData readingbook = null;
                        try
                        {
                            readingbook = Readingbooks.First(b => book.Item1 == b.Title);
                        }
                        catch(NullReferenceException e)
                        {
                            System.Console.WriteLine("readingbook null :" + e);
                        }

                        var time = book.Item2 - readingbook.StartTime;
                        var item = new EndingBookData { Title = book.Item1, ReadingTime = time };
                        Endbooks.Add(item);
                        EndBookEventHandler?.Invoke(Endbooks);
                        break;
                    }
            }
        }
    }
}