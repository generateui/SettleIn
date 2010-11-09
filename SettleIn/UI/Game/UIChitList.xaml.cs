using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SettleInCommon.Board;

namespace SettleIn
{
	/// <summary>
	/// Interaction logic for UIChitList.xaml
	/// </summary>
	public partial class UIChitList : UserControl
	{
		public UIChitList()
		{
			this.InitializeComponent();

            DataContextChanged += new DependencyPropertyChangedEventHandler(UIChitList_DataContextChanged);
		}

        void UIChitList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            XmlChitList chitList = DataContext as XmlChitList;
            chitList.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(chitList_PropertyChanged);
            ResetUI();
        }

        void ResetUI()
        {
            XmlChitList chitList = DataContext as XmlChitList;
            spMain.Children.Clear();
            for (int i = 0; i < chitList.N2; i++)
                spMain.Children.Add(new Image() { Source = (ImageSource)Core.Instance.Icons["IconChit2"] });
            for (int i = 0; i < chitList.N3; i++)
                spMain.Children.Add(new Image() { Source = (ImageSource)Core.Instance.Icons["IconChit3"] });
            for (int i = 0; i < chitList.N4; i++)
                spMain.Children.Add(new Image() { Source = (ImageSource)Core.Instance.Icons["IconChit4"] });
            for (int i = 0; i < chitList.N5; i++)
                spMain.Children.Add(new Image() { Source = (ImageSource)Core.Instance.Icons["IconChit5"] });
            for (int i = 0; i < chitList.N6; i++)
                spMain.Children.Add(new Image() { Source = (ImageSource)Core.Instance.Icons["IconChit6"] });
            for (int i = 0; i < chitList.N8; i++)
                spMain.Children.Add(new Image() { Source = (ImageSource)Core.Instance.Icons["IconChit8"] });
            for (int i = 0; i < chitList.N9; i++)
                spMain.Children.Add(new Image() { Source = (ImageSource)Core.Instance.Icons["IconChit9"] });
            for (int i = 0; i < chitList.N10; i++)
                spMain.Children.Add(new Image() { Source = (ImageSource)Core.Instance.Icons["IconChit10"] });
            for (int i = 0; i < chitList.N11; i++)
                spMain.Children.Add(new Image() { Source = (ImageSource)Core.Instance.Icons["IconChit11"] });
            for (int i = 0; i < chitList.N12; i++)
                spMain.Children.Add(new Image() { Source = (ImageSource)Core.Instance.Icons["IconChit12"] });
        }

        void chitList_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ResetUI();
        }
	}
}