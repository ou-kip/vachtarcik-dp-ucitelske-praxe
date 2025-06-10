namespace InternshipService.Domain.Enums
{
    /// <summary>
    /// The enum with states in which the Internship can be
    /// </summary>
    public enum InternshipStateEnum
    {
        /// <summary>
        /// Internship is created
        /// </summary>
        Created = 0,

        /// <summary>
        /// Internship is published
        /// </summary>
        Published = 1,

        /// <summary>
        /// Internship is chosen by student
        /// </summary>
        Chosen = 2,

        /// <summary>
        /// The internship is closed
        /// </summary>
        Closed = 3,

        /// <summary>
        /// The internship is canceled
        /// </summary>
        Canceled = 4
    }
}
