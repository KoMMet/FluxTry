using TextEditor.Properties;

namespace TextEditor.Action
{
    public class ActionCreator
    {
        private static ActionCreator instance;
        private readonly Dispatcher.Dispatcher dispatcher;

        public ActionCreator(Dispatcher.Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [NotNull]
        public static ActionCreator Get(Dispatcher.Dispatcher dispatcher)
        {
            if(instance == null) instance = new ActionCreator(dispatcher);
            return instance;
        }

        public void Begin(string title)
        {
            dispatcher.Dispatch(ActionType.Begin, title);
        }

        public void End(string title)
        {
            dispatcher.Dispatch(ActionType.End, title);
        }
    }
}