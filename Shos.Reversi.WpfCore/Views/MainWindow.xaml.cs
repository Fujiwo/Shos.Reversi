//#define MLPlayer
#define AIPlayer

using System.Windows;

namespace Shos.Reversi.Wpf.Views
{
    using Shos.Reversi.Core;
#if MLPlayer
    using Shos.Reversi.ML;
#elif AIPlayer // MLPlayer
    using Shos.Reversi.AI;
#endif // MLPlayer
    using ViewModels;

    public partial class MainWindow : Window
    {
#if MLPlayer
        const string modelPath = @"Data\MLModel.zip";
        Game game = new Game(() => new MLPlayer(modelPath));
#elif AIPlayer // MLPlayer
        Game game = new Game(() => new AIPlayer());
#else  // MLPlayer
        Game game = new Game();
#endif // MLPlayer

        public MainWindow()
        {
            InitializeComponent();
            SetDataContext();

            game.PropertyChanged += (_, arg) => {
                switch (arg.PropertyName) {
                    case nameof(game.Board):
                        SetDataContext();
                        break;
                }
            };
        }

        void SetDataContext()
        {
            var boardViewModel = new BoardViewModel(game);
            boardViewModel.StoneClick += (index, stoneViewModel) => game.OnSelectStone(game.Board, index);
            boardView.DataContext = boardViewModel;
            sideBar  .DataContext = new SideBarViewModel(game);
        }
    }
}
