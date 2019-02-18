using System;
using TextEditor.Action;

namespace TextEditor.Dispatcher
{
    public class Dispatcher
    {
        public Action<ReadingBookAction> Action;
        
        public void Dispatch(ActionType type, string title)
        {
            ReadingBookAction.Builder actionBuilder = ReadingBookAction.Type(type);
            actionBuilder.Bundle(title, DateTime.Now);
            var action = actionBuilder.Build();
            Action?.Invoke(action);
        }
    }
}