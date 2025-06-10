namespace InternshipService.Domain
{
    public class CategoryDto
    {
        public CategoryDto(Guid id, string code, string name)
        {
            Id = id;
            CodeName = string.Join("-", code, name);
        }

        public Guid Id { get; set; }
        public string CodeName { get; set; }
    }
}
