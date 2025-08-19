namespace Application.Common
{
    public interface IViewModelBase
    {
        public long? Id { get; set; }
    }

    public interface ICreateViewModel
    {

    }

    public interface IUpdateViewModel
    {
        public long Id { get; set; }
    }
}
