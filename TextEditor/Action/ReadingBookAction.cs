using System;

namespace TextEditor.Action
{
    public class ReadingBookAction
    {
        private readonly ActionType type;
        private (string, DateTime) books;

        public ReadingBookAction(ActionType type, (string, DateTime) books)
        {
            this.type = type;
            this.books = books;
        }

        public static Builder Type(ActionType type)
        {
            return new Builder().With(type);
        }

        public ActionType GetActionType()
        {
            return type;
        }

        public (string, DateTime) GetBookDatas()
        {
            return books;
        }


        public class Builder
        {
            private ActionType type;
            private (string, DateTime) books;

            public Builder With(ActionType type)
            {
                this.type = type;
                return this;
            }

            public Builder Bundle(string title, DateTime dt)
            {
                books = (title, dt);
                return this;
            }

            public ReadingBookAction Build()
            {
                return new ReadingBookAction(type, books);
            }
        }
    }
}