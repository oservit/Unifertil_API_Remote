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
        public virtual long Id { get; set; }
    }
}
