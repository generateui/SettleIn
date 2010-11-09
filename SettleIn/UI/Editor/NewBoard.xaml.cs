using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.ComponentModel;
using System.Collections.ObjectModel;

using SettleInCommon.Board;

namespace SettleIn
{
    /// <summary>
    /// Interaction logic for NewBoard.xaml
    /// </summary>
    public partial class NewBoard : Window
    {
        CollectionViewSource cvs;
        EBoardCreatedType _CurrentFilter = EBoardCreatedType.Template;

        public NewBoard()
        {
            InitializeComponent();
            StackPanel s = new StackPanel();
            ListBoxItem i = new ListBoxItem();
            //CollectionViewSource cv = (CollectionViewSource) CollectionViewSource.GetDefaultView(Core.Instance.BoardsList.Templates);
            //cv.Filter+=new FilterEventHandler(cv_Filter);
            //cvs = (CollectionViewSource)CollectionViewSource.GetDefaultView(Core.GetTemplates());
            cvs = (CollectionViewSource)this.Resources["boards"];
            cvs.Filter+=new FilterEventHandler(cvs_Filter);
        }

        void cv_Filter(object sender, FilterEventArgs e)
        {
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TabItem_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void tiOfficial_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        void cvs_Filter(object sender, FilterEventArgs e)
        {
        }

        private void btnTemplates_MouseUp(object sender, MouseButtonEventArgs e)
        {
            cvs.View.Filter = new Predicate<object>(delegate(object o)
            {
                BoardVisual b = o as BoardVisual;
                return (b.Game.Board.BoardType == EBoardCreatedType.Template);
            });
        }

        private void btnOfficial_MouseUp(object sender, MouseButtonEventArgs e)
        {
            cvs.View.Filter = new Predicate<object>(delegate(object o)
            {
                BoardVisual b = o as BoardVisual;
                return (b.Game.Board.BoardType == EBoardCreatedType.Official);
            });
        }

        private void btnCustom_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _CurrentFilter = EBoardCreatedType.CustomCreated;
            cvs.Filter -= cvs_Filter;
            cvs.Filter += cvs_Filter;
        }

        private void btnDownloaded_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _CurrentFilter = EBoardCreatedType.Downloaded;
            cvs.Filter -= cvs_Filter;
            cvs.Filter += cvs_Filter;
        }

        private void btnDownloaded_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void test_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDownloaded_Click(object sender, RoutedEventArgs e)
        {
            cvs.View.Filter = new Predicate<object>(delegate(object o)
            {
                BoardVisual b = o as BoardVisual;
                return (b.Game.Board.BoardType == EBoardCreatedType.Template);
            });

        }

        private void btnCustom_Click(object sender, RoutedEventArgs e)
        {
            cvs.View.Filter = new Predicate<object>(delegate(object o)
            {
                BoardVisual b = o as BoardVisual;
                return (b.Game.Board.BoardType == EBoardCreatedType.Official);
            });

        }

        private void btnOfficial_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTemplates_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUseBoard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                
        }

    }
}
