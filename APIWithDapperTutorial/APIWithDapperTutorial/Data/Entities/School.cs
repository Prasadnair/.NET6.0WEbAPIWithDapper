namespace APIWithDapperTutorial.Data.Entities
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get;set; }

        public List<Student> Students { get; set; } = new List<Student>();

    }
}
