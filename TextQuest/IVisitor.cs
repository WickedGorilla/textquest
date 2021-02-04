namespace TextQuest
{
    public interface IVisitor
    {
        void Visit(Room room);
        void Visit(Action action);
        void Visit(Choice choice);
        void Visit(SingleChoice singleChoice);

    }

    public interface ISelectable
    {
        string Name { get; }
        void Accept(IVisitor visitor);
    }

    public class Selected : IVisitor
    {
        public void Visit(Room room) =>
            Quest.Selectable(room);

        public void Visit(Action action) =>
            Quest.Selectable(action);

        public void Visit(Choice choice) =>
            Quest.Selectable(choice);

        public void Visit(SingleChoice singleChoice) =>
            Quest.Selectable(singleChoice);
    }
}
