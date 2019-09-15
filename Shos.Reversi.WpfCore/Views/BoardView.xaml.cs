using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

namespace Shos.Reversi.Wpf.Views
{
    using Shos.Reversi.Core.Helpers;
    using Shos.Reversi.Wpf.ViewModels;
    using Shos.Reversi.Wpf.Helpers;

    public partial class BoardView : UserControl
    {
        public BoardView()
        {
            InitializeComponent();

            Enumerable.Range(0, Core.Board.RowNumber).ForEach(
                () => panel.RowDefinitions.Add(new RowDefinition())
            );
            Enumerable.Range(0, Core.Board.ColumnNumber).ForEach(
                () => panel.ColumnDefinitions.Add(new ColumnDefinition())
            );

            DataContextChanged += OnDataContextChanged;
        }

        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((BoardViewModel)DataContext).ForEach((index, stoneViewModel) => {
                var stoneView = new StoneView { DataContext = stoneViewModel };
                stoneView.SetBinding(Ellipse.FillProperty, nameof(stoneViewModel.FillColor), BindingMode.OneWay);
                Grid.SetRow(stoneView, index.Row);
                Grid.SetColumn(stoneView, index.Column);
                panel.Children.Add(stoneView);
            });
        }
    }
}
