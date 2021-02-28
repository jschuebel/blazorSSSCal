using Fluxor;
using SSSCalBlazor.Client.Models;
using System;

namespace SSSCalBlazor.Client.Store
{
    public class PersonState
    {
        public int currentPage { get; set; }
        public PeopleModel peopleModel { get; set; }

        public PersonState() { }
        public PersonState(int currentpage, PeopleModel p)
        {
            currentPage = currentpage;
            peopleModel = p;
        }
    }

    public class PersonFeature : Feature<PersonState>
    {
        public override string GetName() => nameof(PersonState);

        protected override PersonState GetInitialState()
        {
            return new PersonState(0, new PeopleModel())  ;
        }
    }

    public class AddPerson
    {
        public PeopleModel peopleModel { get; set; }

        public AddPerson(PeopleModel p) => peopleModel = p;
    }
    public class ChangePage
    {
        public int page { get; set; }

        public ChangePage(int p) => page = p;
    }


    public static class PersonReducer
    {

        [ReducerMethod]
        public static PersonState ReducePersonAdd(PersonState state, AddPerson action)
        {
            Console.WriteLine("========>ReducePersons  AddPerson...");

            return new PersonState(1, action.peopleModel);
        }

        [ReducerMethod]
        public static PersonState ReducePersonPage(PersonState state, ChangePage action)
        {
            Console.WriteLine("========>LoadTodosActionsReducer  ChangePage...");

            return new PersonState(action.page, state.peopleModel);
        }
    }

}
