using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class GetFilteredTasksCommand : BaseRequest<GetFilteredTasksResponse>
    {
        /// <summary>
        /// The size of the collection
        /// </summary>
        public int Size { get; set; } = 0;

        /// <summary>
        /// The offset
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Name of the property which will perform the ordering
        /// </summary>
        public string OrderProperty { get; set; }

        /// <summary>
        /// The order direction e.g. asc or desc
        /// </summary>
        public int OrderDirection { get; set; }

        /// <summary>
        /// The property which will filter the collection
        /// </summary>
        public string? FilterProperty { get; set; }

        /// <summary>
        /// The value of property to filter the collection
        /// </summary>
        public string? FilterValue { get; set; }

        /// <summary>
        /// The flag indicating whether to filter the collection to show only records creather by me
        /// </summary>
        public bool CreatedByMe { get; set; } = false;
    }
}
