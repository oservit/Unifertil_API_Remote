namespace Application.Features.Base
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
