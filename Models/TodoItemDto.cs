namespace ToDoList_web_API.Models
{
    public class TodoItemDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public required string Prenom { get; set; }
        public string? Email { get; set; }
        public DateTime birthDate { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public string? profilePhoto { get; set; }

        public string? Secret { get; set; }
        public bool isCompleted { get; set; }

    }
}
