namespace Game.DB
{
    public class User
    {
        public string Name;
        public int Age;
        public string Status;

        public User(string name, int age, string status)
        {
            Name = name;
            Age = age;
            Status = status;
        }
    }
}
