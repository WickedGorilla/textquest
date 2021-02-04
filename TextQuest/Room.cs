using System.Collections.Generic;

namespace TextQuest
{
    public class Room : ISelectable, IHaveChoice
    {
        private readonly List<Action> _actions;
        public string Name { get; }

        public Room(string name, List<Action> actions)
        {
            Name = name;
            _actions = actions;
        }

        public IEnumerable<ISelectable> GetChoices() =>
            _actions;

        public void Accept(IVisitor visitor) => 
            visitor.Visit(this);
    }

    public class Action : ISelectable, IHaveChoice
    {
        private readonly List<Choice> _choices;
        public string Name { get; }

        public Action(string name, List<Choice> choices)
        {
            Name = name;
            _choices = choices;
        }

        public IEnumerable<ISelectable> GetChoices() =>
            _choices;

        public void Accept(IVisitor visitor) => 
            visitor.Visit(this);
    }

    public class Choice : ISelectable
    {
        public string Name { get; }
        public string Description { get; }

        public Choice(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public virtual void Accept(IVisitor visitor) => 
            visitor.Visit(this);
    }

    public class SingleChoice : Choice
    {
        public SingleChoice(string name, string description) : base(name, description)
        {
        }

        public override void Accept(IVisitor visitor) =>
            visitor.Visit(this);
    }

    public interface IHaveChoice
    {
        IEnumerable<ISelectable> GetChoices();
    }
}
