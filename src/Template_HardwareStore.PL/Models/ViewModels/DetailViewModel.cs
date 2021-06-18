namespace Template_HardwareStore.PL.Models.ViewModels
{
    public class DetailViewModel
    {
        public DetailViewModel()
        {
            Product = new Product();
        }

        public Product Product { get; set; }

        public bool ExistInCart { get; set; }
    }
}
