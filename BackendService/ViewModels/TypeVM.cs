using BackendService.Models;

namespace BackendService.ViewModels
{
    public class TypeVM
    {
        public string MaLoai { get; set; } = null!;

        public string? TenLoai { get; set; }

        public string? DoiTuongSuDung { get; set; }

        public string? MoTaNgan { get; set; }

        //public virtual ICollection<MotoVM> MotoBikes { get; set; } = new List<MotoVM>();
    }
}
