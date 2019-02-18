using System;
using System.Collections.Generic;
using System.Linq;
using TextEditor.Action;
using TextEditor.model;

namespace TextEditor.Store
{
    public class BookDataStore
    {
        public delegate void ReadingBookChangeEventHandler(ReadingBookData books);

        public event ReadingBookChangeEventHandler ReadingBookEventHandler;

        public delegate void EndBookChangeEventHandler(EndingBookData books);

        public event EndBookChangeEventHandler EndBookEventHandler;

        private static BookDataStore instance;
        private ReadingBookData Readingbook;
        private EndingBookData Endbook;
        private readonly Dispatcher.Dispatcher dispacher;
        private List<ReadingBookData> readingBooks;

        private BookDataStore(Dispatcher.Dispatcher dispacher)
        {
            this.dispacher = dispacher;
            Readingbook = new ReadingBookData();
            Endbook = new EndingBookData();
            readingBooks = new List<ReadingBookData>();
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
                    var item = new ReadingBookData() {Title = book.Item1, StartTime = book.Item2};

                    if(readingBooks.Any(x => x.Title == book.Item1)) return;
                    if(string.IsNullOrEmpty(item.Title)) return;

                    readingBooks.Add(item);
                    Readingbook = item;
                    ReadingBookEventHandler?.Invoke(Readingbook);
                    break;
                }
                case ActionType.End:
                {
                    var book = action.GetBookDatas();
                    if(string.IsNullOrEmpty(book.Item1)) return;

                    TimeSpan time;
                    var findbook = readingBooks.FirstOrDefault(x => x.Title == book.Item1);
                    try
                    {
                        time = book.Item2 - findbook.StartTime;
                    }
                    catch(NullReferenceException e)
                    {
                        Console.WriteLine("readingbook null :" + e);
                        return;
                    }

                    var item = new EndingBookData {Title = book.Item1, ReadingTime = time};

                    Endbook = item;
                    EndBookEventHandler?.Invoke(Endbook);
                    break;
                }
            }
        }
    }
}