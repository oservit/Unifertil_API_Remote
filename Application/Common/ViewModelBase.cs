namespace Application.Common
{
    public class ViewModelBase : IViewModelBase
    {
        public long? Id { get; set; }
    }

    public class CreateViewModel : ICreateViewModel
    {

    }

    public class UpdateViewModel : IUpdateViewModel
    {
        public long Id { get; set; }
    }
}
