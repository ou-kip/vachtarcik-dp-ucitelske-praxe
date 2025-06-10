namespace InternshipService.Domain
{
    public class PersonDto
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PersonDto() { }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="lastName"></param>
        public PersonDto(Guid id, string name, string lastName, string email, int personType)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            PersonType = personType;
        }

        /// <summary>
        /// The Id of the person
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The type of the person
        /// </summary>
        public int PersonType { get; set; }
    }
}
