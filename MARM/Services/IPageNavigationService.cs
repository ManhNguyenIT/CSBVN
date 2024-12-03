namespace MARM.Services
{
    public interface IPageNavigationService
    {
        event Action<int> PageChanged;
        int TotalPage { get; set; }
        void NavigateTo(int pageIndex);
        void NavigateBack();
        void NavigateForward();
    }
}
