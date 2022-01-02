using Microsoft.NetConf2021.Maui.Resources.Strings;
using Command = MvvmHelpers.Commands.Command;

namespace Microsoft.NetConf2021.Maui.ViewModels;

public class DiscoverViewModel : BaseViewModel
{
    private readonly ShowsService showsService;
    private readonly SubscriptionsService subscriptionsService;
    private IEnumerable<ShowViewModel> shows;
    private CategoriesViewModel categoriesVM;
    private string text;

    public ObservableRangeCollection<ShowGroup> PodcastsGroup { get; private set; } = new ObservableRangeCollection<ShowGroup>();
    public ObservableRangeCollection<ShowViewModel> ShowList { get; private set; } = new ObservableRangeCollection<ShowViewModel>();

    public ICommand SearchCommand { get; }

    public ICommand SubscribeCommand => new AsyncCommand<ShowViewModel>(SubscribeCommandExecute);

    public ICommand SeeAllCategoriesCommand => new AsyncCommand(SeeAllCategoriesCommandExecute);

    public string Text
    {
        get { return text; }
        set 
        {
            SetProperty(ref text, value);
        }
    }  

    public CategoriesViewModel CategoriesVM
    {
        get { return categoriesVM; }      
        set {  SetProperty(ref categoriesVM, value); }
    }

    public DiscoverViewModel()
    {
        showsService = ServicesProvider.GetService<ShowsService>();
        subscriptionsService = ServicesProvider.GetService<SubscriptionsService>();

        SearchCommand = new AsyncCommand(OnSearchCommandAsync);
        categoriesVM = new CategoriesViewModel();

        PodcastsGroup.CollectionChanged += (s, e) =>
        {
            Console.WriteLine($"^^^^^^^^^^^^^ PodcastsGroup Changed! ^^^^^^^^^^^^^^^^");
            foreach (var showGroup in PodcastsGroup)
            {
                Console.WriteLine($"          showGroup: {showGroup.Name}");
            }
            Console.WriteLine($"-------------- PodcastsGroup Changed! ---------------");

        };
    }

    internal async Task InitializeAsync()
    {
        await FetchAsync();
    }

    private async Task FetchAsync()
    {
        var podcastsModels = await showsService.GetShowsAsync();

        if (podcastsModels == null)
        {
            await Shell.Current.DisplayAlert(
                AppResource.Error_Title,
                AppResource.Error_Message,
                AppResource.Close);

            return;
        }

        await CategoriesVM.InitializeAsync();
        shows = await ConvertToViewModels(podcastsModels);
        UpdatePodcasts(shows);
    }

    private async Task<List<ShowViewModel>> ConvertToViewModels(IEnumerable<Show> podcasts)
    {
        var viewmodels = new List<ShowViewModel>();
        Console.WriteLine($"######### podcasts Count: {podcasts.Count()} !!");
        foreach (var podcast in podcasts)
        {
            Console.WriteLine($"######### Fetching Title {podcast.Title} ID : {podcast.Id} !!");
            Console.WriteLine($"######### Fetching {podcast.Episodes.Count()} Episodes: {podcast.Episodes} !!");
            foreach (var episode in podcast.Episodes)
            {
                Console.WriteLine($"######### Fetching SHOW: {episode.Title}");
            }
            var podcastViewModel = new ShowViewModel(podcast);
            await podcastViewModel.InitializeAsync();
            viewmodels.Add(podcastViewModel);
        }

        return viewmodels;
    }

    private void UpdatePodcasts(IEnumerable<ShowViewModel> listPodcasts)
    {
        var list = new ObservableRangeCollection<ShowGroup>
        {
            new ShowGroup(AppResource.Whats_New, listPodcasts.Take(3).ToList()),
            new ShowGroup(AppResource.Specially_For_You, listPodcasts.Take(3..).ToList())
        };

        PodcastsGroup.ReplaceRange(list);

        ShowList.ReplaceRange(listPodcasts);
        ShowList[0].Name = AppResource.Whats_New;
        ShowList[1].Name = AppResource.Specially_For_You;
    }

    private async Task OnSearchCommandAsync()
    {     
        var list = await showsService.SearchShowsAsync(Text);
        if (string.IsNullOrWhiteSpace(Text))
        {
            list = await showsService.GetShowsAsync();
        }
        if (list != null)
        {
            UpdatePodcasts(await ConvertToViewModels(list));
        }
    }

    private async Task SubscribeCommandExecute(ShowViewModel vm)
    {
        await subscriptionsService.UnSubscribeFromShowAsync(vm.Show);
        vm.IsSubscribed = subscriptionsService.IsSubscribed(vm.Show.Id);
    }

    private Task SeeAllCategoriesCommandExecute()
    {
        return Shell.Current.GoToAsync($"{nameof(CategoriesPage)}");
    }
}
