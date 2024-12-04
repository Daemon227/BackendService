using BackendService.Models;

namespace BackendService.ViewModels
{
    public class VersionImageVM
    {
        public int ImageId { get; set; }

        public string? MaVersionColor { get; set; }

        public string? ImageUrl { get; set; }

        //public virtual VersionColor? MaVersionColorNavigation { get; set; }
    }
}
