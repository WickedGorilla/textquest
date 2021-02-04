using System;
using System.Collections.Generic;

namespace TextQuest
{
    class Program
    {
        private static void Main(string[] args)
        {
            Quest.Start();
        }
    }

    public static class Quest
    {
        private static readonly List<ISelectable> Selectables = new List<ISelectable>();
        private static readonly List<ISelectable> HistorySelectables = new List<ISelectable>();

        private static readonly List<SingleChoice> SingleChoices = new List<SingleChoice>();

        private static List<Room> _rooms = new List<Room>();

        public static void Start()
        {
            Initialization();
            OutPutRooms();
        }

        private static void Initialization()
        {
            List<Choice> choiceList1 = GetChoices(1, 
                new[] {"Получить предметы от бродяги"},
                new[] {"Вам передали медальон и 50 монет"},
                new[] { true });

            List<Choice> choiceList2 = GetChoices(1, 
                new []{ "Описание окружения в комнате" }, 
                new []{ "Обычная поврежденная российская дорога"}, 
                new [] { false });

            List<Action> actions = GetActions(2, 
                new []{ "Поговорить с бродягой", "Осмотреть окрестности"}, 
                new []{ choiceList1, choiceList2});

            Room road = new Room("Дорога", actions);
            _rooms.Add(road);

            List<Choice> GetChoices(int count, string[] names, string[] descriptions, bool[] isSingle)
            {
                List<Choice> list = new List<Choice>();
                for (int i = 0; i < count; i++)
                {
                    if (isSingle[i])
                    {
                        SingleChoice singleChoice = new SingleChoice(names[i], descriptions[i]);
                        list.Add(singleChoice);
                    }
                    else
                    {
                        Choice choice = new Choice(names[i], descriptions[i]);
                        list.Add(choice);
                    }
                }

                return list;
            }

            List<Action> GetActions(int count, string[] names, List<Choice>[] choices)
            {
                List<Action> actions = new List<Action>();
                for (int i = 0; i < count; i++)
                {
                    Action action = new Action(names[i], choices[i]);
                    actions.Add(action);
                }

                return actions;
            }
        }

        private static int ReadInput()
        {
            Console.WriteLine("Введите индекс выбора:");
            string line = Console.ReadLine();
            int input = Convert.ToInt32(line);
            return input;
        }

        private static void SelectVariable()
        {   
            Console.WriteLine("Введите 0 чтобы выйти из взаимодействия");

            int index;
            do
            {
                index = ReadInput();
            } while (index > Selectables.Count);

            if (index == 0) ComeBack();

            Selectables[index - 1].Accept(new Selected());
        }

        private static void ComeBack()
        {
            int preLastIndex = HistorySelectables.Count - 2;
            if (preLastIndex >= 0)
            {
                var preLast = HistorySelectables[preLastIndex];
                HistorySelectables.RemoveAt(preLastIndex + 1);
                HistorySelectables.Remove(preLast);
                preLast.Accept(new Selected());
            }
            else
            {
                HistorySelectables.Clear();
                OutPutRooms();
            }
        }

        private static void OutPutRooms()
        {
            for (int i = 0; i < _rooms.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_rooms[i].Name}");
                Selectables.Add(_rooms[i]);
            }

            SelectVariable();
        }

        public static void Selectable(Room room)
        {
            Console.WriteLine($"Вы вошли в комнату {room.Name}");
            Console.WriteLine("Выберите действие");
            HistorySelectables.Add(room);
            OutPutChoices(room);
        }

        public static void Selectable(Action action)
        {
            Console.WriteLine("Выберите вариант:");
            HistorySelectables.Add(action);
            OutPutChoices(action);
        }

        public static void Selectable(Choice сhoice)
        {
            OutPutDescriptionChoice(сhoice);
            SelectableChoice(сhoice);
        }

        public static void Selectable(SingleChoice singleChoice)
        {
            if (SingleChoices.Contains(singleChoice))
            {
                SelectableChoice(singleChoice);
                return;
            }

            SingleChoices.Add(singleChoice);
            OutPutDescriptionChoice(singleChoice);
            SelectableChoice(singleChoice);
        }

        private static void SelectableChoice(Choice сhoice)
        {
            HistorySelectables.Add(сhoice);
            Selectables.Clear();
            ComeBack();
        }

        private static void OutPutDescriptionChoice(Choice сhoice)
        {
            Console.WriteLine($"{сhoice.Description}");
        }

        private static void OutPutChoices(IHaveChoice iHaveChoice)
        {
            Selectables.Clear();
            int index = 1;
            foreach (var action in iHaveChoice.GetChoices())
            {
                Console.WriteLine($"{index}. {action.Name}");
                Selectables.Add(action);
                index++;
            }

            SelectVariable();
        }
    }
}