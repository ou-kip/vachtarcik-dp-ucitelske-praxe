using Contracts;
using Core.Domain.Requests;
using InternshipService.Domain.Responses;

namespace InternshipService.Domain.Commands
{
    public class GetAllPersonsCommand : BaseRequest<GetAllPersonsResponse>
    {
        public int Offset { get; set; }
        public int Take { get; set; }
        public string Search {  get; set; }
        public int PersonType { get; set; }
    }
}
