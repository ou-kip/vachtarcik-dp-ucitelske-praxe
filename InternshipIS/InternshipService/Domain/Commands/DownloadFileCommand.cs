namespace InternshipService.Domain.Commands
{
    public class DownloadFileCommand
    {
        public DownloadFileCommand() 
        { 
        }

        public Guid ParentId { get; set; }
        public string FileName { get; set; }
    }
}
