namespace BlogApp.Entities
{
    public class Author
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public Author(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}