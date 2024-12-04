using BackendService.Models;

namespace BackendService.ViewModels
{
    public class LibraryImageVM
    {
        public int ImageId { get; set; }

        public string? MaLibrary { get; set; }

        public string? ImageUrl { get; set; }

        //public virtual MotoLibrary? MaLibraryNavigation { get; set; }
    }
}
