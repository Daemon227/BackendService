using BackendService.Models;

namespace BackendService.ViewModels
{
    public class VersionColorVM
    {
        public string MaVersionColor { get; set; } = null!;

        public string? TenMau { get; set; }

        public string? MaVersion { get; set; }

        //public virtual MotoVersion? MaVersionNavigation { get; set; }

        public virtual ICollection<VersionImageVM> VersionImageVM { get; set; } = new List<VersionImageVM>();
    }
}
