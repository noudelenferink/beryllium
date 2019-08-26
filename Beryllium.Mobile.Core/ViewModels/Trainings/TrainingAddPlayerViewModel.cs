using Beryllium.Shared.Players;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmHelpers;
using System.Linq;
using System.Threading.Tasks;

namespace Beryllium.Mobile.Core.ViewModels.Trainings
{
   public class TrainingAddPlayerViewModel : MvxNavigationViewModel<int[], PlayerListDto>
   {
      private PlayerListDto selectedPlayer;

      public PlayerListDto SelectedPlayer
      {
         get => this.selectedPlayer;
         set
         {
            this.selectedPlayer = value;
            RaisePropertyChanged();
         }
      }
      public IMvxCommand CreateCommand { get; set; }
      private ObservableRangeCollection<PlayerListDto> autoCompleteItems;
      private RankixApi rankixApi;

      public ObservableRangeCollection<PlayerListDto> AutoCompleteItems { get { return autoCompleteItems; } set { autoCompleteItems = value; RaisePropertyChanged(); } }

      public int[] ExcludedPlayers { get; private set; }

      public TrainingAddPlayerViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
      {
         this.rankixApi = new RankixApi();
         this.AutoCompleteItems = new ObservableRangeCollection<PlayerListDto>();

         this.CreateCommand = new MvxCommand(() =>
         {
            this.NavigationService.Close(this, this.SelectedPlayer);
         });
      }

      public async Task RefreshAutoCompleteDataSourceItems(string searchText)
      {
         await Task.Delay(0);

         var searchResult = await this.rankixApi.FindPlayers(searchText);
         if (searchResult.Success)
         {
            var filteredPlayers = searchResult.Data
                                    .Where(p => !this.ExcludedPlayers.Contains(p.PlayerId))
                                    .OrderBy(p => p.LastName)
                                    .ThenBy(p => p.FirstName)
                                    .ToList();

            AutoCompleteItems.ReplaceRange(filteredPlayers);
         }
      }

      public override void Prepare(int[] parameter)
      {
         this.ExcludedPlayers = parameter;
      }
   }
}
