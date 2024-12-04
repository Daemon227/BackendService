using BackendService.Models;

namespace BackendService.ViewModels
{
    public class VersionVM
    {
        public string MaVersion { get; set; } = null!;

        public string? TenVersion { get; set; }

        public string? GiaBanVersion { get; set; }

        public string? MaXe { get; set; }

        //public virtual MotoBike? MaXeNavigation { get; set; }
        public virtual ICollection<VersionColorVM> VersionColorVM { get; set; } = new List<VersionColorVM>();
    }
}
